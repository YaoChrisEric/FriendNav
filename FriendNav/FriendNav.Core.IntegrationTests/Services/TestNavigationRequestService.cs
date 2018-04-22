using Firebase.Database.Query;
using FriendNav.Core.DataTransfer;
using FriendNav.Core.Model;
using FriendNav.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.Services
{
    public class TestNavigationRequestService
    {
        private readonly IFirebaseClientService _firebaseClientService;
        
        public TestNavigationRequestService(IFirebaseClientService firebaseClientService)
        {
            _firebaseClientService = firebaseClientService;
        }

        public async Task ResetNavigationRequest(Chat chat)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            await client
                .Child("BasicChat")
                .Child(chat.FirebaseKey)
                .Child("meetRequest")
                .PutAsync(new NavigateRequestDto
                {
                    InitiatorEmail = "test"
                });
        }

        public async Task SendTestNavigationRequest(Chat chat)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            await client
                .Child("BasicChat")
                .Child(chat.FirebaseKey)
                .Child("meetRequest")
                .PutAsync(new NavigateRequestDto
                {
                    InitiatorEmail = chat.ChatUser.EmailAddress,
                    CallActive = true
                });
        }
    }
}
