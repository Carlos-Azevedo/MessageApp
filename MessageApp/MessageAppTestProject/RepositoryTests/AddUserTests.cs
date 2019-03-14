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
    public class AddUserTests
    {
        private Dictionary<string, User> StoredUsers = new Dictionary<string, User>();

        private readonly IMessageAppRepository MessageAppRepository;

        private User CurrentUser;
        
        public AddUserTests()
        {
            MessageAppRepository = new MessageAppRepository(StoredUsers);
        }

        [SetUp]
        public void SetupTests()
        {
            StoredUsers.Clear();
        }

        [Test]
        public void AddANewUser()
        {
            this.Given(_ => GivenANonExistingUser())
                .When(_ => WhenTheUserIsAdded()).
                Then(_ => ThenTheUserShouldBeSaved()).BDDfy();
        }

        private void GivenANonExistingUser()
        {
            CurrentUser = new User("Linda");
            StoredUsers.Clear();
        }

        private void WhenTheUserIsAdded()
        {
            MessageAppRepository.AddUser(CurrentUser);
        }
        
        private void ThenTheUserShouldBeSaved()
        {
            StoredUsers.ShouldContainKey(CurrentUser.Name.ToLower());
            StoredUsers[CurrentUser.Name.ToLower()].ShouldBe(CurrentUser);
        }
    }
}
