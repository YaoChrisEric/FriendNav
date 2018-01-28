using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Repositories.Interfaces
{
    public interface IMessageRepository : IDisposable
    {
        void GetMessages(Chat chat);
        void CreateMessage(Message message);
        void DeleteMessage(Message message);
    }
}
