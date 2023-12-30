using System.ComponentModel.DataAnnotations;

namespace ODSService.Commands;

public record RegisterSubscriptionRequestCommand(
    [Required] string CustomerId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] DateOnly BirthDate,
    [Required] string CustomerState,
    [Required] string SubscriptionId,
    [Required] decimal LoanAmount,
    [Required] decimal InsuredAmount,
    [Required] string SubscriptionState,
    [Required] DateTime RegisteredOn,
    [Required] string ProductId,
    [Required] string ProcessInstanceKey);