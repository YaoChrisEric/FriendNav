﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.Utilities
{
    public interface ITask
    {
        Task Run(Action action);
    }
}
