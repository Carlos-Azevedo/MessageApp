using System;
using System.Collections.Generic;
using System.Text;

namespace MessageAppModels
{
    public class WallMessage : Message
    {
        public WallMessage(string user, Message originalMessage)
        {
            User = user;
            Contents = originalMessage.Contents;
            PostTime = originalMessage.PostTime;
        }

        public string User { get; set; }
    }
}
