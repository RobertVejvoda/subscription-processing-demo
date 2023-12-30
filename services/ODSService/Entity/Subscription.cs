namespace ODSService.Entity;

public class Subscription
{
    public required string Id { get; init; }
    public int SubscriptionNo { get; init; }
    public required string State { get; init; }
    public decimal LoanAmount { get; init; }
    public decimal InsuredAmount { get; init; }
    public required string ProductId { get; init; }
    public DateTime ReceivedOn { get; init; }
    public DateTime LastUpdatedOn { get; set; } 
    public string? UnderwritingResult { get; set; }
    public string? Message { get; set; }
    public required string ProcessInstanceKey { get; init; }

    public virtual Customer Customer { get; init; } = null!;
}