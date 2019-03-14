using MessageAppInterfaces.Repositories;
using MessageAppInterfaces.Services;
using MessageAppModels;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using TestStack.BDDfy;
namespace MessageAppTestProject.ServiceTests
{
    [TestFixture]
    public class GetMessageTests
    {
        private Mock<IMessageAppRepository> MockedRepostiory;
        private IMessageAppService MessageService;
        List<Message> RetrievedMessages;
        private User CurrentUser;

        [Test]
        public void GetMessagesForUserWithMessages()
        {
            this.Given(_ => GivenAUserHasMessages())
                .When(_ => WhenTheUserMessagesAreRetrieved())
                .Then(_ => ThenNoMessagesShouldBeReturned());
        }

        [Test]
        public void GetMessasgesForUserWithNoMessages()
        {
            this.Given(_ => GivenAUserHasNoMessages())
                .When(_ => WhenTheUserMessagesAreRetrieved())
                .Then(_ => ThenTheUserMessagesShouldBeReturned())
                .And(_ => ThenTheMessagesShouldBeInChronologicalOrder());
        }

        [Test]
        public void GetMessasgesForUserWhichDoesntExist()
        {
            this.Given(_ => GivenAUserDoesntExist())
                .When(_ => WhenTheUserMessagesAreRetrieved())
                .Then(_ => ThenNoMessagesShouldBeReturned());
        }
        private void GivenAUserDoesntExist()
        {
            CurrentUser = new User("Bob");
            MockedRepostiory.Setup(mock => mock.GetUser("Bob")).Returns(() => { return null; });
        }

        private void GivenAUserHasNoMessages()
        {
            CurrentUser = new User("Bob");
            MockedRepostiory.Setup(mock => mock.GetUser("Bob")).Returns(CurrentUser);
        }

        private void GivenAUserHasMessages()
        {
            CurrentUser = new User("Bob");
            CurrentUser.Messages.Add(new Message() { Contents = "Message Contents", PostTime = new DateTime(14546) });
            CurrentUser.Messages.Add(new Message() { Contents = "Message Contents 2", PostTime = new DateTime(700) });
            CurrentUser.Messages.Add(new Message() { Contents = "Message Contents 3", PostTime = new DateTime(9999999) });
            MockedRepostiory.Setup(mock => mock.GetUser("Bob")).Returns(CurrentUser);
        }

        private void WhenTheUserMessagesAreRetrieved()
        {
            RetrievedMessages = MessageService.GetMessages(CurrentUser.Name);
        }

        private void ThenTheUserMessagesShouldBeReturned()
        {
            RetrievedMessages.Count.ShouldBe(CurrentUser.Messages.Count);
            RetrievedMessages.ForEach(retrievedMessage => CurrentUser.Messages.ShouldContain(retrievedMessage));
        }

        private void ThenNoMessagesShouldBeReturned()
        {
            RetrievedMessages.Count.ShouldBe(0);
        }

        private void ThenTheMessagesShouldBeInChronologicalOrder()
        {
            for(int index = 0; index < RetrievedMessages.Count-1; index++)
            {
                RetrievedMessages[index].PostTime.ShouldBeGreaterThan(RetrievedMessages[index + 1].PostTime);
            }
        }
    }
}
