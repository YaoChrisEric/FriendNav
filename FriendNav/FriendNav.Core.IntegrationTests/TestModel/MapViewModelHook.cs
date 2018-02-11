using FriendNav.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.TestModel
{
    class MapViewModelHook : IAsyncHook
    {
        private readonly object _testLock = new object();

        public ManualResetEvent ResetEvent { get; set; } = new ManualResetEvent(false);

        public bool IsCheckComplete { get; set; }

        public Model.Map Map { get; set; }

        public void NotifyOtherThreads()
        {
            lock (_testLock)
            {
                if (IsCheckComplete)
                {
                    return;
                }

                Map.InitiatorLatitude = "498";
                Map.InitiatorLongitude = "498";
                IsCheckComplete = true;
                ResetEvent.Set();
            }
        }
    }
}
