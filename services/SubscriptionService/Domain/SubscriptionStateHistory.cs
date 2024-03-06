namespace SubscriptionService.Domain;

public record SubscriptionStateHistory(SubscriptionState State, DateTime ChangedOn);