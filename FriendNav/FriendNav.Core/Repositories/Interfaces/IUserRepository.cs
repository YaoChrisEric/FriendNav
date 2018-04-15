using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Repositories.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task CreateUser(User user);
        Task<User> GetUser(string emailAddress);
        Task<List<User>> FindUsers(string emailPart);
        Task GetFriendList(User user);
        Task AddUserToFriendList(User user, Friend newFriend);
        Task RemoveUserFromFriendList(User user, Friend removeFriend);
    }
}
