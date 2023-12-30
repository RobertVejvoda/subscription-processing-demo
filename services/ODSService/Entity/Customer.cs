namespace ODSService.Entity;

public class Customer
{
    public required string Id { get; init; }
    public int CustomerNo { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string State { get; init; }
    public DateOnly BirthDate { get; init; }
    public DateTime LastUpdatedOn { get; init; }
    public decimal TotalLoanAmount { get; set; }
    public decimal TotalInsuredAmount { get; set; }
    public required ICollection<Subscription> Subscriptions { get; init; }
}