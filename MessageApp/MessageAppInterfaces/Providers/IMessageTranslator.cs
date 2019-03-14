using MessageAppModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageAppInterfaces.Providers
{
    public interface IMessageTranslator
    {
        string ToReadString(Message message, DateTime currentDateTime);
        string ToWallString(WallMessage message, DateTime currentDateTime);
    }
}
