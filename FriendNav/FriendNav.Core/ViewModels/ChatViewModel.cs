using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Utilities;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel<Chat>
    {
        private Chat _chat;

        private readonly ITask _task;
        private readonly IMessageRepository _messageRepository;
        private readonly INavigateRequestRepository _navigateRequestRepository;

        public ChatViewModel(ITask task,
            INavigateRequestRepository navigateRequestRepository,
            IMessageRepository messageRepository
            )
        {
            _task = task;
            _navigateRequestRepository = navigateRequestRepository;
            _messageRepository = messageRepository;

            AddNewMessageCommand = new MvxCommand(CreateNewMessageAsync);
            SendNavigationRequestCommand = new MvxCommand(SendNavigationRequestAsync);
        }

        public override void Prepare(Chat parameter)
        {
            _chat = parameter;
            SetupModelAsync();
        }

        public MvxCommand AddNewMessageCommand { get; }

        public MvxCommand SendNavigationRequestCommand { get; }

        public IAsyncHook TestHook { get; set; }

        public string ActiveMessage { get; set; }

        public MvxObservableCollection<MessageViewModel> Messages = new MvxObservableCollection<MessageViewModel>();

        private void SetupModelAsync()
        {
            _task.Run(SetupModel);
        }

        private void SetupModel()
        {
            _chat.Messages.CollectionChanged += Messages_CollectionChanged;
            _messageRepository.GetMessages(_chat);

            _navigateRequestRepository.GetNavigateRequest(_chat);

            _chat.NavigateRequest.NavigationReqest += NavigateRequest_NavigationReqest;
        }

        private void CreateNewMessageAsync()
        {
            _task.Run(CreateNewMessage);
        }

        private void CreateNewMessage()
        {
            var message = _chat.CreateNewMessage(ActiveMessage);
            _messageRepository.CreateMessage(_chat, message);
            ActiveMessage = string.Empty;
        }

        private void SendNavigationRequestAsync()
        {
            _task.Run(SendNavigationRequest);
        }

        private void SendNavigationRequest()
        {
            _navigateRequestRepository.SendNavigationRequest(_chat.NavigateRequest);
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

            TestHook?.NotifyOtherThreads();
        }

        private void NavigateRequest_NavigationReqest(object sender, EventArgs e)
        {
            
        }
    }
}
