using Autofac;
using FriendNav.Core.IntegrationTests.TestModel;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.ViewModels
{
    [TestClass]
    public class FriendListViewModelTests
    {
        public FriendListViewModelTests()
        {
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Upon_navigating_to_friendlist_load_friends()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var friendListViewModel = context.TestContainer.Resolve<FriendListViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var user = userRepository.GetUser("c@test.com");

            friendListViewModel.Prepare(user);

            Assert.AreNotEqual(0, friendListViewModel.FriendList.Count);
            Assert.IsTrue(friendListViewModel.FriendList.Any(a => a.EmailAddress == "c1@test.com"));
        }

        [TestMethod]
        public void Search_for_user_with_search_text()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var friendListViewModel = context.TestContainer.Resolve<FriendListViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var user = userRepository.GetUser("c@test.com");

            friendListViewModel.Prepare(user);

            friendListViewModel.UserSearchText = "chris";

            friendListViewModel.SearchForUserCommand.Execute();

            Assert.AreNotEqual(0, friendListViewModel.SearchedUsers.Count);
        }

        [TestMethod]
        public void Add_selected_user_to_friendList()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var friendListViewModel = context.TestContainer.Resolve<FriendListViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var user = userRepository.GetUser("c@test.com");

            friendListViewModel.Prepare(user);

            var newFriend = userRepository.GetUser("christest10@test.com");

            friendListViewModel.SelectedNewFriend = newFriend;

            friendListViewModel.AddUserToFriendListCommand.Execute();

            Assert.IsTrue(friendListViewModel.FriendList.Any(a => a.EmailAddress == newFriend.EmailAddress));

            foreach (var friend in user.FriendList.Where(w => w.EmailAddress == "christest10@test.com").ToArray())
            {
                userRepository.RemoveUserFromFriendList(user, friend);
            }
        }
    }
}
