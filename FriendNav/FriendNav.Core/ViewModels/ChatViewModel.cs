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

        public ChatViewModel(ITask task,
            IMessageRepository messageRepository
            )
        {
            _task = task;
            _messageRepository = messageRepository;

            AddNewMessageCommand = new MvxCommand(CreateNewMessageAsync);
        }

        public override void Prepare(Chat parameter)
        {
            _chat = parameter;
            LoadMessagesAsync();
        }

        public MvxCommand AddNewMessageCommand { get; }

        public IAsyncHook TestHook { get; set; }

        public string ActiveMessage { get; set; }

        public MvxObservableCollection<MessageViewModel> Messages = new MvxObservableCollection<MessageViewModel>();

        private void LoadMessagesAsync()
        {
            _task.Run(LoadMessages);
        }

        private void LoadMessages()
        {
            _chat.Messages.CollectionChanged += Messages_CollectionChanged;
            _messageRepository.GetMessages(_chat);            
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
    }
}
