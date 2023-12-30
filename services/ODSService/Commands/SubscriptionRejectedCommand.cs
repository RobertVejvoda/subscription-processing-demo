using System.ComponentModel.DataAnnotations;

namespace ODSService.Commands;

public record SubscriptionRejectedCommand(
    [Required] string SubscriptionId,
    [Required] string UnderwritingResult,
    [Required] string Message,
    [Required] DateTime RejectedOn);