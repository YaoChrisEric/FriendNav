using Autofac;
using FriendNav.Core.IntegrationTests.TestModel;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.ViewModelParameters;
using FriendNav.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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

            userRepository.Dispose();
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

            userRepository.Dispose();
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

            friendListViewModel.SelectedNewFriend = new UserViewModel(newFriend);

            var testHook = new FriendListViewModelHook();

            friendListViewModel.TestHook = testHook;
            testHook.EmailAddress = newFriend.EmailAddress;
            testHook.ViewModel = friendListViewModel;

            friendListViewModel.AddUserToFriendListCommand.Execute();

            testHook.ResetEvent.WaitOne();

            Assert.IsTrue(friendListViewModel.FriendList.Any(a => a.EmailAddress == newFriend.EmailAddress));

            foreach (var friend in user.FriendList.Where(w => w.EmailAddress == "christest10@test.com").ToArray())
            {
                userRepository.RemoveUserFromFriendList(user, friend);
            }

            userRepository.Dispose();
        }

        [TestMethod]
        public void Navigate_to_chat_viewmodel()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var friendListViewModel = context.TestContainer.Resolve<FriendListViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var user = userRepository.GetUser("c@test.com");

            friendListViewModel.Prepare(user);

            friendListViewModel.SelectedFriend = friendListViewModel.FriendList.First();

            friendListViewModel.NavigateToChatCommand.Execute();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(f => f.Parameter is ChatParameters));

            userRepository.Dispose();
        }
    }
}
