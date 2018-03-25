using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using FriendNav.Core.Services.Interfaces;
using UIKit;

namespace FriendNav.iOS.Services
{
    public class NotificationService : INotificationService
    {
        public void SendNotification(string message)
        {
            //Create Alert
            var okAlertController = UIAlertController.Create("Alert", message, UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
        }
    }
}