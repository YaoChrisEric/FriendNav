using Firebase.Database.Query;
using FriendNav.Core.DataTransfer;
using FriendNav.Core.Model;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Repositories
{
    public class MapRepository : IMapRepository
    {
        private readonly List<IDisposable> _disposable = new List<IDisposable>();
        private readonly IFirebaseClientService _firebaseClientService;

        public MapRepository(IFirebaseClientService firebaseClientService)
        {
            _firebaseClientService = firebaseClientService;
        }

        public Map GetMap(string chatFirebaseKey)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            var mapDto = client
                .Child("BasicChat")
                .Child(chatFirebaseKey)
                .Child("MeetLocation")
                .OnceSingleAsync<MapDto>()
                .Result;

            var map = new Map
            {
                ChatFirebaseKey = chatFirebaseKey,
                InitiatorLatitude = mapDto.InitiatorLatitude,
                InitiatorLongitude = mapDto.InitiatorLongitude,
                ResponderLatitude = mapDto.ResponderLatitude,
                ResponderLongitude = mapDto.ResponderLongitude
            };

            var disposable = client
                .Child("BasicChat")
                .Child(chatFirebaseKey)
                .Child("MeetLocation")
                .AsObservable<MapDto>(elementRoot: "MeetLocation")
                .Subscribe(map.UpdateCordinates);

            _disposable.Add(disposable);

            return map;
        }

        public void Dispose()
        {
            foreach(var disposable in _disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
