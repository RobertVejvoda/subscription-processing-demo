using SubscriptionService.Domain;

namespace SubscriptionService.Dto;

public record Subscription(
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