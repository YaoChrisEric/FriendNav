using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.ViewModels;
using Moq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FriendNav.Core.Model;
using FriendNav.Core.Utilities;
using FriendNav.Core.IntegrationTests.Utilities;
using MvvmCross.Core.Navigation;
using FriendNav.Core.ViewModelParameters;
using System.Threading.Tasks;

namespace FriendNav.Core.Tests.ViewModels
{
    [TestClass]
    public class ChatViewModelTest
    {
        private IFixture _fixture = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture()
                .Customize(new AutoConfiguredMoqCustomization());
        }

        [TestMethod]
        public async Task Upon_navigating_to_chat_load_messages_unit_test()
        {
            var _navigateRequestRepository = new Mock<INavigateRequestRepository>();
            var _navigationRequestService = new Mock<INavigationRequestService>();
            var _mvxNavigationService = new Mock<IMvxNavigationService>();
            var _messageRepository = new Mock<IMessageRepository>();

            _navigateRequestRepository.Setup(s => s.GetNavigationRequest(It.IsAny<Chat>())).Returns(Task.Run(() => new Mock<NavigateRequest>().Object));

            var chat = _fixture.Create<Chat>();

            var sut = new ChatViewModel(new TestTask(),
                _navigateRequestRepository.Object,
                _navigationRequestService.Object,
                _messageRepository.Object,
                _mvxNavigationService.Object
                );

            await sut.PrepareAsync(new ChatParameters { Chat = chat });

            _messageRepository.Verify(v => v.GetMessages(It.Is<Chat>(c => c == chat)));
            _navigateRequestRepository.Verify(v => v.GetNavigationRequest(It.Is<Chat>(c => c == chat)));
        }

        [TestMethod]
        public async Task Adding_new_message_to_chat_test()
        {
            var chatViewModelTestRepository = new Mock<IMessageRepository>();
            var chat = _fixture.Create<Chat>();
            var navigateRequest = _fixture.Create<NavigateRequest>();
            Message message = null;

            // here the callback is taking the same parameter from CreateMessage
            chatViewModelTestRepository.Setup(s => s.CreateMessage(It.IsAny<Message>()))
                .Callback<Message>(c => message = c)
                .Returns(() =>
                {
                    return Task.Run(() => { });
                });

            var sut = new ChatViewModel(new TestTask(), 
                new Mock<INavigateRequestRepository>().Object, 
                null, 
                chatViewModelTestRepository.Object, 
                null);

            await sut.PrepareAsync(new ChatParameters { Chat = chat, NavigateRequest = navigateRequest });

            var messageText = "Test";

            sut.ActiveMessage = messageText;

            await sut.CreateNewMessage();

            chatViewModelTestRepository.Verify(v => v.CreateMessage(It.Is<Message>(i => i == message)));
            Assert.AreEqual(chat.FirebaseKey, message.ChatFirebaseKey);
            Assert.AreEqual(chat.ActiveUser.EmailAddress, message.SenderEmail);
            Assert.AreEqual(messageText, message.Text);
            // TODO: verify that the timestamp is in correct date format, for now just make sure it is a string
            Assert.IsInstanceOfType(message.TimeStamp,typeof(String));
        }

        [TestMethod]
        public async Task Send_request_for_navigation_unit_test()
        {
            var _navigateRequestRepository = new Mock<INavigateRequestRepository>();
            var _navigationRequestService = new Mock<INavigationRequestService>();
            var _mvxNavigationService = new Mock<IMvxNavigationService>();
            var _messageRepository = new Mock<IMessageRepository>();

            var chatParematers = _fixture.Create<ChatParameters>();

            var sut = new ChatViewModel(new TestTask(),
                _navigateRequestRepository.Object,
                _navigationRequestService.Object,
                _messageRepository.Object,
                _mvxNavigationService.Object
                );

            await sut.PrepareAsync(chatParematers);

            await sut.SendNavigationRequest();

            _navigationRequestService.Verify(v => v.InitiatNavigationRequest(It.IsAny<NavigateRequest>()));
            // TODO: figure out the parameter meaning of following line, why null?
            _mvxNavigationService.Verify(v => v.Navigate<RequestViewModel, NavigateRequestParameters>(It.Is<NavigateRequestParameters>(c => c.Chat == chatParematers.Chat),null));
        }

    }
}
