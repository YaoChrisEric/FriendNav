using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FriendNav.Core.ViewModels;
using FriendNav.Core.Utilities;
using MvvmCross.Core.Navigation;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Model;
using FriendNav.Core.IntegrationTests.Utilities;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firebase.Auth;
using FriendNavUser = FriendNav.Core.Model.User;
using System.Threading.Tasks;

namespace FriendNav.Core.Tests.ViewModels
{
    [TestClass]
    public class LoginViewModelTests
    {
        private IFixture _fixture = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture()
                .Customize(new AutoConfiguredMoqCustomization());
        }

        [TestMethod]
        public void User_login_and_navigate_to_FriendList()
        {
            var _mvxNavigationService = new Mock<IMvxNavigationService>();
            var _userRepository = new Mock<IUserRepository>();
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();
            var _notificationService = new Mock<INotificationService>();
            var firebaseAuth = _fixture.Create<FirebaseAuth>();
            var user = new FriendNavUser();

            _firebaseAuthService
                .SetupGet(s => s.FirebaseAuth)
                .Returns(firebaseAuth);

            _mvxNavigationService.Setup(s => s.Navigate<FriendListViewModel, FriendNavUser>(It.IsAny<FriendNavUser>(), null))
                .Returns(() => 
                {
                    return Task.Run(() => { });
                });

            _userRepository.Setup(s => s.GetUser(It.IsAny<string>()))
                .Returns(user);

            var sut = new LoginViewModel(
                new TestTask(),
                _mvxNavigationService.Object,
                _userRepository.Object,
                _notificationService.Object,
                _firebaseAuthService.Object
            )
            {
                EmailAddress = firebaseAuth.User.Email,
                UserPassword = "theday"
            };

            sut.LoginUserCommand.Execute();

            _firebaseAuthService.Verify(v => v.LoginUser(It.Is<string>(i => i == sut.EmailAddress), It.Is<string>(i => i == sut.UserPassword)));
            _userRepository.Verify(v => v.GetUser(It.Is<string>(i => i == firebaseAuth.User.Email)));
            _mvxNavigationService.Verify(x => x.Navigate<FriendListViewModel, FriendNavUser>(It.Is<FriendNavUser>(i => i == user), null));
            _notificationService.Verify(x => x.SendNotification(It.Is<string>(i => i == "Invalid username/password, Login Failed")), Times.Never());
        }

        [TestMethod]
        public void User_null_email()
        {
            var _mvxNavigationService = new Mock<IMvxNavigationService>();
            var _userRepository = new Mock<IUserRepository>();
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();
            var _notificationService = new Mock<INotificationService>();
            var firebaseAuth = _fixture.Create<FirebaseAuth>();
            var user = new FriendNavUser();

            _firebaseAuthService
                .SetupGet(s => s.FirebaseAuth)
                .Returns(firebaseAuth);

            _userRepository.Setup(s => s.GetUser(It.IsAny<string>()))
                .Returns(user);

            var sut = new LoginViewModel(
                new TestTask(),
                _mvxNavigationService.Object,
                _userRepository.Object,
                null,
                _firebaseAuthService.Object
            )
            {
                EmailAddress = null,
                UserPassword = "theday"
            };

            sut.LoginUserCommand.Execute();

            _firebaseAuthService.Verify(v => v.LoginUser(It.Is<string>(i => i == sut.EmailAddress), It.Is<string>(i => i == sut.UserPassword)),Times.Never());
            _userRepository.Verify(v => v.GetUser(It.Is<string>(i => i == firebaseAuth.User.Email)),Times.Never());
            _mvxNavigationService.Verify(x => x.Navigate<FriendListViewModel, FriendNavUser>(It.Is<FriendNavUser>(i => i == user), null), Times.Never());
            _notificationService.Verify(x => x.SendNotification(It.Is<string>(i => i == "Invalid username/password, Login Failed")), Times.Never());
        }

        [TestMethod]
        public void User_null_password()
        {
            var _mvxNavigationService = new Mock<IMvxNavigationService>();
            var _userRepository = new Mock<IUserRepository>();
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();
            var _notificationService = new Mock<INotificationService>();
            var firebaseAuth = _fixture.Create<FirebaseAuth>();
            var user = new FriendNavUser();

            _firebaseAuthService
                .SetupGet(s => s.FirebaseAuth)
                .Returns(firebaseAuth);

            _userRepository.Setup(s => s.GetUser(It.IsAny<string>()))
                .Returns(user);

            var sut = new LoginViewModel(
                new TestTask(),
                _mvxNavigationService.Object,
                _userRepository.Object,
                null,
                _firebaseAuthService.Object
            )
            {
                EmailAddress = "c@test.com",
                UserPassword = null
            };

            sut.LoginUserCommand.Execute();

            _firebaseAuthService.Verify(v => v.LoginUser(It.Is<string>(i => i == sut.EmailAddress), It.Is<string>(i => i == sut.UserPassword)),Times.Never());
            _userRepository.Verify(v => v.GetUser(It.Is<string>(i => i == firebaseAuth.User.Email)),Times.Never());
            _mvxNavigationService.Verify(x => x.Navigate<FriendListViewModel, FriendNavUser>(It.Is<FriendNavUser>(i => i == user), null), Times.Never());
            _notificationService.Verify(x => x.SendNotification(It.Is<string>(i => i == "Invalid username/password, Login Failed")), Times.Never());
        }

        [TestMethod]
        public void User_null_email_and_password()
        {
            var _mvxNavigationService = new Mock<IMvxNavigationService>();
            var _userRepository = new Mock<IUserRepository>();
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();
            var _notificationService = new Mock<INotificationService>();
            var firebaseAuth = _fixture.Create<FirebaseAuth>();
            var user = new FriendNavUser();

            _firebaseAuthService
                .SetupGet(s => s.FirebaseAuth)
                .Returns(firebaseAuth);

            _userRepository.Setup(s => s.GetUser(It.IsAny<string>()))
                .Returns(user);

            var sut = new LoginViewModel(
                new TestTask(),
                _mvxNavigationService.Object,
                _userRepository.Object,
                null,
                _firebaseAuthService.Object
            )
            {
                EmailAddress = null,
                UserPassword = null
            };

            sut.LoginUserCommand.Execute();

            _firebaseAuthService.Verify(v => v.LoginUser(It.Is<string>(i => i == sut.EmailAddress), It.Is<string>(i => i == sut.UserPassword)),Times.Never());
            _userRepository.Verify(v => v.GetUser(It.Is<string>(i => i == firebaseAuth.User.Email)),Times.Never());
            _mvxNavigationService.Verify(x => x.Navigate<FriendListViewModel, FriendNavUser>(It.Is<FriendNavUser>(i => i == user), null), Times.Never());
            _notificationService.Verify(x => x.SendNotification(It.Is<string>(i => i == "Invalid username/password, Login Failed")), Times.Never());
        }

        [TestMethod]
        public void User_bad_auth_send_notification()
        {
            var _mvxNavigationService = new Mock<IMvxNavigationService>();
            var _userRepository = new Mock<IUserRepository>();
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();
            var _notificationService = new Mock<INotificationService>();
            var user = new FriendNavUser();

            _firebaseAuthService.SetupGet(s => s.FirebaseAuth);

            _mvxNavigationService.Setup(s => s.Navigate<FriendListViewModel, FriendNavUser>(It.IsAny<FriendNavUser>(), null))
                .Returns(() => 
                {
                    return Task.Run(() => { });
                });

            _userRepository.Setup(s => s.GetUser(It.IsAny<string>()))
                .Returns(user);

            var sut = new LoginViewModel(
                new TestTask(),
                _mvxNavigationService.Object,
                _userRepository.Object,
                _notificationService.Object,
                _firebaseAuthService.Object
            )
            {
                EmailAddress = "some@string.com",
                UserPassword = "theday"
            };

            sut.LoginUserCommand.Execute();

            _firebaseAuthService.Verify(v => v.LoginUser(It.Is<string>(i => i == sut.EmailAddress), It.Is<string>(i => i == sut.UserPassword)));
            _userRepository.Verify(v => v.GetUser(It.Is<string>(i => i == "some@string.com")),Times.Never());
            _mvxNavigationService.Verify(x => x.Navigate<FriendListViewModel, FriendNavUser>(It.Is<FriendNavUser>(i => i == user), null),Times.Never());
            _notificationService.Verify(x => x.SendNotification(It.Is<string>(i => i == "Invalid username/password, Login Failed")));
        }
    }
}
