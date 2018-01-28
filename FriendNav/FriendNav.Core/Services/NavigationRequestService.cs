using System;
using System.Collections.Generic;
using System.Text;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;

namespace FriendNav.Core.Services.Interfaces
{
    public class NavigationRequestService : INavigationRequestService
    {
        private readonly INavigateRequestRepository _navigationRequestRepository;

        public NavigationRequestService(INavigateRequestRepository navigationRequestRepository)
        {
            _navigationRequestRepository = navigationRequestRepository;
        }

        public void DeclineNavigationRequest(NavigateRequest navigateRequest)
        {
            navigateRequest.InitiatorEmail = string.Empty;
            navigateRequest.IsNavigationActive = false;

            _navigationRequestRepository.UpdateNavigationRequest(navigateRequest);
        }

        public void InitiatNavigationRequest(NavigateRequest navigateRequest)
        {
            navigateRequest.InitiatorEmail = navigateRequest.ActiveUser.EmailAddress;
            navigateRequest.IsNavigationActive = true;

            _navigationRequestRepository.UpdateNavigationRequest(navigateRequest);
        }
    }
}
