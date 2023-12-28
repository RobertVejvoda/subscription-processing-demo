namespace SubscriptionService.Commands;

public record NormalizeSubscriptionCommand(
    [Required] string SubscriptionId,
    [Required] string ProcessInstanceKey);