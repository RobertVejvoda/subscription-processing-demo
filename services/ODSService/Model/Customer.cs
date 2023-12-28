namespace ODSService.Model;

public class Customer
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
    public DateOnly BirthDate { get; set; }
    public decimal TotalLoanAmount { get; set; }
    public decimal TotalInsuredAmount { get; set; }
}