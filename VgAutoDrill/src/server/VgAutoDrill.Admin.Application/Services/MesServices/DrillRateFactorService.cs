using AutoMapper;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.CentralModels;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class DrillRateFactorService : BaseServiceWithoutTree<DrillRateFactor, DrillRateFactorDto, AddOrUpdateDrillRateFactorReq>, IDrillRateFactorService
    {
        private readonly ILogger<DrillRateFactorService> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public DrillRateFactorService(IDrillRateFactorDomainService domainService, ILogger<DrillRateFactorService> logger, IMapper mapper)
            : base(domainService, mapper)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DrillRateFactorDto>>> GetList(GetDrillRateFactorListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DrillRateFactorDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DrillRateFactor>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.DeviceId))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower().Contains(req.DeviceId.ToLower()));
            }
            if (req.Reason.HasValue)
            {
                where = where.And(p => p.Reason == req.Reason);
            }

            if (req.QueryStartTime != null)
            {
                where = where.And(p => p.CreateTime >= req.QueryStartTime.Value || p.ModifyTime >= req.QueryStartTime.Value);
            }
            if (req.QueryEndTime != null)
            {
                where = where.And(p => p.CreateTime <= req.QueryEndTime.Value || p.ModifyTime <= req.QueryEndTime.Value);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DrillRateFactor>, List<DrillRateFactorDto>>(result.ToList());

            foreach (var item in pageDto.List)
            {
                if (item.Reason.HasValue)
                {
                    item.ReasonDesc = GetDescriptionByEnum<DrillRateFactorReason>.GetEnumDescription((DrillRateFactorReason)item.Reason);
                }
            }

            return Success(pageDto);
        }

        /// <summary>
        /// 只有一个时间标记
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddOrUpdate(AddOrUpdateDrillRateFactorReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的入参!");
            }
            if (string.IsNullOrEmpty(req.DeviceId))
            {
                return Fail("未识别有效的DeviceId !");
            }
            if (!req.Reason.HasValue)
            {
                return Fail("未识别有效的稼动率因素!");
            }
            if (req.StartTime == null || req.EndTime == null)
            {
                return Fail("未识别有效的开始时间和结束时间!");
            }

            var data = await _domainService.QueryAsync(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower() == req.DeviceId.ToLower()
            && p.Reason == req.Reason &&
            (SqlFunc.DateDiff(DateType.Second, p.StartTime.Value, req.StartTime.Value) == 0
                || (req.StartTime > p.StartTime && req.StartTime < p.EndTime)
                || SqlFunc.DateDiff(DateType.Second, p.EndTime.Value, req.StartTime.Value) == 0)
            , p => p.CreateTime, OrderByType.Desc);

            if (data == null || data.Count == 0)
            {
                var model = _mapper.Map<DrillRateFactor>(req);
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                model.Status = (int)DataStatusEnum.Enable;
                await _domainService.Add(model);
            }
            else
            {
                if (data[0].StartTime.Value == req.StartTime.Value && data[0].EndTime.Value == req.EndTime.Value)
                {
                    return Success();
                }

                data[0].EndTime = req.EndTime;
                data[0].ModifyTime = DateTime.Now;
                data[0].ModifierId = UserId;

                await _domainService.Update(data[0]);
            }

            return Success();
        }

        /// <summary>
        /// 取设备时间，多个时间标记
        /// </summary>
        /// <param name="reqs"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkAddOrUpdate(List<AddOrUpdateDrillRateFactorReq> reqs)
        {
            if (reqs == null || reqs.Count == 0)
            {
                _logger.LogInformation("DrillRateFactor BulkAddOrUpdate 未识别有效的入参!");
                return Fail("未识别有效的入参!");
            }

            List<DrillRateFactor> addList = new List<DrillRateFactor>();
            List<DrillRateFactor> updateList = new List<DrillRateFactor>();

            foreach (var req in reqs)
            {
                if (string.IsNullOrEmpty(req.DeviceId))
                {
                    _logger.LogInformation($"DrillRateFactor BulkAddOrUpdate DeviceId is null {JsonSerializer.Serialize(req.ToString())}! ");
                    continue;
                }
                if (!req.Reason.HasValue)
                {
                    _logger.LogInformation($"DrillRateFactor BulkAddOrUpdate Reason is null {JsonSerializer.Serialize(req.ToString())}! ");
                    continue;
                }
                if (req.StartTime == null)
                {
                    _logger.LogInformation($"DrillRateFactor BulkAddOrUpdate StartTime is null {JsonSerializer.Serialize(req.ToString())}! ");
                    continue;
                }

                var data = await _domainService.QueryAsync(p => !string.IsNullOrEmpty(p.DeviceId)
                && p.DeviceId.ToLower() == req.DeviceId.ToLower() && p.Reason == req.Reason
                && (SqlFunc.DateDiff(DateType.Second, p.StartTime.Value, req.StartTime.Value) == 0
                || (req.StartTime > p.StartTime && req.StartTime < p.EndTime)
                || SqlFunc.DateDiff(DateType.Second, p.EndTime.Value, req.StartTime.Value) == 0)
                , p => p.CreateTime, OrderByType.Desc);

                if (data == null || data.Count == 0)
                {
                    _logger.LogDebug($"DrillRateFactor BulkAddOrUpdate WithoutData {req.DeviceId} {req.Reason.ToString()} {req.StartTime.ToString()}! ");
                    var model = _mapper.Map<DrillRateFactor>(req);
                    model.CreateTime = DateTime.Now;
                    model.CreatorId = UserId;
                    model.Status = (int)DataStatusEnum.Enable;
                    model.IsDeleted = 0;
                    addList.Add(model);
                }
                else
                {
                    if (data[0].StartTime.Value == req.StartTime.Value
                        && data[0].EndTime.HasValue
                        && req.EndTime.HasValue
                        && data[0].EndTime.Value == req.EndTime.Value)
                    {
                        _logger.LogDebug($"DrillRateFactor BulkAddOrUpdate Data Same ,Req:{req.DeviceId} {req.Reason.ToString()}! Data:StartTime {data[0].StartTime} ,EndTime {data[0].EndTime}");
                        continue;
                    }

                    data[0].EndTime = req.EndTime;
                    data[0].ModifyTime = DateTime.Now;
                    data[0].ModifierId = UserId;

                    updateList.Add(data[0]);
                }
            }

            var addResult = await _domainService.BulkInsert(addList);
            var updateResult = await _domainService.BulkUpdate(updateList);

            _logger.LogDebug($"DrillRateFactor BulkAddOrUpdate addResult {addResult}, updateResult {updateResult} !");

            return Success();
        }

        public async Task<DrillRateFactor> GetRateFactorWithoutEndTime(AddOrUpdateDrillRateFactorReq req)
        {
            var data = await _domainService.QueryAsync(p => !string.IsNullOrEmpty(p.DeviceId)
                && p.DeviceId.ToLower() == req.DeviceId.ToLower() && p.Reason == req.Reason
                && p.EndTime == null
                , p => p.CreateTime, OrderByType.Desc);

            if (data == null || data.Count == 0)
            {
                return null;
            }

            return data[0];
        }

        public override async Task<ResponseDto<string>> Add(AddOrUpdateDrillRateFactorReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的入参!");
            }
            if (string.IsNullOrEmpty(req.DeviceId))
            {
                return Fail("未识别有效的DeviceId !");
            }
            if (!req.Reason.HasValue)
            {
                return Fail("未识别有效的稼动率因素!");
            }
            if (req.StartTime == null || req.EndTime == null)
            {
                return Fail("未识别有效的开始时间和结束时间!");
            }

            var data = await _domainService.QueryAsync(p => !string.IsNullOrEmpty(p.DeviceId)
                && p.DeviceId.ToLower() == req.DeviceId.ToLower() && p.Reason == req.Reason
                && (SqlFunc.DateDiff(DateType.Second, p.StartTime.Value, req.StartTime.Value) == 0
                || (req.StartTime > p.StartTime && req.StartTime < p.EndTime)
                || SqlFunc.DateDiff(DateType.Second, p.EndTime.Value, req.StartTime.Value) == 0)
                , p => p.CreateTime, OrderByType.Desc);

            if (data == null || data.Count == 0)
            {
                _logger.LogDebug($"DrillRateFactor Add WithoutData {req.DeviceId} {req.Reason.ToString()} {req.StartTime.ToString()}! ");
                var model = _mapper.Map<DrillRateFactor>(req);
                model.CreateTime = DateTime.Now;
                model.CreatorId = UserId;
                model.Status = (int)DataStatusEnum.Enable;
                model.IsDeleted = 0;
                await _domainService.Add(model);
            }
            else
            {
                if (data[0].StartTime.Value == req.StartTime.Value
                    && data[0].EndTime.HasValue
                    && req.EndTime.HasValue
                    && data[0].EndTime.Value == req.EndTime.Value)
                {
                    _logger.LogDebug($"DrillRateFactor Add Data Same ,Req:{req.DeviceId} {req.Reason.ToString()}! Data:StartTime {data[0].StartTime} ,EndTime {data[0].EndTime}");
                    return Success();
                }

                data[0].EndTime = req.EndTime;
                data[0].ModifyTime = DateTime.Now;
                data[0].ModifierId = UserId;

                await _domainService.Update(data[0]);
            }

            return Success();
        }
    }
}
