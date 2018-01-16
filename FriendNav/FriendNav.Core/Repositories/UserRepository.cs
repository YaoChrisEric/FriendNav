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
        private readonly List<IDisposable> _disposable = new List<IDisposable>();

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
                .Child(user.FirebaseKey)
                .PostAsync(user)
                .RunSynchronously();
        }

        public User GetUser(string emailAddress)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

           return client.Child("Users")
                .Child(User.CreateFirebaseKey(emailAddress))
                .OnceSingleAsync<User>()
                .Result;
        }

        public List<User> FindUsers(string emailPart)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            var searchUsers = client.Child("Users")                
                .OnceAsync<User>()
                .Result;

            var users = new List<User>();

            //TechDebt: This in memory filtering will not scale but should be fine until a server side
            // approach can be worked out.
            foreach(var user in searchUsers)
            {
                if (user.Object.EmailAddress != null && user.Object.EmailAddress.Contains(emailPart))
                {
                    users.Add(user.Object);
                }
            }

            return users;
        }

        public void GetFriendList(User user)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            var friends = client.Child("FriendMap")
                .Child(user.FirebaseKey)
                .Child("FriendList")
                .OnceAsync<Friend>()
                .Result;

            foreach (var friend in friends)
            {
                friend.Object.FirebaseKey = friend.Key;
                user.FriendList.Add(friend.Object);
            }

            var disposable = client.Child("FriendMap")
                .Child(user.FirebaseKey)
                .Child("FriendList")
                .AsObservable<Friend>()
                .Subscribe(user.UpdateFriendList);

            _disposable.Add(disposable);
        }

        public void AddUserToFriendList(User user, Friend newFriend)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            client
                .Child("FriendMap")
                .Child(user.FirebaseKey)
                .Child("FriendList")
                .PostAsync(newFriend)
                .Wait();
        }

        public void RemoveUserFromFriendList(User user, Friend removeFriend)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            client
                .Child("FriendMap")
                .Child(user.FirebaseKey)
                .Child("FriendList")
                .Child(removeFriend.FirebaseKey)
                .DeleteAsync()
                .Wait();
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
