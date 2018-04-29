using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.ViewModels
{
    public class RegisterViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _mvxNavigationService;
        private readonly INotificationService _notificationService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IUserRepository _userRepository;

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
            RegisterUserCommand = new MvxCommand(RegisterUserAsync);
        }

        public MvxCommand RegisterUserCommand { get; }

        public string EmailAddress { get; set; }

        public string UserPassword { get; set; }

        private void RegisterUserAsync()
        {
            Task.Run(RegisterUser);
        }

        public async Task RegisterUser()
        {
            if (string.IsNullOrWhiteSpace(EmailAddress) || string.IsNullOrWhiteSpace(UserPassword))
            {
                return;
            }

            await _firebaseAuthService.CreateNewUser(EmailAddress, UserPassword);

            if (_firebaseAuthService.FirebaseAuth != null)
            {
                var newUser = new User
                {
                    EmailAddress = EmailAddress
                };

                await _userRepository.CreateUser(newUser);

                await _mvxNavigationService.Navigate<FriendListViewModel, User>(newUser);

                return;
            }

            _notificationService.SendNotification("Failed to create account");
        }
    }
}
