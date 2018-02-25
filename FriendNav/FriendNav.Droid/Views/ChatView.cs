using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FriendNav.Droid.Views
{
    [Activity(Label = "Chat")]
    public class ChatView : BaseView
    {
        protected override int LayoutResource => Resource.Layout.ChatView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}