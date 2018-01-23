using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;
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

        private Chat _chat;

        public RequestViewModel(ITask task,
            INavigationRequestService navigationRequestService
            )
        {
            _task = task;
            _navigationRequestService = navigationRequestService;
            DeclineRequestCommand = new MvxCommand(DeclineRequestAsync);
        }

        public override void Prepare(Chat parameter)
        {
            _chat = parameter;
        }

        public MvxCommand DeclineRequestCommand { get; }

        private void DeclineRequestAsync()
        {
            _task.Run(DeclineRequest);
        }

        private void DeclineRequest()
        {
            _navigationRequestService.DeclineNavigationRequest(_chat.NavigateRequest);
        }
    }
}
