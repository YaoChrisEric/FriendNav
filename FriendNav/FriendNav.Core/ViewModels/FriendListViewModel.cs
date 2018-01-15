using FriendNav.Core.Model;
using FriendNav.Core.Repositories;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.ViewModels
{
    public class FriendListViewModel : MvxViewModel<User>
    {
        private ITask _task;
        private IUserRepository _userRepository;
        private User _user;

        public FriendListViewModel(ITask task, IUserRepository userRepository)
        {
            _task = task;
            _userRepository = userRepository;
            SearchForUserCommand = new MvxCommand(SearchForUserAsync);
            AddUserToFriendListCommand = new MvxCommand(AddUserToFriendListAsync);
        }

        public override void Prepare(User parameter)
        {
            _user = parameter;
            LoadUserFriendsAsync();
        }

        public IAsyncHook TestHook { get; set; }

        public MvxCommand SearchForUserCommand { get; }

        public MvxCommand AddUserToFriendListCommand { get; }

        public MvxObservableCollection<UserViewModel> SearchedUsers { get; set; } = new MvxObservableCollection<UserViewModel>();

        public MvxObservableCollection<FriendViewModel> FriendList { get; set; } = new MvxObservableCollection<FriendViewModel>();

        private string _userSearchText;
        public string UserSearchText
        {
            get { return _userSearchText; }
            set
            {
                SetProperty(ref _userSearchText, value);

                SearchForUserAsync();
            }
        }

        public User SelectedNewFriend { get; set; }

        private void LoadUserFriendsAsync()
        {
            _task.Run(LoadUserFriends);
        }

        private void LoadUserFriends()
        {
            SetupFriendList();
            _userRepository.GetFriendList(_user);
        }

        private void SetupFriendList()
        {
            _user.FriendList.CollectionChanged += FriendList_CollectionChanged;
        }
       
        private void SearchForUserAsync()
        {
            _task.Run(SearchForUser);
        }

        private void SearchForUser()
        {
            var users = _userRepository.FindUsers(UserSearchText);
            UpdateSearchUsers(users);
        }

        private void AddUserToFriendListAsync()
        {
            _task.Run(AddUserToFriendList);
        }

        private void AddUserToFriendList()
        {
            var friend = new Friend
            {
                EmailAddress = SelectedNewFriend.EmailAddress
            };

            _userRepository.AddUserToFriendList(_user, friend);

            SelectedNewFriend = null;
        }

        private void UpdateSearchUsers(List<User> newList)
        {
            SearchedUsers.RemoveItems(SearchedUsers.Where(w => !newList.Any(a => a.FirebaseKey == w.FirebaseKey)).ToArray());

            SearchedUsers.AddRange(newList
                .Where(w => !SearchedUsers.Any(a => a.FirebaseKey == w.FirebaseKey))
                .Select(s => new UserViewModel(s)));
        }

        private void FriendList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach(var friend in e.NewItems.Cast<Friend>())
                {
                    FriendList.Add(new FriendViewModel(friend));
                }
            }
            
            if (e.OldItems != null)
            {
                var list = e.OldItems
                    .Cast<Friend>()
                    .Select(s => new FriendViewModel(s))
                    .ToList();

                foreach (var friend in FriendList.Where(w => list.Any(a => a.FirebaseKey == w.FirebaseKey)).ToList())
                {
                    FriendList.Remove(friend);
                }
            }

            TestHook?.NotifyOtherThreads();
        }
    }
}
