// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace FriendNav.iOS.Views
{
    [Register ("FriendListView")]
    partial class FriendListView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView FriendListTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField FriendSearchBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FriendSearchButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FriendListTable != null) {
                FriendListTable.Dispose ();
                FriendListTable = null;
            }

            if (FriendSearchBar != null) {
                FriendSearchBar.Dispose ();
                FriendSearchBar = null;
            }

            if (FriendSearchButton != null) {
                FriendSearchButton.Dispose ();
                FriendSearchButton = null;
            }
        }
    }
}