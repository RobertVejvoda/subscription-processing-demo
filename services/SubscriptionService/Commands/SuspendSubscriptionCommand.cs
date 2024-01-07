namespace SubscriptionService.Commands;

public record SuspendSubscriptionCommand(
    [Required] string SubscriptionId, 
    [Required] string? Message,
    [Required] string ProcessInstanceKey);