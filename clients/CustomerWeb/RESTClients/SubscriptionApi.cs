using CustomerWeb.Models;
using Refit;

namespace CustomerWeb.RESTClients;

public class SubscriptionApi : ISubscriptionApi
{
    private readonly ISubscriptionApi restClient;

    public SubscriptionApi(IConfiguration config, HttpClient httpClient)
    {
        var apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("SubscriptionApi");
        httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}/api");
        restClient = RestService.For<ISubscriptionApi>(httpClient);
    }
    
    public async Task<string?> Register(RegisterSubscriptionRequest command)
    {
        return await restClient.Register(command);
    }
}