using Firebase.Database.Query;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly List<IDisposable> _disposable= new List<IDisposable>();

        private readonly IFirebaseClientService _firebaseClientService;

        public MessageRepository(IFirebaseClientService firebaseClientService)
        {
            _firebaseClientService = firebaseClientService;
        }

        public async Task CreateMessage(Message message)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            await client
                .Child("BasicChat")
                .Child(message.ChatFirebaseKey)
                .Child("MessageIds")
                .PostAsync(message);
        }

        public async Task DeleteMessage(Message message)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            await client
                .Child("BasicChat")
                .Child(message.ChatFirebaseKey)
                .Child("MessageIds")
                .Child(message.FirebaseKey)
                .DeleteAsync();
        }

        public async Task GetMessages(Chat chat)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            var messages = await client
                .Child("BasicChat")
                .Child(chat.FirebaseKey)
                .Child("MessageIds")
                .OnceAsync<Message>();

            foreach (var message in messages)
            {
                message.Object.FirebaseKey = message.Key;
                chat.Messages.Add(message.Object);
            }

            var disposable = client
                .Child("BasicChat")
                .Child(chat.FirebaseKey)
                .Child("MessageIds")
                .AsObservable<Message>()
                .Subscribe(chat.UpdateMessages);

            _disposable.Add(disposable);
        }

        public void Dispose()
        {
            foreach(var dispose in _disposable)
            {
                dispose.Dispose();
            }
        }
    }
}
