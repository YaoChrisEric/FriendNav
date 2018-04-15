using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.ViewModels;
using Moq;
using Firebase.Auth;

using MvvmCross.Core.Navigation;
using FriendNav.Core.Model;
using FriendNav.Core.Utilities;
using FriendNav.Core.IntegrationTests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace FriendNav.Core.Tests.ViewModels
{
    [TestClass]
    public class RegisterViewModelTests
    {
        private IFixture _fixture = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture()
                .Customize(new AutoConfiguredMoqCustomization());
        }

        [TestMethod]
        public async Task Register_New_User()
        { 
            var _userRepository = new Mock<IUserRepository>();
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();
            var _mvxNavigationService = new Mock<IMvxNavigationService>();
            var _notificationService = new Mock<INotificationService>();
            var firebaseAuth = _fixture.Create<FirebaseAuth>();
            var user = new Model.User()
            {
                EmailAddress = "testemail"
            };
            _firebaseAuthService
                .SetupGet(s => s.FirebaseAuth)
                .Returns(firebaseAuth);

            var sut = new RegisterViewModel(
                            _mvxNavigationService.Object,
                            _notificationService.Object,
                             _firebaseAuthService.Object,
                             _userRepository.Object
                            )
            {
                EmailAddress = firebaseAuth.User.Email,
                UserPassword = "pwd"
            };

            await sut.RegisterUser();

            _firebaseAuthService.Verify(v => v.CreateNewUser(It.IsAny<string>(), It.IsAny<string>()));
            _userRepository.Verify(v => v.CreateUser(It.IsAny<Model.User>()));
            _notificationService.Verify(x => x.SendNotification(It.Is<string>(i => i == "Failed to create account")), Times.Never());

        }
    }
}
