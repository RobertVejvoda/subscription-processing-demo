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

    public Task<List<Customer>> GetCustomers(int take) 
        => restClient.GetCustomers(take);

    public Task<List<Subscription>> GetSubscriptions(int take) 
        => restClient.GetSubscriptions(take);

    public Task<Subscription?> GetSubscriptionByProcessInstanceKey(string processInstanceKey) 
        => restClient.GetSubscriptionByProcessInstanceKey(processInstanceKey);

    public Task<Subscription> GetCustomerSubscriptions(string customerId) 
        => restClient.GetCustomerSubscriptions(customerId);
}