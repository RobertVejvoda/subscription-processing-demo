namespace CustomerService.Commands;

public record RegisterCustomerCommand(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] DateOnly BirthDate);