namespace SubscriptionService.Commands;

public record RequestInformationCommand(
    [Required] string SubscriptionId, 
    [Required] UnderwritingResult UnderwritingResult,
    [Required] string ProcessInstanceKey);