namespace CustomerService.Commands;

public record DetermineExistingCustomerCommand(
    [Required] DateOnly BirthDate, 
    [Required] string Email);
