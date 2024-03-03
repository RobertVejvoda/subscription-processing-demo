namespace SubscriptionService.Repository;

public class SubscriptionRepository(DaprClient daprClient)
{
    private const string StateStore = "statestore";

    public async Task AddAsync(Subscription subscription)
    {
        var dto = subscription.ToDto();
        var key = $"S-{subscription.SubscriptionId}";
        await daprClient.SaveStateAsync(StateStore, key, dto);
    }


    public async Task<Subscription?> GetAsync(string subscriptionId)
    {
        var key = $"S-{subscriptionId}";
        var model = await daprClient.GetStateAsync<Dto.Subscription>(StateStore, key);
        if (model == null)
            return null;

        var subscription = Subscription.FromDto(model);
        return subscription;
    }
}