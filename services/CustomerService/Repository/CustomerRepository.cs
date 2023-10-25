using Dapr.Client;

namespace CustomerService.Repository;

public class CustomerRepository
{
    private readonly DaprClient daprClient;

    public CustomerRepository(DaprClient daprClient)
    {
        this.daprClient = daprClient;
    }
    
    public Task AddAsync(Model.Customer customer)
        => daprClient.SaveStateAsync(Resources.Bindings.StateStore,
            customer.Id,
            customer);

    public Task<Customer?> GetAsync(string customerId)
        => daprClient.GetStateAsync<Customer?>(Resources.Bindings.StateStore,
            customerId);
}