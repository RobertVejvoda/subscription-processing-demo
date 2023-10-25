namespace SubscriptionService.Model;

public record Customer(
    [Required] string Id,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] int Age,
    [Required] string Email)
{
    public static string Key(Guid id) => $"C-{id.ToString().ToUpper()[25..]}";
}