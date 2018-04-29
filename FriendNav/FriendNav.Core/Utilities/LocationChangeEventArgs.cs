using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Utilities
{
    public class LocationChangeEventArgs : EventArgs
    {
        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}

