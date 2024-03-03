namespace CustomerService.Dto;

public record Customer(
    [Required] string CustomerId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] DateOnly BirthDate,
    [Required] string Email,
    [Required] string CustomerState,
    int? Age);