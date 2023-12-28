namespace CustomerService.Commands;

public record NotifyCustomerCommand(
    [Required] string CustomerId, 
    [Required] string Message);