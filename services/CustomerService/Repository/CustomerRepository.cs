using Core.Repository;
using Dapr.Client;

namespace CustomerService.Repository;

public class CustomerRepository(DaprClient daprClient) : RepositoryBase<Customer>
{
    public override Task AddAsync(Customer customer)
    {
        var model = customer.ToModel();
        var key = $"C-{customer.Id}";
        return daprClient.SaveStateAsync("statestore", key, model);
    }

    public async Task<Customer?> GetAsync(string id)
    {
        var key = $"C-{id}";
        var model = await daprClient.GetStateAsync<CustomerModel>("statestore", key);
        if (model == null)
            return null;

        var customer = Customer.FromModel(model);
        return customer;
    }
}