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
        public void Adding_new_message_to_chat_test()
        {
            var chatViewModelTestRepository = new Mock<IMessageRepository>();
            var chat = _fixture.Create<Chat>();
            Message message = null;

            chatViewModelTestRepository.Setup(s => s.CreateMessage(It.IsAny<Message>()))
                .Callback<Message>(c => message = c);

            var sut = new ChatViewModel(new TestTask(), 
                new Mock<INavigateRequestRepository>().Object, 
                null, 
                chatViewModelTestRepository.Object, 
                null);

            sut.Prepare(chat);

            var messageText = "Test";

            sut.ActiveMessage = messageText;
        
            sut.AddNewMessageCommand.Execute();

            chatViewModelTestRepository.Verify(v => v.CreateMessage(It.Is<Message>(i => i == message)));
            Assert.AreEqual(chat.FirebaseKey, message.ChatFirebaseKey);
            Assert.AreEqual(chat.ActiveUser.EmailAddress, message.SenderEmail);
            Assert.AreEqual(messageText, message.Text);
        }
    }
}
