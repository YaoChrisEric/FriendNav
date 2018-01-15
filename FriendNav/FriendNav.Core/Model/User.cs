using Firebase.Database.Streaming;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FriendNav.Core.Model
{
    public class User
    {
        [JsonProperty("emailAddr")]
        public string EmailAddress { get; set; }

        [JsonProperty("receivingMapRequest")]
        public bool IsReceivingMapRequest { get; set; }

        [JsonProperty("currentChatFriend")]
        public string CurrentChatFriend { get; set; }

        public string FirebaseKey => CreateFirebaseKey(EmailAddress);

        public ObservableCollection<Friend> FriendList { get; set; } = new ObservableCollection<Friend>();

        public static string CreateFirebaseKey(string firebasekey)
        {
            return firebasekey.Replace('.', ',');
        }

        public void UpdateFriendList(FirebaseEvent<Friend> observer)
        {
            var friend = FriendList.FirstOrDefault(f => f.FirebaseKey == observer.Key);

            if (observer.EventType == FirebaseEventType.Delete && friend != null)
            {
                FriendList.Remove(friend);
                return;
            }

            if (observer.EventType == FirebaseEventType.Delete)
            {
                return;
            }

            if (friend != null)
            {
                friend.EmailAddress = observer.Object.EmailAddress;
                return;
            }

            observer.Object.FirebaseKey = observer.Key;

            FriendList.Add(observer.Object);
        }
    }
}
