using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;
using FriendNav.Core.ViewModelParameters;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel<ChatParameters>
    {
        private Chat _chat;
        private NavigateRequest _navigateRequest;

        private readonly IMessageRepository _messageRepository;
        private readonly INavigateRequestRepository _navigateRequestRepository;
        private readonly INavigationRequestService _navigationRequestService;
        private readonly IMvxNavigationService _mvxNavigationService;

        public ChatViewModel(
            INavigateRequestRepository navigateRequestRepository,
            INavigationRequestService navigationRequestService,
            IMessageRepository messageRepository,
            IMvxNavigationService mvxNavigationService
            )
        {
            _navigateRequestRepository = navigateRequestRepository;
            _navigationRequestService = navigationRequestService;
            _messageRepository = messageRepository;
            _mvxNavigationService = mvxNavigationService;

            AddNewMessageCommand = new MvxCommand(CreateNewMessageAsync);
            SendNavigationRequestCommand = new MvxCommand(SendNavigationRequestAsync);
        }

        public async override void Prepare(ChatParameters parameter)
        {
            await PrepareAsync(parameter);
        }

        public async Task PrepareAsync(ChatParameters parameter)
        {
            _chat = parameter.Chat;
            _navigateRequest = parameter.NavigateRequest;
            await SetupModel();
        }

        public MvxCommand AddNewMessageCommand { get; }

        public MvxCommand SendNavigationRequestCommand { get; }

        public IAsyncHook TestChatMessageHook { get; set; }

        public IAsyncHook TestNavigationHook { get; set; }

        public string ActiveMessage { get; set; }

        public MvxObservableCollection<MessageViewModel> Messages { get; set; } = new MvxObservableCollection<MessageViewModel>();

        private async Task SetupModel()
        {
            _chat.Messages.CollectionChanged += Messages_CollectionChanged;
            await _messageRepository.GetMessages(_chat);

            if (_navigateRequest == null)
            {
                _navigateRequest = await _navigateRequestRepository.GetNavigationRequest(_chat);
            }

            _navigateRequest.NavigationReqest += NavigateRequest_NavigationReqest;
        }

        private void CreateNewMessageAsync()
        {
            Task.Run(CreateNewMessage);
        }

        public async Task CreateNewMessage()
        {
            var message = _chat.CreateNewMessage(ActiveMessage);
            await _messageRepository.CreateMessage(message);
            ActiveMessage = string.Empty;
        }

        private void SendNavigationRequestAsync()
        {
            Task.Run(SendNavigationRequest);
        }

        public async Task SendNavigationRequest()
        {
            await _navigationRequestService.InitiatNavigationRequest(_navigateRequest);

            await _mvxNavigationService.Navigate<RequestViewModel, NavigateRequestParameters>(new NavigateRequestParameters
            {
                Chat = _chat,
                NavigateRequest = _navigateRequest
            });
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var friend in e.NewItems.Cast<Message>())
                {
                    Messages.Add(new MessageViewModel(friend));
                }
            }

            if (e.OldItems != null)
            {
                var list = e.OldItems
                    .Cast<Message>()
                    .Select(s => new MessageViewModel(s))
                    .ToList();

                foreach (var message in Messages.Where(w => list.Any(a => a.FirebaseKey == w.FirebaseKey)).ToList())
                {
                    Messages.Remove(message);
                }
            }

            TestChatMessageHook?.NotifyOtherThreads();
        }

        private async void NavigateRequest_NavigationReqest(object sender, EventArgs e)
        {
            await _mvxNavigationService.Navigate<RequestViewModel, NavigateRequestParameters>(
                new NavigateRequestParameters
                {
                    Chat = _chat,
                    NavigateRequest = _navigateRequest
                });

            TestNavigationHook?.NotifyOtherThreads();
        }
    }
}
