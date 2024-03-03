namespace SubscriptionService.Domain;

public enum SubscriptionState
{
    Created = 10,
    Registered = 20,
    Normalized = 30,
    Validated = 35, 
    InAnalysis = 40,
    Pending = 50,
    Suspended = 60,
    Accepted = 70,
    Rejected = 80,
    Canceled = 90
}