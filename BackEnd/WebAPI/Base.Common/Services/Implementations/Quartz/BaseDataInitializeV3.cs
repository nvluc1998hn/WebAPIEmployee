using Base.Common.Enum;
using Base.Common.Services.Interfaces.Quartz;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Services.Implementations.Quartz
{
    public abstract class BaseDataInitializeV3 : IDataInitializeV3
    {
        private readonly ILogger<BaseDataInitializeV3> _logger;

        private readonly ISchedulerFactory _schedulerFactory;

        protected abstract string SeedDataJob { get; set; }

        protected List<ICoreService> BaseServices { get; set; }

        /// <summary>
        /// Trạng thái Sync. 0: chưa sync, 1: đang sync, 2: đã sync xong, 3: Sync lỗi
        /// </summary>
        protected SyncDataEnum SyncStatus { get; set; }

        protected BaseDataInitializeV3(ILogger<BaseDataInitializeV3> logger, ISchedulerFactory schedulerFactory)
        {
            _logger = logger;
            SyncStatus = SyncDataEnum.NotSync;
            _schedulerFactory = schedulerFactory;
        }

        /// <summary>
        /// Clone data vào mongodb
        /// </summary>
        /// <returns></returns>
        public virtual async Task CloneToMongoDb()
        {
            SyncStatus = SyncDataEnum.Syncing;
            try
            {
                if (BaseServices != null)
                {
                    foreach (var service in BaseServices)
                    {
                        _logger.LogWarning($"Start Seeding {service.GetType().Name} to Mongo - {DateTime.Now}");
                        await service.CloneToMongoDb();
                    }

                    GC.Collect(2, GCCollectionMode.Forced, true);
                    GC.WaitForPendingFinalizers();
                }

                _logger.LogWarning($"End Seeding into Mongo - {DateTime.Now}");

                SyncStatus = SyncDataEnum.Synchronized;
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error when Sync data into Mongo: {ex}");
                SyncStatus = SyncDataEnum.SyncHasError;
            }
        }

        /// <summary>
        /// Clone data từ mongodb vào ram và đăng ký nghe rabbitmq cho bảng tương ứng, khởi chạy lần đầu khi bật service
        /// </summary>
        /// <returns></returns>
        public virtual async Task InitialCloneAndSubscribe()
        {
            SyncStatus = SyncDataEnum.Syncing;
            try
            {
                if (BaseServices != null)
                {
                    foreach (var service in BaseServices)
                    {
                        _logger.LogWarning($"Start Seeding {service.GetType().Name} into Memory - {DateTime.Now}");
                        await service.CloneToInstanceCache();
                        service.SubscribeSyncInstance();
                    }
                    GC.Collect(2, GCCollectionMode.Forced, true);
                    GC.WaitForPendingFinalizers();
                }
                _logger.LogWarning($"End Seeding into memory - {DateTime.Now}");
                SyncStatus = SyncDataEnum.Synchronized;
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error when Initialize data into memory: {ex}");
                SyncStatus = SyncDataEnum.SyncHasError;
            }
        }

        /// <summary>
        /// chỉ đăng ký nghe rabbitmq cho bảng tương ứng, khởi chạy lần đầu khi run service (có thể override lại để chỉ đăng kí 1 số service thôi )
        /// </summary>
        /// <returns></returns>
        public virtual async Task SubscribeRabbitMQSyncCommon()
        {
            try
            {
                if (BaseServices != null)
                {
                    foreach (var service in BaseServices)
                    {
                        _logger.LogWarning($"đăng ký nghe rabbitmq cho {service.GetType().Name}  - {DateTime.Now}");
                        service.SubscribeSyncInstance();
                    }
                    GC.Collect(2, GCCollectionMode.Forced, true);
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"lỗi đăng ký nghe rabbitmq: {ex}");

            }
        }

        /// <summary>
        /// Clone data từ mongodb vào ram
        /// </summary>
        /// <returns></returns>
        public virtual async Task CloneToInstanceCache()
        {
            SyncStatus = SyncDataEnum.Syncing;
            try
            {
                if (BaseServices != null)
                {
                    foreach (var service in BaseServices)
                    {
                        _logger.LogWarning($"Start Seeding {service.GetType().Name} into Memory - {DateTime.Now}");
                        await service.CloneToInstanceCache();
                    }
                    GC.Collect(2, GCCollectionMode.Forced, true);
                    GC.WaitForPendingFinalizers();
                }
                _logger.LogWarning($"End Seeding into memory - {DateTime.Now}");
                SyncStatus = SyncDataEnum.Synchronized;
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error when Sync data into memory: {ex}");
                SyncStatus = SyncDataEnum.SyncHasError;
            }
        }

        /// <summary>
        /// Kiểm tra quá trình đồng bộ đã xong chưa
        /// </summary>
        /// <returns></returns>
        public virtual SyncDataEnum CheckSync()
        {
            return SyncStatus;
        }

        /// <summary>
        /// Dừng lại một job
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> StopSeedingJob(string jobName = null)
        {
            if (string.IsNullOrEmpty(jobName))
            {
                jobName = SeedDataJob;
            }
            var scheduler = await _schedulerFactory.GetScheduler();
            return await scheduler.DeleteJob(new JobKey(jobName));
        }

        /// <summary>
        /// Đồng bộ định kì data vào mongo
        /// </summary>
        /// <param name="numOfMinutes"></param>
        /// <returns></returns>
        public virtual async Task SyncToMongoDb(int numOfMinutes)
        {
            SyncStatus = SyncDataEnum.Syncing;
            try
            {
                _logger.LogWarning($"{DateTime.Now} - Starting Sync into Mongo - {GetType().Name}");
                if (BaseServices != null)
                {
                    foreach (var service in BaseServices)
                    {
                        await service.SyncToMongoDb(numOfMinutes);
                    }
                    GC.Collect(2, GCCollectionMode.Forced, true);
                    GC.WaitForPendingFinalizers();
                }
                _logger.LogWarning($"{DateTime.Now} - End Sync into Mongo - {GetType().Name}");
                SyncStatus = SyncDataEnum.Synchronized;
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error when Sync data into Mongo: {ex}");
                SyncStatus = SyncDataEnum.SyncHasError;
            }
        }

        /// <summary>
        /// Đồng bộ định kì data từ mongo vào ram
        /// </summary>
        /// <param name="numOfMinutes"></param>
        /// <returns></returns>
        public virtual async Task SyncToInstanceCache(int numOfMinutes)
        {
            SyncStatus = SyncDataEnum.Syncing;
            try
            {
                _logger.LogWarning($"{DateTime.Now} - Starting Sync into RAM - {GetType().Name}");
                if (BaseServices != null)
                {
                    foreach (var service in BaseServices)
                    {
                        await service.SyncToInstanceCache(numOfMinutes);
                    }
                    GC.Collect(2, GCCollectionMode.Forced, true);
                    GC.WaitForPendingFinalizers();
                }
                _logger.LogWarning($"{DateTime.Now} - End Sync into RAM - {GetType().Name}");
                SyncStatus = SyncDataEnum.Synchronized;
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error when Sync data into RAM: {ex}");
                SyncStatus = SyncDataEnum.SyncHasError;
            }
        }
    }
}
