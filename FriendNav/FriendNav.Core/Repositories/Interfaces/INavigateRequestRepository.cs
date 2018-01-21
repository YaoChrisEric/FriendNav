using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Repositories.Interfaces
{
    public interface INavigateRequestRepository : IDisposable
    {
        void GetNavigateRequest(Chat chat);

        void SendNavigationRequest(NavigateRequest navigateRequest);
    }
}
