using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FriendNav.Core.Services.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace FriendNav.Droid.Services
{
    public class NotificationService : INotificationService
    {
        public Context CurrentContext => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public void SendNotification(string message)
        {
            Toast.MakeText(CurrentContext, message, ToastLength.Long);
        }
    }
}