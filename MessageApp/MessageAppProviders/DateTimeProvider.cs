using MessageAppInterfaces.Providers;
using System;

namespace MessageAppProviders
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUTCTimeNow()
        {
            return DateTime.UtcNow;
        }
    }
}
