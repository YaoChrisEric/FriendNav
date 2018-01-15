using FriendNav.Core.Model;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class FriendViewModel : MvxViewModel
    {
        private readonly Friend _friend;

        public FriendViewModel(Friend friend)
        {
            _friend = friend;
        }

        public string FirebaseKey => _friend.FirebaseKey;

        public string EmailAddress
        {
            get { return _friend.EmailAddress; }
            set
            {
                _friend.EmailAddress = value;
                RaisePropertyChanged("EmailAddress");
            }
        }
    }
}
