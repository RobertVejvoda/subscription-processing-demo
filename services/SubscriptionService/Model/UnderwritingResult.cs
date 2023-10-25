namespace SubscriptionService.Model;

public record UnderwritingResult(
    [Required] string UnderwritingResultState, 
    [Required] string Reason);