using System.ComponentModel.DataAnnotations;

namespace CustomerExperienceAPI.Commands;

public record RegisterSubscriptionRequestCommand(
    [Required] string FirstName, 
    [Required] string LastName,
    [Required] string Email,
    [Required] DateOnly BirthDate,
    [Required] string ProductId,
    [Required] decimal LoanAmount,
    [Required] decimal InsuredAmount);