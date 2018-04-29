using Autofac;
using FriendNav.Core.IntegrationTests.TestModel;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Core.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.ViewModels
{
    [TestClass]
    public class MapViewModelTests
    {
        public MapViewModelTests()
        {
        }

        [TestMethod]
        public async Task Navigate_to_friend_list_view_model()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var mapRepository = context.TestContainer.Resolve<IMapRepository>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var navigateRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var navigationRequestService = context.TestContainer.Resolve<INavigationRequestService>();
            var mvxNavigationService = context.TestContainer.Resolve<IMvxNavigationService>();
            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();

            var mapViewModel = context.TestContainer.Resolve<MapViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            mapViewModel.Prepare(new Map());

            await mapViewModel.SendEndNavigationAndMarkAsEnded();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(a => a.Parameter is User));

            mapRepository.Dispose();
            userRepository.Dispose();
            navigateRequestRepository.Dispose();
        }

        [TestMethod]
        public async Task On_location_change()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var mapRepository = context.TestContainer.Resolve<IMapRepository>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var navigateRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var navigationRequestService = context.TestContainer.Resolve<INavigationRequestService>();
            var mvxNavigationService = context.TestContainer.Resolve<IMvxNavigationService>();
            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();

            var mapViewModel = context.TestContainer.Resolve<MapViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var map = await mapRepository.GetMap("c@test,comc1@test,com");

            mapViewModel.Prepare(map);

            var testHook = new MapViewModelHook
            {
                Map = new Map
                {
                    InitiatorLatitude = "499",
                    InitiatorLongitude = "499"
                }
            };

            mapViewModel.TestLocationChangeHook = testHook;

            await mapViewModel.OnLocationChanged();

            testHook.ResetEvent.WaitOne();

            Assert.AreEqual("498", testHook.Map.InitiatorLatitude);
            Assert.AreEqual("498", testHook.Map.InitiatorLongitude);

            mapRepository.Dispose();
            userRepository.Dispose();
            navigateRequestRepository.Dispose();
        }
    }
}
