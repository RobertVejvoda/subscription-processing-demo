namespace SubscriptionService.Proxy;

public record Product(
    [Required] string Id,
    [Required] string Name,
    [Required] DateRange ValidityPeriod,
    [Required] string State)
{
    public bool IsActiveOn(DateTime date) => ValidityPeriod.IsInRange(date);
}

    