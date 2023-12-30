using System.ComponentModel.DataAnnotations;

namespace ODSService.Commands;

public record SubscriptionAcceptedCommand(
    [Required] string SubscriptionId,
    [Required] string UnderwritingResult,
    [Required] string Message,
    [Required] DateTime AcceptedOn);