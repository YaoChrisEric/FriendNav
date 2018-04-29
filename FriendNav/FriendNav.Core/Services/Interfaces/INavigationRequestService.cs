using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Services.Interfaces
{
    public interface INavigationRequestService
    {
        Task InitiatNavigationRequest(NavigateRequest navigateRequest);
        Task DeclineNavigationRequest(NavigateRequest navigateRequest);
        Task AcceptNavigationRequest(NavigateRequest navigateRequest);
    }
}
