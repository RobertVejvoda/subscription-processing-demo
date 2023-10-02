namespace EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync<TIntegrationEvent>(string pubSubName, TIntegrationEvent @event)
        where TIntegrationEvent : IntegrationEvent;
}