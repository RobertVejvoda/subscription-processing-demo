using CustomerWeb.Models;
using Refit;

namespace CustomerWeb.RESTClients;

public class OdsApi : IOdsApi
{
    private readonly IOdsApi restClient;
    
    public OdsApi(IConfiguration config, HttpClient httpClient)
    {
        var apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("OdsApi");
        httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}/api");
        restClient = RestService.For<IOdsApi>(httpClient);
    }

    public async Task<List<Customer>> GetCustomers()
    {
        return await restClient.GetCustomers();
    }

    public async Task<List<Subscription>> GetSubscriptions()
    {
        return await restClient.GetSubscriptions();
    }

    public async Task<Subscription?> GetSubscriptionByProcessInstanceKey(string processInstanceKey)
    {
        return await restClient.GetSubscriptionByProcessInstanceKey(processInstanceKey);
    }

    public async Task<Subscription> GetCustomerSubscriptions(string customerId)
    {
        return await restClient.GetCustomerSubscriptions(customerId);
    }
}