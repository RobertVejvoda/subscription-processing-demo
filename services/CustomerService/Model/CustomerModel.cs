namespace CustomerService.Model;

public record CustomerModel(
    [Required] string CustomerId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] DateOnly BirthDate,
    [Required] string Email,
    [Required] string CustomerState,
    int? Age);