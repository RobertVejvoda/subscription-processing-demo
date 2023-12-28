using System.ComponentModel.DataAnnotations;
using EventBus.Abstractions;

namespace ODSService.Events;

public record CustomerRegisteredIntegrationEvent(
    [Required] string CustomerId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    [Required] string Status,
    [Required] DateOnly BirthDate) : IntegrationEvent;