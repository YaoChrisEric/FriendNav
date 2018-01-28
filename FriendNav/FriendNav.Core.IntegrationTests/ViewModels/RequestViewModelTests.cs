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

            navigationRequestRepository.GetNavigationRequest(chat);
            navigationRequestRepository.GetNavigationRequest(otherChat);

            var testHook = new NavigateRequestHook();

            sut.TestHook = testHook;

            sut.Prepare(chat);

            requestNavigationService.DeclineNavigationRequest(otherChat.NavigateRequest);

            testHook.ResetEvent.WaitOne();

            context.MockNavigationService.Verify(v => v.Navigate<ChatViewModel, Chat>(It.Is<Chat>(i => i == chat), null));
            Assert.AreEqual(string.Empty, chat.NavigateRequest.InitiatorEmail);
            Assert.AreEqual(false, chat.NavigateRequest.IsNavigationActive);
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

            navigationRequestRepository.GetNavigationRequest(chat);

            sut.Prepare(chat);

            sut.DeclineRequestCommand.Execute();

            context.MockNavigationService.Verify(v => v.Navigate<ChatViewModel, Chat>(It.Is<Chat>(i => i == chat), null));
            Assert.AreEqual(string.Empty, chat.NavigateRequest.InitiatorEmail);
            Assert.AreEqual(false, chat.NavigateRequest.IsNavigationActive);
        }
    }
}
