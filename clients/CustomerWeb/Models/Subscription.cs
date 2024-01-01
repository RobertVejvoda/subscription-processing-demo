using System.ComponentModel.DataAnnotations;

namespace CustomerWeb.Models;

public class Subscription
{
    public string? ProcessInstanceKey { get; set; }
    public string? SubscriptionId { get; set; }
    public string? CustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
    public string? SubscriptionState { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal InsuredAmount { get; set; }
    public string? UnderwritingResult { get; set; }
    public string? Message { get; set; }
    public DateTime? ReceivedOn { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
}