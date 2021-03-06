﻿using MessageAppInterfaces.Repositories;
using MessageAppInterfaces.Services;
using MessageAppModels;
using MessageAppServices;
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

        public FollowUserTests()
        {
            MockRepository mockRepository = new MockRepository(MockBehavior.Strict);
            MockedRepository = mockRepository.Create<IMessageAppRepository>();
            MessageService = new MessageAppService(MockedRepository.Object, null);
        }

        [Test]
        public void FollowAUserWhichExists()
        {
            this.Given(_ => GivenAUserExists())
                .And(_ => GivenTheFollowedUserExists())
                .When(_ => WhenUserFollowsAnother())
                .Then(_ => ThenTheUserShouldBeUpdated()).BDDfy();
        }

        [Test]
        public void FollowAUserWhichIsFollowed()
        {
            this.Given(_ => GivenAUserExists())
                .And(_ => GivenTheUserAlreadyFollowsAnother())
                .When(_ => WhenUserFollowsAnother())
                .Then(_ => ThenTheUserShouldNotBeUpdated()).BDDfy();
        }

        [Test]
        public void FollowAUserWhichDoesNotExist()
        {
            this.Given(_ => GivenAUserExists())
                .And(_ => GivenTheFollowedUserDoesNotExist())
                .When(_ => WhenUserFollowsAnother())
                .Then(_ => ThenTheUserShouldNotBeUpdatedAndBeEmpty()).BDDfy();
        }

        private void GivenTheFollowedUserDoesNotExist()
        {
            FollowedUser = new User("Linda");
            MockedRepository.Setup(mock => mock.GetUser(FollowedUser.Name)).Returns(() => { return null; });
        }

        private void GivenTheUserAlreadyFollowsAnother()
        {
            CurrentUser = new User("Bob");
            FollowedUser = new User("Linda");
            CurrentUser.Following.Add("linda");
            MockedRepository.Setup(mock => mock.GetUser(CurrentUser.Name)).Returns(CurrentUser);
            MockedRepository.Setup(mock => mock.GetUser(FollowedUser.Name)).Returns(FollowedUser);
        }

        private void GivenAUserExists()
        {
            CurrentUser = new User("Bob");
            MockedRepository.Setup(mock => mock.GetUser(CurrentUser.Name)).Returns(CurrentUser);
        }

        private void GivenTheFollowedUserExists()
        {
            FollowedUser = new User("Linda");
            MockedRepository.Setup(mock => mock.GetUser(FollowedUser.Name)).Returns(FollowedUser);
            MockedRepository.Setup(mock => mock.UpdateUser(CurrentUser));
        }

        private void WhenUserFollowsAnother()
        {
            MessageService.FollowUser(CurrentUser.Name, FollowedUser.Name);
        }

        private void ThenTheUserShouldBeUpdated()
        {
            CurrentUser.Following.Count.ShouldBe(1);
            CurrentUser.Following.ShouldContain(FollowedUser.Name.ToLower());
        }

        private void ThenTheUserShouldNotBeUpdatedAndBeEmpty()
        {
            CurrentUser.Following.Count.ShouldBe(0);
        }

        private void ThenTheUserShouldNotBeUpdated()
        {
            CurrentUser.Following.Count.ShouldBe(1);
            CurrentUser.Following.ShouldContain(FollowedUser.Name.ToLower());
        }
    }
}
