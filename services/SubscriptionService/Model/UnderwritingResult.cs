namespace SubscriptionService.Model;

public record UnderwritingResult(
    [Required] UnderwritingResultState State, 
    [Required] string Reason);