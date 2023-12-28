using Core.Domain;

namespace SubscriptionService.Model;

public class Subscription : IAggregateRoot
{
    public Subscription(string productId, decimal loanAmount, decimal insuredAmount)
    {
        dateTimeProvider = new UtcDateTimeProvider();
        SubscriptionId = Guid.NewGuid().ToString("n");
        ProductId = productId;
        LoanAmount = loanAmount;
        InsuredAmount = insuredAmount;
        StateHistory = new Queue<SubscriptionStateHistory>();
        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Created, dateTimeProvider.Now()));
    }

    private Subscription(string subscriptionId, string? customerId, 
        string productId, Product? product,
        decimal loanAmount, decimal insuredAmount, 
        IEnumerable<SubscriptionStateHistory> stateHistory,
        UnderwritingResult? underwritingResult)
    {
        dateTimeProvider = new UtcDateTimeProvider();
        SubscriptionId = subscriptionId;
        CustomerId = customerId;
        ProductId = productId;
        Product = product;
        LoanAmount = loanAmount;
        InsuredAmount = insuredAmount;
        StateHistory = new Queue<SubscriptionStateHistory>(stateHistory);
        UnderwritingResult = underwritingResult;
    }

    private IDateTimeProvider dateTimeProvider;
    public string? CustomerId { get; set; }
    public string SubscriptionId { get; }
    public string ProductId { get; }
    public Product? Product { get; private set; }
    public decimal LoanAmount { get; }
    public decimal InsuredAmount { get; private set; }
    public SubscriptionState State => StateHistory.Last().State;
    public UnderwritingResult? UnderwritingResult { get; private set; }
    public Queue<SubscriptionStateHistory> StateHistory { get; }

    public Subscription Register()
    {
        if (State > SubscriptionState.Registered)
            return this;

        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Registered, dateTimeProvider.Now()));
        return this;
    }

    public Subscription Normalize(ProductProxyService productService)
    {
        if (State > SubscriptionState.Normalized)
            return this;

        // enrich subscription
        Product = productService.GetProduct(ProductId);

        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Normalized, dateTimeProvider.Now()));
        return this;
    }

    // semantic validations
    public SubscriptionValidationResult Validate()
    {
        if (State > SubscriptionState.Validated)
            return SubscriptionValidationResult.Valid;

        if (Product == null)
            throw new InvalidOperationException();

        if (!Product.IsActiveOn(dateTimeProvider.Now()))
            return new SubscriptionValidationResult(false, "Product is not active.");

        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Validated, dateTimeProvider.Now()));
        return SubscriptionValidationResult.Valid;
    }

    public Subscription RequestInformation(UnderwritingResult underwritingResult)
    {
        Guard.ArgumentNotNull(underwritingResult);

        UnderwritingResult = underwritingResult;
        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Pending, dateTimeProvider.Now()));
        return this;
    }

    public Subscription OnInformationReceived()
    {
        if (State < SubscriptionState.Pending)
            return this;

        UnderwritingResult = null;
        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.InAnalysis, dateTimeProvider.Now()));
        return this;
    }

    public Subscription Accept(string reason)
    {
        UnderwritingResult = new UnderwritingResult(UnderwritingResultState.Accepted, reason);
        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Accepted, dateTimeProvider.Now()));
        return this;
    }

    public Subscription Reject(string reason)
    {
        UnderwritingResult = new UnderwritingResult(UnderwritingResultState.Rejected, reason);
        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Rejected, dateTimeProvider.Now()));
        return this;
    }

    public Subscription Suspend(string reason)
    {
        UnderwritingResult = new UnderwritingResult(UnderwritingResultState.Pending, reason);
        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Suspended, dateTimeProvider.Now()));
        return this;
    }

    public SubscriptionModel ToModel() => new(SubscriptionId, CustomerId, ProductId, Product,
        LoanAmount, InsuredAmount, State.Name, UnderwritingResult, StateHistory.ToArray());

    public static Subscription FromModel(SubscriptionModel model) => new(
        model.SubscriptionId,
        model.CustomerId,
        model.ProductId,
        model.Product,
        model.LoanAmount,
        model.InsuredAmount,
        new Queue<SubscriptionStateHistory>(model.History),
        model.UnderwritingResult);
}