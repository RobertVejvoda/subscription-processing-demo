namespace SubscriptionService.Commands;

public record AcceptSubscriptionCommand(
    [Required] string SubscriptionId,
    [Required] string? Reason);