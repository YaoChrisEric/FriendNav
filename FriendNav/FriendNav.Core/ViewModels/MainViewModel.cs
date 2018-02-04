using FriendNav.Core.Services.Interfaces;
using MvvmCross.Core.Navigation;
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
        private readonly IMvxNavigationService _mvxNavigationService;

        public MainViewModel(IFirebaseAuthService firebaseAuthService, IMvxNavigationService mvxNavigationService)
        {
            _firebaseAuthService = firebaseAuthService;
            _mvxNavigationService = mvxNavigationService;
            LoginUserCommand = new MvxCommand(LoginUser);
            RegisterUserCommand = new MvxCommand(RegisterUser);
        }

        public async override Task Initialize()
        {
            await base.Initialize();

            if (_firebaseAuthService.FirebaseAuth != null && !_firebaseAuthService.FirebaseAuth.IsExpired())
            {
                _mvxNavigationService.Navigate<FriendListViewModel>().Wait();
                return;
            }
        }

        public MvxCommand LoginUserCommand { get; }

        public MvxCommand RegisterUserCommand { get; }

        private void LoginUser()
        {
            _mvxNavigationService.Navigate<LoginViewModel>().Wait();
        }

        private void RegisterUser()
        {
            _mvxNavigationService.Navigate<RegisterViewModel>().Wait();
        }
     }
}
