using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.DataTransfer
{
    public class NavigateRequestDto
    {
        public string InitiatorEmail { get; set; }

        public bool CallActive { get; set; }
    }
}
