using Core.Exceptions;
using SubscriptionAPI.Model;

namespace SubscriptionService.Model;

// Note: This is a demo.
// Proper implementation should throw DomainEvents which are handled by DomainEventHandlers.
// DomainEventHandlers do 2 tasks:
// 1/ save state of current object
// 2/ create and trigger those IntegrationEvents 

public class Subscription
{
    public string RequestId { get; private set; }
    public string Id { get; private set; }
    public Client? Client { get; private set; }
    public string ProductId { get; private set; }
    public Product? Product { get; private set; }
    public decimal LoanAmount { get; private set; }
    public decimal InsuredAmount { get; private set; }
    public SubscriptionState State { get; private set; }
    public string? UnderwritingResult { get; private set; }

    // naive
    public static string Key(Guid subscriptionId) => $"S-{subscriptionId.ToString()[25..]}".ToUpper();

    public static Subscription Create(string requestId, string productId, decimal loanAmount, decimal insuredAmount)
    {
        Guard.ArgumentNotNullOrEmpty(requestId);
        Guard.ArgumentNotNullOrEmpty(productId);
        Guard.ArgumentPositive(loanAmount);
        Guard.ArgumentPositive(insuredAmount);

        var subscriptionId = Key(Guid.NewGuid());
        var subscription = new Subscription(subscriptionId, productId, loanAmount, insuredAmount,
            SubscriptionState.Created, null, requestId);

        return subscription;
    }
    
    private Subscription(string id, string productId, decimal loanAmount, decimal insuredAmount, SubscriptionState state, string? underwritingResult, string requestId)
    {
        Id = id;
        ProductId = productId;
        LoanAmount = loanAmount;
        InsuredAmount = insuredAmount;
        State = state;
        UnderwritingResult = underwritingResult;
        RequestId = requestId;
    }

    public Subscription Register()
    {
        if (State.Id >= SubscriptionState.Registered.Id)
            return this;

        State = SubscriptionState.Registered;
        return this;
    }

    public Subscription Normalize(NormalizationService normalizationService)
    {
        Guard.ArgumentNotNull(normalizationService);
        
        if (State.Id >= SubscriptionState.Normalized.Id)
            return this;

        // ensure active product
        normalizationService.Normalize(this);
        
        State = SubscriptionState.Normalized;
        return this;
    }

    public async Task<Subscription> Analyze(SubscriptionAssessmentService subscriptionAssessmentService)
    {
        Guard.ArgumentNotNull(subscriptionAssessmentService);

        if (State.Id >= SubscriptionState.InAnalysis.Id)
            return this;
        
        await subscriptionAssessmentService.Assess(this);

        State = SubscriptionState.InAnalysis;
        return this;
    }

    public Subscription Accept()
    {
        if (State.Id >= SubscriptionState.Accepted.Id)
            return this;
        
        State = SubscriptionState.Accepted;
        return this;
    }

    public Subscription Reject(string reason)
    {
        if (State.Id >= SubscriptionState.Rejected.Id)
            return this;
        
        State = SubscriptionState.Rejected;
        return this;
    }

    public Subscription Suspend()
    {
        if (State.Equals(SubscriptionState.Canceled))
            return this;

        if (!State.Equals(SubscriptionState.Pending))
            throw new DomainException("Subscription is not pending and can't be suspended.");

        State = SubscriptionState.Suspended;
        return this;
    }

    public Subscription Pending(string reason)
    {
        if (State.Id >= SubscriptionState.Pending.Id)
            return this;
        
        State = SubscriptionState.Pending;
        return this;
    }

    public Subscription Cancel()
    {
        if (State.Id >= SubscriptionState.Canceled.Id)
            return this;
        
        State = SubscriptionState.Canceled;
        return this;
    }

    public Subscription EnrichWith(Product product)
    {
        if (!ProductId.Equals(product.ProductId, StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException("Product does not match.");
        
        this.Product = product;
        return this;
    }

    public Subscription EnrichWith(Client client)
    {
        this.Client = client;
        return this;
    }

    public bool IsAccepted => State.Equals(SubscriptionState.Accepted);
    
    public bool IsPending => State.Equals(SubscriptionState.Pending);
    
    public bool IsRejected => State.Equals(SubscriptionState.Rejected);
}
