using FriendNav.Core.Model;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel<User>
    {
        private User _user;

        public override void Prepare(User parameter)
        {
            _user = parameter;
        }
    }
}
