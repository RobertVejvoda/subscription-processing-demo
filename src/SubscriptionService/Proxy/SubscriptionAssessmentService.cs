using SubscriptionService.Events;
using SubscriptionService.Model;

namespace SubscriptionService.Proxy;

public class SubscriptionAssessmentService
{
    private readonly IEventBus eventBus;

    public SubscriptionAssessmentService(IEventBus eventBus)
    {
        this.eventBus = eventBus;
    }
    
    public async Task Assess(Subscription subscription)
    {
        var integrationEvent = new SubscriptionAssessmentRequestedIntegrationEvent(
            subscription.Id, 
            subscription.Client!, 
            subscription.LoanAmount, 
            subscription.InsuredAmount);
        
        await eventBus.PublishAsync(Resources.Bindings.PubSub, integrationEvent);
    }
}