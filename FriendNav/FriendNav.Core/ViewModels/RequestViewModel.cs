using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;
using FriendNav.Core.ViewModelParameters;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.ViewModels
{
    public class RequestViewModel : MvxViewModel<NavigateRequestParameters>
    {
        private readonly INavigationRequestService _navigationRequestService;
        private readonly IMvxNavigationService _mvxNavigationService;
        private readonly IMapRepository _mapRepository;

        private Chat _chat;
        private NavigateRequest _navigateRequest;

        public RequestViewModel(
            INavigationRequestService navigationRequestService,
            IMapRepository mapRepository,
            IMvxNavigationService mvxNavigationService
            )
        {
            _navigationRequestService = navigationRequestService;
            _mvxNavigationService = mvxNavigationService;
            _mapRepository = mapRepository;
            DeclineRequestCommand = new MvxCommand(DeclineRequestAsync);
            AcceptRequestCommand = new MvxCommand(AcceptRequestAsync);
        }

        public IAsyncHook TestHook { get; set; }

        public IAsyncHook AcceptedHook { get; set; }

        public override void Prepare(NavigateRequestParameters parameter)
        {
            _chat = parameter.Chat;
            _navigateRequest = parameter.NavigateRequest;
            _navigateRequest.NavigationDeclined += NavigateRequest_NavigationDeclined;
            _navigateRequest.NavigationAccepted += NavigateRequest_NavigationAccepted;
        }

        public MvxCommand DeclineRequestCommand { get; }

        public MvxCommand AcceptRequestCommand { get; }

        private void DeclineRequestAsync()
        {
            Task.Run(DeclineRequest);
        }

        public async Task DeclineRequest()
        {
            _navigateRequest.NavigationDeclined -= NavigateRequest_NavigationDeclined;
            _navigateRequest.NavigationAccepted -= NavigateRequest_NavigationAccepted;

            await _navigationRequestService.DeclineNavigationRequest(_navigateRequest);

            await _mvxNavigationService.Navigate<ChatViewModel, ChatParameters>(new ChatParameters
            {
                Chat = _chat,
                NavigateRequest = _navigateRequest
            });
        }

        private void AcceptRequestAsync()
        {
            Task.Run(AcceptRequest);
        }

        public async Task AcceptRequest()
        {
            var map = await _mapRepository.GetMap(_navigateRequest.ChatFirebaseKey);

            await _navigationRequestService.AcceptNavigationRequest(_navigateRequest);

            await _mvxNavigationService.Navigate<MapViewModel, Map>(map);
        }

        private async void NavigateRequest_NavigationDeclined(object sender, EventArgs e)
        {
            _navigateRequest.NavigationDeclined -= NavigateRequest_NavigationDeclined;
            _navigateRequest.NavigationAccepted -= NavigateRequest_NavigationAccepted;

            await _mvxNavigationService.Navigate<ChatViewModel, ChatParameters>(new ChatParameters
            {
                Chat = _chat,
                NavigateRequest = _navigateRequest
            });

            TestHook?.NotifyOtherThreads();
        }

        private async void NavigateRequest_NavigationAccepted(object sender, EventArgs e)
        {
            var map = await _mapRepository.GetMap(_navigateRequest.ChatFirebaseKey);

            await _mvxNavigationService.Navigate<MapViewModel, Map>(map);

            AcceptedHook.NotifyOtherThreads();
        }
    }
}
