using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace FriendNav.iOS.TableViewSource
{
    class FriendListTableViewSource : MvxTableViewSource
    {
        public FriendListTableViewSource(UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = (FriendListCell)tableView.DequeueReusableCell("friend_cell", indexPath);
            return cell;
        }
    }
}