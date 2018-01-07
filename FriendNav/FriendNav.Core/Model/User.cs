using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Model
{
    public class User
    {
        public string EmailAddress { get; set; }

        public bool IsReceivingMapRequest { get; set; }

        public string CurrentChatFriend { get; set; }

        public string FirebaseKey => EmailAddress.Replace('.', ',');
    }
}
