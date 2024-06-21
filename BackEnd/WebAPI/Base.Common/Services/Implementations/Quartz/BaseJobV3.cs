using Base.Common.Constant;
using Base.Common.Enum;
using Base.Common.Helper;
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
    public class BaseJobV3 : IJob
    {
        protected ILogger<BaseJobV3> _logger;
        protected IDataInitializeV3 _dbInitializer;
        protected IServiceCommunication _serviceCommunication;

        public virtual string CheckSyncUri { get; set; }
        public virtual TypeOfJob TypeOfJob { get; set; }

        protected BaseJobV3(ILogger<BaseJobV3> logger, IDataInitializeV3 dataInitialize, IServiceCommunication serviceCommunication)
        {
            _logger = logger;
            _dbInitializer = dataInitialize;
            _serviceCommunication = serviceCommunication;
        }

        public virtual async Task Execute(IJobExecutionContext context)
        {
            try
            {
                switch (TypeOfJob)
                {
                    case TypeOfJob.InstanceStartingJob:
                        var checkStart = await _serviceCommunication.GetDataAsync<ApiResponse>("BaseJobV3.Execute()", CheckSyncUri, false);
                        // Nếu có kết quả trả về từ Service Sync
                        // Và object dbInitialize chưa Sync thì call hàm sync
                        if (checkStart != null && checkStart.StatusCode == 200 && checkStart.Data != null)
                        {
                            var result = byte.TryParse(checkStart.Data.ToString(), out byte syncStatus);
                            // Nếu Service Sync đã xong và thành công, và chưa sync lần nào
                            if (result && syncStatus == (byte)SyncDataEnum.Synchronized && _dbInitializer.CheckSync() == SyncDataEnum.NotSync)
                            {
                                await _dbInitializer.InitialCloneAndSubscribe();
                                await _dbInitializer.StopSeedingJob();
                            }
                        }
                        break;

                    case TypeOfJob.SyncToMongoDbJob:
                        if (_dbInitializer.CheckSync() != SyncDataEnum.Syncing)
                        {
                            if (CheckFirstSyncOfDay(context)) // đã chuyển ngày đồng bộ all
                            {
                                await _dbInitializer.CloneToMongoDb();
                            }
                            else // đồng bộ theo thời gian
                            {
                                var minutes = ConvertMinutesExpression(context) + 1;
                                await _dbInitializer.SyncToMongoDb(minutes);
                            }
                        }
                        break;

                    case TypeOfJob.SyncToInstanceCacheJob:
                        byte code = 0;
                        int retry = 0;
                        while (code != (byte)SyncDataEnum.Synchronized && retry < 20)
                        {
                            if (retry > 0)
                            {
                                await Task.Delay(3000);
                            }
                            var checkSeeding = await _serviceCommunication.GetDataAsync<ApiResponse>("BaseJobV3.Execute()", CheckSyncUri, false);
                            if (checkSeeding != null && checkSeeding.StatusCode == 200 && checkSeeding.Data != null)
                            {
                                byte.TryParse(checkSeeding.Data.ToString(), out code);
                            }
                            retry++;
                        }
                        // Service Sync đã xong và không lỗi
                        // Và quá trình sync của service hiện tại đã chạy 1 lần (khác notSync) và đang không chạy
                        if (code == (byte)SyncDataEnum.Synchronized && _dbInitializer.CheckSync() != SyncDataEnum.NotSync && _dbInitializer.CheckSync() != SyncDataEnum.Syncing)
                        {
                            if (CheckFirstSyncOfDay(context)) // đã chuyển ngày đồng bộ all
                            {
                                await _dbInitializer.CloneToInstanceCache();
                            }
                            else // đồng bộ theo thời gian
                            {
                                //Cộng thêm 3p do đồng bộ vào instance sau mongo
                                var minutes = ConvertMinutesExpression(context) + 3 + (int)Math.Ceiling(retry * 0.05);
                                await _dbInitializer.SyncToInstanceCache(minutes);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"BaseJobV3.Execute(), CheckSyncUri: {CheckSyncUri} có lỗi: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thời gian
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual int ConvertMinutesExpression(IJobExecutionContext context)
        {
            double minutes = 0;
            try
            {
                if (context != null)
                {
                    var next = context.NextFireTimeUtc.Value.LocalDateTime;
                    var current = context.FireTimeUtc.LocalDateTime;
                    var pre = context.PreviousFireTimeUtc;
                    if (pre.HasValue)
                    {
                        minutes = (current - pre.Value.LocalDateTime).TotalMinutes;
                    }
                    else
                    {
                        minutes = (next - current).TotalMinutes;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"BaseJobV3.ConvertMinutesExpression() có lỗi: {ex.Message}", ex);
            }
            return (int)Math.Ceiling(minutes);
        }

        /// <summary>
        /// Kiểm tra xem thời gian kế tiếp đã chuyển ngày chưa
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool CheckFirstSyncOfDay(IJobExecutionContext context)
        {
            var result = false;
            try
            {
                if (context != null)
                {
                    var currentRun = context.FireTimeUtc.LocalDateTime;
                    var previousRun = context.PreviousFireTimeUtc;
                    if (previousRun.HasValue && currentRun.Day != previousRun.Value.LocalDateTime.Day)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"BaseJobV3.CheckFirstSyncOfDay() có lỗi: {ex.Message}", ex);
            }
            return result;
        }
    }
}
