using ODSService.Entity;

namespace ODSService;

public class Customer
{
    public string Id { get; set; }
    public int CustomerNo { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
    public DateOnly BirthDate { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
}