namespace CustomerBusinessAPI.Commands;

public record SubscriptionRejectedCommand(
    [Required] string ProcessInstanceKey,
    [Required] string SubscriptionState,
    [Required] string Reason);