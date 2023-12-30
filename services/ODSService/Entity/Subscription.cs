namespace ODSService.Entity;

public class Subscription
{
    public string Id { get; set; }
    public int SubscriptionNo { get; set; }
    public string State { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal InsuredAmount { get; set; }
    public string ProductId { get; set; }
    public DateTime ReceivedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; } 
    public string? UnderwritingResult { get; set; }
    public string? Message { get; set; }
    public string ProcessInstanceKey { get; set; }
    public string CustomerId { get; set; }
    
    public virtual Customer? Customer { get; set; }
}