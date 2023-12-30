namespace SubscriptionService.Commands;

public record RejectSubscriptionCommand(
    [Required] string SubscriptionId, 
    [Required] string Message,
    [Required] string ProcessInstanceKey);