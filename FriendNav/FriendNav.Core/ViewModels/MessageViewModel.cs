using FriendNav.Core.Model;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class MessageViewModel : MvxViewModel
    {
        private readonly Message _message;

        public MessageViewModel(Message message)
        {
            _message = message;
        }

        public string FirebaseKey => _message.FirebaseKey;

        public string Text => _message.Text;

        public string SenderEmail => _message.SenderEmail;

        public string TimeStamp => _message.TimeStamp;
    }
}
