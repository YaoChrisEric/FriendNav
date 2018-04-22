using Foundation;
using FriendNav.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using System;
using UIKit;

namespace FriendNav.iOS
{
    public partial class FriendListCell : MvxTableViewCell
    {
        public FriendListCell (IntPtr handle) : base (handle)
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<FriendListCell, FriendViewModel>();
                set.Bind(FriendLabel).To(m => m.EmailAddress);
                set.Apply();
            });
        }
    }
}