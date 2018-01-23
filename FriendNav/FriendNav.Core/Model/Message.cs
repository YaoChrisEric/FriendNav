using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Model
{
    public class Message
    {
        public string ChatFirebaseKey { get; set; }

        public string FirebaseKey { get; set; }

        [JsonProperty("message")]
        public string Text { get; set; }

        public string SenderEmail { get; set; }

        public string TimeStamp { get; set; }
    }
}
