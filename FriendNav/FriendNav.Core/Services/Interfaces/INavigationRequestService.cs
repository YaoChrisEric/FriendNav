using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Services.Interfaces
{
    public interface INavigationRequestService
    {
        void InitiatNavigationRequest(NavigateRequest navigateRequest);
        void DeclineNavigationRequest(NavigateRequest navigateRequest);
    }
}
