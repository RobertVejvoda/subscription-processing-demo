namespace CustomerExperienceAPI.Commands;

public record SubscriptionSuspendedCommand(
    [Required] string ProcessInstanceKey,
    [Required] string SubscriptionState,
    [Required] string Reason);