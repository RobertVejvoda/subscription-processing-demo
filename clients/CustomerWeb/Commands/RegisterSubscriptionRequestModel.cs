namespace CustomerWeb.Commands;

public class RegisterSubscriptionRequestCommand
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required DateOnly BirthDate { get; init; }
    public required decimal LoanAmount { get; init; }
    public required decimal InsuredAmount { get; init; }
    public required string ProductId { get; init; } 
}