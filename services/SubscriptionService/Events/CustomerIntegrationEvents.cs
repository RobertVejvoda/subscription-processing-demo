namespace SubscriptionService.Events;

public record CustomerRegisteredIntegrationEvent(
    [Required] string SubscriptionId,
    [Required] string CustomerId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age) : IntegrationEvent;

public class CustomerIntegrationEventHandler : IIntegrationEventHandler<CustomerRegisteredIntegrationEvent>
{
    private readonly SubscriptionRepository subscriptionRepository;
    private readonly ProductProxyService productProxyService;
    private readonly IEventBus eventBus;
    private readonly ILogger<CustomerIntegrationEventHandler> logger;

    public CustomerIntegrationEventHandler(
        SubscriptionRepository subscriptionRepository,
        ProductProxyService productProxyService,
        IEventBus eventBus,
        ILogger<CustomerIntegrationEventHandler> logger
    )
    {
        this.subscriptionRepository = subscriptionRepository;
        this.productProxyService = productProxyService;
        this.eventBus = eventBus;
        this.logger = logger;
    }

    public async Task Handle(CustomerRegisteredIntegrationEvent @event)
    {
        // retrieve original request
        var subscription = await subscriptionRepository.GetAsync(@event.SubscriptionId);
        if (subscription == null)
            throw new InvalidOperationException("Missing original subscription.");

        // normalize subscription 
        subscription.Customer = new Customer(@event.CustomerId, @event.FirstName, @event.LastName, @event.Age, @event.Email);
        subscription.Product = productProxyService.GetProduct(subscription.ProductId);

        await subscriptionRepository.AddAsync(subscription);
        
        // request assessment
        await eventBus.PublishAsync(
            Resources.Bindings.PubSub, 
            Resources.Topics.Subscription.AssessmentRequested, 
            new SubscriptionAssessmentRequestedIntegrationEvent(
                subscription.Id, 
                subscription.Customer!, 
                subscription.LoanAmount, 
                subscription.InsuredAmount));
        
        logger.LogInformation("{SubscriptionId} registered & assessment requested", subscription.Id);
    }
}