using Core.Types;

namespace SubscriptionService.Model;

public class UnderwritingResult : Enumeration
{
    public static UnderwritingResult Accepted = new UnderwritingResult(10, nameof(Accepted));
    public static UnderwritingResult Pending = new UnderwritingResult(20, nameof(Pending));
    public static UnderwritingResult Rejected = new UnderwritingResult(30, nameof(Rejected));
    
    public UnderwritingResult(int id, string name) : base(id, name)
    {
    }
}