namespace SubscriptionService.Model;

public record UnderwritingResult(
    [Required] UnderwritingResultState State, 
    string? Reason);