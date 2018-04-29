using FriendNav.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Services.Interfaces
{
    public interface ILocationUpdateService
    {
        event EventHandler<LocationChangeEventArgs> LocationChanged;

        void OnLocationChanged(LocationChangeEventArgs locationChangeEventArgs);
    }
}

