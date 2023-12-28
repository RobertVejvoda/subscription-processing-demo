namespace ODSService.Entity;

public class Subscription
{
    public string Id { get; set; }
    public int SubscriptionNo { get; set; }
    public string Status { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal InsuredAmount { get; set; }
    public DateTime ReceivedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; } 
    public string UnderwritingResult { get; set; }
    
    
    public virtual Customer Customer { get; set; }
}