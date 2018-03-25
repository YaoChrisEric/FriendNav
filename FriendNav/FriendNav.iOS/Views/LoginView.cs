
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
    public partial class LoginView : MvxViewController
    {
        public LoginView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(LoginUserButton)
                .To(t => t.LoginUserCommand);
            set.Bind(EmailAddressTextField)
                .To(t => t.EmailAddress);
            set.Bind(PasswordTextField)
                .To(t => t.UserPassword);
            set.Apply();
        }
    }
}