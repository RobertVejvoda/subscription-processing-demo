namespace ODSService.Model;

public class Customer
{
    public required string CustomerId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string CustomerState { get; init; }
    public required DateOnly BirthDate { get; init; }
    public int Age => DateTime.Now.Year - BirthDate.Year;
    public required decimal TotalLoanAmount { get; init; }
    public required decimal TotalInsuredAmount { get; init; }
}