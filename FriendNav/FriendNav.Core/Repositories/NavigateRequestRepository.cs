﻿using Firebase.Database.Query;
using FriendNav.Core.DataTransfer;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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

        public void GetNavigateRequest(Chat chat)
        {
            var navigateRequest = new NavigateRequest
            {
                ActiveUser = chat.ActiveUser,
                ChatFirebaseKey = chat.FirebaseKey
            };

            chat.NavigateRequest = navigateRequest;

            var client = _firebaseClientService.CreateFirebaseClient();

            var navigateRequestDto = client
                .Child("BasicChat")
                .Child(chat.FirebaseKey)
                .Child("meetRequest")
                .OnceSingleAsync<NavigateRequestDto>()
                .Result;

            if (navigateRequestDto == null)
            {
                navigateRequestDto = new NavigateRequestDto
                {
                    InitiatorEmail = string.Empty,
                    CallActive = false
                };

                client
                .Child("BasicChat")
                .Child(chat.FirebaseKey)
                .Child("meetRequest")
                .PutAsync(navigateRequestDto)
                .Wait();
            }

            navigateRequest.InitiatorEmail = navigateRequestDto.InitiatorEmail;
            navigateRequest.IsNavigationActive = navigateRequestDto.CallActive;

            var disposable = client
                .Child("BasicChat")
                .Child(chat.FirebaseKey)
                .Child("meetRequest")
                .AsObservable<NavigateRequestDto>()
                .Subscribe(navigateRequest.IncomingNavigationRequest);

            _disposable.Add(disposable);
        }

        public void SendNavigationRequest(NavigateRequest navigateRequest)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            client
                .Child("BasicChat")
                .Child(navigateRequest.ChatFirebaseKey)
                .Child("meetRequest")
                .PutAsync(new NavigateRequestDto
                {
                    InitiatorEmail = navigateRequest.ActiveUser.EmailAddress,
                    CallActive = false
                })
                .Wait();
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
