using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Utilities
{
    public class TaskWrapper : ITask
    {
        public Task Run(Action action)
        {
            return Task.Run(action);
        }
    }
}
