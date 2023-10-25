using System;

namespace Core.Providers
{
    public class UtcDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now() => DateTime.UtcNow;
    }
}