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
            LoginUserCommand = new MvxCommand(LoginUserAsync);
            RegisterUserCommand = new MvxCommand(RegisterUserAsync);
        }

        public async override Task Initialize()
        {
            await base.Initialize();

            if (_firebaseAuthService.FirebaseAuth != null && !_firebaseAuthService.FirebaseAuth.IsExpired())
            {
                await _mvxNavigationService.Navigate<FriendListViewModel>();
            }
        }

        public MvxCommand LoginUserCommand { get; }

        public MvxCommand RegisterUserCommand { get; }

        public void LoginUserAsync()
        {
            Task.Run(LoginUser);
        }

        public void RegisterUserAsync()
        {
            Task.Run(RegisterUser);
        }

        public async Task LoginUser()
        {
            await _mvxNavigationService.Navigate<LoginViewModel>();
        }

        public async Task RegisterUser()
        {
            await _mvxNavigationService.Navigate<RegisterViewModel>();
        }
     }
}
