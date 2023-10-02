using Dapr.Client;
using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EventBus;

public class DaprEventBus : IEventBus
{
    private readonly DaprClient dapr;
    private readonly ILogger logger;

    public DaprEventBus(DaprClient dapr, ILogger<DaprEventBus> logger)
    {
        this.dapr = dapr;
        this.logger = logger;
    }

    public async Task PublishAsync<TIntegrationEvent>(string pubSubName, TIntegrationEvent @event)
        where TIntegrationEvent : IntegrationEvent
    {
        var topicName = @event.GetType().Name;

        logger.LogInformation("Publishing event {@Event} to {PubSubName}.{TopicName}", @event,
            pubSubName, topicName);

        // We need to make sure that we pass the concrete type to PublishEventAsync,
        // which can be accomplished by casting the event to dynamic. This ensures
        // that all event fields are properly serialized.
        await dapr.PublishEventAsync(pubSubName, topicName, (object)@event);
    }
}