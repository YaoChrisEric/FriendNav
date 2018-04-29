using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Services.Interfaces
{
    public interface IFirebaseAuthService
    {
        void LoginUser(string email, string password);
        Task<bool> CreateNewUser(string email, string password);
        FirebaseAuth FirebaseAuth { get; }
    }
}
