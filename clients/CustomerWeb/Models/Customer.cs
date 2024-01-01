namespace CustomerWeb.Models;

public class Customer
{
    public string? CustomerId { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public DateOnly BirthDate { get; set; }
    
    public string? Email { get; set; }
    
    public string? CustomerState { get; set; }
    
    public decimal TotalLoanAmount { get; set; }
    
    public decimal TotalInsuredAmount { get; set; }
    
    public int Age => DateTime.Now.Year - BirthDate.Year;
}