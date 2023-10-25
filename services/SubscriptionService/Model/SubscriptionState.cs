using Core.Types;

namespace SubscriptionService.Model;

public class SubscriptionState : Enumeration
{
    public static SubscriptionState Created = new SubscriptionState(10, nameof(Created));
    public static SubscriptionState Registered = new SubscriptionState(20, nameof(Registered));
    public static SubscriptionState Normalized = new SubscriptionState(30, nameof(Normalized));
    public static SubscriptionState InAnalysis = new SubscriptionState(40, "In analysis");
    public static SubscriptionState Pending = new SubscriptionState(50, nameof(Pending));
    public static SubscriptionState Suspended = new SubscriptionState(60, nameof(Suspended));
    public static SubscriptionState Accepted = new SubscriptionState(70, nameof(Accepted));
    public static SubscriptionState Rejected = new SubscriptionState(80, nameof(Rejected));
    public static SubscriptionState Canceled = new SubscriptionState(90, nameof(Canceled));

    private SubscriptionState(int id, string name) : base(id, name)
    {
    }
}