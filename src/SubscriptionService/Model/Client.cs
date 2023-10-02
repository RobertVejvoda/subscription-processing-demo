namespace SubscriptionAPI.Model;

public record Client(
    [Required] string ClientId, 
    [Required] string FirstName, 
    [Required] string LastName, 
    [Required] int Age, 
    [Required] string Email);