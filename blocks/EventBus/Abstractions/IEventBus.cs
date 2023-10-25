namespace EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync<TIntegrationEvent>(string pubSubName, string topicName, TIntegrationEvent @event)
        where TIntegrationEvent : IntegrationEvent;
}