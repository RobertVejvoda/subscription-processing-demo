using Core.Exceptions;

namespace SubscriptionService.Proxy;

public class ProductProxyService(IDateTimeProvider dateTimeProvider)
{
    public void EnsureActiveProduct(string productId)
    {
        // fake impl.
        var product = new Product(productId, "Demo product",
            new DateRange(dateTimeProvider.Now().AddDays(-7), dateTimeProvider.Now().AddDays(7)), "Active");

        if (!product.IsActiveOn(dateTimeProvider.Now()))
            throw new DomainException("Product is not active.", productId);
    }
}