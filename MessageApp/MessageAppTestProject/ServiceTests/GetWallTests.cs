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
    public class GetWallTests
    {
        private Mock<IMessageAppRepository> MockedRepostiory;
        private IMessageAppService MessageService;
        List<WallMessage> RetrievedMessages;
        List<WallMessage> ExpectedMessages;
        private User CurrentUser;
        private User FollowedUser;
        private User AnotherFollowedUser;

        [Test]
        public void GetWallForUserWithFollows()
        {
            this.Given(_ => GivenAUserHasFollows())
                .When(_ => WhenTheUserWallMessagesAreRetrieved())
                .Then(_ => ThenExpectedMessagesShouldBeReturned())
                .And(_ => ThenRetrievedMessagesShouldBeInChronologicalOrder());
        }

        private void GivenAUserHasFollows()
        {
            CurrentUser = new User("Bob");
            CurrentUser.Messages.Add(new Message() { Contents = "Bob Message", PostTime = new DateTime(350) });
            FollowedUser = new User("Linda");
            FollowedUser.Messages.Add(new Message() { Contents = "Linda Message", PostTime = new DateTime(10) });
            AnotherFollowedUser = new User("Joe");
            AnotherFollowedUser.Messages.Add(new Message() { Contents = "Joe Message", PostTime = new DateTime(300) });

            ExpectedMessages = new List<WallMessage>()
            {
                new WallMessage(CurrentUser.Name, CurrentUser.Messages[0]),
                new WallMessage(FollowedUser.Name, FollowedUser.Messages[0]),
                new WallMessage(AnotherFollowedUser.Name, AnotherFollowedUser.Messages[0])
            };
        }

        private void WhenTheUserWallMessagesAreRetrieved()
        {
            MockedRepostiory.Setup(mock => mock.GetUser(CurrentUser.Name)).Returns(CurrentUser);
            MockedRepostiory.Setup(mock => mock.GetUser(FollowedUser.Name)).Returns(FollowedUser);
            MockedRepostiory.Setup(mock => mock.GetUser(AnotherFollowedUser.Name)).Returns(AnotherFollowedUser);
            RetrievedMessages = MessageService.GetWall(CurrentUser.Name);
        }

        private void ThenRetrievedMessagesShouldBeInChronologicalOrder()
        {
            for (int index = 0; index < RetrievedMessages.Count - 1; index++)
            {
                RetrievedMessages[index].PostTime.ShouldBeGreaterThan(RetrievedMessages[index + 1].PostTime);
            }
        }

        private void ThenExpectedMessagesShouldBeReturned()
        {
            RetrievedMessages.Count.ShouldBe(ExpectedMessages.Count);
            foreach (var expectedMessage in ExpectedMessages)
            {
                RetrievedMessages.ShouldContain(
                    retrieved => retrieved.Contents == expectedMessage.Contents &&
                    retrieved.User == expectedMessage.User &&
                    retrieved.PostTime == expectedMessage.PostTime
                );
            };
        }

    }
}
