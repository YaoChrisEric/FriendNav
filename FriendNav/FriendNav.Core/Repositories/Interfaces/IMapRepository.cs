using FriendNav.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Repositories.Interfaces
{
    public interface IMapRepository : IDisposable
    {
        Task<Map> GetMap(string chatFirebaseKey);
		void UpdateMap(Map map);
    }
}
