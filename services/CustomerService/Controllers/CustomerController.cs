using Dapr.Client;

namespace CustomerService.Controllers;

[ApiController]
[Route("/api/customers")]
public class CustomerController(ILogger<CustomerController> logger) : ControllerBase
{
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
    
    // ZEEBE endpoints should start with root path /

    [HttpPost("/register-customer")]
    public async Task<ActionResult> RegisterCustomer(
        [Required] RegisterCustomerCommand command,
        [FromServices] CustomerRepository repository)
    {

        var customer = new Customer(command.FirstName, command.LastName, command.BirthDate, command.Email);
        await repository.AddAsync(customer);

        logger.LogInformation("New customer: {FirstName} {LastName} {Email}", customer.FirstName, customer.LastName,
            customer.Email);

        return Ok(new CustomerIdModel(customer.Id));
    }

    [HttpPost("/determine-existing-customer")]
    public async Task<ActionResult> DetermineExistingCustomer(
        [Required] DetermineExistingCustomerCommand command,
        [FromServices] CustomerRepository repository)
    {
        var id = Customer.GetId(command.Email, command.BirthDate);
        var customer = await repository.GetAsync(id);
        return Ok(customer == null ? new CustomerIdModel(null) : new CustomerIdModel(id));
    }

    [HttpPost("/know-your-customer")]
    public async Task<ActionResult> KnowYourCustomer(
        [Required] KnowYourCustomerCommand command,
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

        return Ok(new CustomerIdModel(customer.Id));
    }

    [HttpPost("/notify-customer")]
    public async Task<ActionResult> NotifyCustomer(
        [Required] NotifyCustomerCommand command,
        [FromServices] CustomerRepository customerRepository,
        [FromServices] DaprClient daprClient)
    {
        var customer = await customerRepository.GetAsync(command.CustomerId);
        if (customer == null)
            return NotFound();
        
        // send email
        var body = $"Dear {customer.FirstName}, {command.Message}";
        var metadata = new Dictionary<string, string>
        {
            ["emailFrom"] = "noreply@incredible.inc",
            ["emailTo"] = customer.Email,
            ["subject"] = "Message from your insurance company"
        };

        await daprClient.InvokeBindingAsync("send-email", "create", body, metadata);

        return Ok(new CustomerIdModel(customer.Id));
    }

    
}