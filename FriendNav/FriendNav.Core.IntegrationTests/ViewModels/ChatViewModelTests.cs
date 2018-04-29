using Autofac;
using FriendNav.Core.IntegrationTests.Services;
using FriendNav.Core.IntegrationTests.TestModel;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.ViewModelParameters;
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
        public async Task Send_request_for_navigation()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var messageRepository = context.TestContainer.Resolve<IMessageRepository>();
            var navigateRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var testNavigationRequestService = context.TestContainer.Resolve<TestNavigationRequestService>();
            var chatViewModel = context.TestContainer.Resolve<ChatViewModel>();
            var receivingChatViewModel = context.TestContainer.Resolve<ChatViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = await userRepository.GetUser("c@test.com");

            var responder = await userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);
            
            await chatViewModel.PrepareAsync(new ChatParameters { Chat = chat });

            await chatViewModel.SendNavigationRequest();

            var navigateRequestNavigation = context.TestNavigationService.TestNavigations.First(f => f.Parameter is NavigateRequestParameters);

            var navigateRequestParameters = (NavigateRequestParameters)navigateRequestNavigation.Parameter;

            Assert.AreEqual(initiator.EmailAddress, navigateRequestParameters.NavigateRequest.InitiatorEmail);

            userRepository.Dispose();
            messageRepository.Dispose();
            navigateRequestRepository.Dispose();
        }

        [TestMethod]
        public async Task Recieve_incoming_navigation_request()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var messageRepository = context.TestContainer.Resolve<IMessageRepository>();
            var navigateRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var testNavigationRequestService = context.TestContainer.Resolve<TestNavigationRequestService>();
            var chatViewModel = context.TestContainer.Resolve<ChatViewModel>();
            var receivingChatViewModel = context.TestContainer.Resolve<ChatViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = await userRepository.GetUser("c@test.com");

            var responder = await userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            await testNavigationRequestService.ResetNavigationRequest(chat);

            await chatViewModel.PrepareAsync(new ChatParameters { Chat = chat });

            var testHook = new NavigateRequestHook();

            chatViewModel.TestNavigationHook = testHook;

            await testNavigationRequestService.SendTestNavigationRequest(chat);

            testHook.ResetEvent.WaitOne();

            var navigateRequestNavigation = context.TestNavigationService.TestNavigations.First(f => f.Parameter is NavigateRequestParameters);

            var navigateRequestParameters = (NavigateRequestParameters)navigateRequestNavigation.Parameter;

            Assert.AreEqual(responder.EmailAddress, navigateRequestParameters.NavigateRequest.InitiatorEmail);

            userRepository.Dispose();
            messageRepository.Dispose();
            navigateRequestRepository.Dispose();
        }

        [TestMethod]
        public async Task Add_new_chat_message_to_chat()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var messageRepository = context.TestContainer.Resolve<IMessageRepository>();
            var navigateRequestRepository = context.TestContainer.Resolve<INavigateRequestRepository>();
            var chatViewModel = context.TestContainer.Resolve<ChatViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = await userRepository.GetUser("c@test.com");

            var responder = await userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder);

            await chatViewModel.PrepareAsync(new ChatParameters { Chat = chat });

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

            await messageRepository.DeleteMessage( 
                chat.Messages.First(f => f.FirebaseKey == testHook
                    .CapturedTestMessage
                    .FirebaseKey));

            userRepository.Dispose();
            messageRepository.Dispose();
            navigateRequestRepository.Dispose();
        }
    }
}
