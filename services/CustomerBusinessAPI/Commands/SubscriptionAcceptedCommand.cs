namespace CustomerBusinessAPI.Commands;

public record SubscriptionAcceptedCommand(
    [Required] string ProcessInstanceKey,
    [Required] string SubscriptionState,
    [Required] string Reason);