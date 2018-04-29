using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _mvxNavigationService;
        private readonly INotificationService _notificationService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IUserRepository _userRepository;


        public LoginViewModel(
            IMvxNavigationService mvxNavigationService,
            IUserRepository userRepository,
            INotificationService notificationService,           
            IFirebaseAuthService firebaseAuthService)
        {
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
            Task.Run(LoginUser);
        }

        public async Task LoginUser()
        {
            if (string.IsNullOrWhiteSpace(EmailAddress) || string.IsNullOrWhiteSpace(UserPassword))
            {
                return;
            }

            _firebaseAuthService.LoginUser(EmailAddress, UserPassword);

            if (_firebaseAuthService.FirebaseAuth != null)
            {
                var user = await _userRepository.GetUser(_firebaseAuthService.FirebaseAuth.User.Email);

                await _mvxNavigationService.Navigate<FriendListViewModel, User>(user);
                return;
            }

            _notificationService.SendNotification("Invalid username/password, Login Failed");
        }
    }
}
