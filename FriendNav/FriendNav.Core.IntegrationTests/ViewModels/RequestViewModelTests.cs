using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FriendNav.Core.IntegrationTests.TestModel;
using FriendNav.Core.ViewModels;
using Autofac;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.IntegrationTests.Utilities;
using Moq;
using FriendNav.Core.Model;
using FriendNav.Core.ViewModelParameters;
using System.Linq;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.ViewModels
{
    [TestClass]
    public class RequestViewModelTests
    {
        public RequestViewModelTests()
        {
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task Accept_navigation_request()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var mapRepository = context.TestContainer.Resolve<IMapRepository>();
            var navigationRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var requestNavigationService = context.TestContainer.Resolve<INavigationRequestService>();

            var sut = context.TestContainer.Resolve<RequestViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = await userRepository.GetUser("c@test.com");

            var responder = await userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            var navigationRequest = await navigationRequestRepository.GetNavigationRequest(chat);

            sut.Prepare(new NavigateRequestParameters { Chat = chat, NavigateRequest = navigationRequest });

            await sut.AcceptRequest();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(a => a.Parameter is Map));

            userRepository.Dispose();
            navigationRequestRepository.Dispose();
            mapRepository.Dispose();
        }

        [TestMethod]
        public async Task Accept_incoming_navigation_request()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var mapRepository = context.TestContainer.Resolve<IMapRepository>();
            var navigationRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var requestNavigationService = context.TestContainer.Resolve<INavigationRequestService>();

            var sut = context.TestContainer.Resolve<RequestViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = await userRepository.GetUser("c@test.com");

            var responder = await userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            var otherChat = chatRepository.GetChat(responder, initiator);

            var navigationRequest = await navigationRequestRepository.GetNavigationRequest(chat);
            var otherNavigationRequest = await navigationRequestRepository.GetNavigationRequest(otherChat);

            var testHook = new NavigateRequestHook();

            sut.AcceptedHook = testHook;

            sut.Prepare(new NavigateRequestParameters { Chat = chat, NavigateRequest = navigationRequest });

            await requestNavigationService.InitiatNavigationRequest(navigationRequest);
            await requestNavigationService.AcceptNavigationRequest(otherNavigationRequest);

            testHook.ResetEvent.WaitOne();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(a => a.Parameter is Map));

            userRepository.Dispose();
            navigationRequestRepository.Dispose();
            mapRepository.Dispose();
        }

        [TestMethod]
        public async Task Incoming_decline_of_request()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var navigationRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var requestNavigationService = context.TestContainer.Resolve<INavigationRequestService>();

            var sut = context.TestContainer.Resolve<RequestViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = await userRepository.GetUser("c@test.com");

            var responder = await userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            var otherChat = chatRepository.GetChat(responder, initiator);

            var navigateRequest = await navigationRequestRepository.GetNavigationRequest(chat);
            var otherNavigateRequest = await navigationRequestRepository.GetNavigationRequest(otherChat);

            var testHook = new NavigateRequestHook();

            sut.TestHook = testHook;

            sut.Prepare(new NavigateRequestParameters { Chat = chat, NavigateRequest = navigateRequest });

            await requestNavigationService.DeclineNavigationRequest(otherNavigateRequest);

            testHook.ResetEvent.WaitOne();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(a => a.Parameter is ChatParameters));
            Assert.AreEqual(string.Empty, navigateRequest.InitiatorEmail);
            Assert.AreEqual(false, navigateRequest.IsNavigationActive);

            userRepository.Dispose();
            navigationRequestRepository.Dispose();
        }

        [TestMethod]
        public async Task User_decline_request()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var navigationRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var requestNavigationService = context.TestContainer.Resolve<INavigationRequestService>();

            var sut = context.TestContainer.Resolve<RequestViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = await userRepository.GetUser("c@test.com");

            var responder = await userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            var navigateRequest = await navigationRequestRepository.GetNavigationRequest(chat);

            sut.Prepare(new NavigateRequestParameters { Chat = chat, NavigateRequest = navigateRequest });

            await sut.DeclineRequest();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(a => a.Parameter is ChatParameters));
            Assert.AreEqual(string.Empty, navigateRequest.InitiatorEmail);
            Assert.AreEqual(false, navigateRequest.IsNavigationActive);

            userRepository.Dispose();
            navigationRequestRepository.Dispose();
        }
    }
}
