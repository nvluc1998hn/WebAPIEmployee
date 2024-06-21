using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Enum
{
    public enum SyncDataEnum : byte
    {
        NotSync = 0,
        Syncing = 1,
        Synchronized = 2,
        SyncHasError = 3
    }
}
