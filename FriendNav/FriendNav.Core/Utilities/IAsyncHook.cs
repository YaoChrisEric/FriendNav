﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FriendNav.Core.Utilities
{
    public interface IAsyncHook
    {
        void NotifyOtherThreads();
    }
}
