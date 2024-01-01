using CustomerWeb.Models;
using Refit;

namespace CustomerWeb.RESTClients;

public interface ISubscriptionApi
{
    [Post("/subscriptions")]
    Task<string?> Register(RegisterSubscriptionRequest command);
}