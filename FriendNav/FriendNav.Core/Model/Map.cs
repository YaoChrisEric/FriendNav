using Firebase.Database.Streaming;
using FriendNav.Core.DataTransfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Model
{
    public class Map
    {
        public string ChatFirebaseKey { get; set; }

        public string InitiatorLatitude { get; set; }

        public string InitiatorLongitude { get; set; }

        public string ResponderLatitude { get; set; }

        public string ResponderLongitude { get; set; }

        public EventHandler InitiatorCordinatesUpdated;

        public EventHandler ResponderCordinatesUpdated;

        public void UpdateCordinates(FirebaseEvent<MapDto> observer)
        {
            if (InitiatorLatitude != observer.Object.InitiatorLatitude ||
                InitiatorLongitude != observer.Object.InitiatorLongitude)
            {
                InitiatorLatitude = observer.Object.InitiatorLatitude;
                InitiatorLongitude = observer.Object.InitiatorLongitude;

                InitiatorCordinatesUpdated?.Invoke(this, new EventArgs());
            }

            if (ResponderLatitude != observer.Object.ResponderLatitude ||
                ResponderLongitude != observer.Object.ResponderLongitude)
            {
                ResponderLatitude = observer.Object.ResponderLatitude;
                ResponderLongitude = observer.Object.ResponderLongitude;

                ResponderCordinatesUpdated?.Invoke(this, new EventArgs());
            }
        }
    }
}
