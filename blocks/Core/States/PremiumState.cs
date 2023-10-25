using System;
using Core.Types;

namespace Core.States
{
    public class PremiumState : Enumeration
    {
        public static PremiumState Issued = new(1, "Issued");
        public static PremiumState Sent = new(2, "Sent");
        public static PremiumState Paid = new(3, "Paid");
        public static PremiumState Cancel = new(4, "Canceled");
        public static PremiumState Refunded = new(5, "Refunded");

        public PremiumState(int id, string name) : base(id, name)
        {
        }
    }
}