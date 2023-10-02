
namespace SubscriptionAPI.Commands;

public record RegisterSubscriptionCommand(
    [Required] string FirstName, 
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age,
    [Required] string ProductId,
    [Required] decimal LoanAmount, 
    [Required] decimal InsuredAmount);