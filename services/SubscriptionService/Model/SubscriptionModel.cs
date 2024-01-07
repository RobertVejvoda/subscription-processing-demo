namespace SubscriptionService.Model;

public record SubscriptionModel(
    string SubscriptionId,
    string? CustomerId,
    string ProductId,
    decimal LoanAmount,
    decimal InsuredAmount,
    string SubscriptionState,
    DateTime UpdatedOn,
    string? UnderwritingResultState,
    string? UnderwritingResultMessage,
    SubscriptionStateHistory[] History);