namespace SubscriptionService.Proxy;

public class ProductProxyService(IDateTimeProvider dateTimeProvider)
{
    public Product GetProduct(string productId)
    {
        // fake impl.
        return new Product(productId, "Demo product",
            new DateRange(dateTimeProvider.Now().AddDays(-7), dateTimeProvider.Now().AddDays(7)), "Active");
    }
}