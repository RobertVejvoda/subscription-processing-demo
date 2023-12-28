using Dapr.Client;
using Microsoft.Extensions.Logging;

namespace EventBus;

public class DaprEventBus : IEventBus
{
    private readonly DaprClient dapr;
    private readonly ILogger<DaprEventBus> logger;

    public DaprEventBus(DaprClient dapr, ILogger<DaprEventBus> logger)
    {
        this.dapr = dapr;
        this.logger = logger;
    }

    public async Task PublishAsync<TIntegrationEvent>(string pubSubName, string topicName, TIntegrationEvent @event)
        where TIntegrationEvent : IntegrationEvent
    {
        logger.LogInformation("Publishing event {@Event} to {PubSubName}.{TopicName}", @event,
            pubSubName, topicName);

        // We need to make sure that we pass the concrete type to PublishEventAsync,
        // which can be accomplished by casting the event to dynamic. This ensures
        // that all event fields are properly serialized.
        await dapr.PublishEventAsync(pubSubName, topicName, (object)@event);
    }
    
    public async Task PublishAsync<TIntegrationEvent>(string topicName, TIntegrationEvent @event)
        where TIntegrationEvent : IntegrationEvent
    {
        var pubSubName = Environment.GetEnvironmentVariable("DAPR_BINDINGS_PUBSUB");
        if (string.IsNullOrWhiteSpace(pubSubName))
            throw new Exception("Environment variable DAPR_BINDINGS_PUBSUB is not set.");

        await PublishAsync(pubSubName, topicName, @event);
    }
}