using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Services
{
    public class LocationService : ILocationUpdateService
    {
        public event EventHandler<LocationChangeEventArgs> LocationChanged;

        public void OnLocationChanged(LocationChangeEventArgs locationChangeEventArgs)
        {
            LocationChanged?.Invoke(this, locationChangeEventArgs);
        }
    }
}