using CustomerService.Repository;

namespace CustomerService.Events;

public record SubscriptionRequestReceivedIntegrationEvent(
    [Required] string SubscriptionId,
    [Required] string FirstName, 
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age,
    [Required] decimal LoanAmount, 
    [Required] decimal InsuredAmount) : IntegrationEvent;

public record CustomerRegisteredIntegrationEvent(
    [Required] string SubscriptionId,
    [Required] string CustomerId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age) : IntegrationEvent;

public class SubscriptionIntegrationEventHandler
    : IIntegrationEventHandler<SubscriptionRequestReceivedIntegrationEvent>
{
    private readonly CustomerRepository repository;
    private readonly IEventBus eventBus;

    public SubscriptionIntegrationEventHandler(CustomerRepository repository, IEventBus eventBus)
    {
        this.repository = repository;
        this.eventBus = eventBus;
    }
    
    public async Task Handle(SubscriptionRequestReceivedIntegrationEvent @event)
    {
        // find client in database or create a new record
        // note: client lifecycle including validations is not part of this demo
        var key = Customer.Key(@event.Email);
        var customer = await repository.GetAsync(@event.Email);
        if (customer == null)
        {
            customer = new Customer(key, @event.FirstName, @event.LastName, @event.Email, @event.Age, "valid");
            await repository.AddAsync(customer);
        }

        await eventBus.PublishAsync(Resources.Bindings.PubSub, Resources.Topics.Customer.Registered,
            new CustomerRegisteredIntegrationEvent(
                @event.SubscriptionId,
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.Age));
    }
}