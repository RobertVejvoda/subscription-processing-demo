namespace ODSService.Model;

public class Subscription
{
    public string Id { get; set; }
    public string State { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal InsuredAmount { get; set; }
    public DateTime ReceivedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; } 
    public string? UnderwritingResult { get; set; }
}