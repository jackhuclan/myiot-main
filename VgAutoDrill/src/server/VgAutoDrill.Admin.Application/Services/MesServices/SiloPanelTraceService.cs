using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System.Collections.Concurrent;
using VgAutoDrill.Admin.Application.Cache.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 料仓板料追溯
    /// </summary>
    public class SiloPanelTraceService : BaseServiceWithoutTree<SiloPanelTrace, SiloPanelTraceDto, AddOrUpdateSiloPanelTraceReq>, ISiloPanelTraceService
    {
        private readonly ISiloPanelTraceDomainService _siloPanelTraceDomainService;
        private readonly ISiloPanelTraceDetailDomainService _siloPanelTraceDetailDomainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SiloPanelTraceService> _logger;
        private readonly IAlarmService _alarmService;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IMemoryCacheManager _memoryCacheManager;
        private readonly InnerOptions _innerOptions;
        private readonly IScheduleDataProvider _scheduleDataProvider;
        private readonly ISiloPanelTraceDataProvider _siloPanelTraceDataProvider;

        private static readonly ConcurrentDictionary<string, string> _locationSiloCache = new ConcurrentDictionary<string, string>();

        // 存储缓存过期时间
        private static readonly ConcurrentDictionary<string, DateTime> _cacheExpiration = new ConcurrentDictionary<string, DateTime>();

        public SiloPanelTraceService(
            ISiloPanelTraceDomainService siloPanelTraceDomainService,
            ISiloPanelTraceDetailDomainService siloPanelTraceDetailDomainService,
            IUnitOfWork unitOfWork,
            ILogger<SiloPanelTraceService> logger,
            IMapper mapper,
            IAlarmService alarmService,
            ISysConfigManager sysConfigManager,
            IMemoryCacheManager memoryCacheManager,
            IOptions<InnerOptions> innerOptions,
            IScheduleDataProvider scheduleDataProvider,
            ISiloPanelTraceDataProvider siloPanelTraceDataProvider)
            : base(siloPanelTraceDomainService, mapper)
        {
            _siloPanelTraceDomainService = siloPanelTraceDomainService;
            _siloPanelTraceDetailDomainService = siloPanelTraceDetailDomainService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _alarmService = alarmService;
            _sysConfigManager = sysConfigManager;
            _memoryCacheManager = memoryCacheManager;
            _innerOptions = innerOptions.Value;
            _scheduleDataProvider = scheduleDataProvider;
            _siloPanelTraceDataProvider = siloPanelTraceDataProvider;
        }

        /// <summary>
        /// 获取料仓板料追溯列表
        /// </summary>
        /// <param name="request">查询请求参数</param>
        /// <returns>符合条件的追溯记录列表</returns>
        public async Task<ResponseDto<PageDto<SiloPanelTraceDto>>> GetList(GetSiloPanelTraceListReq request)
        {
            try
            {
                if (request.PageNum < 1) request.PageNum = 1;
                if (request.PageSize < 1) request.PageSize = 10;

                var result = await _siloPanelTraceDomainService.GetList(request);
                return Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取料仓板料追溯列表时发生错误");
                return Fail<PageDto<SiloPanelTraceDto>>($"查询失败: {ex.Message}");
            }
        }

        [Obsolete("请使用方法AddAndReturnId", true)]
        public override Task<ResponseDto<string>> Add(AddOrUpdateSiloPanelTraceReq req)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新增料仓板料追溯记录
        /// </summary>
        /// <param name="req">新增请求参数</param>
        /// <returns>新增结果</returns>
        public override async Task<ResponseDto<int>> AddAndReturnId(AddOrUpdateSiloPanelTraceReq req)
        {
            if (string.IsNullOrWhiteSpace(req.Location))
            {
                return Fail("位置信息不能为空", 0);
            }

            if (string.IsNullOrWhiteSpace(req.Subject))
            {
                return Fail("操作主题描述不能为空", 0);
            }

            // 自动生成追溯记录编号（使用GUID）
            if (string.IsNullOrWhiteSpace(req.Code))
            {
                req.Code = System.Guid.NewGuid().ToString();
            }

            if (!string.IsNullOrWhiteSpace(req.DrilledItem))
            {
                var drilledItems = req.DrilledItem.Split(',', StringSplitOptions.RemoveEmptyEntries);
                req.HasMultipleDrilled = drilledItems.Length > 1;
            }

            req.CreatorName = UserName;

            await CheckAndSetWarningAsync(req);

            var model = _mapper.Map<SiloPanelTrace>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            var id = await _domainService.AddReturnId(model);

            if (id > 0)
            {
                await UpdateLocationCacheAsync(req.Location, req.SiloCode);
                return Success(id);
            }
            else
            {
                return Fail($"保存至db时报错", 0);
            }
        }

        /// <summary>
        /// 更新料仓板料追溯记录
        /// </summary>
        /// <param name="req">更新请求参数</param>
        /// <returns>更新结果</returns>
        public override async Task<ResponseDto<string>> Update(AddOrUpdateSiloPanelTraceReq req)
        {
            if (req == null || req.Id <= 0)
            {
                return Fail("更新请求参数无效");
            }

            if (string.IsNullOrWhiteSpace(req.Location))
            {
                return Fail("位置信息不能为空");
            }
            if (string.IsNullOrWhiteSpace(req.Subject))
            {
                return Fail("操作主题描述不能为空");
            }

            if (!string.IsNullOrWhiteSpace(req.DrilledItem))
            {
                var drilledItems = req.DrilledItem.Split(',', StringSplitOptions.RemoveEmptyEntries);
                req.HasMultipleDrilled = drilledItems.Length > 1;
            }

            req.ModifierName = UserName;

            await CheckAndSetWarningAsync(req);

            var result = await base.Update(req);

            if (result.Code == ResponseCode.Success)
            {
                await UpdateLocationCacheAsync(req.Location, req.SiloCode);
            }

            return result;
        }

        /// <summary>
        /// 删除料仓板料追溯记录（级联删除详细表）
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>删除结果</returns>
        public override async Task<ResponseDto<string>> Delete(long id)
        {
            _unitOfWork.BeginTran();

            try
            {
                await _siloPanelTraceDetailDomainService.DeleteAsync(p => p.MasterId == id);

                var result = await base.Delete(id);
                if (result.Code != ResponseCode.Success)
                {
                    _unitOfWork.RollbackTran();
                    return result;
                }

                _unitOfWork.CommitTran();

                return Success("删除成功");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTran();
                _logger.LogError(ex, $"删除记录ID={id}时发生错误");
                return Fail($"删除失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量删除料仓板料追溯记录（级联删除详细表）
        /// </summary>
        /// <param name="idList">记录ID数组</param>
        /// <returns>删除结果</returns>
        public override async Task<ResponseDto<string>> DeleteList(object[] idList)
        {
            if (idList == null || idList.Length == 0)
            {
                return Fail("删除列表不能为空");
            }

            _unitOfWork.BeginTran();

            try
            {
                var longIdList = idList.OfType<long>().ToList();

                await _siloPanelTraceDetailDomainService.DeleteAsync(p => longIdList.Contains(p.MasterId));

                var result = await base.DeleteList(idList);
                if (result.Code != ResponseCode.Success)
                {
                    _unitOfWork.RollbackTran();
                    return result;
                }

                _unitOfWork.CommitTran();

                return Success("批量删除成功");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTran();
                _logger.LogError(ex, "批量删除记录时发生错误");
                return Fail($"批量删除失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从PanelList添加料仓板料追溯记录（接受List<Panel>参数）
        /// </summary>
        /// <param name="panels">板料列表</param>
        /// <param name="subject">操作主题描述</param>
        /// <param name="scheduleId">调度ID</param>
        /// <param name="transportationTaskId">运输任务ID</param>
        /// <returns>添加结果</returns>
        public async Task<ResponseDto<int>> AddFromPanelList(PanelList panels, string subject, long? scheduleId = null, long? transportationTaskId = null)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                return Fail("操作主题描述不能为空", 0);
            }

            var siloCode = panels.SiloCode;
            var locationCode = panels.LocationCode;

            if (string.IsNullOrWhiteSpace(locationCode))
            {
                return Fail("位置信息不能为空", 0);
            }

            var undrilledItemCodes = panels.UndrilledItemCodes.ToList();
            var drilledItemCodes = panels.DrilledItemCodes.ToList();
            var summaryPanelInfo = panels.SummaryPanelInfo();

            var standardRequest = new AddOrUpdateSiloPanelTraceReq
            {
                SiloCode = siloCode,
                Location = locationCode,
                SiloSummary = summaryPanelInfo,
                Subject = subject,
                UndrilledItem = undrilledItemCodes.Any() ? string.Join(",", undrilledItemCodes) : null,
                DrilledItem = drilledItemCodes.Any() ? string.Join(",", drilledItemCodes) : null,
                HasMultipleDrilled = drilledItemCodes.Count > 1
            };

            if (scheduleId > 0)
            {
                standardRequest.ScheduleId = scheduleId.Value;
            }

            if (transportationTaskId > 0)
            {
                standardRequest.TransportationTaskId = transportationTaskId.Value;
            }

            bool needValidateSiloCode = await DetermineNeedValidateSiloCode(standardRequest.SiloCode, standardRequest.ScheduleId);
            standardRequest.NeedValidateSiloCode = needValidateSiloCode;

            _unitOfWork.BeginTran();

            try
            {
                var masterResult = await AddAndReturnId(standardRequest);
                if (masterResult.Code != ResponseCode.Success)
                {
                    _unitOfWork.RollbackTran();
                    return masterResult;
                }

                var masterId = masterResult.Data;
                if (masterId <= 0)
                {
                    _unitOfWork.RollbackTran();
                    return Fail("无法获取主表记录ID", 0);
                }

                var detailList = new List<SiloPanelTraceDetail>();
                foreach (var panel in panels.Where(p => p != null))
                {
                    var detail = new SiloPanelTraceDetail
                    {
                        MasterId = masterId,
                        LocationCode = panel.LocationCode,
                        SiloCode = panel.SiloCode,
                        ItemCode = panel.ItemCode,
                        PanelCode = panel.PanelCode,
                        FloorNum = panel.Layer,
                        ProductStatus = panel.ProductStatus,
                        Pcs = panel.Pcs,
                        PanelWidth = (decimal)panel.PanelWidth,
                        PanelLength = (decimal)panel.PanelLength,
                        PinOffset = (decimal)panel.PinOffset,
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                        CreatorName = UserName,
                        Status = 1
                    };
                    detailList.Add(detail);
                }

                if (detailList.Any())
                {
                    await _siloPanelTraceDetailDomainService.BulkInsert(detailList);
                }

                _unitOfWork.CommitTran();

                await UpdateLocationCacheAsync(locationCode, siloCode);

                _logger.LogInformation($"校验条件检查 - SiloCode: {siloCode}, ScheduleId: {scheduleId}");

                // 检查 SiloCode 校验功能是否启用
                bool isValidationEnabled = await _sysConfigManager.GetBoolValue("ENABLE_SILO_CODE_VALIDATION");
                if (!isValidationEnabled)
                {
                    _logger.LogInformation("SiloCode 校验功能已禁用，跳过校验逻辑");
                    return Success(masterResult.Data);
                }

                if (string.IsNullOrEmpty(siloCode))
                {
                    _logger.LogInformation($"SiloCode 为空，跳过校验 - 记录ID: {masterResult.Data}");
                }
                else if (scheduleId > 0 && !string.IsNullOrEmpty(siloCode))
                {
                    _logger.LogInformation($"ScheduleId 和 SiloCode 都不为空，检查调度状态 - 记录ID: {masterResult.Data}, SiloCode: {siloCode}, ScheduleId: {scheduleId}");

                    // 获取调度记录并检查状态
                    var schedule = await _scheduleDataProvider.GetScheduleByIdAsync(scheduleId.Value);
                    if (schedule != null && !string.IsNullOrEmpty(schedule.RequireDeviceId))
                    {
                        if (schedule.ScheduledTaskStatus == ScheduledTaskStatus.Failed)
                        {
                            var cacheKey = $"SiloCodeValidation_{schedule.RequireDeviceId}";
                            var cacheData = new { SiloCode = siloCode, RequireDeviceId = schedule.RequireDeviceId };
                            var cacheExpirationMinutes = await GetSiloCodeCacheExpirationMinutes();

                            // 直接设置缓存，过期时自动删除
                            _memoryCacheManager.Set(cacheKey, cacheData, cacheExpirationMinutes);
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"无法获取调度记录或 RequireDeviceId 为空 - ScheduleId: {scheduleId}");
                    }
                }
                else if ((!scheduleId.HasValue || scheduleId <= 0) && !string.IsNullOrEmpty(siloCode))
                {
                    try
                    {
                        var cacheKey = $"SiloCodeValidation_{locationCode}";

                        if (_memoryCacheManager.Exists(cacheKey))
                        {
                            var cachedValue = _memoryCacheManager.Get<object>(cacheKey);
                            if (cachedValue != null)
                            {
                                var siloCodeProperty = cachedValue.GetType().GetProperty("SiloCode");
                                var requireDeviceIdProperty = cachedValue.GetType().GetProperty("RequireDeviceId");

                                if (siloCodeProperty != null && requireDeviceIdProperty != null)
                                {
                                    var cachedSiloCode = siloCodeProperty.GetValue(cachedValue)?.ToString();
                                    var cachedRequireDeviceId = requireDeviceIdProperty.GetValue(cachedValue)?.ToString();

                                    if (cachedRequireDeviceId == locationCode)
                                    {
                                        if (cachedSiloCode != siloCode)
                                        {
                                            _logger.LogWarning($"SiloCode 不一致 - 缓存SiloCode: {cachedSiloCode}, 当前SiloCode: {siloCode}");

                                            var warningDescription = $"料仓编码不一致：原追溯记录中SiloCode为{cachedSiloCode}，当前追溯记录中SiloCode为{siloCode}";

                                            await _siloPanelTraceDomainService.UpdateAsync(
                                                s => new SiloPanelTrace
                                                {
                                                    IsWarning = true,
                                                    WarningDescription = warningDescription,
                                                    ModifyTime = DateTime.Now,
                                                    ModifierId = UserId
                                                },
                                                s => s.Id == masterResult.Data);

                                            var alarmCode = $"SiloCodeValidation-{locationCode}";

                                            var alarmReq = new AddOrUpdateAlarmReq
                                            {
                                                AlarmCode = alarmCode,
                                                AlarmName = "料仓编码校验不一致告警",
                                                AlarmContent = warningDescription,
                                                AlarmLevel = 2,
                                                AlarmKind = AlarmKind.LoationScheduleException,
                                                LocationCode = locationCode,
                                                PartitionCode = "SILO_CODE_VALIDATION",
                                                ItemCode = siloCode,
                                                AlarmTime = DateTime.Now,
                                                EventId = 0,
                                                EventData = warningDescription
                                            };

                                            var result = await _alarmService.AddData(alarmReq);
                                            if (result.Code != 0)
                                            {
                                                _logger.LogError($"[告警失败] 创建告警失败: {result.Message}, Code: {result.Code}");
                                            }
                                        }

                                        _memoryCacheManager.Remove(cacheKey);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"访问缓存时发生异常: {ex.Message}");
                    }
                }

                return Success(masterResult.Data);
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTran();
                _logger.LogError(ex, "创建主表和详细表记录时发生错误");
                return Fail($"创建失败: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// 获取导出Excel的数据列表
        /// </summary>
        /// <param name="request">查询请求参数</param>
        /// <returns>导出数据列表</returns>
        public async Task<List<SiloPanelTraceToExcelDto>> GetToExcelList(GetSiloPanelTraceListReq request)
        {
            var list = new List<SiloPanelTraceToExcelDto>();

            var exportRequest = new GetSiloPanelTraceListReq
            {
                ID = request.ID,
                SiloCode = request.SiloCode,
                Location = request.Location,
                Subject = request.Subject,
                UndrilledItem = request.UndrilledItem,
                DrilledItem = request.DrilledItem,
                HasMultipleDrilled = request.HasMultipleDrilled,
                ScheduleId = request.ScheduleId,
                TransportationTaskId = request.TransportationTaskId,
                CreatorName = request.CreatorName,
                PageNum = 1,
                PageSize = int.MaxValue
            };

            var result = await GetList(exportRequest);

            if (result != null && result.Data?.List != null)
            {
                foreach (var item in result.Data.List)
                {
                    var model = new SiloPanelTraceToExcelDto
                    {
                        Id = item.Id,
                        SiloCode = item.SiloCode ?? string.Empty,
                        Location = item.Location ?? string.Empty,
                        Subject = item.Subject ?? string.Empty,
                        UndrilledItem = item.UndrilledItem ?? string.Empty,
                        DrilledItem = item.DrilledItem ?? string.Empty,
                        HasMultipleDrilled = item.HasMultipleDrilled,
                        ScheduleId = item.ScheduleId?.ToString() ?? string.Empty,
                        TransportationTaskId = item.TransportationTaskId?.ToString() ?? string.Empty,
                        SiloSummary = item.SiloSummary ?? string.Empty,
                        CreatorName = item.CreatorName ?? string.Empty,
                        CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        IsWarning = item.IsWarning,
                        WarningDescription = item.WarningDescription ?? string.Empty
                    };
                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 检查并设置告警信息
        /// </summary>
        /// <param name="req">记录请求</param>
        private async Task CheckAndSetWarningAsync(AddOrUpdateSiloPanelTraceReq req)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(req.Location))
                {
                    return;
                }

                bool hasMultipleDrilled = req.HasMultipleDrilled;

                bool siloCodeChanged = false;
                string existingSiloCode = null;
                long? previousRecordId = null;

                var (cachedSiloCode, recordId) = await GetLatestSiloCodeAsync(req.Location);
                if (!string.IsNullOrEmpty(cachedSiloCode))
                {
                    existingSiloCode = cachedSiloCode;
                    previousRecordId = recordId;
                    siloCodeChanged = ShouldTriggerWarning(req.SiloCode, cachedSiloCode);
                }

                await SetWarningDescriptionAsync(req, existingSiloCode, siloCodeChanged);

                if (hasMultipleDrilled)
                {
                    _logger.LogWarning($"[告警触发] 位置 {req.Location} ，存在多个熟料告警");
                }

                if (siloCodeChanged)
                {
                    _logger.LogWarning($"[告警触发] 位置 {req.Location} ，料仓变更告警 - 原SiloCode: {existingSiloCode}, 新SiloCode: {req.SiloCode}");
                }

                if (siloCodeChanged && previousRecordId.HasValue)
                {
                    req.ReferenceRecordId = previousRecordId.Value;
                }

                if (req.IsWarning)
                {
                    await InsertAlarmRecordAsync(req, existingSiloCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[告警检查异常] Location: {req.Location}, SiloCode: {req.SiloCode}");
            }
        }

        /// <summary>
        /// 获取位置的最新料仓代码和记录ID
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns>料仓代码和记录ID的元组</returns>
        private async Task<(string? SiloCode, long? RecordId)> GetLatestSiloCodeAsync(string location)
        {
            try
            {
                // 从数据库查询最新记录
                var existingRecord = await _siloPanelTraceDomainService.QueryAsync(
                    x => x.Location == location &&
                         x.IsDeleted == 0 &&
                         x.Status == 1,
                    x => x.CreateTime,
                    SqlSugar.OrderByType.Desc
                );

                var record = existingRecord?.FirstOrDefault();

                if (record != null && !string.IsNullOrEmpty(record.SiloCode))
                {
                    // 更新缓存和过期时间
                    _locationSiloCache.AddOrUpdate(location, record.SiloCode, (key, oldValue) => record.SiloCode);
                    var newExpirationTime = DateTime.Now.AddHours(_innerOptions.LocationSiloCacheExpirationHours);
                    _cacheExpiration.AddOrUpdate(location, newExpirationTime, (key, oldValue) => newExpirationTime);
                    return (record.SiloCode, record.Id);
                }

                return (null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[缓存异常] 获取位置料仓代码时发生错误，Location: {location}");
                return (null, null);
            }
        }

        /// <summary>
        /// 判断是否应该触发告警
        /// </summary>
        /// <param name="newSiloCode">新记录的SiloCode</param>
        /// <param name="existingSiloCode">已存在记录的SiloCode</param>
        /// <returns>是否触发告警</returns>
        private static bool ShouldTriggerWarning(string? newSiloCode, string? existingSiloCode)
        {
            if (string.IsNullOrWhiteSpace(newSiloCode) || string.IsNullOrWhiteSpace(existingSiloCode))
            {
                return false;
            }

            var normalizedNewSiloCode = newSiloCode.Trim().ToUpperInvariant();
            var normalizedExistingSiloCode = existingSiloCode.Trim().ToUpperInvariant();

            bool isEqual = string.Equals(normalizedNewSiloCode, normalizedExistingSiloCode, StringComparison.Ordinal);

            return !isEqual;
        }

        /// <summary>
        /// 更新指定位置的缓存
        /// </summary>
        /// <param name="location">位置</param>
        /// <param name="siloCode">料仓号</param>
        private async Task UpdateLocationCacheAsync(string location, string siloCode)
        {
            try
            {
                // 更新缓存和过期时间
                _locationSiloCache.AddOrUpdate(location, siloCode, (key, oldValue) => siloCode);
                var expirationTime = DateTime.Now.AddHours(_innerOptions.LocationSiloCacheExpirationHours);
                _cacheExpiration.AddOrUpdate(location, expirationTime, (key, oldValue) => expirationTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[缓存更新异常] 更新位置缓存时发生错误，Location: {location}");
            }
        }

        /// <summary>
        /// 清理指定位置的缓存
        /// </summary>
        /// <param name="location">位置</param>
        public async Task ClearLocationCacheAsync(string location)
        {
            try
            {
                _locationSiloCache.TryRemove(location, out _);
                _cacheExpiration.TryRemove(location, out _);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[缓存清理异常] 清理位置缓存时发生错误，Location: {location}");
            }
        }

        /// <summary>
        /// 获取缓存信息
        /// </summary>
        /// <param name="location">位置</param>
        public async Task<string> GetCacheInfoAsync(string location)
        {
            try
            {
                if (_locationSiloCache.TryGetValue(location, out var siloCode))
                {
                    return $"位置: {location}, 料仓代码: {siloCode}";
                }
                else
                {
                    return "缓存不存在";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[缓存信息异常] 获取缓存信息时发生错误，Location: {location}");
                return $"获取缓存信息异常: {ex.Message}";
            }
        }



        private async Task InsertAlarmRecordAsync(AddOrUpdateSiloPanelTraceReq req, string existingSiloCode)
        {
            try
            {
                if (!req.IsWarning || string.IsNullOrEmpty(req.WarningDescription))
                {
                    return;
                }

                var alarmCode = $"SiloPanelTrace-{req.Location}";

                if (await _alarmService.HasUnhandledAlarm(alarmCode))
                {
                    return;
                }

                var alarmReq = new AddOrUpdateAlarmReq
                {
                    AlarmCode = alarmCode,
                    AlarmName = "料仓板料追溯告警",
                    AlarmContent = req.WarningDescription,
                    AlarmLevel = 2,
                    AlarmKind = AlarmKind.LoationScheduleException,
                    LocationCode = req.Location,
                    PartitionCode = "SILO_PANEL_TRACE",
                    ItemCode = req.SiloCode,
                    AlarmTime = DateTime.Now,
                    EventId = 0,
                    EventData = req.WarningDescription
                };

                var result = await _alarmService.AddData(alarmReq);
                if (result.Code != 0)
                {
                    _logger.LogError($"[告警失败] 创建告警失败: {result.Message}, Code: {result.Code}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[告警异常] 插入告警记录时发生错误，Location: {req.Location}");
            }
        }

        private async Task SetWarningDescriptionAsync(AddOrUpdateSiloPanelTraceReq req, string existingSiloCode = null, bool siloCodeChanged = false)
        {
            var warningDescriptions = new List<string>();

            if (req.HasMultipleDrilled)
            {
                warningDescriptions.Add("同一料仓存在多个熟料");
            }

            if (siloCodeChanged && !string.IsNullOrEmpty(existingSiloCode) && !string.IsNullOrEmpty(req.SiloCode))
            {
                warningDescriptions.Add($"{req.Location}上，料仓由{existingSiloCode}变为{req.SiloCode}，请人工确认");
            }

            if (warningDescriptions.Any())
            {
                req.WarningDescription = string.Join("；", warningDescriptions);
                req.IsWarning = true;
            }
        }

        /// <summary>
        /// 获取 SiloCode 校验缓存过期时间(分钟)
        /// </summary>
        /// <returns>过期时间(分钟)</returns>
        private async Task<int> GetSiloCodeCacheExpirationMinutes()
        {
            try
            {
                var response = await _sysConfigManager.GetIntValue("SILO_CODE_CACHE_EXPIRATION_MINUTES");
                if (response <= 0)
                {
                    // 默认30分钟
                    return 30;
                }

                return response;
            }
            catch (Exception)
            {
                return 30;
            }
        }


        /// <summary>
        /// 判断是否需要校验SiloCode
        /// </summary>
        /// <param name="siloCode">料仓编码</param>
        /// <param name="scheduleId">调度记录ID</param>
        /// <returns>是否需要校验</returns>
        private async Task<bool> DetermineNeedValidateSiloCode(string siloCode, long? scheduleId)
        {
            try
            {
                if (string.IsNullOrEmpty(siloCode))
                {
                    return false;
                }

                if (!scheduleId.HasValue)
                {
                    return false;
                }

                var schedule = await _scheduleDataProvider.GetScheduleByIdAsync(scheduleId.Value);
                if (schedule == null)
                {
                    return false;
                }

                // 只有当调度状态为-1(失败)时才需要校验（建立缓存）
                return schedule.ScheduledTaskStatus == ScheduledTaskStatus.Failed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"判断是否需要校验SiloCode时发生错误 - SiloCode: {siloCode}, ScheduleId: {scheduleId}");
                return false;
            }
        }


    }

    public interface IScheduleDataProvider
    {
        Task<List<Schedule>> GetFailedSchedulesAsync(List<long> scheduleIds);
        Task<Schedule> GetScheduleByIdAsync(long scheduleId);
    }


    public interface ISiloPanelTraceDataProvider
    {
        Task<string> GetSiloCodeByLocationAsync(string location);
    }


    public class ScheduleDataProvider : IScheduleDataProvider
    {
        private readonly IScheduleDomainService _scheduleDomainService;

        public ScheduleDataProvider(IScheduleDomainService scheduleDomainService)
        {
            _scheduleDomainService = scheduleDomainService;
        }

        public async Task<List<Schedule>> GetFailedSchedulesAsync(List<long> scheduleIds)
        {
            return await _scheduleDomainService.QueryAsync(
                s => scheduleIds.Contains(s.Id) && s.ScheduledTaskStatus == ScheduledTaskStatus.Failed,
                s => s.Id,
                SqlSugar.OrderByType.Asc);
        }

        public async Task<Schedule> GetScheduleByIdAsync(long scheduleId)
        {
            return await _scheduleDomainService.FindSingleAsync(s => s.Id == scheduleId);
        }
    }


    public class SiloPanelTraceDataProvider : ISiloPanelTraceDataProvider
    {
        private readonly ISiloPanelTraceDomainService _siloPanelTraceDomainService;
        private readonly ILogger<SiloPanelTraceDataProvider> _logger;

        public SiloPanelTraceDataProvider(ISiloPanelTraceDomainService siloPanelTraceDomainService, ILogger<SiloPanelTraceDataProvider> logger)
        {
            _siloPanelTraceDomainService = siloPanelTraceDomainService;
            _logger = logger;
        }

        public async Task<string> GetSiloCodeByLocationAsync(string location)
        {
            try
            {
                var traces = await _siloPanelTraceDomainService.QueryAsync(
                    t => t.Location == location,
                    t => t.CreateTime,
                    SqlSugar.OrderByType.Desc);
                return traces?.FirstOrDefault()?.SiloCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"通过设备编码 {location} 查找 SiloCode 时发生错误");
                return null;
            }
        }
    }

}
