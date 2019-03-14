using MessageAppInterfaces.Repositories;
using MessageAppModels;
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

        private IMessageAppRepository MessageAppRepository;

        private User CurrentUser;
        private User ExpectedUser;
        
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
