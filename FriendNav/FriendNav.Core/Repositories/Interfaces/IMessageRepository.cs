using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Repositories.Interfaces
{
    public interface IMessageRepository : IDisposable
    {
        Task GetMessages(Chat chat);
        Task CreateMessage(Message message);
        Task DeleteMessage(Message message);
    }
}
