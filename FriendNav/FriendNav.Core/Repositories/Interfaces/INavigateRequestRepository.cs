using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Repositories.Interfaces
{
    public interface INavigateRequestRepository : IDisposable
    {
        NavigateRequest GetNavigationRequest(Chat chat);

        void UpdateNavigationRequest(NavigateRequest navigateRequest);
    }
}
