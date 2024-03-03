namespace SubscriptionService.Commands;

public record RegisterUnderwritingRequestCommand(
    [Required] string RequestId, 
    [Required] string CustomerId, 
    [Required] int Age,
    [Required] decimal InsuredAmount);