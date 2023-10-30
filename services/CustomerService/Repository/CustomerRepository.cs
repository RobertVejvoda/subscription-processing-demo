using Dapr.Client;

namespace CustomerService.Repository;

public class CustomerRepository
{
    private readonly DaprClient daprClient;
    private readonly DaprOptions daprOptions;

    public CustomerRepository(DaprClient daprClient, IOptions<DaprOptions> daprOptions)
    {
        this.daprClient = daprClient;
        this.daprOptions = daprOptions.Value;
    }
    
    public Task AddAsync(Model.Customer customer)
        => daprClient.SaveStateAsync(daprOptions.StateStore,
            customer.Id,
            customer);

    public Task<Customer?> GetAsync(string customerId)
        => daprClient.GetStateAsync<Customer?>(daprOptions.StateStore,
            customerId);
}