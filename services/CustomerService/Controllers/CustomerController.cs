using Dapr.Client;

namespace CustomerService.Controllers;

[ApiController]
[Route("/api/customers")]
public class CustomerController(ILogger<CustomerController> logger) : ControllerBase
{
    [HttpGet("{customerId}")]
    public async Task<ActionResult<string>> GetCustomer(
        [Required, FromRoute] string customerId, 
        [FromServices] CustomerRepository repository)
    {
        var customer = await repository.GetAsync(customerId);
        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost("/register-customer")]
    public async Task<ActionResult<CustomerModel>> RegisterCustomer(
        [Required] RegisterCustomerCommand command,
        [FromServices] CustomerRepository repository,
        [FromServices] IEventBus eventBus)
    {

        var customer = new Customer(command.FirstName, command.LastName, command.Email, command.BirthDate, "valid");
        await repository.AddAsync(customer);

        logger.LogInformation("New customer: {FirstName} {LastName} {Email}", customer.FirstName, customer.LastName,
            customer.Email);

        await eventBus.PublishAsync("customer-registered",
            new CustomerRegisteredIntegrationEvent(
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.State.Name,
                customer.BirthDate));
        
        return Ok(customer.ToModel());
    }

    [HttpPost("/determine-existing-customer")]
    public async Task<ActionResult<string>> DetermineExistingCustomer(DetermineExistingCustomerCommand command)
    {
        var customerId = await Queries.CustomerQueries.IdentifyCustomer(command.Email, command.BirthDate);
        if (string.IsNullOrWhiteSpace(customerId))
        {
            return NotFound();
        }

        return Ok(customerId);
    }

    [HttpPost("/know-your-customer")]
    public async Task<ActionResult> KnowYourCustomer(
        [Required] KnowYourCustomerCommand command,
        [FromServices] CustomerRepository customerRepository)
    {
        // fake
        var customer = await customerRepository.GetAsync(command.CustomerId);
        
        // it's not a valid state, customer should have already been registered
        if (customer == null)
            return BadRequest();

        customer.Activate();

        await customerRepository.AddAsync(customer);

        return Ok();
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