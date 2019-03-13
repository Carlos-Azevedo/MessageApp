using System;
using System.Collections.Generic;
using System.Text;

namespace MessageAppInterfaces.Providers
{
    public interface IDateTimeProvider
    {
        DateTime GetUTCTimeNow();
    }
}
