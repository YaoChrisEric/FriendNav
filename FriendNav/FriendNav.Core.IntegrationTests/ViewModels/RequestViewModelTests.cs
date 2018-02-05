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
        public void Accept_navigation_request()
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

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            var navigationRequest = navigationRequestRepository.GetNavigationRequest(chat);

            sut.Prepare(new NavigateRequestParameters { Chat = chat, NavigateRequest = navigationRequest });

            sut.AcceptRequestCommand.Execute();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(a => a.Parameter is Map));

            userRepository.Dispose();
            navigationRequestRepository.Dispose();
            mapRepository.Dispose();
        }

        [TestMethod]
        public void Accept_incoming_navigation_request()
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

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            var otherChat = chatRepository.GetChat(responder, initiator);

            var navigationRequest = navigationRequestRepository.GetNavigationRequest(chat);
            var otherNavigationRequest = navigationRequestRepository.GetNavigationRequest(otherChat);

            var testHook = new NavigateRequestHook();

            sut.AcceptedHook = testHook;

            sut.Prepare(new NavigateRequestParameters { Chat = chat, NavigateRequest = navigationRequest });

            requestNavigationService.InitiatNavigationRequest(otherNavigationRequest);

            testHook.ResetEvent.WaitOne();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(a => a.Parameter is Map));

            userRepository.Dispose();
            navigationRequestRepository.Dispose();
            mapRepository.Dispose();
        }

        [TestMethod]
        public void Incoming_decline_of_request()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var navigationRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var requestNavigationService = context.TestContainer.Resolve<INavigationRequestService>();

            var sut = context.TestContainer.Resolve<RequestViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            var otherChat = chatRepository.GetChat(responder, initiator);

            var navigateRequest = navigationRequestRepository.GetNavigationRequest(chat);
            var otherNavigateRequest = navigationRequestRepository.GetNavigationRequest(otherChat);

            var testHook = new NavigateRequestHook();

            sut.TestHook = testHook;

            sut.Prepare(new NavigateRequestParameters { Chat = chat, NavigateRequest = navigateRequest });

            requestNavigationService.DeclineNavigationRequest(otherNavigateRequest);

            testHook.ResetEvent.WaitOne();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(a => a.Parameter is ChatParameters));
            Assert.AreEqual(string.Empty, navigateRequest.InitiatorEmail);
            Assert.AreEqual(false, navigateRequest.IsNavigationActive);

            userRepository.Dispose();
            navigationRequestRepository.Dispose();
        }

        [TestMethod]
        public void User_decline_request()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var navigationRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var requestNavigationService = context.TestContainer.Resolve<INavigationRequestService>();

            var sut = context.TestContainer.Resolve<RequestViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            var navigateRequest = navigationRequestRepository.GetNavigationRequest(chat);

            sut.Prepare(new NavigateRequestParameters { Chat = chat, NavigateRequest = navigateRequest });

            sut.DeclineRequestCommand.Execute();

            Assert.IsTrue(context.TestNavigationService.TestNavigations.Any(a => a.Parameter is ChatParameters));
            Assert.AreEqual(string.Empty, navigateRequest.InitiatorEmail);
            Assert.AreEqual(false, navigateRequest.IsNavigationActive);

            userRepository.Dispose();
            navigationRequestRepository.Dispose();
        }
    }
}
