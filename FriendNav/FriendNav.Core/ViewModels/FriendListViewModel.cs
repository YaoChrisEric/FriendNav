using FriendNav.Core.Model;
using FriendNav.Core.Services.Interfaces;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class FriendListViewModel : MvxViewModel<User>
    {
        private User user;

        public MvxCommand DisplayFriendListCommand { get; }

        public FriendListViewModel(IFirebaseAuthService firebaseAuthService)
        {
            DisplayFriendListCommand = new MvxCommand(DisplayFriendList);
        }

        public override void Prepare(User parameter)
        {
            user = parameter;
        }

        private void DisplayFriendList()
        {
            // List<User> friendList = ;
        }
    }
}
