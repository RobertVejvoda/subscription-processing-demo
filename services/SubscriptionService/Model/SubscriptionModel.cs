namespace SubscriptionService.Model;

public record SubscriptionModel(
    string SubscriptionId,
    string? CustomerId,
    string ProductId,
    decimal LoanAmount,
    decimal InsuredAmount,
    string SubscriptionState,
    DateTime UpdatedOn,
    UnderwritingResult? UnderwritingResult,
    SubscriptionStateHistory[] History)
{
    public string? ProcessInstanceKey { get; set; }
};