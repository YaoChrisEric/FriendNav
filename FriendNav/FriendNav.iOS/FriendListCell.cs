using Foundation;
using System;
using UIKit;

namespace FriendNav.iOS
{
    public partial class FriendListCell : UITableViewCell
    {
        public FriendListCell (IntPtr handle) : base (handle)
        {
        }

        internal void UpdateCell(String friend)
        {
            FriendLabel.Text = friend;
        }
    }
}