using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using FriendNav.Core.ViewModels;
using UIKit;

namespace FriendNav.iOS.Tables
{
    public class FriendListViewTable : UITableViewSource
    {
        string[] TableItems;

        public FriendListViewTable(string[] items)
        {
            TableItems = items;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("friend_cell");

            string item = TableItems[indexPath.Row];

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, "friend_cell");
            }

            cell.TextLabel.Text = item;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Length;
        }
    }
}