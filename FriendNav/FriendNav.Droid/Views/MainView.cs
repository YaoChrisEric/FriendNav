using Android.App;
using Android.OS;
using Android.Runtime;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace FriendNav.Droid.Views
{
    [Activity(Label = "Login Or Register")]
    public class MainView : BaseView
    {
        protected override int LayoutResource => Resource.Layout.MainView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }
    }
}