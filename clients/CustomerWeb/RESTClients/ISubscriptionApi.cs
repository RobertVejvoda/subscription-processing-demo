using CustomerWeb.Models;
using Refit;

namespace CustomerWeb.RESTClients;

public interface ISubscriptionApi
{
    [Headers("Content-Type: application/json", "dapr-app-id: subscription-service")]
    [Post("/subscriptions")]
    Task<string?> Register([Body] RegisterSubscriptionRequest command);
}