using Firebase.Database.Query;
using FriendNav.Core.DataTransfer;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Repositories
{
    public class NavigateRequestRepository : INavigateRequestRepository
    {
        private readonly List<IDisposable> _disposable = new List<IDisposable>();

        private readonly IFirebaseClientService _firebaseClientService;

        public NavigateRequestRepository(IFirebaseClientService firebaseClientService)
        {
            _firebaseClientService = firebaseClientService;
        }

        public async Task<NavigateRequest> GetNavigationRequest(Chat chat)
        {
            var navigateRequest = new NavigateRequest
            {
                ActiveUser = chat.ActiveUser,
                ChatFirebaseKey = chat.FirebaseKey
            };

            var client = _firebaseClientService.CreateFirebaseClient();

            var navigateRequestDto = await client
                .Child("BasicChat")
                .Child(chat.FirebaseKey)
                .Child("meetRequest")
                .OnceSingleAsync<NavigateRequestDto>();

            if (navigateRequestDto == null)
            {
                navigateRequestDto = new NavigateRequestDto
                {
                    InitiatorEmail = string.Empty,
                };

                await client
                .Child("BasicChat")
                .Child(chat.FirebaseKey)
                .Child("meetRequest")
                .PutAsync(navigateRequestDto);
            }

            navigateRequest.InitiatorEmail = navigateRequestDto.InitiatorEmail;
            navigateRequest.IsNavigationActive = navigateRequestDto.CallActive;
            navigateRequest.IsRequestAccepted = navigateRequestDto.IsRequestedAccepted;
            navigateRequest.IsRequestDeclined = navigateRequestDto.IsRequestDeclined;

            var disposable = client
                .Child("BasicChat")
                .Child(chat.FirebaseKey)
                .Child("meetRequest")
                .AsObservable<NavigateRequestDto>(elementRoot: "meetRequest")
                .Subscribe(navigateRequest.IncomingNavigationRequest);

            _disposable.Add(disposable);

            return navigateRequest;
        }

        public async Task UpdateNavigationRequest(NavigateRequest navigateRequest)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            await client
                .Child("BasicChat")
                .Child(navigateRequest.ChatFirebaseKey)
                .Child("meetRequest")
                .PutAsync(new NavigateRequestDto
                {
                    InitiatorEmail = navigateRequest.InitiatorEmail,
                    CallActive = navigateRequest.IsNavigationActive,
                    IsRequestedAccepted = navigateRequest.IsRequestAccepted,
                    IsRequestDeclined = navigateRequest.IsRequestDeclined
                });
        }

        public void Dispose()
        {
            foreach (var dispose in _disposable)
            {
                dispose.Dispose();
            }
        }   
    }
}
