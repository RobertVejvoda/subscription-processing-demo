using Dapr.Client;
using SubscriptionService.Model;

namespace SubscriptionService.Repository;

public class SubscriptionRepository
{
    private readonly DaprClient daprClient;

    public SubscriptionRepository(DaprClient daprClient)
    {
        this.daprClient = daprClient;
    }
    
    public Task AddAsync(Model.Subscription subscription)
        => daprClient.SaveStateAsync(Resources.Bindings.StateStore,
            subscription.Id,
            subscription);

    public Task<Subscription> GetAsync(string subscriptionId)
        => daprClient.GetStateAsync<Subscription>(Resources.Bindings.StateStore,
            subscriptionId);
}