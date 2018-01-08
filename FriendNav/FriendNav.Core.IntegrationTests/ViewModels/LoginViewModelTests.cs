using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FriendNav.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Core.Navigation;
using Moq;
using Autofac;
using FriendNav.Core.Repositories;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Services;
using FriendNav.Core.IntegrationTests.TestModel;
using FriendNav.Core.Model;

namespace FriendNav.Core.IntegrationTests.ViewModels
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class LoginViewModelTests
    {
        public LoginViewModelTests()
        {
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void User_login_and_navigate_to_FriendList()
        {
            var context = ConstructTestAppContext();

            var loginViewModel = context.TestContainer.Resolve<LoginViewModel>();

            loginViewModel.EmailAddress = "c@test.com";

            loginViewModel.UserPassword = "theday";

            loginViewModel.LoginUserCommand.Execute();

            context.MockNavigationService.Verify(v => v.Navigate<FriendListViewModel, User>(It.IsAny<User>(), null));
        }

        private TestAppContext ConstructTestAppContext()
        {
            var builder = new ContainerBuilder();

            var mockNavigationService = new Mock<IMvxNavigationService>();

            builder.RegisterInstance(mockNavigationService.Object);
            builder.RegisterInstance(new Mock<INotificationService>().Object);

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>();

            builder.RegisterType<FirebaseAuthService>()
                .As<IFirebaseAuthService>();

            builder.RegisterType<LoginViewModel>();

            return new TestAppContext
            {
                TestContainer = builder.Build(),
                MockNavigationService = mockNavigationService
            };
        }
    }
}
