namespace SubscriptionService.Commands;

public record InformationReceivedCommand(
    [Required] string RequestId);