using Firebase.Database;
using Firebase.Database.Query;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IFirebaseClientService _firebaseClientService;

        public UserRepository(IFirebaseClientService firebaseClientService)
        {
            _firebaseClientService = firebaseClientService;
        }

        public void CreateUser(User user)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            user.IsReceivingMapRequest = false;
            user.CurrentChatFriend = string.Empty;

            client.Child("Users")
                .Child(user.EmailAddress)
                .PostAsync(user)
                .Wait();
        }
    }
}
