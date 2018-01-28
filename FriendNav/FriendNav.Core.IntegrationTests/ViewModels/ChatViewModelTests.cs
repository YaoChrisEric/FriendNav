using Autofac;
using FriendNav.Core.IntegrationTests.Services;
using FriendNav.Core.IntegrationTests.TestModel;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.ViewModels
{
    [TestClass]
    public class ChatViewModelTests
    {
        public ChatViewModelTests()
        {
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Upon_navigating_to_chat_load_messages()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var messageRepository = context.TestContainer.Resolve<IMessageRepository>();
            var navigateRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var chatViewModel = context.TestContainer.Resolve<ChatViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            chatViewModel.Prepare(chat);

            Assert.AreNotEqual(0, chatViewModel.Messages.Count);

            userRepository.Dispose();
            messageRepository.Dispose();
            navigateRequestRepository.Dispose();
        }

        [TestMethod]
        public void Upon_navigating_to_chat_setup_navigation_request()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var messageRepository = context.TestContainer.Resolve<IMessageRepository>();
            var navigateRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var chatViewModel = context.TestContainer.Resolve<ChatViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            chatViewModel.Prepare(chat);

            Assert.IsNotNull(chat.NavigateRequest?.ActiveUser);
            Assert.AreEqual(initiator, chat.NavigateRequest.ActiveUser);

            userRepository.Dispose();
            messageRepository.Dispose();
            navigateRequestRepository.Dispose();
        }

        [TestMethod]
        public void Send_request_for_navigation()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var messageRepository = context.TestContainer.Resolve<IMessageRepository>();
            var navigateRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var testNavigationRequestService = context.TestContainer.Resolve<TestNavigationRequestService>();
            var chatViewModel = context.TestContainer.Resolve<ChatViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            chatViewModel.Prepare(chat);

            var testHook = new NavigateRequestHook();

            chat.NavigateRequest.TestHook = testHook;

            chatViewModel.SendNavigationRequestCommand.Execute();

            testHook.ResetEvent.WaitOne();

            Assert.AreEqual(initiator.EmailAddress, chat.NavigateRequest.InitiatorEmail);

            context.MockNavigationService.Verify(v => v.Navigate<RequestViewModel, Chat>(It.IsAny<Chat>(), null));

            userRepository.Dispose();
            messageRepository.Dispose();
            navigateRequestRepository.Dispose();
        }

        [TestMethod]
        public void Recieve_incoming_navigation_request()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var messageRepository = context.TestContainer.Resolve<IMessageRepository>();
            var navigateRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var testNavigationRequestService = context.TestContainer.Resolve<TestNavigationRequestService>();
            var chatViewModel = context.TestContainer.Resolve<ChatViewModel>();


            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            chatViewModel.Prepare(chat);

            var testHook = new NavigateRequestHook();

            chatViewModel.TestNavigationHook = testHook;

            testNavigationRequestService.SendTestNavigationRequest(chat);

            testHook.ResetEvent.WaitOne();

            Assert.AreEqual(responder.EmailAddress, chat.NavigateRequest.InitiatorEmail);

            context.MockNavigationService.Verify(v => v.Navigate<RequestViewModel, Chat>(It.IsAny<Chat>(), null));

            userRepository.Dispose();
            messageRepository.Dispose();
            navigateRequestRepository.Dispose();
        }

        [TestMethod]
        public void Add_new_chat_message_to_chat()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var messageRepository = context.TestContainer.Resolve<IMessageRepository>();
            var navigateRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var chatViewModel = context.TestContainer.Resolve<ChatViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            chatViewModel.Prepare(chat);

            var testMessage = Guid.NewGuid().ToString();

            chatViewModel.ActiveMessage = testMessage;

            var testHook = new ChatViewModelHook
            {
                ViewModel = chatViewModel,
                TestMessage = testMessage,
                ActiveTestUser = initiator
            };

            chatViewModel.TestChatMessageHook = testHook;
            chatViewModel.AddNewMessageCommand.Execute();

            testHook.ResetEvent.WaitOne();

            Assert.IsNotNull(testHook.CapturedTestMessage);
            Assert.AreEqual(testHook.ActiveTestUser.EmailAddress, testHook.CapturedTestMessage.SenderEmail);

            messageRepository.DeleteMessage( 
                chat.Messages.First(f => f.FirebaseKey == testHook
                    .CapturedTestMessage
                    .FirebaseKey));

            userRepository.Dispose();
            messageRepository.Dispose();
            navigateRequestRepository.Dispose();
        }
    }
}
