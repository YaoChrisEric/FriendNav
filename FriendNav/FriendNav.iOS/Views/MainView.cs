using System;
using System.Drawing;
using Foundation;
using FriendNav.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace FriendNav.iOS.Views
{
    [MvxFromStoryboard]
    public partial class MainView : MvxViewController
    {
        public MainView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<MainView, MainViewModel>();
            set.Bind(LoginButton)
                .To(t => t.LoginUserCommand);
            set.Apply();
        }
    }
}