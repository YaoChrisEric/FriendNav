using Firebase.Database.Streaming;
using FriendNav.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FriendNav.Core.Model
{
    public class Chat
    {
        public User ActiveUser { get; set; }

        public User ChatUser { get; set; }

        public NavigateRequest NavigateRequest { get; set; }

        public string FirebaseKey => GenerateChatFirebaseKey(ActiveUser, ChatUser);

        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        public void UpdateMessages(FirebaseEvent<Message> observer)
        {
            var message = Messages.FirstOrDefault(f => f.FirebaseKey == observer.Key);

            if (observer.EventType == FirebaseEventType.Delete && message != null)
            {
                Messages.Remove(message);
                return;
            }

            if (observer.EventType == FirebaseEventType.Delete)
            {
                return;
            }

            if (message != null)
            {
                message.Text = observer.Object.Text;
                message.TimeStamp = observer.Object.TimeStamp;
                return;
            }

            observer.Object.FirebaseKey = observer.Key;

            Messages.Add(observer.Object);
        }

        public Message CreateNewMessage(string text)
        {
            var message = new Message
            {
                ChatFirebaseKey = FirebaseKey,
                SenderEmail = ActiveUser.EmailAddress,
                Text = text,
                TimeStamp = DateTime
                    .Now
                    .TimeOfDay
                    .ToString()
            };

            return message;
        }

        public static string GenerateChatFirebaseKey(User initiator, User responder)
        {
            if (string.Compare(initiator.FirebaseKey, responder.FirebaseKey) > 0)
            {
                return responder.FirebaseKey + initiator.FirebaseKey;
            }

            return initiator.FirebaseKey + responder.FirebaseKey;
        }
    }
}
