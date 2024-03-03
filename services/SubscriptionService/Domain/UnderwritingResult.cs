namespace SubscriptionService.Domain;

public record UnderwritingResult(
    [Required] UnderwritingResultState State, 
    string? Reason);