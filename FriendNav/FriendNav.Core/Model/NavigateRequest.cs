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
        private readonly object _updateLock = new object();

        public string ChatFirebaseKey { get; set; }

        public User ActiveUser { get; set; }

        public event EventHandler NavigationReqest;

        public event EventHandler NavigationDeclined;

        public event EventHandler NavigationAccepted;

        public bool IsInitiator => ActiveUser.EmailAddress == InitiatorEmail;

        public string InitiatorEmail { get; set; }

        public bool IsNavigationActive { get; set; }

        public bool IsRequestAccepted { get; set; }

        public bool IsRequestDeclined { get; set; }

        public void IncomingNavigationRequest(FirebaseEvent<NavigateRequestDto> observer)
        {
            lock (_updateLock)
            {
                IsNavigationActive = observer.Object.CallActive;
                InitiatorEmail = observer.Object.InitiatorEmail;
                IsRequestAccepted = observer.Object.IsRequestedAccepted;
                IsRequestDeclined = observer.Object.IsRequestDeclined;

                if (observer.Object.InitiatorEmail != ActiveUser.EmailAddress
                    && observer.Object.InitiatorEmail != string.Empty
                    && IsNavigationActive)
                {
                    NavigationReqest?.Invoke(this, new EventArgs());
                }

                if (IsRequestDeclined)
                {
                    NavigationDeclined?.Invoke(this, new EventArgs());
                }

                if (IsInitiator && IsNavigationActive && IsRequestAccepted)
                {
                    NavigationAccepted?.Invoke(this, new EventArgs());
                }             
            }
        }
    }
}
