namespace SubscriptionService.Commands;

public record RegisterCustomerCommand(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age);