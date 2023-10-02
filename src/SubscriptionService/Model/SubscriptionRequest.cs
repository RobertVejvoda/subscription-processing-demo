namespace SubscriptionService.Model;

public class SubscriptionRequest
{
    public SubscriptionRequest([Required] string RequestId,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string Email,
        [Required] int Age,
        [Required] string ProductId,
        [Required] decimal LoanAmount,
        [Required] decimal InsuredAmount,
        string? SubscriptionId,
        string? SubscriptionStatus)
    {
        this.RequestId = RequestId;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Email = Email;
        this.Age = Age;
        this.ProductId = ProductId;
        this.LoanAmount = LoanAmount;
        this.InsuredAmount = InsuredAmount;
        this.SubscriptionId = SubscriptionId;
        this.SubscriptionStatus = SubscriptionStatus;
    }

    // naive
    public static string Key(Guid requestId) => $"SR-{requestId.ToString()[25..]}".ToUpper();

    public void UpdateSubscription(string subscriptionId, string subscriptionStatus)
    {
        SubscriptionId = subscriptionId;
        SubscriptionStatus = subscriptionStatus;
    }

    public string RequestId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public int Age { get; init; }
    public string ProductId { get; init; }
    public decimal LoanAmount { get; init; }
    public decimal InsuredAmount { get; init; }
    public string? SubscriptionId { get; private set; }
    public string? SubscriptionStatus { get; private set; }
}