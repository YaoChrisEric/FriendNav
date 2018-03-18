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

        public bool IsRequestedAccepted { get; set; }

        public void IncomingNavigationRequest(FirebaseEvent<NavigateRequestDto> observer)
        {
            lock (_updateLock)
            {
                if(IsNavigationActive == observer.Object.CallActive &&
                    InitiatorEmail == observer.Object.InitiatorEmail &&
                    IsRequestedAccepted == observer.Object.IsRequestedAccepted)
                {
                    return;
                }

                IsNavigationActive = observer.Object.CallActive;
                InitiatorEmail = observer.Object.InitiatorEmail;
                IsRequestedAccepted = observer.Object.IsRequestedAccepted;

                if (observer.Object.InitiatorEmail != ActiveUser.EmailAddress && IsNavigationActive)
                {
                    NavigationReqest?.Invoke(this, new EventArgs());
                }

                if (observer.Object.InitiatorEmail == string.Empty)
                {
                    NavigationDeclined?.Invoke(this, new EventArgs());
                }

                if (IsInitiator && IsNavigationActive)
                {
                    if (true == IsRequestedAccepted)
                    {
                        NavigationAccepted?.Invoke(this, new EventArgs());
                    }
                }             
            }
        }
    }
}
