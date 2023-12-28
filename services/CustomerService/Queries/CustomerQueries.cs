using System.Diagnostics.CodeAnalysis;

namespace CustomerService.Queries;

public class CustomerQueries
{
    public static Task<string> IdentifyCustomer(string email, DateOnly birthDate)
    {
        // fake 
        return Task.FromResult(string.Empty);
    }
}