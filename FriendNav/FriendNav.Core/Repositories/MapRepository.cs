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
    public class MapRepository : IMapRepository
    {
        private readonly List<IDisposable> _disposable = new List<IDisposable>();
        private readonly IFirebaseClientService _firebaseClientService;

        public MapRepository(IFirebaseClientService firebaseClientService)
        {
            _firebaseClientService = firebaseClientService;
        }

        public async Task<Map> GetMap(string chatFirebaseKey)
        {
            var client = _firebaseClientService.CreateFirebaseClient();

            var mapDto = await client
                .Child("BasicChat")
                .Child(chatFirebaseKey)
                .Child("MeetLocation")
                .OnceSingleAsync<MapDto>();

            if (null == mapDto)
            {
                mapDto = new MapDto
                {
                    InitiatorLatitude = "500",
                    InitiatorLongitude = "500",
                    ResponderLatitude = "500",
                    ResponderLongitude = "500"
                };

                await client
                .Child("BasicChat")
                .Child(chatFirebaseKey)
                .Child("MeetLocation")
                .PutAsync(mapDto);
            }

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

        public void UpdateMap(Map map)
        {

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
