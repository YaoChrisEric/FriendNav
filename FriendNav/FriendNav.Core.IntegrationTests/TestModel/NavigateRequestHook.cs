using FriendNav.Core.Model;
using FriendNav.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.TestModel
{
    public class NavigateRequestHook : IAsyncHook
    {
        private readonly object _testLock = new object();

        public ManualResetEvent ResetEvent { get; set; } = new ManualResetEvent(false);

        public bool IsCheckComplete { get; set; }

        public string IntiatorEmail { get; set; }

        public NavigateRequest NavigateRequest { get; set; }

        public void NotifyOtherThreads()
        {
            lock (_testLock)
            {
                if (IsCheckComplete)
                {
                    return;
                }

                Assert.AreEqual(IntiatorEmail, NavigateRequest.InitiatorEmail);

                IsCheckComplete = true;
                ResetEvent.Set();
            }
        }
    }
}
