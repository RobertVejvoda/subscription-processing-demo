using System.ComponentModel.DataAnnotations;

namespace CustomerWeb.Models;

public class RegisterSubscriptionRequest
{
    [Required]
    [StringLength(32)]
    public string? FirstName { get; set; }
    
    [Required] 
    [StringLength(64)]
    public string? LastName { get; set; }
    
    [Required] 
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    
    [Required] 
    [DataType(DataType.Date)]
    public DateOnly BirthDate { get; set; }
    
    [Required] 
    [DataType(DataType.Currency)]
    public decimal LoanAmount { get; set; }
    
    [Required] 
    [DataType(DataType.Currency)]
    public decimal InsuredAmount { get; set; }
    
    [Required]
    [StringLength(16)]
    public string? ProductId { get; set; } 
}