using System;

namespace Core.Types
{
    public class DateRange
    {
        public DateRange(DateTime dateStart, DateTime dateEnd)
        {
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public DateTime DateStart { get; }
        public DateTime DateEnd { get; }
    }
}