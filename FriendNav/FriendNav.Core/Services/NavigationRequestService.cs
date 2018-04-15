using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public async Task AcceptNavigationRequest(NavigateRequest navigateRequest)
        {
            navigateRequest.IsRequestedAccepted = true;

            await _navigationRequestRepository.UpdateNavigationRequest(navigateRequest);
        }

        public async Task DeclineNavigationRequest(NavigateRequest navigateRequest)
        {
            navigateRequest.InitiatorEmail = string.Empty;
            navigateRequest.IsNavigationActive = false;
            //redundant; but just in case
            navigateRequest.IsRequestedAccepted = false;

            await _navigationRequestRepository.UpdateNavigationRequest(navigateRequest);
        }

        public async Task InitiatNavigationRequest(NavigateRequest navigateRequest)
        {
            navigateRequest.IsRequestedAccepted = false;
            navigateRequest.InitiatorEmail = navigateRequest.ActiveUser.EmailAddress;
            navigateRequest.IsNavigationActive = true;

            await _navigationRequestRepository.UpdateNavigationRequest(navigateRequest);
        }
    }
}
