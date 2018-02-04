using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FriendNav.Core.ViewModels;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using Moq;
using MvvmCross.Core.Navigation;
using FriendNav.Core.Model;

namespace FriendNav.Core.Tests.ViewModels
{
    [TestClass]
    public class RequestViewModelUnitTests
    {
        private IFixture _fixture = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture()
                .Customize(new AutoConfiguredMoqCustomization());
        }
        [TestMethod]
        public void User_Accepting_Nav_Request_Unit_Test()
        {
            var _navigationRequestService = new Mock<INavigationRequestService>();
            var _mapRepository = new Mock<IMapRepository>();

            var _mvxNavigationService = new Mock<IMvxNavigationService>();

            var chat = _fixture.Create<Chat>();

            var sut = new RequestViewModel(new IntegrationTests.Utilities.TestTask(),
                _navigationRequestService.Object,
                _mapRepository.Object,
                _mvxNavigationService.Object
                );
            sut.Prepare(chat);

            sut.AcceptRequestCommand.Execute();

            _mvxNavigationService.Verify(v => v.Navigate<MapViewModel, Map>(It.IsAny<Map>(), null));
        }

        [TestMethod]
        public void User_Decline_NavRequest_Unit_Test()
        {
            var _navigationRequestService = new Mock<INavigationRequestService>();
            var _mapRepository = new Mock<IMapRepository>();

            var _mvxNavigationService = new Mock<IMvxNavigationService>();

            var chat = _fixture.Create<Chat>();


            var sut = new RequestViewModel(new IntegrationTests.Utilities.TestTask(),
                _navigationRequestService.Object,
                _mapRepository.Object,
                _mvxNavigationService.Object
                );
            sut.Prepare(chat);

            sut.DeclineRequestCommand.Execute();

            _mvxNavigationService.Verify(v => v.Navigate<ChatViewModel, Chat>(It.Is<Chat>(i => i == chat), null));

        }
    }
}
