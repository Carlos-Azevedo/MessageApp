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
    public class FollowUserTests
    {
        private Mock<IMessageAppRepository> MockedRepository;
        private IMessageAppService MessageService;
        private User CurrentUser;
        private User FollowedUser;

        [Test]
        public void FollowAUserWhichExists()
        {
            this.Given(_ => GivenAUserExists())
                .And(_ => GivenTheFollowedUserExists())
                .When(_ => WhenUserFollowsAnother())
                .Then(_ => ThenTheUserShouldBeUpdated());
        }

        [Test]
        public void FollowAUserWhichIsFollowed()
        {
            this.Given(_ => GivenAUserExists())
                .And(_ => GivenTheUserAlreadyFollowsAnother())
                .When(_ => WhenUserFollowsAnother())
                .Then(_ => ThenTheUserShouldNotBeUpdated());
        }

        [Test]
        public void FollowAUserWhichDoesNotExist()
        {
            this.Given(_ => GivenAUserExists())
                .And(_ => GivenTheFollowedUserDoesNotExist())
                .When(_ => WhenUserFollowsAnother())
                .Then(_ => ThenTheUserShouldNotBeUpdated());
        }

        private void GivenTheFollowedUserDoesNotExist()
        {
            FollowedUser = new User("Linda");
            MockedRepository.Setup(mock => mock.GetUser(FollowedUser.Name)).Returns(() => { return null; });
        }

        private void GivenTheUserAlreadyFollowsAnother()
        {
            CurrentUser = new User("Bob");
            CurrentUser.Following.Add("Linda");
            MockedRepository.Setup(mock => mock.GetUser(CurrentUser.Name)).Returns(CurrentUser);
        }

        private void GivenAUserExists()
        {
            CurrentUser = new User("Bob");
            MockedRepository.Setup(mock => mock.GetUser(CurrentUser.Name)).Returns(CurrentUser);
            MockedRepository.Setup(mock => mock.UpdateUser(CurrentUser));
        }

        private void GivenTheFollowedUserExists()
        {
            FollowedUser = new User("Linda");
        }

        private void WhenUserFollowsAnother()
        {
            MessageService.FollowUser(CurrentUser.Name, FollowedUser.Name);
        }

        private void ThenTheUserShouldBeUpdated()
        {
            CurrentUser.Following.Count.ShouldBe(1);
            CurrentUser.Following.ShouldContain(FollowedUser.Name);
        }

        private void ThenTheUserShouldNotBeUpdated()
        {
            CurrentUser.Following.Count.ShouldBe(1);
            CurrentUser.Following.ShouldContain(FollowedUser.Name);
        }
    }
}
