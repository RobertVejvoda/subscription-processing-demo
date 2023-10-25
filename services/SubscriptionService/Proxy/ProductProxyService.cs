using Core.Types;

namespace SubscriptionService.Proxy;

public class ProductProxyService
{
    private readonly IDateTimeProvider dateTimeProvider;

    public ProductProxyService(IDateTimeProvider dateTimeProvider)
    {
        this.dateTimeProvider = dateTimeProvider;
    }
    
    public Product GetProduct(string productId)
    {
        // fake impl.
        var product = new Product(productId, "Demo product",
            new DateRange(DateTime.Today, DateTime.Today.AddDays(30)), "Active");

        if (!product.IsActive(dateTimeProvider.Now()))
            throw new DomainException("This product is terminated.", product.ProductId);

        return product;
    }
}