using FriendNav.Core.Model;
using FriendNav.Core.Repositories;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;
using FriendNav.Core.ViewModelParameters;
using MvvmCross.Core.Navigation;
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
        private readonly ITask _task;
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private User _user;
        private readonly IMvxNavigationService _mvxNavigationService;

        public FriendListViewModel(ITask task,
            IUserRepository userRepository,
            IChatRepository chatRepository,
            IMvxNavigationService navagtionService
            )
        {
            _mvxNavigationService = navagtionService;
            _task = task;
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            SearchForUserCommand = new MvxCommand(SearchForUserAsync);
            AddUserToFriendListCommand = new MvxCommand(AddUserToFriendListAsync);
            NavigateToChatCommand = new MvxCommand(NavigateToSelectedFriendChatAsync);
        }

        public async override void Prepare(User parameter)
        {
            await PrepareAsync(parameter);
        }

        public async Task PrepareAsync(User parameter)
        {
            _user = parameter;
            await LoadUserFriends();
        }

        public IAsyncHook TestHook { get; set; }

        public MvxCommand SearchForUserCommand { get; }

        public MvxCommand AddUserToFriendListCommand { get; }

        public MvxCommand NavigateToChatCommand { get; }

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

        public UserViewModel SelectedNewFriend { get; set; }

        public FriendViewModel SelectedFriend { get; set; }

        private async Task LoadUserFriends()
        {
            SetupFriendList();
            await _userRepository.GetFriendList(_user);
        }

        private void SetupFriendList()
        {
            _user.FriendList.CollectionChanged += FriendList_CollectionChanged;
        }
       
        private void SearchForUserAsync()
        {
            _task.Run(async () => await SearchForUser());
        }

        public async Task SearchForUser()
        {
            var users = await _userRepository.FindUsers(UserSearchText);
            UpdateSearchUsers(users);
        }

        private void AddUserToFriendListAsync()
        {
            _task.Run( async () => await AddUserToFriendList());
        }

        public async Task AddUserToFriendList()
        {
            var friend = new Friend
            {
                EmailAddress = SelectedNewFriend.EmailAddress
            };

            await _userRepository.AddUserToFriendList(_user, friend);

            SelectedNewFriend = null;
        }

        private void NavigateToSelectedFriendChatAsync()
        {
            _task.Run(async () => await NavigateToSelectedFriendChat());
        }

        public async Task NavigateToSelectedFriendChat()
        {
            if (SelectedFriend == null)
            {
                return;
            }

            var chatFriend = await _userRepository.GetUser(SelectedFriend.EmailAddress);

            var chat = _chatRepository.GetChat(_user, chatFriend);

            await _mvxNavigationService.Navigate<ChatViewModel, ChatParameters>(new ChatParameters
            {
                Chat = chat
            });
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
