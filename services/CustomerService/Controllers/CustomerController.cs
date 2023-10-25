namespace CustomerService.Controllers;

[ApiController]
[Route("/api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> logger;

    public CustomerController(ILogger<CustomerController> logger)
    {
        this.logger = logger;
    }

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

    [HttpPost]
    public async Task<ActionResult<string>> RegisterCustomer(
        [Required] RegisterCustomerCommand command, 
        [FromServices] CustomerRepository repository)
    {
        var key = Customer.Key(command.Email);
        var customer = await repository.GetAsync(key);
        if (customer == null)
        {
            customer = new Customer(key, command.FirstName, command.LastName, command.Email, command.Age, "valid");
            await repository.AddAsync(customer);

            logger.LogInformation("New customer: {FirstName} {LastName} {Email}", customer.FirstName, customer.LastName, customer.Email);

            var url = UrlEncoder.Default.Encode(key);
            return Created(url, customer);
        }

        return Ok(customer);
    }

    [HttpPost("OnSubscriptionRequestReceived")]
    [Topic(Resources.Bindings.PubSub, "subscription-received")]
    public Task HandleAsync([Required] SubscriptionRequestReceivedIntegrationEvent @event,
        [FromServices] SubscriptionIntegrationEventHandler handler)
        => handler.Handle(@event);
}