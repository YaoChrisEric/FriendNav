using FriendNav.Core.Model;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class RequestViewModel : MvxViewModel<NavigateRequest>
    {
        private NavigateRequest _navigateRequest;

        public override void Prepare(NavigateRequest parameter)
        {
            _navigateRequest = parameter;
        }
    }
}
