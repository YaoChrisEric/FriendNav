using AutoFixture;
using AutoFixture.AutoMoq;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.ViewModelParameters;
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
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public async Task Search_for_user_command_success()
        {
            var _userRepository = new Mock<IUserRepository>();
            var _chatRepository = new Mock<IChatRepository>();
            var _navigationService = new Mock<IMvxNavigationService>();
            var friendListViewModel = new FriendListViewModel(
                _userRepository.Object,
                _chatRepository.Object,
                _navigationService.Object
                );

            List<User> userList = new List<User>();
            userList.Add(new User());
            userList.Add(new User());
            userList.Add(new User());

            _userRepository.Setup(x => x.FindUsers(It.IsAny<string>())).Returns(Task.Run(() => userList));

            await friendListViewModel.SearchForUser();

            _userRepository.Verify(x => x.FindUsers(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task Add_user_to_friend_list_command_success()
        {
            var _userRepository = new Mock<IUserRepository>();
            User user = new User();
            var friendListViewModel = new FriendListViewModel(
                _userRepository.Object,
                null,
                null 
                );

            await friendListViewModel.PrepareAsync(user);
            friendListViewModel.SelectedNewFriend = new UserViewModel(user);
            friendListViewModel.SelectedNewFriend.EmailAddress = "test@test.com";;

            await friendListViewModel.AddUserToFriendList();

            _userRepository.Verify(x => x.AddUserToFriendList(It.IsAny<User>(), It.IsAny<Friend>()));
        }

        [TestMethod]
        public async Task Navigate_to_chat_command_success()
        {
            var _userRepository = new Mock<IUserRepository>();
            var _chatRepository = new Mock<IChatRepository>();
            var _navigationService = new Mock<IMvxNavigationService>();
            var friendListViewModel = new FriendListViewModel(
                _userRepository.Object,
                _chatRepository.Object,
                _navigationService.Object
                );

            friendListViewModel.SelectedFriend = new FriendViewModel(new Friend());

            await friendListViewModel.NavigateToSelectedFriendChat();

            _userRepository.Verify(x => x.GetUser(It.IsAny<string>()));
            _chatRepository.Verify(x => x.GetChat(It.IsAny<User>(), It.IsAny<User>()));
            _navigationService.Verify(x => x.Navigate<ChatViewModel, ChatParameters>(It.IsAny<ChatParameters>(), null));
        }

        [TestMethod]
        public async Task Navigate_to_chat_command_null_selected_friend()
        {
            var _userRepository = new Mock<IUserRepository>();
            var _chatRepository = new Mock<IChatRepository>();
            var _navigationService = new Mock<IMvxNavigationService>();
            var friendListViewModel = new FriendListViewModel(
                _userRepository.Object,
                _chatRepository.Object,
                _navigationService.Object
                );

            await friendListViewModel.NavigateToSelectedFriendChat();

            _userRepository.Verify(x => x.GetUser(It.IsAny<string>()),Times.Never());
            _chatRepository.Verify(x => x.GetChat(It.IsAny<User>(), It.IsAny<User>()),Times.Never());
            _navigationService.Verify(x => x.Navigate<ChatViewModel, ChatParameters>(It.IsAny<ChatParameters>(), null),Times.Never());
        }
    }
}
