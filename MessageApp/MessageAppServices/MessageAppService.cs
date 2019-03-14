using MessageAppInterfaces.Providers;
using MessageAppInterfaces.Repositories;
using MessageAppInterfaces.Services;
using MessageAppModels;
using System;
using System.Collections.Generic;

namespace MessageAppServices
{
    public class MessageAppService : IMessageAppService
    {
        private readonly IMessageAppRepository MessageAppRepository;
        private readonly IDateTimeProvider DateTimeProvider;

        public MessageAppService(IMessageAppRepository messageAppRepository, IDateTimeProvider dateTimeProvider)
        {
            MessageAppRepository = messageAppRepository;
            DateTimeProvider = dateTimeProvider;
        }

        public void FollowUser(string userName, string followedName)
        {
            var currentUser = MessageAppRepository.GetUser(userName);
            var followedUser = MessageAppRepository.GetUser(followedName);

            if (!DoUsersExist(currentUser, followedUser) ||
                IsUserAlreadyFollowed(followedName, currentUser))
            {
                return;
            }

            currentUser.Following.Add(followedName.ToLower());
            MessageAppRepository.UpdateUser(currentUser);
        }

        private static bool DoUsersExist(User currentUser, User followedUser)
        {
            return currentUser != null && followedUser != null;
        }

        private static bool IsUserAlreadyFollowed(string followedName, User currentUser)
        {
            return currentUser.Following.Contains(followedName.ToLower());
        }

        public List<Message> GetMessages(string userName)
        {
            var currentUser = MessageAppRepository.GetUser(userName);
            if (currentUser == null)
            {
                return new List<Message>();
            }

            var userMessages = currentUser.Messages;
            userMessages.Sort((a, b) => a.PostTime.CompareTo(b.PostTime));
            userMessages.Reverse();
            return userMessages;
        }

        public List<WallMessage> GetWall(string userName)
        {
            var currentUser = MessageAppRepository.GetUser(userName);
            if (currentUser == null)
            {
                return new List<WallMessage>();
            }

            var followedUsers = GetFollowedUsers(currentUser.Following);
            return AggregateWallMessagesList(currentUser, followedUsers);
        }

        private List<WallMessage> AggregateWallMessagesList(User currentUser, List<User> followedUsers)
        {
            var wallMessages = new List<WallMessage>();
            wallMessages.AddRange(ConvertUserMessagesToWallMessages(currentUser));
            foreach (var followedUser in followedUsers)
            {
                wallMessages.AddRange(ConvertUserMessagesToWallMessages(followedUser));
            }
            wallMessages.Sort((a, b) => a.PostTime.CompareTo(b.PostTime));
            wallMessages.Reverse();
            return wallMessages;
        }

        private List<WallMessage> ConvertUserMessagesToWallMessages(User user)
        {
            List<WallMessage> wallMessages = new List<WallMessage>();
            foreach(var message in user.Messages)
            {
                wallMessages.Add(new WallMessage(user.Name, message));
            }
            return wallMessages;
        }

        private List<User> GetFollowedUsers(List<string> followedUserNames)
        {
            List<User> followedUsers = new List<User>();
            foreach(var followedUserName in followedUserNames)
            {
                followedUsers.Add(MessageAppRepository.GetUser(followedUserName));
            }
            return followedUsers;
        }

        public void PostMessage(string userName, string message)
        {
            var currentUser = MessageAppRepository.GetUser(userName);
            if (currentUser == null)
            {
                currentUser = CreateNewUserWithMessage(userName, message);
                return;
            }
            currentUser.Messages.Add(new Message() { Contents = message, PostTime = DateTimeProvider.GetUTCTimeNow() });
            MessageAppRepository.UpdateUser(currentUser);
        }

        private User CreateNewUserWithMessage(string userName, string message)
        {
            User currentUser = new User(userName);
            currentUser.Messages.Add(new Message() { Contents = message, PostTime = DateTimeProvider.GetUTCTimeNow() });
            MessageAppRepository.AddUser(currentUser);
            return currentUser;
        }
    }
}
