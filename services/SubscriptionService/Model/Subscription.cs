namespace SubscriptionService.Model;

public class Subscription
{
    public Subscription([Required] string Id,
        [Required] string ProductId,
        [Required] decimal LoanAmount,
        [Required] decimal InsuredAmount,
        [Required] string SubscriptionState,
        Customer? Customer,
        Product? Product,
        UnderwritingResult? UnderwritingResult)
    {
        this.Id = Id;
        this.ProductId = ProductId;
        this.LoanAmount = LoanAmount;
        this.InsuredAmount = InsuredAmount;
        this.SubscriptionState = SubscriptionState;
        this.Customer = Customer;
        this.Product = Product;
        this.UnderwritingResult = UnderwritingResult;
    }

    public string Id { get; init; }
    public string ProductId { get; init; }
    public decimal LoanAmount { get; init; }
    public decimal InsuredAmount { get; init; }
    public string SubscriptionState { get; set; }
    public Customer? Customer { get; set; }
    public Product? Product { get; set; }
    public UnderwritingResult? UnderwritingResult { get; set; }
    
    public static string Key(Guid id) => $"S-{id.ToString().ToUpper()[25..]}";
}
