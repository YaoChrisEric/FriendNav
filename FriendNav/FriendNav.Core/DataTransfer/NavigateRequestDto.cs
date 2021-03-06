﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.DataTransfer
{
    public class NavigateRequestDto
    {
        [JsonProperty("initiatorEmailAddr")]
        public string InitiatorEmail { get; set; }

        [JsonProperty("CallActive")]
        public bool CallActive { get; set; }
    }
}
