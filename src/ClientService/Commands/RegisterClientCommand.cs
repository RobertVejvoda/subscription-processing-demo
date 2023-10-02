namespace ClientService.Commands;

public record RegisterClientCommand(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age);