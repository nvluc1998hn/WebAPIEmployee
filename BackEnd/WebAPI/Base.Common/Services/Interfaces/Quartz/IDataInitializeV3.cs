using Base.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Services.Interfaces.Quartz
{
    /// <summary>
    /// Các hàm khi khởi tạo service 
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lucnv 21/06/2024 created
    /// </Modified>
    public interface IDataInitializeV3
    {
        Task CloneToMongoDb();

        Task InitialCloneAndSubscribe();

        Task CloneToInstanceCache();

        SyncDataEnum CheckSync();

        Task<bool> StopSeedingJob(string jobName = null);

        Task SyncToMongoDb(int numOfMinutes);

        Task SyncToInstanceCache(int numOfMinutes);

        Task SubscribeRabbitMQSyncCommon();
    }
}
