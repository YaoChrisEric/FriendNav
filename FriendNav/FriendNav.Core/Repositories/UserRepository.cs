using Firebase.Database;
using Firebase.Database.Query;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IFirebaseClientService _firebaseClientService;

        public UserRepository(IFirebaseClientService firebaseClientService)
        {
            _firebaseClientService = firebaseClientService;
        }

        public Task CreateUser(User user)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            user.IsReceivingMapRequest = false;
            user.CurrentChatFriend = string.Empty;

            return client.Child("Users")
                .Child(user.EmailAddress)
                .PostAsync(user);
        }

        public Task<User> GetUser(string emailAddress)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            return client.Child("Users")
                .Child(emailAddress)
                .OnceSingleAsync<User>();
        }


        public Task GetFriendList(User user)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            return client.Child("FriendMap")
                .Child(user.FirebaseKey)
                .Child("FriendList")
                .OnceAsync<User>()
                .ContinueWith(ObservableCollection<User> _beginLambda => );
        }
    }
}
