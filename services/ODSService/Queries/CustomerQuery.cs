namespace ODSService.Queries;

public class CustomerQuery(OdsDataContext context)
{
    public async Task<CustomerModel?> FindCustomerById(string customerId)
    {
        var result = await context.Set<Customer>()
            .Where(x => x.Id.Equals(customerId))
            .Select(x => new CustomerModel
            {
                CustomerId = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                CustomerState = x.State,
                BirthDate = x.BirthDate,
                TotalLoanAmount = x.TotalLoanAmount,
                TotalInsuredAmount = x.TotalInsuredAmount
            }).SingleOrDefaultAsync();

        return result;
    }

    public async Task<SubscriptionModel?> FindSubscriptionById(string subscriptionId)
    {
        var result = await context.Set<Subscription>()
            .Where(x => x.Id.Equals(subscriptionId))
            .Select(x => new SubscriptionModel
            {
                SubscriptionId = x.Id,
                SubscriptionState = x.State,
                LoanAmount = x.LoanAmount,
                InsuredAmount = x.InsuredAmount,
                ReceivedOn = x.ReceivedOn,
                LastUpdatedOn = x.LastUpdatedOn,
                UnderwritingResult = x.UnderwritingResult,
                Message = x.Message
            }).SingleOrDefaultAsync();

        return result;
    }

    public async Task<List<CustomerModel>> FindCustomersAsync(int limit)
    {
        var results = 
            await context.Set<Customer>()
                .OrderByDescending(x => x.LastUpdatedOn)
                .Take(limit)
                .Select(x => new CustomerModel
                {
                    CustomerId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    CustomerState = x.State,
                    BirthDate = x.BirthDate,
                    TotalLoanAmount = x.TotalLoanAmount,
                    TotalInsuredAmount = x.TotalInsuredAmount
                }).ToListAsync();

        return results;
    }

    public async Task<List<SubscriptionModel>> FindSubscriptionsForCustomer(string customerId)
    {
        var results =
            await context.Set<Subscription>()
                .Where(x => x.Customer.Id.Equals(customerId))
                .Select(x => new SubscriptionModel
                {
                    SubscriptionId = x.Id,
                    SubscriptionState = x.State,
                    LoanAmount = x.LoanAmount,
                    InsuredAmount = x.InsuredAmount,
                    ReceivedOn = x.ReceivedOn,
                    LastUpdatedOn = x.LastUpdatedOn,
                    UnderwritingResult = x.UnderwritingResult,
                    Message = x.Message
                })
                .OrderBy(x => x.ReceivedOn)
                .ToListAsync();
        
        return results;
    }
}