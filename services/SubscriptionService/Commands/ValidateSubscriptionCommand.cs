namespace SubscriptionService.Commands;

public record ValidateSubscriptionCommand(
    [Required] string SubscriptionId,
    [Required] string CustomerId);