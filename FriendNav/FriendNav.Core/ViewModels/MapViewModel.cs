using FriendNav.Core.Model;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class MapViewModel : MvxViewModel<Map>
    {
        private Map _map;

        public override void Prepare(Map parameter)
        {
            _map = parameter;
        }

    }
}
