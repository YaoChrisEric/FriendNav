using Firebase.Database.Streaming;
using FriendNav.Core.DataTransfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Model
{
    public class NavigateRequest
    {
        public User ActiveUser { get; set; }

        public event EventHandler NavigationReqest;

        public bool IsNavigationActive { get; set; }

        public void IncomingNavigationRequest(FirebaseEvent<NavigateRequestDto> observer)
        {
            if (observer.Object.CallActive && 
                observer.Object.InitiatorEmail != ActiveUser.EmailAddress)
            {
                NavigationReqest(this, new EventArgs());
            }

            IsNavigationActive = observer.Object.CallActive;
        }
    }
}
