namespace SubscriptionService.Commands;

public record RegisterSubscriptionCommand(
    [Required] string ProductId,
    [Required] decimal LoanAmount,
    [Required] decimal InsuredAmount,
    [Required] string ProcessInstanceKey);