namespace ODSService.Model;

public class CustomerModel
{
    public string CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string CustomerState { get; set; }
    public DateOnly BirthDate { get; set; }
    public int Age => DateTime.Now.Year - BirthDate.Year;
    public decimal TotalLoanAmount { get; set; }
    public decimal TotalInsuredAmount { get; set; }
}