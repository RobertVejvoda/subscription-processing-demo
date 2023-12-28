namespace SubscriptionService.Model;

public record SubscriptionModel(
    string SubscriptionId,
    string? CustomerId,
    string ProductId,
    Product? Product,
    decimal LoanAmount,
    decimal InsuredAmount,
    string State,
    UnderwritingResult? UnderwritingResult,
    SubscriptionStateHistory[] History)
{
    public string? ProcessInstanceKey { get; set; }
};