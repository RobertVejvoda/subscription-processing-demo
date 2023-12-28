namespace SubscriptionService.Commands;

public record RejectSubscriptionCommand(
    [Required] string SubscriptionId, 
    [Required] string Reason,
    [Required] string ProcessInstanceKey);