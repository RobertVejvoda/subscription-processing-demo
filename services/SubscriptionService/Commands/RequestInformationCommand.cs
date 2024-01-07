namespace SubscriptionService.Commands;

public record RequestInformationCommand(
    [Required] string SubscriptionId, 
    [Required] string UnderwritingResultState,
    [Required] string UnderwritingResultMessage);