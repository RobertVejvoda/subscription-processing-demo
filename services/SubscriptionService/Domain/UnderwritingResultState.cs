namespace SubscriptionService.Domain;

public enum UnderwritingResultState
{
    Registered = 5,
    Accepted = 10,
    Pending = 20,
    InEvaluation = 25,
    Rejected = 30
}