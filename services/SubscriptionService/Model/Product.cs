using Core.Types;

namespace SubscriptionService.Model;

public record Product(
    [Required]string ProductId, 
    string Name, 
    DateRange ValidityPeriod, 
    string State)
{
    public bool IsActive(DateTime decisiveDate)
    {
        if (ValidityPeriod == null)
            throw new InvalidOperationException();
        
        return ValidityPeriod.DateStart <= decisiveDate && ValidityPeriod.DateEnd > decisiveDate;
    }
}