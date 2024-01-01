using Subscription = ODSService.Model.Subscription;

namespace ODSService.Queries;

public class Queries(OdsDataContext context)
{
    public async Task<ICollection<Subscription>> GetSubscriptions(int? limit = 15)
    {
        // ensure reasonable limits
        if (limit is null or <= 0)
            limit = 15;

        var results =
            await context.Set<Entity.Subscription>()
                .Select(x => new Subscription
                {
                    ProcessInstanceKey = x.ProcessInstanceKey,
                    SubscriptionId = x.Id,
                    CustomerId = x.Customer.Id,
                    FirstName = x.Customer.FirstName,
                    LastName = x.Customer.LastName,
                    Email = x.Customer.Email,
                    SubscriptionState = x.State,
                    ProductId = x.ProductId,
                    LoanAmount = x.LoanAmount,
                    InsuredAmount = x.InsuredAmount,
                    ReceivedOn = x.ReceivedOn,
                    LastUpdatedOn = x.LastUpdatedOn,
                    UnderwritingResult = x.UnderwritingResult,
                    Message = x.Message
                })
                .OrderByDescending(x => x.ReceivedOn)
                .Take(limit.Value)
                .ToListAsync();
        
        return results;
    }
    
    public async Task<Model.Customer?> GetCustomerById(string customerId)
    {
        var result = await context.Set<Entity.Customer>()
            .Where(x => x.Id.Equals(customerId))
            .Select(x => new Model.Customer
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

    public async Task<List<Model.Customer>> GetCustomersAsync(int limit)
    {
        var results = 
            await context.Set<Entity.Customer>()
                .OrderByDescending(x => x.LastUpdatedOn)
                .Take(limit)
                .Select(x => new Model.Customer
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

    public async Task<List<Subscription>> GetCustomerSubscriptions(string customerId)
    {
        var results =
            await context.Set<Entity.Subscription>()
                .Where(x => x.Customer.Id.Equals(customerId))
                .Select(x => new Subscription
                {
                    ProcessInstanceKey = x.ProcessInstanceKey,
                    CustomerId = x.Customer.Id,
                    FirstName = x.Customer.FirstName,
                    LastName = x.Customer.LastName,
                    Email = x.Customer.Email,
                    SubscriptionId = x.Id,
                    SubscriptionState = x.State,
                    LoanAmount = x.LoanAmount,
                    InsuredAmount = x.InsuredAmount,
                    ProductId = x.ProductId,
                    ReceivedOn = x.ReceivedOn,
                    LastUpdatedOn = x.LastUpdatedOn,
                    UnderwritingResult = x.UnderwritingResult,
                    Message = x.Message
                })
                .OrderBy(x => x.ReceivedOn)
                .ToListAsync();
        
        return results;
    }

    public async Task<Subscription?> FindSubscriptionByProcessInstanceKey(string processInstanceKey)
    {
        return 
            await context.Set<Entity.Subscription>()
                .Where(x => x.ProcessInstanceKey.Equals(processInstanceKey))
                .Select(x => new Subscription
                {
                    ProcessInstanceKey = x.ProcessInstanceKey,
                    SubscriptionId = x.Id,
                    CustomerId = x.Customer.Id,
                    FirstName = x.Customer.FirstName,
                    LastName = x.Customer.LastName,
                    Email = x.Customer.Email,
                    SubscriptionState = x.State,
                    ProductId = x.ProductId,
                    LoanAmount = x.LoanAmount,
                    InsuredAmount = x.InsuredAmount,
                    ReceivedOn = x.ReceivedOn,
                    LastUpdatedOn = x.LastUpdatedOn,
                    UnderwritingResult = x.UnderwritingResult,
                    Message = x.Message
                })
                .SingleOrDefaultAsync();
    }
}