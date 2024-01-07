using CustomerWeb.Commands;
using CustomerWeb.Models;
using Refit;

namespace CustomerWeb.RESTClients;

public class CustomerExperienceApi : ICustomerExperienceApi
{
    private readonly ICustomerExperienceApi restClient;
    
    public CustomerExperienceApi(IConfiguration config, HttpClient httpClient)
    {
        var apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("CustomerExperienceApi");
        httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}/api");
        restClient = RestService.For<ICustomerExperienceApi>(httpClient);
    }

    public Task<string> RegisterSubscription(RegisterSubscriptionRequestCommand command)
        => restClient.RegisterSubscription(command);

    public Task<SubscriptionRequestModel> GetSubscriptionRequestByProcessInstanceKey(string processInstanceKey)
        => restClient.GetSubscriptionRequestByProcessInstanceKey(processInstanceKey);

    public Task<IEnumerable<SubscriptionRequestModel>?> GetSubscriptionRequests(int? take)
        => restClient.GetSubscriptionRequests(take);
}