using ClientService.Model;
using Dapr.Client;

namespace ClientService.Repository;

public class ClientRepository
{
    private readonly DaprClient daprClient;

    public ClientRepository(DaprClient daprClient)
    {
        this.daprClient = daprClient;
    }
    
    public Task AddAsync(Model.Client client)
        => daprClient.SaveStateAsync(Resources.Bindings.StateStore,
            client.Id,
            client);

    public Task<Client> GetAsync(string clientId)
        => daprClient.GetStateAsync<Client>(Resources.Bindings.StateStore,
            clientId);
}