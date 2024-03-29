﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Cache.Redis.Interface
{
    public enum RedisDatabaseNumber
    {
        /// <summary>
        /// Database for caching
        /// </summary>
        Cache = 1,
        /// <summary>
        /// Database for plugins
        /// </summary>
        Plugin = 2,
        /// <summary>
        /// Database for data protection keys
        /// </summary>
        DataProtectionKeys = 3
    }
}
