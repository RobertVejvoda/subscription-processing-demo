namespace SubscriptionService.Repository;

public class SubscriptionRepository(DaprClient daprClient)
{
    private const string StateStore = "statestore";

    public async Task AddAsync(Subscription subscription, string processInstanceKey)
    {
        var model = subscription.ToModel();
        model.ProcessInstanceKey = processInstanceKey;
        var key = $"S-{subscription.SubscriptionId}";
        await daprClient.SaveStateAsync(StateStore, key, model);
    }


    public async Task<Subscription?> GetAsync(string subscriptionId)
    {
        var key = $"S-{subscriptionId}";
        var model = await daprClient.GetStateAsync<SubscriptionModel>(StateStore, key);
        if (model == null)
            return null;

        var subscription = Subscription.FromModel(model);
        return subscription;
    }
}