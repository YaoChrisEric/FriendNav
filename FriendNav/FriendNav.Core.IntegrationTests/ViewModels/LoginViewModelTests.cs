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
using Firebase.Auth;
using User = FriendNav.Core.Model.User;
using FriendNav.Core.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.ViewModels
{
    [TestClass]
    public class LoginViewModelTests
    {
        public LoginViewModelTests()
        {
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task User_login_and_navigate_to_FriendList()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var loginViewModel = context.TestContainer.Resolve<LoginViewModel>();

            loginViewModel.EmailAddress = "c@test.com";

            loginViewModel.UserPassword = "theday";

            await loginViewModel.LoginUser();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(a => a.Parameter is User));
        }
    }
}
