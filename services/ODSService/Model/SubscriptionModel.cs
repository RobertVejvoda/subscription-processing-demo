namespace ODSService.Model;

public class SubscriptionModel
{
    public required string SubscriptionId { get; init; }
    public required string SubscriptionState { get; init; }
    public decimal LoanAmount { get; init; }
    public decimal InsuredAmount { get; init; }
    public string? UnderwritingResult { get; init; }
    public string? Message { get; init; }
    public DateTime ReceivedOn { get; init; }
    public DateTime LastUpdatedOn { get; init; }
}