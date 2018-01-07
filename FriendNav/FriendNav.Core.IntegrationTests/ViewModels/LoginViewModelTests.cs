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

namespace FriendNav.Core.IntegrationTests.ViewModels
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class LoginViewModelTests
    {
        private Mock<IMvxNavigationService> mockNavigationService;
        private IContainer _container;

        public LoginViewModelTests()
        {
        }

        [ClassInitialize]
        public void ClassInitialize(TestContext context)
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

            _container = builder.Build();
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            var loginViewModel = _container.Resolve<LoginViewModel>();

            loginViewModel.EmailAddress = "c@test.com";

            loginViewModel.UserPassword = "theday";

            loginViewModel.LoginUserCommand.Execute();

            
        }
    }
}
