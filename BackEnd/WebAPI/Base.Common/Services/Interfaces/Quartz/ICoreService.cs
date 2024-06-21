using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Services.Interfaces.Quartz
{
    public interface ICoreService
    {
        Task CloneToMongoDb();
        Task CloneToInstanceCache();
        Task SyncToMongoDb(int numOfMinutes);
        Task SyncToInstanceCache(int numOfMinutes);
        void SubscribeSyncInstance();
    }
}
