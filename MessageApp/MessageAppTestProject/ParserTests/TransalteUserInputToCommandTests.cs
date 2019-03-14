using MessageAppInterfaces.Providers;
using MessageAppModels;
using MessageAppProviders;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;

namespace MessageAppTestProject.ParserTests
{
    [TestFixture]
    public class TransalteUserInputToCommandTests
    {
        private IMessageAppCommandParser MessageAppCommandParser;
        private string UserInput;
        private Command ExpectedParsedCommand;
        private Command ParsedCommand;

        public TransalteUserInputToCommandTests()
        {
            MessageAppCommandParser = new MessageAppCommandParser();
        }

        [Test]
        public void TranslatePostCommand()
        {
            this.Given(_ => GivenAPostMessageCommandIsInput())
                .When(_ => WhenTheInputIsParsed())
                .Then(_ => ThenTheCommandShouldMatchExpected()).BDDfy();
        }

        [Test]
        public void TranslateFollowCommand()
        {
            this.Given(_ => GivenAFollowCommandIsInput())
                .When(_ => WhenTheInputIsParsed())
                .Then(_ => ThenTheCommandShouldMatchExpected()).BDDfy();
        }

        [Test]
        public void TranslateWallCommand()
        {
            this.Given(_ => GivenAWallCommandIsInput())
                .When(_ => WhenTheInputIsParsed())
                .Then(_ => ThenTheCommandShouldMatchExpected()).BDDfy();
        }
        
        [Test]
        public void TranslateGetMessagesCommand()
        {
            this.Given(_ => GivenAGetMessagesCommandIsInput())
                .When(_ => WhenTheInputIsParsed())
                .Then(_ => ThenTheCommandShouldMatchExpected()).BDDfy();
        }

        private void GivenAGetMessagesCommandIsInput()
        {
            UserInput = "Bob";
            ExpectedParsedCommand = new Command()
            {
                User = "Bob",
                MethodCall = CommandEnums.GetMessages
            };
        }

        private void GivenAPostMessageCommandIsInput()
        {
            UserInput = "Bob -> This is my message.  ";
            ExpectedParsedCommand = new Command()
            {
                User = "Bob",
                MethodCall = CommandEnums.PostMessage,
                Argument = "This is my message."
            };
        }

        private void GivenAFollowCommandIsInput()
        {
            UserInput = "Bob follows  Linda ";
            ExpectedParsedCommand = new Command()
            {
                User = "Bob",
                MethodCall = CommandEnums.FollowUser,
                Argument = "Linda"
            };
        }

        private void GivenAWallCommandIsInput()
        {
            UserInput = "Bob wall";
            ExpectedParsedCommand = new Command()
            {
                User = "Bob",
                MethodCall = CommandEnums.GetWall
            };
        }

        private void WhenTheInputIsParsed()
        {
            ParsedCommand = MessageAppCommandParser.TranslateUserInputToCommand(UserInput);
        }

        private void ThenTheCommandShouldMatchExpected()
        {
            ParsedCommand.User.ShouldBe(ExpectedParsedCommand.User);
            ParsedCommand.MethodCall.ShouldBe(ExpectedParsedCommand.MethodCall);
            ParsedCommand.Argument.ShouldBe(ExpectedParsedCommand.Argument);
        }

    }
}
