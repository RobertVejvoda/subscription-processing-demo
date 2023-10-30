using SubscriptionService.Commands;

namespace SubscriptionService.Proxy;

public class CustomerProxyService
{
    private readonly DaprClient daprClient;

    public CustomerProxyService(DaprClient daprClient)
    {
        this.daprClient = daprClient;
    }

    public async Task<Customer> RegisterCustomerAsync(RegisterCustomerCommand command)
    {
        var customer = await daprClient.InvokeMethodAsync<RegisterCustomerCommand,Customer>(
            "customer-service",
            "api/customers",
            command);

        return customer;
    }
}