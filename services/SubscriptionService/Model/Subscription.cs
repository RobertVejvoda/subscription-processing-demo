using Core.Domain;
using Microsoft.OpenApi.Extensions;

namespace SubscriptionService.Model;

public class Subscription : IAggregateRoot
{
    // new Subscription
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

    // saved Subscription
    private Subscription(string subscriptionId, string? customerId, string productId,
        decimal loanAmount, decimal insuredAmount, 
        IEnumerable<SubscriptionStateHistory> stateHistory,
        UnderwritingResult? underwritingResult)
    {
        dateTimeProvider = new UtcDateTimeProvider();
        SubscriptionId = subscriptionId;
        CustomerId = customerId;
        ProductId = productId;
        LoanAmount = loanAmount;
        InsuredAmount = insuredAmount;
        StateHistory = new Queue<SubscriptionStateHistory>(stateHistory);
        UnderwritingResult = underwritingResult;
    }

    private readonly IDateTimeProvider dateTimeProvider;
    
    public string? CustomerId { get; set; }
    public string SubscriptionId { get; }
    public string ProductId { get; }
    public decimal LoanAmount { get; }
    public decimal InsuredAmount { get; private set; }
    public SubscriptionState State => StateHistory.Last().State;
    public DateTime LastUpdatedOn => StateHistory.Last().ChangedOn;
    public UnderwritingResult? UnderwritingResult { get; private set; }
    public Queue<SubscriptionStateHistory> StateHistory { get; }

    public Subscription Register()
    {
        if (State > SubscriptionState.Registered)
            return this;

        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Registered, dateTimeProvider.Now()));
        return this;
    }

    public Subscription Normalize()
    {
        if (State > SubscriptionState.Normalized)
            return this;

        // todo: normalize subscription

        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Normalized, dateTimeProvider.Now()));
        return this;
    }

    // semantic validations
    public SubscriptionValidationResult Validate(ProductProxyService productService)
    {
        if (State > SubscriptionState.Validated)
            return SubscriptionValidationResult.Valid;

        productService.EnsureActiveProduct(ProductId);

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

    public Subscription Accept(string? reason)
    {
        UnderwritingResult = new UnderwritingResult(UnderwritingResultState.Accepted, reason);
        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Accepted, dateTimeProvider.Now()));
        return this;
    }

    public Subscription Reject(string? reason)
    {
        UnderwritingResult = new UnderwritingResult(UnderwritingResultState.Rejected, reason);
        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Rejected, dateTimeProvider.Now()));
        return this;
    }

    public Subscription Suspend(string? reason)
    {
        UnderwritingResult = new UnderwritingResult(UnderwritingResultState.Pending, reason);
        StateHistory.Enqueue(new SubscriptionStateHistory(SubscriptionState.Suspended, dateTimeProvider.Now()));
        return this;
    }

    public SubscriptionModel ToModel() => new(SubscriptionId, CustomerId, ProductId,
        LoanAmount, InsuredAmount, State.GetDisplayName(), LastUpdatedOn, 
        UnderwritingResult?.State.GetDisplayName(), UnderwritingResult?.Reason, StateHistory.ToArray());

    public static Subscription FromModel(SubscriptionModel model)
    {
        UnderwritingResult? underwritingResult = null;
        if (Enum.TryParse(model.UnderwritingResultState?.ToCharArray(),
                true, out UnderwritingResultState underwritingResultState))
        {
            underwritingResult = new UnderwritingResult(underwritingResultState, model.UnderwritingResultMessage);
        }
        
        return new Subscription(
            model.SubscriptionId,
            model.CustomerId,
            model.ProductId,
            model.LoanAmount,
            model.InsuredAmount,
            new Queue<SubscriptionStateHistory>(model.History),
            underwritingResult);      
        
    } 
}