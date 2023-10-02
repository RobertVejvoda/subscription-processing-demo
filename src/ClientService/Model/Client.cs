namespace ClientService.Model;

public record Client(
    [Required] string Id,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age,
    [Required] string Status);  