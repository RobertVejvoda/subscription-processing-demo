namespace CustomerExperienceAPI.Commands;

public record SubscriptionRejectedCommand(
    [Required] string ProcessInstanceKey,
    [Required] string SubscriptionState,
    [Required] string Reason);