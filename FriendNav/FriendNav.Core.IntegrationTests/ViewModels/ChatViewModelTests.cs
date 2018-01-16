using Autofac;
using FriendNav.Core.IntegrationTests.TestModel;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var chatViewModel = context.TestContainer.Resolve<ChatViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder, true);

            chatViewModel.Prepare(chat);

            Assert.AreNotEqual(0, chatViewModel.Messages.Count);

            userRepository.Dispose();
            messageRepository.Dispose();
        }

        [TestMethod]
        public void Add_new_chat_message_to_chat()
        {
            var context = TestAppContext.ConstructTestAppContext();

            var firebaseAuthService = context.TestContainer.Resolve<IFirebaseAuthService>();
            var userRepository = context.TestContainer.Resolve<IUserRepository>();
            var chatRepository = context.TestContainer.Resolve<IChatRepository>();
            var messageRepository = context.TestContainer.Resolve<IMessageRepository>();
            var chatViewModel = context.TestContainer.Resolve<ChatViewModel>();

            firebaseAuthService.LoginUser("c@test.com", "theday");

            var initiator = userRepository.GetUser("c@test.com");

            var responder = userRepository.GetUser("c1@test.com");

            var chat = chatRepository.GetChat(initiator, responder, true);

            chatViewModel.Prepare(chat);

            var testMessage = Guid.NewGuid().ToString();

            chatViewModel.ActiveMessage = testMessage;

            var testHook = new ChatViewModelHook
            {
                ViewModel = chatViewModel,
                TestMessage = testMessage,
                ActiveTestUser = initiator
            };

            chatViewModel.TestHook = testHook;
            chatViewModel.AddNewMessageCommand.Execute();

            testHook.ResetEvent.WaitOne();

            messageRepository.DeleteMessage(chat, 
                chat.Messages.First(f => f.FirebaseKey == testHook
                    .CapturedTestMessage
                    .FirebaseKey));

            userRepository.Dispose();
            messageRepository.Dispose();
        }
    }
}
