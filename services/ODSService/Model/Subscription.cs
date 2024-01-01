namespace ODSService.Model;

public class Subscription
{
    public required string ProcessInstanceKey { get; init; }
    public required string SubscriptionId { get; init; }
    public required string CustomerId { get; init; }
    public required string FirstName  { get; init; }
    public required string LastName  { get; init; }
    public required string Email  { get; init; }
    public required string SubscriptionState  { get; init; }
    public required string ProductId { get; init; }
    public required decimal LoanAmount  { get; init; }
    public required decimal InsuredAmount  { get; init; }
    public string? UnderwritingResult  { get; init; }
    public string? Message  { get; init; }
    public required DateTime ReceivedOn  { get; init; }
    public required DateTime LastUpdatedOn { get; init; }
}