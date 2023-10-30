namespace SubscriptionService.Repository;

public class SubscriptionRepository
{
    private readonly DaprClient daprClient;
    private readonly DaprOptions daprOptions;

    public SubscriptionRepository(DaprClient daprClient, IOptions<DaprOptions> daprOptions)
    {
        this.daprClient = daprClient;
        this.daprOptions = daprOptions.Value;
    }
    
    public Task AddAsync(Subscription subscription)
        => daprClient.SaveStateAsync(daprOptions.StateStore,
            subscription.Id,
            subscription);

    public Task<Subscription> GetAsync(string subscriptionId)
        => daprClient.GetStateAsync<Subscription>(daprOptions.StateStore,
            subscriptionId);
}