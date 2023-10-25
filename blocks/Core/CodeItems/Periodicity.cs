using System;
using Core.Types;

namespace Core.CodeItems
{
    public class Periodicity : Enumeration
    {
        public static Periodicity Undefined = new Periodicity(0, "N/A", "N/A");
        public static Periodicity Daily = new Periodicity(1, "Daily", "DAILY");
        public static Periodicity Weekly = new Periodicity(2, "Weekly", "WEEKLY");
        public static Periodicity Monthly = new Periodicity(3, "Monthly", "MONTHLY");
        public static Periodicity Bimonthly = new Periodicity(4, "Bimonthly", "BIMONTHLY");
        public static Periodicity Quarterly = new Periodicity(5, "Quarterly", "QUARTERLY");
        public static Periodicity Annual = new Periodicity(6, "Annual", "ANNUAL");

        public Periodicity(int id, string name, string reference) : base(id, name)
        {
            Reference = reference;
        }

        public string Reference { get; }
    }
}