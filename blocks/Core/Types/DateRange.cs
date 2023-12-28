using System;

namespace Core.Types
{
    public record DateRange(DateTime DateStart, DateTime DateEnd)
    {
        public bool IsInRange(DateTime date) => DateStart <= date && date <= DateEnd;
    }
}