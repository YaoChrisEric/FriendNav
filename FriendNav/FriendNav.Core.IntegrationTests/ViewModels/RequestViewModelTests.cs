using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FriendNav.Core.IntegrationTests.TestModel;
using FriendNav.Core.ViewModels;
using Autofac;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;

namespace FriendNav.Core.IntegrationTests.ViewModels
{
    [TestClass]
    public class RequestViewModelTests
    {
        public RequestViewModelTests()
        {
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Setup_correctly_for_initiator()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Decline_request_for_intiator()
        {
            Assert.Fail();
        }
    }
}
