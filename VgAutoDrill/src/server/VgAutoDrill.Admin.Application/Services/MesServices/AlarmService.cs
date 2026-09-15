using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices.External;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AlarmService : BaseServiceWithoutTree<Alarm, AlarmDto, AddOrUpdateAlarmReq>, IAlarmService
    {
        private readonly IAlarmDomainService _alarmDomainService;
        private readonly IExternalWorkOrderDomainService _externalWorkOrderDomainService;
        private readonly ITaskDomainService _taskDomainService;
        private readonly IOptions<InnerOptions> _innerOptions;
        private readonly ILogger<AlarmService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public AlarmService(IAlarmDomainService domainService,
            IExternalWorkOrderDomainService externalWorkOrderDomainService,
            ITaskDomainService taskDomainService,
            IOptions<InnerOptions> innerOptions,
            ILogger<AlarmService> logger,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _alarmDomainService = domainService;
            _externalWorkOrderDomainService = externalWorkOrderDomainService;
            _taskDomainService = taskDomainService;
            _innerOptions = innerOptions;
            _logger = logger;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<AlarmDto>>> GetList(GetAlarmListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<AlarmDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<Alarm>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.AlarmCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.AlarmCode) && p.AlarmCode.Contains(req.AlarmCode));
            }
            if (!string.IsNullOrEmpty(req.AlarmName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.AlarmName) && p.AlarmName.Contains(req.AlarmName));
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }
            if (req.AlarmLevel > -1)
            {
                where = where.And(p => p.AlarmLevel == req.AlarmLevel);
            }

            if (req.IsHandled.HasValue)
            {
                where = where.And(p => p.IsHandled == req.IsHandled);
            }

            if (req.AlarmKinds != null && req.AlarmKinds.Any())
            {
                where = where.And(p => p.AlarmKind != null && req.AlarmKinds.Contains((AlarmKind)p.AlarmKind));
            }

            if (req.QueryStartTime != null)
            {
                where = where.And(p => p.AlarmTime >= req.QueryStartTime.Value);
            }
            if (req.QueryEndTime != null)
            {
                where = where.And(p => p.AlarmTime <= req.QueryEndTime.Value);
            }
            if (!string.IsNullOrEmpty(req.LocationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.LocationCode) && p.LocationCode.ToLower().Contains(req.LocationCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.PartitionCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.PartitionCode) && p.PartitionCode.ToLower().Contains(req.PartitionCode.ToLower()));
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<Alarm>, List<AlarmDto>>(result.ToList());

            foreach (var item in pageDto.List)
            {
                item.AlarmKindDesc = item.AlarmName;
            }

            return Success(pageDto);
        }

        /// <summary>
        /// 获取导出List
        /// </summary>
        /// <returns></returns>
        public async Task<List<AlarmToExcelDto>> GetToExcelList(GetAlarmListReq req)
        {
            List<AlarmToExcelDto> list = await _alarmDomainService.GetToExcelList(req);
            return list;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateAlarmReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.AlarmCode == req.AlarmCode && p.IsHandled == false);
            if (isExsitCode)
            {
                return Fail("已存在未处理的告警!");
            }

            var model = _mapper.Map<Alarm>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.IsHandled = false;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);
            return Success();
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateAlarmReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.AlarmCode == req.AlarmCode && p.Id != req.Id && p.IsHandled == false);
            if (isExsitCode)
            {
                return Fail($"已存在相同编码{req.AlarmCode}的未处理的告警!");
            }

            var model = _mapper.Map<Alarm>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }

        public override async Task<ResponseDto<string>> Delete(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            entity.ModifierId = UserId;
            entity.ModifyTime = DateTime.Now;
            entity.IsHandled = true;
            entity.HandledTime = DateTime.Now;
            if (await _domainService.Update(entity))
            {
                //PublishAlarmHandled(entity);
            }
            return Success();
        }

        public async Task<bool> TryHandleBySyncId(long syncId)
        {
            var entity = await _domainService.FindSingleAsync(x => x.SyncId == syncId);
            if (entity == null)
            {
                return false;
            }

            entity.ModifierId = UserId;
            entity.ModifyTime = DateTime.Now;
            entity.IsHandled = true;
            entity.HandledTime = DateTime.Now;
            if (await _domainService.Update(entity))
            {
                //PublishAlarmHandled(entity);
            }
            return true;
        }

        public async Task<AlarmDto> FindSingleBySyncId(long syncId)
        {
            var alarm = await _domainService.FindSingleAsync(x => x.SyncId == syncId);
            return _mapper.Map<AlarmDto>(alarm);
        }

        public override async Task<ResponseDto<string>> DeleteList(object[] delList)
        {
            var entities = await _domainService.QueryAsync(x => delList.Contains(x.Id), x => x.Id, SqlSugar.OrderByType.Asc);
            if (!entities.Any())
            {
                return Fail("信息不存在!");
            }
            foreach (var entity in entities)
            {
                entity.ModifierId = UserId;
                entity.ModifyTime = DateTime.Now;
                entity.IsHandled = true;
                entity.HandledTime = DateTime.Now;
            }

            if (await _domainService.BulkUpdate(entities))
            {
                //PublishAlarmHandled(entities);
            }

            return Success();
        }

        public async Task RegularDeleteData()
        {
            DateTime recordsTime = DateTime.Now.AddMonths(-1);
            await _domainService.DeleteAsync(p => p.CreateTime < recordsTime);
        }

        /// <summary>
        /// 检查指定前缀开头的外部工单，如果没有生产任务时产生告警记录
        /// 只对指定时间范围内的外部工单产生告警信息
        /// </summary>
        /// <returns>新增的告警记录数量</returns>
        public async Task<int> CheckSampleOrderAlarms()
        {
            try
            {
                // 从配置中获取时间限制和工单前缀
                int timeLimitHours = _innerOptions.Value.SampleOrderAlarmTimeLimitHours;
                int bufferMinutes = _innerOptions.Value.SampleOrderAlarmBufferMinutes;
                string workOrderPrefixConfig = _innerOptions.Value.SampleOrderAlarmWorkOrderPrefix;

                // 解析工单前缀配置，支持逗号分隔的多个前缀
                var workOrderPrefixes = !string.IsNullOrEmpty(workOrderPrefixConfig)
                    ? workOrderPrefixConfig.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray()
                    : new string[] { "S" }; // 默认前缀

                // 计算时间限制：72小时内创建的外部工单，但排除最近10分钟内创建的
                DateTime cutoffTimeHours = DateTime.Now.AddHours(-timeLimitHours);
                DateTime cutoffTimeBuffer = DateTime.Now.AddMinutes(-bufferMinutes);

                string prefixDisplay = string.Join(",", workOrderPrefixes);
                _logger.LogDebug($"开始检查{prefixDisplay}开头的外部工单告警，时间范围：{cutoffTimeHours:yyyy-MM-dd HH:mm:ss} 到 {cutoffTimeBuffer:yyyy-MM-dd HH:mm:ss} 之间创建的外部工单");

                // 获取所有指定前缀且创建时间在指定时间范围内的外部工单
                var sampleOrders = await _externalWorkOrderDomainService.QueryAsync(
                    p => p.IsDeleted == 0 &&
                         !string.IsNullOrEmpty(p.Code) &&
                         workOrderPrefixes.Any(prefix => p.Code.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) &&
                         p.CreateTime >= cutoffTimeHours &&
                         p.CreateTime <= cutoffTimeBuffer, // 只检查72小时内但10分钟前创建的外部工单
                    p => p.Id,
                    SqlSugar.OrderByType.Asc);

                _logger.LogDebug($"查询到 {sampleOrders?.Count ?? 0} 个{prefixDisplay}开头且在指定时间范围内的外部工单");

                if (sampleOrders == null || !sampleOrders.Any())
                {
                    _logger.LogDebug($"没有找到符合条件的{prefixDisplay}开头外部工单，退出检查");
                    _logger.LogDebug($"时间范围：{cutoffTimeHours:yyyy-MM-dd HH:mm:ss} 到 {cutoffTimeBuffer:yyyy-MM-dd HH:mm:ss}");
                    _logger.LogDebug($"当前时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    return 0;
                }

                // 输出查询到的工单详情用于调试
                foreach (var order in sampleOrders)
                {
                    _logger.LogDebug($"符合条件的工单: Code={order.Code}, ItemCode={order.ItemCode}, CreateTime={order.CreateTime:yyyy-MM-dd HH:mm:ss}");
                }

                int processedCount = 0;
                int alarmCreatedCount = 0;

                foreach (var sampleOrder in sampleOrders)
                {
                    _logger.LogDebug($"处理{prefixDisplay}开头外部工单: Code={sampleOrder.Code}, SourceCode={sampleOrder.SourceCode}, ItemCode={sampleOrder.ItemCode}, CreateTime={sampleOrder.CreateTime:yyyy-MM-dd HH:mm:ss}");

                    bool hasAlarm = await ProcessSampleOrderAlarm(sampleOrder);
                    processedCount++;

                    if (hasAlarm)
                    {
                        alarmCreatedCount++;
                    }
                }

                // 自动处理已有告警（如果启用）
                int autoHandledCount = 0;
                if (_innerOptions.Value.EnableSampleOrderAlarmAutoHandle)
                {
                    autoHandledCount = await AutoHandleExistingAlarms();
                    if (autoHandledCount > 0)
                    {
                        _logger.LogInformation($"自动处理了 {autoHandledCount} 个已有告警");
                    }
                }

                _logger.LogDebug($"{prefixDisplay}开头外部工单告警检查完成: 处理了 {processedCount} 个外部工单，创建了 {alarmCreatedCount} 个告警，自动处理了 {autoHandledCount} 个已有告警");
                return alarmCreatedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行S开头外部工单告警检查时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 处理指定前缀开头外部工单的告警检查
        /// 此方法只处理指定时间范围内的外部工单
        /// </summary>
        /// <param name="sampleOrder">外部工单</param>
        /// <returns>是否创建了告警</returns>
        private async Task<bool> ProcessSampleOrderAlarm(ExternalWorkOrder sampleOrder)
        {
            try
            {
                string workOrderPrefixConfig = _innerOptions.Value.SampleOrderAlarmWorkOrderPrefix;
                // 解析工单前缀配置，支持逗号分隔的多个前缀
                var workOrderPrefixes = !string.IsNullOrEmpty(workOrderPrefixConfig)
                    ? workOrderPrefixConfig.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray()
                    : new string[] { "S" }; // 默认前缀

                // 检查当前工单是否匹配任一前缀
                bool isMatchingPrefix = workOrderPrefixes.Any(prefix =>
                    sampleOrder.Code.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

                if (!isMatchingPrefix)
                {
                    _logger.LogDebug($"外部工单 {sampleOrder.Code} 不匹配配置的前缀 {string.Join(",", workOrderPrefixes)}，跳过处理");
                    return false;
                }

                string prefixDisplay = string.Join(",", workOrderPrefixes);
                _logger.LogDebug($"开始检查{prefixDisplay}开头外部工单 {sampleOrder.Code} (物料代码: {sampleOrder.ItemCode}) 的生产任务");

                // 检查外部工单的item_code是否为空
                if (string.IsNullOrEmpty(sampleOrder.ItemCode))
                {
                    return false;
                }

                // 检查是否存在生产任务 
                var tasks = await _taskDomainService.QueryAsync(
                    p => p.IsDeleted == 0 &&
                         !string.IsNullOrEmpty(p.ItemCode) &&
                         (p.ItemCode == sampleOrder.ItemCode ||
                          p.ItemCode.Trim() == sampleOrder.ItemCode.Trim() ||
                          p.ItemCode.Contains(sampleOrder.ItemCode)),
                    p => p.Id,
                    SqlSugar.OrderByType.Asc);

                _logger.LogDebug($"查询任务表结果: 找到 {tasks?.Count ?? 0} 个匹配的任务");

                if (tasks != null && tasks.Any())
                {
                    foreach (var task in tasks)
                    {
                        _logger.LogDebug($"找到匹配任务: ID={task.Id}, ItemCode='{task.ItemCode}' (长度: {task.ItemCode?.Length ?? 0})");
                    }
                }

                if (tasks != null && tasks.Any())
                {
                    // 存在生产任务，不需要创建告警
                    _logger.LogDebug($"{prefixDisplay}开头外部工单 {sampleOrder.Code} (物料代码: {sampleOrder.ItemCode}) 存在对应的生产任务，无需创建告警");
                    return false;
                }

                _logger.LogDebug($"{prefixDisplay}开头外部工单 {sampleOrder.Code} (物料代码: {sampleOrder.ItemCode}) 没有找到对应的生产任务，准备生成告警");

                var alarmCode = $"SampleOrder-{sampleOrder.ItemCode}";
                // 检查是否已存在未处理的相同告警代码
                if (await HasUnhandledAlarm(alarmCode))
                {
                    _logger.LogDebug($"已存在未处理告警 {alarmCode}，跳过创建");
                    return false;
                }

                _logger.LogDebug($"创建新告警: {alarmCode}");

                // 创建告警记录
                var alarmReq = new AddOrUpdateAlarmReq
                {
                    AlarmCode = alarmCode,
                    AlarmName = $"样品工单 {sampleOrder.Code} 无生产任务",
                    AlarmContent = $"外部工单 {sampleOrder.Code} (物料代码: {sampleOrder.ItemCode}) 没有对应的生产任务，请及时处理",
                    AlarmLevel = 2,
                    AlarmKind = null,
                    LocationCode = "SYSTEM",
                    PartitionCode = "SAMPLE_ORDER",
                    ItemCode = sampleOrder.ItemCode, // 设置物料代码字段
                    AlarmTime = DateTime.Now,
                    EventId = 0,
                    EventData = $"外部工单ID:{sampleOrder.Id},物料代码:{sampleOrder.ItemCode}" // 简化事件数据，移除内部工单ID
                };
                var addResult = await AddData(alarmReq);
                if (addResult.Code == 0)
                {
                    _logger.LogInformation($"成功创建{prefixDisplay}开头外部工单告警: {alarmCode}");
                    return true;
                }
                else
                {
                    _logger.LogError($"创建{prefixDisplay}开头外部工单告警失败: {addResult.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                string workOrderPrefixConfig = _innerOptions.Value.SampleOrderAlarmWorkOrderPrefix;
                var workOrderPrefixes = !string.IsNullOrEmpty(workOrderPrefixConfig)
                    ? workOrderPrefixConfig.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray()
                    : new string[] { "S" };
                string prefixDisplay = string.Join(",", workOrderPrefixes);
                _logger.LogError(ex, $"处理{prefixDisplay}开头外部工单 {sampleOrder.Code} 告警时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 自动处理已有告警：检查未处理的SampleOrder告警是否有对应的生产任务
        /// </summary>
        /// <returns>自动处理的告警数量</returns>
        private async Task<int> AutoHandleExistingAlarms()
        {
            try
            {
                _logger.LogDebug("开始自动处理已有告警：检查未处理的SampleOrder告警");

                // 查询所有未处理的SampleOrder告警，使用ItemCode字段
                var unhandledAlarms = await _alarmDomainService.QueryAsync(
                    p => p.IsDeleted == 0 &&
                         p.IsHandled == false &&
                         !string.IsNullOrEmpty(p.ItemCode), // 直接检查ItemCode字段是否为空
                    p => p.Id,
                    SqlSugar.OrderByType.Asc);

                if (unhandledAlarms == null || !unhandledAlarms.Any())
                {
                    _logger.LogDebug("没有找到未处理的SampleOrder告警");
                    return 0;
                }

                _logger.LogDebug($"找到 {unhandledAlarms.Count} 个未处理的SampleOrder告警");

                // 提取所有需要检查的物料代码，直接使用ItemCode字段
                var itemCodesToCheck = unhandledAlarms
                    .Select(alarm => alarm.ItemCode)
                    .Where(itemCode => !string.IsNullOrEmpty(itemCode))
                    .Distinct()
                    .ToList();

                if (!itemCodesToCheck.Any())
                {
                    _logger.LogDebug("没有有效的物料代码需要检查");
                    return 0;
                }

                _logger.LogDebug($"需要检查的物料代码: {string.Join(",", itemCodesToCheck)}");

                // 一次性查询所有相关物料代码的生产任务（只调用一次t_task表）
                var allTasks = await _taskDomainService.QueryAsync(
                    p => p.IsDeleted == 0 &&
                         !string.IsNullOrEmpty(p.ItemCode) &&
                         itemCodesToCheck.Any(checkItemCode =>
                             p.ItemCode == checkItemCode ||
                             p.ItemCode.Trim() == checkItemCode.Trim() ||
                             p.ItemCode.Contains(checkItemCode)),
                    p => p.ItemCode,
                    SqlSugar.OrderByType.Asc);

                // 创建已存在生产任务的物料代码集合
                var itemCodesWithTasks = allTasks
                    .Select(t => t.ItemCode)
                    .Where(ic => !string.IsNullOrEmpty(ic))
                    .Distinct()
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                _logger.LogDebug($"找到有生产任务的物料代码: {string.Join(",", itemCodesWithTasks)}");

                // 批量处理告警
                int processedCount = 0;
                foreach (var alarm in unhandledAlarms)
                {
                    try
                    {
                        var itemCode = alarm.ItemCode;
                        if (string.IsNullOrEmpty(itemCode))
                        {
                            continue;
                        }

                        // 检查内存中的结果，避免重复查询数据库
                        if (itemCodesWithTasks.Contains(itemCode))
                        {
                            _logger.LogDebug($"告警 {alarm.AlarmCode} 对应的物料代码 {itemCode} 已存在生产任务，自动处理告警");

                            alarm.IsHandled = true;
                            alarm.HandledTime = DateTime.Now;
                            alarm.ModifierId = 1; // 修改为admin账号的modifier_id
                            alarm.ModifyTime = DateTime.Now;

                            if (await _alarmDomainService.Update(alarm))
                            {
                                processedCount++;
                                _logger.LogDebug($"成功自动处理告警: {alarm.AlarmCode}");
                            }
                            else
                            {
                                _logger.LogWarning($"自动处理告警失败: {alarm.AlarmCode}");
                            }
                        }
                        else
                        {
                            _logger.LogDebug($"告警 {alarm.AlarmCode} 对应的物料代码 {itemCode} 暂无生产任务，不进行自动处理");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"自动处理告警 {alarm.AlarmCode} 时发生异常");
                    }
                }

                _logger.LogInformation($"自动处理SampleOrder告警完成，成功处理 {processedCount} 个告警");
                return processedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "自动处理SampleOrder告警时发生异常");
                return 0;
            }
        }

        /// <summary>
        /// 检查是否存在未处理的相同告警代码
        /// </summary>
        /// <param name="alarmCode">告警代码</param>
        /// <returns></returns>
        public async Task<bool> HasUnhandledAlarm(string alarmCode)
        {
            try
            {
                var existingAlarm = await _alarmDomainService.FindSingleAsync(
                    p => p.IsDeleted == 0 &&
                         p.AlarmCode == alarmCode &&
                         p.IsHandled == false);

                return existingAlarm != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查未处理告警时发生异常");
                return false;
            }
        }
    }
}
