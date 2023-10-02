using Core.Exceptions;
using Core.Types;
using SubscriptionService.Model;

namespace SubscriptionService.Proxy;

public class NormalizationService
{
    private readonly IDateTimeProvider dateTimeProvider;

    public NormalizationService(IDateTimeProvider dateTimeProvider)
    {
        this.dateTimeProvider = dateTimeProvider;
    }
    
    public void Normalize(Subscription subscription)
    {
        // fake impl.
        var product = new Product(subscription.ProductId, "Mortgage demo",
            new DateRange(DateTime.Today, DateTime.Today.AddDays(30)), "Active");

        if (!product.IsActive(dateTimeProvider.Now()))
            throw new DomainException("This product is terminated.");

        subscription.EnrichWith(product);
    }
}