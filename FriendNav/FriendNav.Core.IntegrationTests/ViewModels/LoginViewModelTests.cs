using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FriendNav.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Core.Navigation;
using Moq;
using Autofac;

namespace FriendNav.Core.IntegrationTests.ViewModels
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class LoginViewModelTests
    {
        private IContainer _container;

        public LoginViewModelTests()
        {
        }

        [ClassInitialize]
        public void ClassInitialize(TestContext context)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new Mock<IMvxNavigationService>().Object);

            builder.RegisterType<LoginViewModel>();

            _container = builder.Build();
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            
        }
    }
}
