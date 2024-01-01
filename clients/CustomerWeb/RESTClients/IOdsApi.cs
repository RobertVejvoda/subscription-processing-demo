using CustomerWeb.Models;
using Refit;

namespace CustomerWeb.RESTClients;

public interface IOdsApi
{
    [Get("/customers")]
    Task<List<Customer>> GetCustomers();
    
    [Get("/subscriptions")]
    Task<List<Subscription>> GetSubscriptions();

    [Get("/subscriptions/{processInstanceKey}")]
    Task<Subscription?> GetSubscriptionByProcessInstanceKey(string processInstanceKey);

    [Get("/customers/{customerId}/subscriptions")]
    Task<Subscription> GetCustomerSubscriptions(string customerId);
}