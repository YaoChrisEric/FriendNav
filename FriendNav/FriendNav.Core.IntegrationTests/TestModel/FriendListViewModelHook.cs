using FriendNav.Core.Utilities;
using FriendNav.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.TestModel
{
    class FriendListViewModelHook : IAsyncHook
    {
        private readonly object _testLock = new object();

        public ManualResetEvent ResetEvent { get; set; } = new ManualResetEvent(false);

        public string EmailAddress { get; set; }

        public FriendListViewModel ViewModel { get; set; }

        public bool IsCheckComplete { get; set; }

        public void NotifyOtherThreads()
        {
            lock (_testLock)
            {
                if (IsCheckComplete)
                {
                    return;
                }

                IsCheckComplete = true;
                ResetEvent.Set();
            }
        }
    }
}
