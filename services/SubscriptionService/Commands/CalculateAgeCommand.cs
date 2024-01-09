namespace SubscriptionService.Commands;

public record CalculateAgeCommand([Required] DateOnly BirthDate);