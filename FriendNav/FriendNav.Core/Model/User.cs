using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FriendNav.Core.Model
{
    public class User
    {
        public string EmailAddress { get; set; }

        public bool IsReceivingMapRequest { get; set; }

        public string CurrentChatFriend { get; set; }

        public string FirebaseKey => EmailAddress.Replace('.', ',');

        public List<User> FriendList { get; set; }
    }
}
