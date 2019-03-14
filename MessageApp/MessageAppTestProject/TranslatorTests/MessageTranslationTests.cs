using MessageAppInterfaces.Providers;
using MessageAppModels;
using MessageAppProviders;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;

namespace MessageAppTestProject.TranslatorTests
{
    [TestFixture]
    public class MessageTranslationTests
    {
        private IMessageTranslator MessageTranslator;

        private Message MessageToBeTranslated;
        private WallMessage WallMessageToBeTranslated;
        private DateTime CurrentTime;
        private string ExpectedTranslation;
        private string TranslatedMessage;

        public MessageTranslationTests()
        {
            MessageTranslator = new MessageTranslator();
        }

        [Test]
        public void TranslateMessageFromSecondsAgo()
        {
            this.Given(_ => GivenAMessageFromSecondsAgo())
                .When(_ => WhenTheMessageIsTranslated())
                .Then(_ => ThenTranslatedMessageShouldBeExpected()).BDDfy();
        }

        [Test]
        public void TranslateMessageFromOneSecondAgo()
        {
            this.Given(_ => GivenAMessageFromOneSecondAgo())
                .When(_ => WhenTheMessageIsTranslated())
                .Then(_ => ThenTranslatedMessageShouldBeExpected()).BDDfy();
        }

        private void GivenAMessageFromOneSecondAgo()
        {
            MessageToBeTranslated = new Message();
            MessageToBeTranslated.Contents = "Contents of the Message";
            MessageToBeTranslated.PostTime = new DateTime(1000);
            CurrentTime = MessageToBeTranslated.PostTime.AddSeconds(1);
            ExpectedTranslation = "Contents of the Message (1 second ago)";
        }

        [Test]
        public void TranslateMessageFromMinutesAgo()
        {
            this.Given(_ => GivenAMessageFromMinutesAgo())
                .When(_ => WhenTheMessageIsTranslated())
                .Then(_ => ThenTranslatedMessageShouldBeExpected()).BDDfy();
        }

        [Test]
        public void TranslateMessageFromHoursAgo()
        {
            this.Given(_ => GivenAMessageFromHoursAgo())
                .When(_ => WhenTheMessageIsTranslated())
                .Then(_ => ThenTranslatedMessageShouldBeExpected()).BDDfy();
        }

        [Test]
        public void TranslateWallMessageFromSecondsAgo()
        {
            this.Given(_ => GivenAWallMessageFromHoursAgo())
                .When(_ => WhenTheWallMessageIsTranslated())
                .Then(_ => ThenTranslatedMessageShouldBeExpected()).BDDfy();
        }

        private void GivenAWallMessageFromHoursAgo()
        {
            var message = new Message();
            message.Contents = "Contents of the Message";
            message.PostTime = new DateTime(1000);
            WallMessageToBeTranslated = new WallMessage("Bob", message);
            CurrentTime = message.PostTime.AddHours(10);

            ExpectedTranslation = "Bob - Contents of the Message (10 hours ago)";
        }

        private void GivenAMessageFromSecondsAgo()
        {
            MessageToBeTranslated = new Message();
            MessageToBeTranslated.Contents = "Contents of the Message";
            MessageToBeTranslated.PostTime = new DateTime(1000);
            CurrentTime = MessageToBeTranslated.PostTime.AddSeconds(10);
            ExpectedTranslation = "Contents of the Message (10 seconds ago)";
        }

        private void GivenAMessageFromMinutesAgo()
        {
            MessageToBeTranslated = new Message();
            MessageToBeTranslated.Contents = "Contents of the Message";
            MessageToBeTranslated.PostTime = new DateTime(1000);
            CurrentTime = MessageToBeTranslated.PostTime.AddMinutes(10);
            ExpectedTranslation = "Contents of the Message (10 minutes ago)";
        }

        private void GivenAMessageFromHoursAgo()
        {
            MessageToBeTranslated = new Message();
            MessageToBeTranslated.Contents = "Contents of the Message";
            MessageToBeTranslated.PostTime = new DateTime(1000);
            CurrentTime = MessageToBeTranslated.PostTime.AddHours(10);
            ExpectedTranslation = "Contents of the Message (10 hours ago)";
        }

        private void WhenTheMessageIsTranslated()
        {
            TranslatedMessage = MessageTranslator.ToReadString(MessageToBeTranslated, CurrentTime);
        }

        private void WhenTheWallMessageIsTranslated()
        {
            TranslatedMessage = MessageTranslator.ToWallString(WallMessageToBeTranslated, CurrentTime);
        }

        private void ThenTranslatedMessageShouldBeExpected()
        {
            TranslatedMessage.ShouldBe(ExpectedTranslation);
        }
    }
}
