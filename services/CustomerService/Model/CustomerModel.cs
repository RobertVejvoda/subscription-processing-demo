namespace CustomerService.Model;

public record CustomerModel(
    [Required] string Id,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] DateOnly BirthDate,
    [Required] string State);