using CustomerWeb.Commands;
using CustomerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace CustomerWeb.RESTClients;

public interface ICustomerExperienceApi
{
    [Headers("Content-Type: application/json")]
    [Post("/subscriptions/register")]
    Task<string> RegisterSubscription(RegisterSubscriptionRequestCommand command);

    [Headers("Content-Type: application/json")]
    [Get("/subscriptions/{processInstanceKey}")]
    Task<SubscriptionRequestModel> GetSubscriptionRequestByProcessInstanceKey(string processInstanceKey);

    [Headers("Content-Type: application/json")]
    [Get("/subscriptions")]
    Task<IEnumerable<SubscriptionRequestModel>?> GetSubscriptionRequests([FromQuery] int? take = 15);
}