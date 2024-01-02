using CustomerWeb.Models;
using Refit;

namespace CustomerWeb.RESTClients;

public interface IOdsApi
{
    [Headers("Content-Type: application/json", "dapr-app-id: ods-service")]
    [Get("/customers?take={take}")]
    Task<List<Customer>> GetCustomers([Query] int take);
    
    [Headers("Content-Type: application/json", "dapr-app-id: ods-service")]
    [Get("/subscriptions?take={take}")]
    Task<List<Subscription>> GetSubscriptions([Query] int take);

    [Headers("Content-Type: application/json", "dapr-app-id: ods-service")]
    [Get("/subscriptions/{processInstanceKey}")]
    Task<Subscription?> GetSubscriptionByProcessInstanceKey(string processInstanceKey);

    [Headers("Content-Type: application/json", "dapr-app-id: ods-service")]
    [Get("/customers/{customerId}/subscriptions")]
    Task<Subscription> GetCustomerSubscriptions(string customerId);
}