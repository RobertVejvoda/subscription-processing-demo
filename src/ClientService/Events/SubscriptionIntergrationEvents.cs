using ClientService.Model;

namespace ClientService.Events;

public record SubscriptionRequestReceivedIntegrationEvent(
    [Required] string RequestId,
    [Required] string FirstName, 
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age,
    [Required] decimal LoanAmount, 
    [Required] decimal InsuredAmount) : IntegrationEvent;

public record ClientRegisteredIntegrationEvent(
    [Required] string RequestId,
    [Required] string ClientId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] int Age) : IntegrationEvent;

public class SubscriptionIntegrationEventHandler
    : IIntegrationEventHandler<SubscriptionRequestReceivedIntegrationEvent>
{
    private readonly ClientRepository repository;
    private readonly IEventBus eventBus;

    public SubscriptionIntegrationEventHandler(ClientRepository repository, IEventBus eventBus)
    {
        this.repository = repository;
        this.eventBus = eventBus;
    }
    
    public async Task Handle(SubscriptionRequestReceivedIntegrationEvent @event)
    {
        // find client in database or create a new record
        // note: client lifecycle including validations is not part of this demo
        var id = @event.Email.Trim().ToLower();
        var client = await repository.GetAsync(id);
        if (client == null)
        {
            client = new Client(id, @event.FirstName, @event.LastName, @event.Email, @event.Age, "valid");
        }

        await eventBus.PublishAsync(Resources.Bindings.PubSub,
            new ClientRegisteredIntegrationEvent(
                @event.RequestId,
                client.Id,
                client.FirstName,
                client.LastName,
                client.Email,
                client.Age));
    }
}