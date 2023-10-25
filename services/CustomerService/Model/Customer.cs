namespace CustomerService.Model;

public record Customer(
    [Required] string Id,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age,
    [Required] string Status)
{
    public static string Key(string emailAddress) => $"C-{emailAddress.Trim().ToLowerInvariant()}";
}