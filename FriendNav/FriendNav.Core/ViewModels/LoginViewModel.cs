using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly ITask _task;
        private readonly IMvxNavigationService _mvxNavigationService;
        private readonly INotificationService _notificationService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IUserRepository _userRepository;


        public LoginViewModel(
            ITask task,
            IMvxNavigationService mvxNavigationService,
            IUserRepository userRepository,
            INotificationService notificationService,           
            IFirebaseAuthService firebaseAuthService)
        {
            _task = task;
            _mvxNavigationService = mvxNavigationService;
            _userRepository = userRepository;
            _firebaseAuthService = firebaseAuthService;
            _notificationService = notificationService;
            LoginUserCommand = new MvxCommand(LoginUserAsync);
        }

        public MvxCommand LoginUserCommand { get; }

        public string EmailAddress { get; set; }

        public string UserPassword { get; set; }

        private void LoginUserAsync()
        {
            _task.Run(LoginUser);
        }

        private void LoginUser()
        {
            if (string.IsNullOrWhiteSpace(EmailAddress) || string.IsNullOrWhiteSpace(UserPassword))
            {
                return;
            }

            _firebaseAuthService.LoginUser(EmailAddress, UserPassword);

            if (_firebaseAuthService.FirebaseAuth != null)
            {
                var user = _userRepository.GetUser(_firebaseAuthService.FirebaseAuth.User.Email);

                _mvxNavigationService.Navigate<FriendListViewModel, User>(user).Wait();
                return;
            }

            _notificationService.SendNotification("Invalid username/password, Login Failed");
        }
    }
}
