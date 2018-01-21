using Firebase.Database.Streaming;
using FriendNav.Core.DataTransfer;
using FriendNav.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Model
{
    public class NavigateRequest
    {
        public IAsyncHook TestHook { get; set; }

        public string ChatFirebaseKey { get; set; }

        public User ActiveUser { get; set; }

        public event EventHandler NavigationReqest;

        public string InitiatorEmail { get; set; }

        public bool IsNavigationActive { get; set; }

        public void IncomingNavigationRequest(FirebaseEvent<NavigateRequestDto> observer)
        {
            if (observer.Object.CallActive && 
                observer.Object.InitiatorEmail != ActiveUser.EmailAddress)
            {
                NavigationReqest(this, new EventArgs());
            }

            IsNavigationActive = observer.Object.CallActive;
            InitiatorEmail = observer.Object.InitiatorEmail;

            TestHook.NotifyOtherThreads();
        }
    }
}
