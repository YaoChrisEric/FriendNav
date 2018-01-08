using Autofac;
using Moq;
using MvvmCross.Core.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.TestModel
{
    public class TestAppContext
    {
        public IContainer TestContainer { get; set; }

        public Mock<IMvxNavigationService> MockNavigationService { get; set; }
    }
}
