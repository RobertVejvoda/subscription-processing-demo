namespace CustomerBusinessAPI.Entity;

public class SubscriptionRequestEntity
{
    public required string ProcessInstanceKey { get; init; }
    public int SubscriptionRequestNo { get; init; }
    public string? CustomerId { get; set; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required DateOnly BirthDate { get; init; }
    public string? SubscriptionId { get; set; }
    public decimal LoanAmount { get; init; }
    public decimal InsuredAmount { get; init; }
    public required string ProductId { get; init; }
    public string? SubscriptionState { get; set; }
    public DateTime ReceivedOn { get; init; }
    public string? UnderwritingResultMessage { get; set; }
    public DateTime LastUpdatedOn { get; set; }
}