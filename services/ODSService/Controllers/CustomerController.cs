using Dapr;
using Microsoft.AspNetCore.Mvc;
using ODSService.Events;
using ODSService.Model;

namespace ODSService.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> logger;
    private readonly CustomerRepository customerRepository;
    private readonly CustomerQuery customerQuery;

    public CustomerController(ILogger<CustomerController> logger, CustomerRepository customerRepository, CustomerQuery customerQuery)
    {
        this.logger = logger;
        this.customerRepository = customerRepository;
        this.customerQuery = customerQuery;
    }

    [HttpGet]
    public async Task<ICollection<Model.Customer>> Get(int limit = 5)
    {
        return await customerQuery.GetCustomersAsync(limit);
    }

    [HttpGet("{customerId}/subscriptions")]
    public async Task<ICollection<Subscription>> GetSubscriptions(string customerId)
    {
        return await customerQuery.GetSubscriptionsForCustomer(customerId);
    }

    [HttpPost("/customer-registered")]
    public async Task OnCustomerRegistered(CustomerRegisteredIntegrationEvent @event, [FromServices] CustomerRepository repository)
    {
        var customer = await repository.GetById(@event.CustomerId);
        if (customer != null)
        {
            logger.LogInformation("Customer {CustomerId} is already registered.", @event.CustomerId);
            return;
        }

        await repository.Add(new Customer
            {
                Id = @event.CustomerId,
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                Email = @event.Email,
                Status = @event.Status,
                BirthDate = @event.BirthDate,
                LastUpdatedOn = default
            });
    }
}