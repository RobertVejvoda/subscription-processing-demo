using Dapr.Client;
using SubscriptionService.Model;

namespace SubscriptionService.Repository;

public class SubscriptionRequestRepository
{
    private readonly DaprClient daprClient;

    public SubscriptionRequestRepository(DaprClient daprClient)
    {
        this.daprClient = daprClient;
    }
    
    public Task AddAsync(Model.SubscriptionRequest subscriptionRequest)
        => daprClient.SaveStateAsync(Resources.Bindings.StateStore,
            subscriptionRequest.RequestId,
            subscriptionRequest);

    public Task<SubscriptionRequest> GetAsync(string subscriptionRequestId)
        => daprClient.GetStateAsync<SubscriptionRequest>(Resources.Bindings.StateStore,
            subscriptionRequestId);
}