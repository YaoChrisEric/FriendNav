﻿// WARNING
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

        void ReleaseDesignerOutlets ()
        {
            if (FriendListTable != null) {
                FriendListTable.Dispose ();
                FriendListTable = null;
            }
        }
    }
}