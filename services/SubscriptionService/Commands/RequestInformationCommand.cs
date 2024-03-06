namespace SubscriptionService.Commands;

public record RequestInformationCommand(
    [Required] string RequestId, 
    [Required] string UnderwritingResultState,
    [Required] string UnderwritingResultMessage);