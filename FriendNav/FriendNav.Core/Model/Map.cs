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
		
		public bool IsInitiator { get; set; }

        public string InitiatorLatitude { get; set; }

        public string InitiatorLongitude { get; set; }

        public string ResponderLatitude { get; set; }

        public string ResponderLongitude { get; set; }
		
		public void UpdateActiveUserCords(string latitude, string longitude)
        {
            if (IsInitiator)
            {
                InitiatorLatitude = latitude;
                InitiatorLongitude = longitude;
            }
            else
            {
                ResponderLatitude = latitude;
                ResponderLongitude = longitude;
            }
        }

		public EventHandler OtherUserCordinatesUpdated;
		
		public void UpdateCordinates(FirebaseEvent<MapDto> observer)
        {
            if (IsInitiator)
            {
                if (InitiatorLatitude != observer.Object.InitiatorLatitude ||
                InitiatorLongitude != observer.Object.InitiatorLongitude)
                {
                    InitiatorLatitude = observer.Object.InitiatorLatitude;
                    InitiatorLongitude = observer.Object.InitiatorLongitude;

                    OtherUserCordinatesUpdated?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                if (ResponderLatitude != observer.Object.ResponderLatitude ||
                    ResponderLongitude != observer.Object.ResponderLongitude)
                {
                    ResponderLatitude = observer.Object.ResponderLatitude;
                    ResponderLongitude = observer.Object.ResponderLongitude;

                    OtherUserCordinatesUpdated?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
