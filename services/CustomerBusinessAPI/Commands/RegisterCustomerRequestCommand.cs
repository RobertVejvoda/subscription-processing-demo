namespace CustomerBusinessAPI.Commands;

public record RegisterCustomerRequestCommand(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] DateOnly BirthDate);