using System.ComponentModel.DataAnnotations;

namespace CustomerExperienceAPI.Commands;

public record SubscriptionSuspendedCommand(
    [Required] string ProcessInstanceKey,
    [Required] string SubscriptionState,
    [Required] string SubscriptionId,
    [Required] string Reason);