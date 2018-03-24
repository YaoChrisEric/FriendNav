using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.DataTransfer
{
    public class MapDto
    {
        [JsonProperty("InitiatorLatitude")]
        public string InitiatorLatitude { get; set; }

        [JsonProperty("InitiatorLongitude")]
        public string InitiatorLongitude { get; set; }

        [JsonProperty("ResponderLatitude")]
        public string ResponderLatitude { get; set; }

        [JsonProperty("ResponderLongitude")]
        public string ResponderLongitude { get; set; }
    }
}
