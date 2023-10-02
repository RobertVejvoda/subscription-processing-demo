using SubscriptionAPI.Model;
using SubscriptionService.Model;
using SubscriptionService.Repository;

namespace SubscriptionService.Events;

public record SubscriptionRequestReceivedIntegrationEvent(
    [Required] string RequestId,
    [Required] string FirstName, 
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age,
    [Required] decimal LoanAmount, 
    [Required] decimal InsuredAmount) : IntegrationEvent;

public record SubscriptionAssessmentRequestedIntegrationEvent(
        [Required] string SubscriptionId, 
        [Required] Client Client, 
        [Required] decimal LoanAmount, 
        [Required] decimal InsuredAmount) : IntegrationEvent;
public record SubscriptionAssessmentFinishedIntegrationEvent(
        [Required] string SubscriptionId, 
        [Required] UnderwritingResult UnderwritingResult,
        [Required] string Reason) : IntegrationEvent;

public record SubscriptionAcceptedIntegrationEvent(
        [Required] Subscription Subscription) : IntegrationEvent;

public record SubscriptionPendingIntegrationEvent(
    [Required] Subscription Subscription) : IntegrationEvent;

public record SubscriptionRejectedIntegrationEvent(
    [Required] Subscription Subscription) : IntegrationEvent;

public class SubscriptionIntegrationEventHandler :
        IIntegrationEventHandler<SubscriptionAssessmentRequestedIntegrationEvent>, 
        IIntegrationEventHandler<SubscriptionAssessmentFinishedIntegrationEvent>
{
    private readonly SubscriptionRepository repository;
    private readonly IEventBus eventBus;

    public SubscriptionIntegrationEventHandler(SubscriptionRepository repository, IEventBus eventBus)
    {
        this.repository = repository;
        this.eventBus = eventBus;
    }
    
    public async Task Handle(SubscriptionAssessmentRequestedIntegrationEvent @event)
    {
        // decide risk
        double risk;
        if (@event.Client.Age < 25) risk = 1;                                           // high risk
        else if (@event.Client.Age < 60 && @event.InsuredAmount < 50000) risk = 0.1;    // low risk
        else if (@event.Client.Age < 60 && @event.InsuredAmount < 200000) risk = 0.5;   // medium risk
        else if (@event.Client.Age < 60 && @event.InsuredAmount >= 200000) risk = 1;    // high risk
        else risk = 1;                                                                  // high risk

        // evaluate risk
        UnderwritingResult result;
        var reason = string.Empty;
        switch (risk)
        {
            case < 0.5:
                result = UnderwritingResult.Accepted;
                break;
            case < 1:
                result = UnderwritingResult.Pending;
                reason = "Need more information";
                break;
            default:
                result = UnderwritingResult.Rejected;
                reason = "Too risky";
                break;
        }
        
        await eventBus.PublishAsync(Resources.Bindings.PubSub,
            new SubscriptionAssessmentFinishedIntegrationEvent(@event.SubscriptionId, result, reason));
    }

    public async Task Handle(SubscriptionAssessmentFinishedIntegrationEvent @event)
    {
        var subscription = await repository.GetAsync(@event.SubscriptionId);

        if (@event.UnderwritingResult.Equals(UnderwritingResult.Accepted))
            subscription.Accept();

        if (@event.UnderwritingResult.Equals(UnderwritingResult.Rejected))
            subscription.Reject(@event.Reason);

        if (@event.UnderwritingResult.Equals(UnderwritingResult.Pending))
            subscription.Pending(@event.Reason);

        await repository.AddAsync(subscription);
        
        // for accepted subscriptions trigger event to process into PMS
        if (subscription.IsAccepted)
            await eventBus.PublishAsync(Resources.Bindings.PubSub,
                new SubscriptionAcceptedIntegrationEvent(subscription));
        
        // for pending subscriptions request more information from client
        if (subscription.IsPending)
            await eventBus.PublishAsync(Resources.Bindings.PubSub,
                new SubscriptionPendingIntegrationEvent(subscription));
        
        // for rejected subscriptions inform all parties with next steps
        if (subscription.IsRejected)
            await eventBus.PublishAsync(Resources.Bindings.PubSub,
                new SubscriptionRejectedIntegrationEvent(subscription));
    } 
}