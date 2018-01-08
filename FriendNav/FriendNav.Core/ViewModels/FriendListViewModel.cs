using FriendNav.Core.Model;
using FriendNav.Core.Repositories;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class FriendListViewModel : MvxViewModel<User>
    {
        private IUserRepository _userRepository;
        private User user;

        public MvxCommand DisplayFriendListCommand { get; }
        public MvxObservableCollection<User> FriendList;

        public FriendListViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            DisplayFriendListCommand = new MvxCommand(DisplayFriendList);
        }

        public override void Prepare(User parameter)
        {
            user = parameter;
        }

        private void DisplayFriendList()
        {
            _userRepository.GetFriendList(user);
            PopulateObservaleCollection(user);
        }

        private void PopulateObservaleCollection(User user)
        {
            foreach (var friend in user.FriendList)
            {
                FriendList.Add(friend);
            }
        }
    }
}
