using System.Text.Encodings.Web;
using SubscriptionService.Commands;

namespace SubscriptionService.Controllers;

[ApiController]
[Route("/api/subscriptions")]
public class SubscriptionController : ControllerBase
{
    private readonly DaprOptions daprOptions;

    public SubscriptionController(IOptions<DaprOptions> daprOptions)
    {
        this.daprOptions = daprOptions.Value;
    }
    
    [HttpGet("{subscriptionId}")]
    public async Task<ActionResult<Subscription>> Get(string subscriptionId, [FromServices] SubscriptionRepository repository)
    {
        Guard.ArgumentNotNullOrEmpty(subscriptionId);

        var subscription = await repository.GetAsync(subscriptionId);
        if (subscription == null)
            return NotFound();

        return Ok(subscription);
    }
    
    // fully asynchronous subscription processing
    [HttpPost]
    public async Task<ActionResult<string>> Register(
        [Required] RegisterSubscriptionCommand command, 
        [FromServices] SubscriptionRepository repository,
        [FromServices] IEventBus eventBus)
    {
        // todo: validate parameters
        
        // create a new subscription
        var key = Subscription.Key(Guid.NewGuid());
        var subscription = new Subscription(
            key,
            command.ProductId,
            command.LoanAmount,
            command.InsuredAmount,
            SubscriptionState.Registered.Name,
            null,
            null,
            null);
        await repository.AddAsync(subscription);
        
        // trigger integration event
        var @event = new SubscriptionRequestReceivedIntegrationEvent(key, command.FirstName, command.LastName, 
            command.Email, command.Age, command.LoanAmount, command.InsuredAmount);
        await eventBus.PublishAsync(daprOptions.PubSub, "subscription-received", @event);

        var url = UrlEncoder.Default.Encode(key);
        return Accepted(url);
    }
    
     
    //  synchronous subscription processing, but assessment is still async
    [HttpPost("register")]
    public async Task<ActionResult<string>> RegisterSync(
        [Required] RegisterSubscriptionCommand command, 
        [FromServices] CustomerProxyService customerProxy,
        [FromServices] ProductProxyService productProxyService,
        [FromServices] IEventBus eventBus,
        [FromServices] SubscriptionRepository repository)
    {
        // get customer
        var customer = await customerProxy.RegisterCustomerAsync(
            new RegisterCustomerCommand(command.FirstName, command.LastName, command.Email, command.Age));
    
        // get product
        var product = productProxyService.GetProduct(command.ProductId);
        
        // create a new subscription
        var key = Subscription.Key(Guid.NewGuid());
        var subscription = new Subscription(
            key,
            command.ProductId,
            command.LoanAmount,
            command.InsuredAmount,
            SubscriptionState.Registered.Name,
            customer,
            product,
            null);
        await repository.AddAsync(subscription);

        // request assessment
        await eventBus.PublishAsync(daprOptions.PubSub, "subscription-assessment-requested",
            new SubscriptionAssessmentRequestedIntegrationEvent(key, customer, command.LoanAmount,
                command.InsuredAmount));

        var url = UrlEncoder.Default.Encode(key);
        return Created(url, key);
    }

    //
    // bind topics to handlers
    // 
    
    [HttpPost("/customer-registered")]
    public Task HandleAsync([Required] CustomerRegisteredIntegrationEvent @event,
        CustomerIntegrationEventHandler handler)
        => handler.Handle(@event);
   
    [HttpPost("/subscription-assessment-requested")]
    public Task HandleAsync([Required] SubscriptionAssessmentRequestedIntegrationEvent @event,
        [FromServices] SubscriptionIntegrationEventHandler handler)
        => handler.Handle(@event);
    
    [HttpPost("/subscription-assessment-finished")]
    public Task HandleAsync([Required] SubscriptionAssessmentFinishedIntegrationEvent @event,
        [FromServices] SubscriptionIntegrationEventHandler handler)
        => handler.Handle(@event);

}