using SubscriptionAPI.Model;
using SubscriptionService.Model;
using SubscriptionService.Repository;

namespace SubscriptionService.Events;

public record ClientRegisteredIntegrationEvent(
    [Required] string RequestId,
    [Required] string ClientId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age) : IntegrationEvent;

public class ClientIntegrationEventHandler : IIntegrationEventHandler<ClientRegisteredIntegrationEvent>

{
    private readonly SubscriptionRequestRepository subscriptionRequestRepository;
    private readonly SubscriptionRepository subscriptionRepository;
    private readonly NormalizationService normalizationService;
    private readonly SubscriptionAssessmentService subscriptionAssessmentService;
    private readonly ILogger<ClientIntegrationEventHandler> logger;


    public ClientIntegrationEventHandler(
        SubscriptionRequestRepository subscriptionRequestRepository,
        SubscriptionRepository subscriptionRepository,
        NormalizationService normalizationService,
        SubscriptionAssessmentService subscriptionAssessmentService,
        ILogger<ClientIntegrationEventHandler> logger
    )
    {
        this.subscriptionRequestRepository = subscriptionRequestRepository;
        this.subscriptionRepository = subscriptionRepository;
        this.normalizationService = normalizationService;
        this.subscriptionAssessmentService = subscriptionAssessmentService;
        this.logger = logger;
    }

    public async Task Handle(ClientRegisteredIntegrationEvent @event)
    {
        // Demo: Subscription processing simulation

        // retrieve original request
        var subscriptionRequest = await subscriptionRequestRepository.GetAsync(@event.RequestId);
        if (subscriptionRequest == null)
            throw new InvalidOperationException("Missing original subscription request.");

        // create subscription
        var key = Subscription.Key(Guid.NewGuid());
        var subscription = Subscription.Create(
                subscriptionRequest.RequestId,
                subscriptionRequest.ProductId,
                subscriptionRequest.LoanAmount,
                subscriptionRequest.InsuredAmount)
            .Register()
            .EnrichWith(new Client(@event.ClientId, @event.FirstName, @event.LastName, @event.Age, @event.Email));
        await subscriptionRepository.AddAsync(subscription);
        logger.LogInformation("{SubscriptionId} registered", subscription.Id);

        // update original request?
        subscriptionRequest.UpdateSubscription(subscription.Id, subscription.State.Name);
        await subscriptionRequestRepository.AddAsync(subscriptionRequest);

        // normalize/enrich subscription
        subscription.Normalize(normalizationService);
        await subscriptionRepository.AddAsync(subscription);
        logger.LogInformation("{SubscriptionId} normalized", subscription.Id);

        // update original request?
        subscriptionRequest.UpdateSubscription(subscription.Id, subscription.State.Name);
        await subscriptionRequestRepository.AddAsync(subscriptionRequest);

        // assess subscription
        await subscription.Analyze(subscriptionAssessmentService);
        await subscriptionRepository.AddAsync(subscription);
        logger.LogInformation("{SubscriptionId} analyzed", subscription.Id);

        // update original request?
        subscriptionRequest.UpdateSubscription(subscription.Id, subscription.State.Name);
        await subscriptionRequestRepository.AddAsync(subscriptionRequest);
        
    }
}