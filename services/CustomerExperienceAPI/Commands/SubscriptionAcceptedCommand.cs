namespace CustomerExperienceAPI.Commands;

public record SubscriptionAcceptedCommand(
    [Required] string ProcessInstanceKey,
    [Required] string SubscriptionState,
    [Required] string Reason);