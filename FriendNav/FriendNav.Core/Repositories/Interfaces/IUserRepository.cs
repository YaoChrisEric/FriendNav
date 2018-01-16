using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Repositories.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        void CreateUser(User user);
        User GetUser(string emailAddress);
        List<User> FindUsers(string emailPart);
        void GetFriendList(User user);
        void AddUserToFriendList(User user, Friend newFriend);
        void RemoveUserFromFriendList(User user, Friend removeFriend);
    }
}
