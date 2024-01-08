namespace CustomerExperienceAPI.Queries;

public class Queries(CustomerDataContext context)
{
    public async Task<ICollection<SubscriptionRequestModel>> GetSubscriptionRequests(int? limit = 15)
    {
        // ensure reasonable limits
        if (limit is null or <= 0)
            limit = 15;

        var results =
            await context.Set<SubscriptionRequestEntity>()
                .Select(x => new SubscriptionRequestModel()
                {
                    ProcessInstanceKey = x.ProcessInstanceKey,
                    SubscriptionId = x.SubscriptionId,
                    CustomerId = x.CustomerId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    BirthDate = x.BirthDate,
                    SubscriptionState = x.SubscriptionState,
                    UnderwritingResultMessage = x.UnderwritingResultMessage,
                    ProductId = x.ProductId,
                    LoanAmount = x.LoanAmount,
                    InsuredAmount = x.InsuredAmount,
                    ReceivedOn = x.ReceivedOn,
                    LastUpdatedOn = x.LastUpdatedOn
                })
                .OrderByDescending(x => x.ReceivedOn)
                .Take(limit.Value)
                .ToListAsync();
        
        return results;
    }

    public async Task<SubscriptionRequestModel?> FindSubscriptionRequestByProcessInstanceKey(string processInstanceKey)
    {
        return 
            await context.Set<SubscriptionRequestEntity>()
                .Where(x => x.ProcessInstanceKey.Equals(processInstanceKey))
                .Select(x => new SubscriptionRequestModel
                {
                    ProcessInstanceKey = x.ProcessInstanceKey,
                    SubscriptionId = x.SubscriptionId,
                    CustomerId = x.CustomerId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    BirthDate = x.BirthDate,
                    SubscriptionState = x.SubscriptionState,
                    UnderwritingResultMessage = x.UnderwritingResultMessage,
                    ProductId = x.ProductId,
                    LoanAmount = x.LoanAmount,
                    InsuredAmount = x.InsuredAmount,
                    ReceivedOn = x.ReceivedOn,
                    LastUpdatedOn = x.LastUpdatedOn
                    
                })
                .SingleOrDefaultAsync();
    }
}