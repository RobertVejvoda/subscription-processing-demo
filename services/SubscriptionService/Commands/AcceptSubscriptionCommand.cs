namespace SubscriptionService.Commands;

public record AcceptSubscriptionCommand(
    [Required] string SubscriptionId,
    [Required] string? Message,
    [Required] string ProcessInstanceKey);