using FriendNav.Core.Services.Interfaces;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private IFirebaseAuthService _firebaseAuthService;

        public LoginViewModel(IFirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
            LoginUserCommand = new MvxCommand(LoginUser);
        }

        public MvxCommand LoginUserCommand { get; }

        public string EmailAddress { get; set; }

        public string UserPassword { get; set; }

        private void LoginUser()
        {
            _firebaseAuthService.LoginUser(EmailAddress, UserPassword);

            if (_firebaseAuthService.FirebaseAuth != null)
            {

            }
        }
    }
}
