using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class RequestViewModel : MvxViewModel<Chat>
    {
        private readonly ITask _task;
        private readonly INavigationRequestService _navigationRequestService;
        private readonly IMvxNavigationService _mvxNavigationService;
        private readonly IMapRepository _mapRepository;

        private Chat _chat;

        public RequestViewModel(ITask task,
            INavigationRequestService navigationRequestService,
            IMapRepository mapRepository,
            IMvxNavigationService mvxNavigationService
            )
        {
            _task = task;
            _navigationRequestService = navigationRequestService;
            _mvxNavigationService = mvxNavigationService;
            _mapRepository = mapRepository;
            DeclineRequestCommand = new MvxCommand(DeclineRequestAsync);
            AcceptRequestCommand = new MvxCommand(AcceptRequestAsync);
        }

        public IAsyncHook TestHook { get; set; }

        public IAsyncHook AcceptedHook { get; set; }

        public override void Prepare(Chat parameter)
        {
            _chat = parameter;

            _chat.NavigateRequest.NavigationDeclined += NavigateRequest_NavigationDeclined;
            _chat.NavigateRequest.NavigationAccepted += NavigateRequest_NavigationAccepted;
        }

        public MvxCommand DeclineRequestCommand { get; }

        public MvxCommand AcceptRequestCommand { get; }

        private void DeclineRequestAsync()
        {
            _task.Run(DeclineRequest);
        }

        private void DeclineRequest()
        {
            _chat.NavigateRequest.NavigationDeclined -= NavigateRequest_NavigationDeclined;
            _chat.NavigateRequest.NavigationAccepted -= NavigateRequest_NavigationAccepted;

            _navigationRequestService.DeclineNavigationRequest(_chat.NavigateRequest);

            _mvxNavigationService.Navigate<ChatViewModel, Chat>(_chat);
        }

        private void AcceptRequestAsync()
        {
            _task.Run(AcceptRequest);
        }

        private void AcceptRequest()
        {
            _navigationRequestService.InitiatNavigationRequest(_chat.NavigateRequest);

            var map = _mapRepository.GetMap(_chat.NavigateRequest.ChatFirebaseKey);

            _mvxNavigationService.Navigate<MapViewModel, Map>(map);
        }

        private void NavigateRequest_NavigationDeclined(object sender, EventArgs e)
        {
            _chat.NavigateRequest.NavigationDeclined -= NavigateRequest_NavigationDeclined;
            _chat.NavigateRequest.NavigationAccepted -= NavigateRequest_NavigationAccepted;

            _mvxNavigationService.Navigate<ChatViewModel, Chat>(_chat);

            TestHook?.NotifyOtherThreads();
        }

        private void NavigateRequest_NavigationAccepted(object sender, EventArgs e)
        {
            if (_chat.NavigateRequest.IsInitiator)
            {
                return;
            }

            var map = _mapRepository.GetMap(_chat.NavigateRequest.ChatFirebaseKey);

            _mvxNavigationService.Navigate<MapViewModel, Map>(map);

            AcceptedHook.NotifyOtherThreads();
        }
    }
}
