using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Services.Interfaces
{
    public interface INotificationService
    {
        void SendNotification(string message);
    }
}
