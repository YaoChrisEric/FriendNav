using FriendNav.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.Utilities
{
    public class TestTask : ITask
    {
        public Task Run(Action action)
        {
            var task = new Task(action);

            task.RunSynchronously();

            return task;

        }
    }
}
