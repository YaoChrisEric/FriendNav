﻿
using System;
using System.Drawing;

using Foundation;
using FriendNav.Core.ViewModels;
using FriendNav.iOS.Tables;
using FriendNav.iOS.TableViewSource;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace FriendNav.iOS.Views
{
    [MvxFromStoryboard]
    public partial class FriendListView : MvxViewController
    {
        public FriendListView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<FriendListView, FriendListViewModel>();

            FriendListTableViewSource friendListTableViewSource = new FriendListTableViewSource(FriendListTable);
            set.Bind(friendListTableViewSource).To(vm => vm.FriendList);
            set.Apply();

            FriendListTable.Source = friendListTableViewSource;
            FriendListTable.ReloadData();
        }
    }
}