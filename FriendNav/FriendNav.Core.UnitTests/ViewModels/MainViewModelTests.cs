﻿using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Core.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Firebase.Auth;

namespace FriendNav.Core.IntegrationTests.ViewModels
{
    [TestClass]
    public class MainViewModelTests
    {
        private IFixture _fixture = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture()
                .Customize(new AutoConfiguredMoqCustomization());
        }

        [TestMethod]
        public async Task MainView_Initialize_fail()
        {
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();
            var _mvxNavigationService = new Mock<IMvxNavigationService>();

            MainViewModel mainViewModel = new MainViewModel(
                _firebaseAuthService.Object,
                _mvxNavigationService.Object
                );

            await mainViewModel.Initialize();

            _mvxNavigationService.Verify(x => x.Navigate<FriendListViewModel>(null), Times.Never());
        }

        [TestMethod]
        public async Task MainView_Initialize_success()
        {
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();
            var _mvxNavigationService = new Mock<IMvxNavigationService>();
            var firebaseAuth = new FirebaseAuth
            {
                Created = DateTime.Now,
                ExpiresIn = 10000
            };

            _firebaseAuthService.SetupGet(x => x.FirebaseAuth).Returns(firebaseAuth);
            _mvxNavigationService.Setup(x => x.Navigate<FriendListViewModel>(null))
                .Returns(() =>
                {
                    var task = new Task(() => { });
                    task.RunSynchronously();
                    return task;
                });

            MainViewModel mainViewModel = new MainViewModel(
                _firebaseAuthService.Object,
                _mvxNavigationService.Object
                );

            await mainViewModel.Initialize();

            _mvxNavigationService.Verify(x => x.Navigate<FriendListViewModel>(null));
        }

        [TestMethod]
        public async Task Login_user_command()
        {
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();
            var _mvxNavigationService = new Mock<IMvxNavigationService>();

            _mvxNavigationService.Setup(x => x.Navigate<LoginViewModel>(null))
                .Returns(() =>
                {
                    return Task.Run(() => { });
                });

            var mainViewModel = new MainViewModel(
                _firebaseAuthService.Object,
                _mvxNavigationService.Object
                );

            await mainViewModel.LoginUser();

            _mvxNavigationService.Verify(x => x.Navigate<LoginViewModel>(null));
        }

        [TestMethod]
        public async Task Register_user_command()
        {
            var _firebaseAuthService = new Mock<IFirebaseAuthService>();
            var _mvxNavigationService = new Mock<IMvxNavigationService>();

            _mvxNavigationService.Setup(x => x.Navigate<RegisterViewModel>(null))
                .Returns(() =>
                {
                    return Task.Run(() => { });
                });

            MainViewModel mainViewModel = new MainViewModel(
                _firebaseAuthService.Object,
                _mvxNavigationService.Object
                );

            await mainViewModel.RegisterUser();

            _mvxNavigationService.Verify(x => x.Navigate<RegisterViewModel>(null));
        }
    }
}
