using MessageAppInterfaces.Providers;
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
    public class PostMessageTests
    {
        private Mock<IMessageAppRepository> MockedRepository;
        private Mock<IDateTimeProvider> MockedDateTimeProvider;
        private IMessageAppService MessageService;
        private User CurrentUser;
        private string PostedMessage;

        [Test]
        public void PostMessageForExistingUser()
        {
            this.Given(_ => GivenAUserExists())
                .When(_ => WhenAMessageIsPosted())
                .Then(_ => ThenTheUserShouldHaveThatMessageAdded());
        }
        [Test]
        public void PostMessageForANewUser()
        {
            this.Given(_ => GivenAUserDoesNotExist())
                .When(_ => WhenAMessageIsPostedByANewUser())
                .Then(_ => ThenTheUserShouldHaveThatMessageAdded());
        }
        private void GivenAUserDoesNotExist()
        {
            CurrentUser = new User("Bob");
            MockedRepository.Setup(mock => mock.GetUser("Bob")).Returns(() => { return null; });
        }

        private void GivenAUserExists()
        {
            CurrentUser = new User("Bob");
            MockedRepository.Setup(mock => mock.GetUser("Bob")).Returns(CurrentUser);
        }

        private void WhenAMessageIsPosted()
        {
            PostedMessage = "A New Message";
            MockedDateTimeProvider.Setup(mock => mock.GetUTCTimeNow()).Returns(new DateTime(100));
            MockedRepository.Setup(mock => mock.GetUser(CurrentUser.Name)).Returns(CurrentUser);
            MockedRepository.Setup(mock => mock.UpdateUser(CurrentUser));

            MessageService.PostMessage(CurrentUser.Name, PostedMessage);
        }

        private void WhenAMessageIsPostedByANewUser()
        {
            PostedMessage = "A New Message";
            MockedDateTimeProvider.Setup(mock => mock.GetUTCTimeNow()).Returns(new DateTime(100));
            MockedRepository.Setup(mock => mock.AddUser(CurrentUser));

            MessageService.PostMessage(CurrentUser.Name, PostedMessage);
        }

        private void ThenTheUserShouldHaveThatMessageAdded()
        {
            CurrentUser.Messages.Count.ShouldBe(1);
            CurrentUser.Messages[0].Contents.ShouldBe(PostedMessage);
            CurrentUser.Messages[0].PostTime.ShouldBe(new DateTime(100));
        }
    }
}
