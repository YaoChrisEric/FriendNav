using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUser(User user);
        Task<User> GetUser(string emailAddress);
    }
}
