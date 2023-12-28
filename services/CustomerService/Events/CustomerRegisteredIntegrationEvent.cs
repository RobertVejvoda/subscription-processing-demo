namespace CustomerService.Events;

public record CustomerRegisteredIntegrationEvent(
    [Required] string CustomerId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] string Status,
    [Required] DateOnly BirthDate) : IntegrationEvent;

