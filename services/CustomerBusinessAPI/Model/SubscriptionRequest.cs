using Core.Helpers;

namespace CustomerBusinessAPI.Model;

public class SubscriptionRequest
{
    public required string ProcessInstanceKey { get; init; }
    public string? SubscriptionId { get; init; }
    public string? CustomerId { get; init; }
    public required string FirstName  { get; init; }
    public required string LastName  { get; init; }
    public required string Email  { get; init; }
    public required DateOnly BirthDate { get; init; }
    public string? SubscriptionState  { get; init; }
    public string? UnderwritingResultMessage { get; init; }
    public required string ProductId { get; init; }
    public required decimal LoanAmount  { get; init; }
    public required decimal InsuredAmount  { get; init; }
    public required DateTime ReceivedOn  { get; init; }
    public required DateTime LastUpdatedOn { get; init; }

    public int Age => Calculator.CalculateAge(BirthDate);
}