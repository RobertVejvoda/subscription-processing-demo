using Core.Repository;
using Dapr.Client;

namespace CustomerService.Repository;

public class CustomerRepository(DaprClient daprClient) : RepositoryBase<Domain.Customer>
{
    private const string StateStoreName = "statestore";

    public override Task AddAsync(Domain.Customer customer)
    {
        var model = customer.ToDto();
        var key = $"C-{customer.Id}";
        return daprClient.SaveStateAsync(StateStoreName, key, model);
    }

    public async Task<Domain.Customer?> GetAsync(string id)
    {
        var key = $"C-{id}";
        var dto = await daprClient.GetStateAsync<Dto.Customer>(StateStoreName, key);
        if (dto == null)
            return null;

        var customer = Domain.Customer.FromDto(dto);
        return customer;
    }
}