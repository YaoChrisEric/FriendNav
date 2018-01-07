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

namespace FriendNav.Droid.Views
{
    [Activity(Label = "Register")]
    public class RegisterView : BaseView
    {
        protected override int LayoutResource => Resource.Layout.RegisterView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}