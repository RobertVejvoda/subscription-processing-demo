using Dapr;
using SubscriptionService.Events;
using SubscriptionService.Model;
using SubscriptionService.Repository;

namespace SubscriptionService.Controllers;

[ApiController]
[Route("/api/subscriptions")]
public class SubscriptionController : ControllerBase
{
    private readonly ILogger<SubscriptionController> logger;


    public SubscriptionController(ILogger<SubscriptionController> logger)
    {
        this.logger = logger;
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
    
    [HttpGet("requests/{requestId}")]
    public async Task<ActionResult<SubscriptionRequest>> GetRequest(string requestId, [FromServices] SubscriptionRequestRepository repository)
    {
        Guard.ArgumentNotNullOrEmpty(requestId);

        var subscriptionRequest = await repository.GetAsync(requestId);
        if (subscriptionRequest == null)
            return NotFound();

        return Ok(subscriptionRequest);
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> Register(
        [Required] RegisterSubscriptionCommand command, 
        [FromServices] SubscriptionRequestRepository repository,
        [FromServices] IEventBus eventBus)
    {
        // 1. save request
        var key = SubscriptionRequest.Key(Guid.NewGuid());
        var subscriptionRequest = new SubscriptionRequest(key, command.FirstName, command.LastName, command.Email,
            command.Age, command.ProductId, command.LoanAmount, command.InsuredAmount, null, null);
        await repository.AddAsync(subscriptionRequest);        
        
        // 2. trigger integration event
        var @event = new SubscriptionRequestReceivedIntegrationEvent(key, command.FirstName, command.LastName, 
            command.Email, command.Age, command.LoanAmount, command.InsuredAmount);
        await eventBus.PublishAsync(Resources.Bindings.PubSub, @event);

        return Accepted(key);
    }

    [HttpPost("OnClientRegistered")]
    [Topic(Resources.Bindings.PubSub, nameof(ClientRegisteredIntegrationEvent))]
    public Task HandleAsync([Required] ClientRegisteredIntegrationEvent @event,
        ClientIntegrationEventHandler handler)
        => handler.Handle(@event);
   
    [HttpPost("OnSubscriptionAssessmentRequested")]
    [Topic(Resources.Bindings.PubSub, nameof(SubscriptionAssessmentRequestedIntegrationEvent))]
    public Task HandleAsync([Required] SubscriptionAssessmentRequestedIntegrationEvent @event,
        [FromServices] SubscriptionIntegrationEventHandler handler)
        => handler.Handle(@event);
    
    [HttpPost("OnSubscriptionAssessmentFinished")]
    [Topic(Resources.Bindings.PubSub, nameof(SubscriptionAssessmentFinishedIntegrationEvent))]
    public Task HandleAsync([Required] SubscriptionAssessmentFinishedIntegrationEvent @event,
        [FromServices] SubscriptionIntegrationEventHandler handler)
        => handler.Handle(@event);
    

    [HttpPost("OnSubscriptionAccepted")]
    [Topic(Resources.Bindings.PubSub, nameof(SubscriptionAcceptedIntegrationEvent))]
    public Task HandleAsync([FromBody, Required] SubscriptionAcceptedIntegrationEvent @event)
    {
        logger.LogInformation("{SubscriptionId} accepted", @event.Subscription.Id);
        return Task.CompletedTask;
    }
    
    [HttpPost("OnSubscriptionPending")]
    [Topic(Resources.Bindings.PubSub, nameof(SubscriptionPendingIntegrationEvent))]
    public Task HandleAsync([FromBody, Required] SubscriptionPendingIntegrationEvent @event)
    {
        logger.LogInformation("{SubscriptionId} pending", @event.Subscription.Id);
        return Task.CompletedTask;
    }
    
    [HttpPost("OnSubscriptionRejected")]
    [Topic(Resources.Bindings.PubSub, nameof(SubscriptionRejectedIntegrationEvent))]
    public Task HandleAsync([FromBody, Required] SubscriptionRejectedIntegrationEvent @event)
    {
        logger.LogInformation("{SubscriptionId} rejected", @event.Subscription.Id);
        return Task.CompletedTask;
    }

}