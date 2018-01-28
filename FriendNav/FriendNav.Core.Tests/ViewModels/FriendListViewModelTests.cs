using AutoFixture;
using AutoFixture.AutoMoq;
using FriendNav.Core.IntegrationTests.Utilities;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Core.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Tests.ViewModels
{
    [TestClass]
    public class FriendListViewModelTests
    {
        private IFixture _fixture = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture()
                .Customize(new AutoConfiguredMoqCustomization());
        }

        [TestMethod]
        public void Search_for_user_command_success()
        {
            var _userRepository = new Mock<IUserRepository>();
            var _chatRepository = new Mock<IChatRepository>();
            var _navigationService = new Mock<IMvxNavigationService>();
            var friendListViewModel = new FriendListViewModel(
                new TestTask(),
                _userRepository.Object,
                _chatRepository.Object,
                _navigationService.Object
                );

            List<User> userList = new List<User>();
            userList.Add(new User());
            userList.Add(new User());
            userList.Add(new User());

            _userRepository.Setup(x => x.FindUsers(It.IsAny<string>())).Returns(userList);

            friendListViewModel.SearchForUserCommand.Execute();

            _userRepository.Verify(x => x.FindUsers(It.IsAny<string>()));
        }

        [TestMethod]
        public void Add_user_to_friend_list_command_success()
        {
            var _userRepository = new Mock<IUserRepository>();
            User _user = new User();
            var friendListViewModel = new FriendListViewModel(
                new TestTask(),
                _userRepository.Object,
                null,
                null 
                );

            friendListViewModel.Prepare(_user);
            friendListViewModel.SelectedNewFriend = new UserViewModel(_user);
            friendListViewModel.SelectedNewFriend.EmailAddress = "test@test.com";;

            friendListViewModel.AddUserToFriendListCommand.Execute();

            _userRepository.Verify(x => x.AddUserToFriendList(It.IsAny<User>(), It.IsAny<Friend>()));
        }

        [TestMethod]
        public void Navigate_to_chat_command_success()
        {
            var _userRepository = new Mock<IUserRepository>();
            var _chatRepository = new Mock<IChatRepository>();
            var _navigationService = new Mock<IMvxNavigationService>();
            var friendListViewModel = new FriendListViewModel(
                new TestTask(),
                _userRepository.Object,
                _chatRepository.Object,
                _navigationService.Object
                );

            friendListViewModel.SelectedFriend = new FriendViewModel(new Friend());

            friendListViewModel.NavigateToChatCommand.Execute();

            _userRepository.Verify(x => x.GetUser(It.IsAny<string>()));
            _chatRepository.Verify(x => x.GetChat(It.IsAny<User>(), It.IsAny<User>()));
            _navigationService.Verify(x => x.Navigate<ChatViewModel, Chat>(It.IsAny<Chat>(), null));
        }

        [TestMethod]
        public void Navigate_to_chat_command_null_selected_friend()
        {
            var _userRepository = new Mock<IUserRepository>();
            var _chatRepository = new Mock<IChatRepository>();
            var _navigationService = new Mock<IMvxNavigationService>();
            var friendListViewModel = new FriendListViewModel(
                new TestTask(),
                _userRepository.Object,
                _chatRepository.Object,
                _navigationService.Object
                );

            friendListViewModel.NavigateToChatCommand.Execute();

            _userRepository.Verify(x => x.GetUser(It.IsAny<string>()),Times.Never());
            _chatRepository.Verify(x => x.GetChat(It.IsAny<User>(), It.IsAny<User>()),Times.Never());
            _navigationService.Verify(x => x.Navigate<ChatViewModel, Chat>(It.IsAny<Chat>(), null),Times.Never());
        }
    }
}
