using Core.Types;

namespace SubscriptionService.Model;

public class UnderwritingResultState : Enumeration
{
    public static UnderwritingResultState Accepted = new(10, nameof(Accepted));
    public static UnderwritingResultState Pending = new(20, nameof(Pending));
    public static UnderwritingResultState Rejected = new(30, nameof(Rejected));
    
    public UnderwritingResultState(int id, string name) : base(id, name)
    {
    }
}