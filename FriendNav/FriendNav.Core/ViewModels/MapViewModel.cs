using FriendNav.Core.DataTransfer;
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
    public class MapViewModel : MvxViewModel<Map>
    {
        private Map _map;
        private readonly ITask _task;
        private readonly IMapRepository _mapRepository;
        private readonly IUserRepository _userRepository;
        private readonly INavigateRequestRepository _navigateRequestRepository;
        private readonly INavigationRequestService _navigationRequestService;
        private readonly IMvxNavigationService _mvxNavigationService;
        private readonly IFirebaseAuthService _firebaseAuthService;

        private Boolean _endNavigation, _isCallingActivityInitiator;

        public MvxCommand SendNavigationFriendListRequestCommand { get; }
        public MvxCommand OnLocationChangeCommand { get; }

        public IAsyncHook TestNavigationHook { get; set; }
        public IAsyncHook TestLocationChangeHook { get; set; }

        public MapViewModel(ITask task,
            IMapRepository mapRepository,
            IUserRepository userRepository,
            INavigateRequestRepository navigateRequestRepository,
            INavigationRequestService navigationRequestService,
            IMvxNavigationService mvxNavigationService,
            IFirebaseAuthService firebaseAuthService
            )
        {
            _task = task;
            _mapRepository = mapRepository;
            _userRepository = userRepository;
            _navigateRequestRepository = navigateRequestRepository;
            _navigationRequestService = navigationRequestService;
            _mvxNavigationService = mvxNavigationService;
            _firebaseAuthService = firebaseAuthService;

            SendNavigationFriendListRequestCommand = new MvxCommand(SendEndNavigationAndMarkAsEnded);
            OnLocationChangeCommand = new MvxCommand(OnLocationChangedAsync);
        }

        public override void Prepare(Map parameter)
        {
            _map = parameter;
        }

        
        private void OnMapReady(Map map)
        {
            _map = _mapRepository.GetMap(_map.ChatFirebaseKey);
        }

        private void OnLocationChangedAsync()
        {
            _task.Run(OnLocationChanged);
        }

        // map argument is from google location
        private void OnLocationChanged()
        {
            if (!_endNavigation)
            {
                _map = _mapRepository.GetMap(_map.ChatFirebaseKey);

                TestLocationChangeHook?.NotifyOtherThreads();
            }
            return;
        }

        private void SendEndNavigationAndMarkAsEndedAsync()
        {
            _task.Run(SendEndNavigationAndMarkAsEnded);
        }

        private void SendEndNavigationAndMarkAsEnded()
        {
            if (_isCallingActivityInitiator)
            {
                _map.InitiatorLatitude = "500";
                _map.InitiatorLatitude = "500";
            }
            else
            {
                _map.ResponderLatitude = "500";
                _map.ResponderLongitude = "500";
            }


            NavigateToChatActivity();
        }

        private void NavigateToChatActivity()
        {
            var user = _userRepository.GetUser(_firebaseAuthService.FirebaseAuth.User.Email);
            _mvxNavigationService.Navigate<FriendListViewModel, User>(user).Wait();

            TestNavigationHook?.NotifyOtherThreads();
        }
    }
}
