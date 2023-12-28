using ODSService.Model;

namespace ODSService.Queries;

public class CustomerQuery
{
    private readonly ODSDataContext context;

    public CustomerQuery(ODSDataContext context)
    {
        this.context = context;
    }

    public async Task<List<Model.Customer>> GetCustomersAsync(int limit)
    {
        var now = DateTime.Now;
        
        var results = 
            await context.Customers
                .OrderByDescending(x => x.LastUpdatedOn)
                .Take(limit)
                .Select(x => new Model.Customer
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Status = x.Status,
                    BirthDate = x.BirthDate,
                    TotalLoanAmount = x.Subscriptions.Select(y => y.LoanAmount).DefaultIfEmpty().Sum(),
                    TotalInsuredAmount = x.Subscriptions.Select(y => y.InsuredAmount).DefaultIfEmpty().Sum()
                }).ToListAsync();

        return results;
    }

    public async Task<List<Subscription>> GetSubscriptionsForCustomer(string customerId)
    {
        var results =
            await context.Subscriptions
                .Where(x => x.Customer.Id.Equals(customerId))
                .Select(x => new Subscription
                {
                    Id = x.Id,
                    State = x.Status,
                    LoanAmount = x.LoanAmount,
                    InsuredAmount = x.InsuredAmount,
                    ReceivedOn = x.ReceivedOn,
                    LastUpdatedOn = x.LastUpdatedOn,
                    UnderwritingResult = x.UnderwritingResult
                })
                .OrderBy(x => x.ReceivedOn)
                .ToListAsync();
        
        return results;
    }
}