using Autofac;
using FriendNav.Core.IntegrationTests.TestModel;
using FriendNav.Core.Model;
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
    public class RegisterViewModelTests
    {
        public RegisterViewModelTests()
        {
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void User_register_and_navigate_to_FriendList()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var loginViewModel = context.TestContainer.Resolve<RegisterViewModel>();

            loginViewModel.EmailAddress = "c2353453646@test.com";

            loginViewModel.UserPassword = "theday";

            loginViewModel.RegisterUserCommand.Execute();




        }
    }
}
