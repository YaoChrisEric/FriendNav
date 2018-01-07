using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class RegisterViewModel : MvxViewModel
    {
        private IMvxNavigationService _mvxNavigationService;
        private INotificationService _notificationService;
        private IFirebaseAuthService _firebaseAuthService;
        private IUserRepository _userRepository;

        public RegisterViewModel(
            IMvxNavigationService mvxNavigationService,
            INotificationService notificationService, 
            IFirebaseAuthService firebaseAuthService,
            IUserRepository userRepository)
        {
            _mvxNavigationService = mvxNavigationService;
            _firebaseAuthService = firebaseAuthService;
            _notificationService = notificationService;
            _userRepository = userRepository;
            RegisterUserCommand = new MvxCommand(RegisterUser);
        }

        public MvxCommand RegisterUserCommand { get; }

        public string EmailAddress { get; set; }

        public string UserPassword { get; set; }

        private void RegisterUser()
        {
            if (string.IsNullOrWhiteSpace(EmailAddress) || string.IsNullOrWhiteSpace(UserPassword))
            {
                return;
            }

            _firebaseAuthService.CreateNewUser(EmailAddress, UserPassword);

            if (_firebaseAuthService.FirebaseAuth != null)
            {
                var newUser = new User
                {
                    EmailAddress = EmailAddress
                };

                _userRepository.CreateUser(newUser);

                _mvxNavigationService.Navigate<FriendListViewModel, User>(newUser);

                return;
            }

            _notificationService.SendNotification("Failed to create account");
        }
    }
}
