namespace ODSService.Model;

public class CustomerModel
{
    public required string CustomerId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string CustomerState { get; init; }
    public DateOnly BirthDate { get; init; }
    public int Age => DateTime.Now.Year - BirthDate.Year;
    public decimal TotalLoanAmount { get; init; }
    public decimal TotalInsuredAmount { get; init; }
}