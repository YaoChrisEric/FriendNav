using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FriendNav.Core.ViewModels;
using FriendNav.Core.Utilities;
using MvvmCross.Core.Navigation;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Model;

namespace FriendNav.Core.Tests.ViewModels
{
    [TestClass]
    public class LoginViewModelTests
    {
        [TestMethod]
        public void User_login_and_navigate_to_FriendList()
        {
            var _task = new Mock<ITask>();
            var _mvxNavigationService = new Mock<IMvxNavigationService>();
            var _userRepository = new Mock<IUserRepository>();
            var _notificationService = new Mock<INotificationService>();
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();

            var _chatRepository = new Mock<IChatRepository>(); 
            var _mvxNavigationServiceFL = new Mock<IMvxNavigationService>();

            // _mvxNavigationService.Setup(x => x.Navigate<FriendListViewModel>())));
            _firebaseAuthService.Setup(x => x.LoginUser("", "")).Raises(_firebaseAuthService.Object.FirebaseAuth = new Firebase.Auth.FirebaseAuth);

            var loginViewModel = new LoginViewModel(
                _task.Object,
                _mvxNavigationService.Object,
                _userRepository.Object,
                _notificationService.Object,
                _firebaseAuthService.Object
            );

            loginViewModel.EmailAddress = "c@test.com";
            loginViewModel.UserPassword = "theday";

            loginViewModel.LoginUserCommand.Execute();
            _mvxNavigationService.Verify(x => x.Navigate<FriendListViewModel, User>(It.IsAny<User>(), null));
        }
    }
}
