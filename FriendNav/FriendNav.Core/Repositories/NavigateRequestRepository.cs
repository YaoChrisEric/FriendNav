using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Repositories
{
    public class NavigateRequestRepository : INavigateRequestRepository
    {
        public void GetNavigateRequest(Chat chat)
        {
            var navigateRequest = new NavigateRequest();

            navigateRequest.ActiveUser = chat.ActiveUser;

            var 
        }
    }
}
