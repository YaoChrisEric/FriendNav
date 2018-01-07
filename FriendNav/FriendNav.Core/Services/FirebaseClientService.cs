using Firebase.Database;
using FriendNav.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Services
{
    public class FirebaseClientService : IFirebaseClientService
    {
        private readonly IFirebaseAuthService _firebaseAuthService;

        public FirebaseClientService(IFirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
        }

        public FirebaseClient CreateFirebaseClient()
        {
            if (_firebaseAuthService.FirebaseAuth == null)
            {
                throw new Exception("User not logged in");
            }

            return new FirebaseClient(
                "https://friendnavigation-ac7bb.firebaseio.com",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(_firebaseAuthService.FirebaseAuth.FirebaseToken)
                });
        }
    }
}
