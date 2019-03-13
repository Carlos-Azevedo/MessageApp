using MessageAppModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageAppInterfaces.Services
{
    public interface IMessageAppService
    {
        List<Message> GetMessages(string userName);

        List<WallMessage> GetWall(string userName);

        void PostMessage(string userName, string Message);

        void FollowUser(string userName, string FollowedName);
    }
}
