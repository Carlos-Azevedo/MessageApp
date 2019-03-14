using MessageAppInterfaces.Providers;
using MessageAppInterfaces.Services;
using MessageAppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp
{
    public class MessageAppController
    {
        private readonly IMessageAppService MessageAppService;
        private readonly IMessageAppCommandParser CommandParser;
        private readonly IMessageTranslator MessageTranslator;
        private readonly IDateTimeProvider DateTimeProvider;

        public MessageAppController(IMessageAppService mesageAppService, IMessageAppCommandParser commandParser, IMessageTranslator messageTranslator, IDateTimeProvider dateTimeProvider)
        {
            MessageAppService = mesageAppService;
            CommandParser = commandParser;
            MessageTranslator = messageTranslator;
            DateTimeProvider = dateTimeProvider;
        }

        public void ExecuteCommand(string userInput)
        {
            var command = CommandParser.TranslateUserInputToCommand(userInput);
            if(command == null)
            {
                return;
            }
            switch(command.MethodCall)
            {
                case CommandEnums.FollowUser:
                    RunFollowCommand(command);
                    break;
                case CommandEnums.GetMessages:
                    RunGetMessagesCommand(command);
                    break;
                case CommandEnums.GetWall:
                    RunWallCommand(command);
                    break;
                case CommandEnums.PostMessage:
                    RunPostMessageCommand(command);
                    break;
            }
        }

        private void RunPostMessageCommand(Command command)
        {
            MessageAppService.PostMessage(command.User, command.Argument);
        }

        private void RunWallCommand(Command command)
        {
            var wallMessages = MessageAppService.GetWall(command.User);
            foreach (var message in wallMessages)
            {
                var translation = MessageTranslator.ToWallString(message, DateTimeProvider.GetUTCTimeNow());
                Console.WriteLine(translation);
            }
        }

        private void RunGetMessagesCommand(Command command)
        {
            var messages = MessageAppService.GetMessages(command.User);
            foreach(var message in messages)
            {
                var translation = MessageTranslator.ToReadString(message, DateTimeProvider.GetUTCTimeNow());
                Console.WriteLine(translation);
            }
        }

        private void RunFollowCommand(Command command)
        {
            MessageAppService.FollowUser(command.User, command.Argument);
        }
    }                               
}                                   

