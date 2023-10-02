using Core.Types;

namespace SubscriptionService.Model;

public record Product(string ProductId, string Name, DateRange ValidityPeriod, string State)
{
    public bool IsActive(DateTime decisiveDate)
    {
        return ValidityPeriod.DateStart <= decisiveDate && ValidityPeriod.DateEnd > decisiveDate;
    }
}