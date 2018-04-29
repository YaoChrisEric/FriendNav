using Firebase.Auth;
using FriendNav.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly IFirebaseAuthProvider _firebaseAuthProvider;

        public FirebaseAuthService(IFirebaseAuthProvider firebaseAuthProvider)
        {
            _firebaseAuthProvider = firebaseAuthProvider;
        }

        public FirebaseAuth FirebaseAuth { get; private set; }

        public async Task<bool> CreateNewUser(string email, string password)
        {
            FirebaseAuth = await _firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(email, password);

            return true;
        }

        public void LoginUser(string email, string password)
        {
            FirebaseAuth = _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password).Result;
        }
    }
}
