using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Model
{
    public class Friend
    {
        public string FirebaseKey { get; set; }

        [JsonProperty("friendEmailAddr")]
        public string EmailAddress { get; set; }
    }
}
