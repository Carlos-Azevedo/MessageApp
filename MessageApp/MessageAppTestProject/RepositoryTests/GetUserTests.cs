using MessageAppInterfaces.Repositories;
using MessageAppModels;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using TestStack.BDDfy;
namespace MessageAppTestProject.RepositoryTests
{
    [TestFixture]
    public class GetUserTests
    {
        private Dictionary<string, User> StoredUsers;

        private IMessageAppRepository MessageAppRepository;

        private string CurrentUser;
        private User ReturnedUser;
        private User ExpectedUser;

        [Test]
        public void RetrieveAnExistingUser()
        {
            this.Given(_ => GivenAnExistingUser())
                .When(_ => WhenTheUserIsRetrieved()).
                Then(_ => ThenTheUserShouldBeReturned()).BDDfy();
        }

        [Test]
        public void RetrieveAnNonExistingUser()
        {
            this.Given(_ => GivenANonExistingUser())
                .When(_ => WhenTheUserIsRetrieved()).
                Then(_ => ThenNullShouldBeReturned()).BDDfy();
        }

        private void GivenANonExistingUser()
        {
            CurrentUser = "Linda";
            ExpectedUser = null;
            StoredUsers.Clear();
        }

        private void GivenAnExistingUser()
        {
            CurrentUser = "Bob";
            ExpectedUser = new User(CurrentUser);
            StoredUsers.Add(CurrentUser.ToLower(), ExpectedUser);
        }

        private void WhenTheUserIsRetrieved()
        {
            ReturnedUser = MessageAppRepository.GetUser(CurrentUser.ToUpper());
        }

        private void ThenTheUserShouldBeReturned()
        {
            ReturnedUser.ShouldBe(ExpectedUser);
        }

        private void ThenNullShouldBeReturned()
        {
            ReturnedUser.ShouldBeNull();
        }

    }
}
