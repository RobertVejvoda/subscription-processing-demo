using Camunda.Abstractions;
using Camunda.Command;
using Dapr.Client;

namespace CustomerService.Controllers;

[ApiController]
[Route("/api/customers")]
public class CustomerController(ILogger<CustomerController> logger) : ControllerBase
{
    private const string BpmnProcessId = "register-customer-process_1434vxu";
    
    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerModel>> GetCustomer(
        [Required, FromRoute] string customerId, 
        [FromServices] CustomerRepository repository)
    {
        var customer = await repository.GetAsync(customerId);
        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer.ToModel());
    }

    [HttpPost]
    public async Task<ActionResult<string>> Register(
        [Required, FromBody] RegisterCustomerCommand command,
        [FromServices] IZeebeClient zeebeClient)
    {
        var request = new CreateInstanceRequest(BpmnProcessId, null, null, command);
        var response = await zeebeClient.CreateInstanceAsync(request);

        return Ok(new { ProcessInstanceKey = response.ProcessInstanceKey.ToString() });
    }
    
    // ZEEBE endpoints should start with root path /

    [HttpPost("/register-customer")]
    public async Task<ActionResult<CustomerModel>> RegisterCustomer(
        [Required, FromBody] RegisterCustomerCommand command,
        [FromServices] CustomerRepository repository)
    {

        var customer = new Customer(command.FirstName, command.LastName, command.BirthDate, command.Email);
        await repository.AddAsync(customer);

        logger.LogInformation("New customer: {FirstName} {LastName} {Email}", customer.FirstName, customer.LastName,
            customer.Email);

        return Ok(customer.ToModel());
    }

    [HttpPost("/determine-existing-customer")]
    public async Task<ActionResult<CustomerIdModel>> DetermineExistingCustomer(
        [Required, FromBody] DetermineExistingCustomerCommand command,
        [FromServices] CustomerRepository repository)
    {
        var id = Customer.GetId(command.Email, command.BirthDate);
        var customer = await repository.GetAsync(id);
        return Ok(customer == null ? new CustomerIdModel(null) : new CustomerIdModel(id));
    }

    [HttpPost("/know-your-customer")]
    public async Task<ActionResult<CustomerModel>> KnowYourCustomer(
        [Required, FromBody] KnowYourCustomerCommand command,
        [FromServices] CustomerRepository customerRepository)
    {
        var customer = await customerRepository.GetAsync(command.CustomerId);
        
        // it's not a valid state, customer should have already been registered
        if (customer == null)
            return BadRequest();

        // simulate activation process
        customer.Activate();

        // save
        await customerRepository.AddAsync(customer);

        return Ok(customer.ToModel());
    }

    [HttpPost("/notify-customer")]
    public async Task<ActionResult> NotifyCustomer(
        [Required, FromBody] NotifyCustomerCommand command,
        [FromServices] CustomerRepository customerRepository,
        [FromServices] DaprClient daprClient)
    {
        var customer = await customerRepository.GetAsync(command.CustomerId);
        if (customer == null)
            return NotFound();
        
        // send email
        var body = $"Dear {customer.FirstName}\r\n\r\n, {command.Message}.";
        var metadata = new Dictionary<string, string>
        {
            ["emailFrom"] = "noreply@incredible.inc",
            ["emailTo"] = customer.Email,
            ["subject"] = "Message from your insurance company"
        };

        await daprClient.InvokeBindingAsync("send-email", "create", body, metadata);

        return Ok();
    }
}