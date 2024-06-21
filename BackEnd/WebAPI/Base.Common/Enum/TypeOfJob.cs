using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Enum
{
    public enum TypeOfJob
    {
        InstanceStartingJob = 0,
        SyncToMongoDbJob = 1,
        SyncToInstanceCacheJob = 2
    }
}
