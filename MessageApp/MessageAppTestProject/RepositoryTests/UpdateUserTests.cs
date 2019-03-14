using MessageAppInterfaces.Repositories;
using MessageAppModels;
using MessageAppRepositories;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using TestStack.BDDfy;

namespace MessageAppTestProject.RepositoryTests
{
    [TestFixture]
    public class UpdateUserTests
    {
        private Dictionary<string, User> StoredUsers = new Dictionary<string, User>();

        private IMessageAppRepository MessageAppRepository;

        private User CurrentUser;
        private User UpdatedUser;

        public UpdateUserTests()
        {
            MessageAppRepository = new MessageAppRepository(StoredUsers);
        }

        [SetUp]
        public void SetupTests()
        {
            StoredUsers.Clear();
        }

        [Test]
        public void UpdateAUser()
        {
            this.Given(_ => GivenAnExistingUser())
                .When(_ => WhenTheUserIsUpdated()).
                Then(_ => ThenTheUserShouldBeUpdated()).BDDfy();
        }

        private void GivenAnExistingUser()
        {
            CurrentUser = new User("Linda");
            StoredUsers.Add(CurrentUser.Name.ToLower(), CurrentUser);
        }

        private void WhenTheUserIsUpdated()
        {
            UpdatedUser = new User("Linda")
            {
                Messages = new List<Message>() { new Message() },
                Following = new List<string>() { "bob" }
            };
            MessageAppRepository.UpdateUser(UpdatedUser);
        }

        private void ThenTheUserShouldBeUpdated()
        {
            StoredUsers.ShouldContainKey(UpdatedUser.Name.ToLower());
            StoredUsers[UpdatedUser.Name.ToLower()].ShouldBe(UpdatedUser);
        }
    }
}
