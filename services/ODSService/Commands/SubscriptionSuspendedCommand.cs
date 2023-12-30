using System.ComponentModel.DataAnnotations;

namespace ODSService.Commands;

public record SubscriptionSuspendedCommand(
    [Required] string SubscriptionId,
    [Required] string UnderwritingResult,
    [Required] string Message,
    [Required] DateTime SuspendedOn);