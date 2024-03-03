using SubscriptionService.Dto;

namespace SubscriptionService.Repository;

public class UnderwritingRepository(DaprClient daprClient)
{
    private const string StateStore = "statestore";

    public async Task AddAsync(UnderwritingRequest request)
    {
        var key = $"UW-{request.RequestId}";
        await daprClient.SaveStateAsync(StateStore, key, request);
    }


    public async Task<UnderwritingRequest?> GetAsync(string requestId)
    {
        var key = $"UW-{requestId}";
        var model = await daprClient.GetStateAsync<UnderwritingRequest>(StateStore, key);
        return model ?? null;
    }
}