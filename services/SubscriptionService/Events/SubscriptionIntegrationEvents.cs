using Core.Types;

namespace SubscriptionService.Events;

public class SubscriptionIntegrationEventHandler :
    IIntegrationEventHandler<SubscriptionAssessmentFinishedIntegrationEvent>
{
    private readonly SubscriptionRepository repository;
    private readonly IEventBus eventBus;
    private readonly ILogger<SubscriptionIntegrationEventHandler> logger;
    private readonly DaprOptions daprOptions;

    public SubscriptionIntegrationEventHandler(
        SubscriptionRepository repository, 
        IEventBus eventBus, 
        IOptions<DaprOptions> daprOptions,
        ILogger<SubscriptionIntegrationEventHandler> logger)
    {
        this.repository = repository;
        this.eventBus = eventBus;
        this.logger = logger;
        this.daprOptions = daprOptions.Value;
    }
    
    public async Task Handle(SubscriptionAssessmentRequestedIntegrationEvent @event)
    {
        // decide risk
        double risk;
        if (@event.Customer.Age < 25) risk = 1;                                           // high risk
        else if (@event.Customer.Age < 60 && @event.InsuredAmount < 50000) risk = 0.1;    // low risk
        else if (@event.Customer.Age < 60 && @event.InsuredAmount < 200000) risk = 0.5;   // medium risk
        else if (@event.Customer.Age < 60 && @event.InsuredAmount >= 200000) risk = 1;    // high risk
        else risk = 1;                                                                  // high risk

        // evaluate risk
        UnderwritingResultState result;
        string reason;
        switch (risk)
        {
            case < 0.5:
                result = UnderwritingResultState.Accepted;
                reason = "Accepted";
                break;
            case < 1:
                result = UnderwritingResultState.Pending;
                reason = "Need more information";
                break;
            default:
                result = UnderwritingResultState.Rejected;
                reason = "Too risky";
                break;
        }
        
        logger.LogInformation("{CustomerId} - {SubscriptionId} - assessed: {Risk} -> {Reason}", 
            @event.Customer.Id, @event.SubscriptionId, risk, reason);

        await eventBus.PublishAsync(daprOptions.PubSub, "subscription-assessment-finished",
            new SubscriptionAssessmentFinishedIntegrationEvent(@event.SubscriptionId, new UnderwritingResult(result.Name, reason)));
    }
    
    public async Task Handle(SubscriptionAssessmentFinishedIntegrationEvent @event)
    {
        var subscription = await repository.GetAsync(@event.SubscriptionId);
        if (subscription == null)
            throw new DomainException($"Subscription {@event.SubscriptionId} not found.",
                @event.SubscriptionId);


        subscription.UnderwritingResult = @event.UnderwritingResult;

        var underwritingResultState =
            Enumeration.FromDisplayName<UnderwritingResultState>(@event.UnderwritingResult.UnderwritingResultState);
        
        if (underwritingResultState.Equals(UnderwritingResultState.Accepted))
        {
            subscription.SubscriptionState = SubscriptionState.Accepted.Name;
        }

        if (underwritingResultState.Equals(UnderwritingResultState.Pending))
        {
            subscription.SubscriptionState = SubscriptionState.Pending.Name;
        }

        if (underwritingResultState.Equals(UnderwritingResultState.Rejected))
        {
            subscription.SubscriptionState = SubscriptionState.Rejected.Name;
        }
        
        await repository.AddAsync(subscription);
    }
}

public record SubscriptionAssessmentRequestedIntegrationEvent(
    [Required] string SubscriptionId, 
    [Required] Customer Customer, 
    [Required] decimal LoanAmount, 
    [Required] decimal InsuredAmount) : IntegrationEvent;


public record SubscriptionRequestReceivedIntegrationEvent(
    [Required] string SubscriptionId,
    [Required] string FirstName, 
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age,
    [Required] decimal LoanAmount, 
    [Required] decimal InsuredAmount) : IntegrationEvent;

public record SubscriptionAssessmentFinishedIntegrationEvent(
    [Required] string SubscriptionId, 
    [Required] UnderwritingResult UnderwritingResult) : IntegrationEvent;