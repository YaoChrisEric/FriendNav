using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.ViewModels;
using Moq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


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
            var chatViewModelTestRepository = _fixture.Freeze<Mock<IMessageRepository>>();
            var userRepository = _fixture.Freeze<Mock<IUserRepository>>();
            var chatRepository = _fixture.Freeze<Mock<IChatRepository>>();

            var sut = _fixture.Create<ChatViewModel>();




            sut.Prepare(chat);

            

            sut.AddNewMessageCommand.Execute();
            var message = chat.CreateNewMessage(sut.ActiveMessage);

            chatViewModelTestRepository.Verify(v => v.CreateMessage(It.Is<Model.Message>(i=>i==message)));
        }
    }
}
