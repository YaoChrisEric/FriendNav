using FriendNav.Core.Services.Interfaces;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private INotificationService _notificationService;
        private IFirebaseAuthService _firebaseAuthService;

        public LoginViewModel(INotificationService notificationService, IFirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
            _notificationService = notificationService;
            LoginUserCommand = new MvxCommand(LoginUser);
        }

        public MvxCommand LoginUserCommand { get; }

        public string EmailAddress { get; set; }

        public string UserPassword { get; set; }

        private void LoginUser()
        {
            if (string.IsNullOrWhiteSpace(EmailAddress) || string.IsNullOrWhiteSpace(UserPassword))
            {
                return;
            }

            _firebaseAuthService.LoginUser(EmailAddress, UserPassword);

            if (_firebaseAuthService.FirebaseAuth != null)
            {
                ShowViewModel<FriendListViewModel>();
                return;
            }

            _notificationService.SendNotification("Invalid username/password, Login Failed");
        }
    }
}
