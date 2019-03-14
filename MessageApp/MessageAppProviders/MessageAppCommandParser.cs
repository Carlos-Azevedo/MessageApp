using MessageAppInterfaces.Providers;
using MessageAppModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageAppProviders
{
    public class MessageAppCommandParser : IMessageAppCommandParser
    {
        public Command TranslateUserInputToCommand(string userInput)
        {
            var trimmedInput = userInput.Trim();
            var splitInput = userInput.Split(new char[] { ' ' }, 2);
            if(splitInput.Length == 1)
            {
                return ParseGetMessagesCommand(splitInput[0]);
            }
            if(splitInput[1].StartsWith("-> "))
            {
                return ParsePostCommand(splitInput[0], splitInput[1].Substring(3));
            }
            if (splitInput[1].StartsWith("follows "))
            {
                return ParseFollowCommand(splitInput[0], splitInput[1].Substring(7));
            }
            if (splitInput[1].StartsWith("wall"))
            {
                return ParseWallCommand(splitInput[0]);
            }
            return null;
        }

        private Command ParseGetMessagesCommand(string userName)
        {
            var command = new Command();
            command.User = userName;
            command.MethodCall = CommandEnums.GetMessages;
            return command;
        }

        private Command ParseWallCommand(string userName)
        {
            var command = new Command();
            command.User = userName;
            command.MethodCall = CommandEnums.GetWall;
            return command;
        }

        private Command ParseFollowCommand(string userName, string followedUser)
        {
            var command = new Command();
            command.User = userName;
            command.MethodCall = CommandEnums.FollowUser;
            command.Argument = followedUser.Trim();
            return command;
        }

        private Command ParsePostCommand(string userName, string message)
        {
            var command = new Command();
            command.User = userName;
            command.MethodCall = CommandEnums.PostMessage;
            command.Argument = message.Trim();
            return command;
        }
    }
}
