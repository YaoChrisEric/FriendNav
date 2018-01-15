using FriendNav.Core.Utilities;
using FriendNav.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.TestModel
{
    class FriendListViewModelHook : IAsyncHook
    {
        public string EmailAddress { get; set; }

        public FriendListViewModel ViewModel { get; set; }

        public void NotifyOtherThreads()
        {
            if (ViewModel.FriendList.Any(a => a.EmailAddress == EmailAddress))
            {

            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
