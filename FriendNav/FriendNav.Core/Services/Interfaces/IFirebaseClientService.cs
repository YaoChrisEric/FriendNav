using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Services.Interfaces
{
    public interface IFirebaseClientService
    {
        FirebaseClient CreateFirebaseClient();
    }
}
