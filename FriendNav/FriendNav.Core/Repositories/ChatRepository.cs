using Firebase.Database.Query;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IFirebaseClientService _firebaseClientService;

        public ChatRepository(IFirebaseClientService firebaseClientService)
        {
            _firebaseClientService = firebaseClientService;
        }

        public Chat GetChat(User currentUser, User chatUser)
        {
            var chat = new Chat
            {
                ActiveUser = currentUser,
                ChatUser = chatUser
            };

            return chat;
        }
    }
}
