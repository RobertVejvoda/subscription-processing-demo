using CustomerWeb.Commands;
using CustomerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace CustomerWeb.RESTClients;

public interface ICustomerExperienceApi
{
    [Headers("Content-Type: application/json", "dapr-app-id: customer-experience-api")]
    [Post("/subscriptions/register")]
    Task<string> RegisterSubscription(RegisterSubscriptionRequestCommand command);

    [Headers("Content-Type: application/json", "dapr-app-id: customer-experience-api")]
    [Get("/subscriptions/{processInstanceKey}")]
    Task<SubscriptionRequestModel> GetSubscriptionRequestByProcessInstanceKey(string processInstanceKey);

    [Headers("Content-Type: application/json", "dapr-app-id: customer-experience-api")]
    [Get("/subscriptions")]
    Task<IEnumerable<SubscriptionRequestModel>?> GetSubscriptionRequests([FromQuery] int? take = 15);
}