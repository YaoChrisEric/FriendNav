using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Repositories.Interfaces
{
    public interface INavigateRequestRepository : IDisposable
    {
        Task<NavigateRequest> GetNavigationRequest(Chat chat);

        Task UpdateNavigationRequest(NavigateRequest navigateRequest);
    }
}
