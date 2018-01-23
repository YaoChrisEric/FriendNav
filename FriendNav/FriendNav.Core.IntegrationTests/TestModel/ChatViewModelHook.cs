using FriendNav.Core.Model;
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
    public class ChatViewModelHook : IAsyncHook
    {
        private readonly object _testLock = new object();

        public ManualResetEvent ResetEvent { get; set; } = new ManualResetEvent(false);

        public string TestMessage { get; set; }

        public User ActiveTestUser { get; set; }

        public ChatViewModel ViewModel { get; set; }

        public MessageViewModel CapturedTestMessage { get; set; }

        public bool IsCheckComplete { get; set; }

        public void NotifyOtherThreads()
        {
            lock (_testLock)
            {
                if (IsCheckComplete)
                {
                    return;
                }

                CapturedTestMessage = ViewModel.Messages.FirstOrDefault(a => a.Text == TestMessage);

                IsCheckComplete = true;
                ResetEvent.Set();
            }
        }
    }
}
