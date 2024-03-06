namespace SubscriptionService.Domain;

public record SubscriptionValidationResult(bool IsValid, string? Reason = default)
{
    public static SubscriptionValidationResult Valid => new(true);
}