namespace ODSService.Model;

public class SubscriptionModel
{
    public string SubscriptionId { get; set; }
    public string subscriptionState { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal InsuredAmount { get; set; }
    public string? UnderwritingResult { get; set; }
    public string? Message { get; set; }
    public DateTime ReceivedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
}