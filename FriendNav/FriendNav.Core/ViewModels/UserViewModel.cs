using FriendNav.Core.Model;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class UserViewModel : MvxViewModel
    {
        private readonly User _user;

        public UserViewModel(User user)
        {
            _user = user;
        }

        public string FirebaseKey => _user.FirebaseKey;

        public string EmailAddress
        {
            get { return _user.EmailAddress; }
            set
            {
                _user.EmailAddress = value;
                RaisePropertyChanged("EmailAddress");
            }
        }
    }
}
