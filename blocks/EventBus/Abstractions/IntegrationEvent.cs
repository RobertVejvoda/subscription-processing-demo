using System.ComponentModel.DataAnnotations;

namespace EventBus.Abstractions;

public record IntegrationEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
}