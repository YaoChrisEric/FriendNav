using FriendNav.Core.Services.Interfaces;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IFirebaseAuthService _firebaseAuthService;

        public MainViewModel(IFirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
            LoginUserCommand = new MvxCommand(LoginUser);
            RegisterUserCommand = new MvxCommand(RegisterUser);
        }

        public async override Task Initialize()
        {
            await base.Initialize();

            if (_firebaseAuthService.FireBaseAuth != null && !_firebaseAuthService.FireBaseAuth.IsExpired())
            {
                ShowViewModel<FriendListViewModel>();
                return;
            }
        }

        public MvxCommand LoginUserCommand { get; }

        public MvxCommand RegisterUserCommand { get; }

        private void LoginUser()
        {
            ShowViewModel<LoginViewModel>();
        }

        private void RegisterUser()
        {
            ShowViewModel<RegisterViewModel>();
        }
     }
}
