using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ScheduleHistoryRepository : BaseRepository<ScheduleHistory>, IScheduleHistoryRepository
    {
        private readonly IMapper _mapper;

        public ScheduleHistoryRepository(IUnitOfWork unitOfWork,
            IMapper mapper) : base(unitOfWork)
        {
            this._mapper = mapper;
        }

        public async Task<IPageList<ScheduleDto>> GetList(GetScheduleListReq req)
        {
            var query = GetQueryCondition(req);

            if (req.IsFromBigScreenWeb)
            {
                //根据设备类型，钻机排在前面，再根据呼叫时间
                query = query.OrderBy(s => s.RequestDeviceKind).OrderBy(s => s.CreateTime, OrderByType.Desc);
            }
            else
            {
                if (req.OrderByIDDesc)
                {
                    query = query.OrderByDescending((s) => s.Id);
                }
                else
                {
                    query = query.OrderBy((s) => s.Id);
                }
            }
            var convertedResult = new List<ScheduleDto>();
            var totalCount = await query.CountAsync();
            var pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;

            if (totalCount == 0)
            {
                req.PageNum = 0;
                return new PageList<ScheduleDto>(convertedResult, req.PageNum, req.PageSize, totalCount);
            }
            else
            {
                if (pageCount < req.PageNum)
                {
                    req.PageNum = pageCount;
                }

                var data = await query.ToPageListAsync(req.PageNum, req.PageSize, totalCount);
                data.ForEach(d => convertedResult.Add(Convert<ScheduleDto>(d)));
                return new PageList<ScheduleDto>(convertedResult, req.PageNum, req.PageSize, totalCount);
            }
        }

        public async Task<IPageList<ScheduleDto>> GetScheduleWithRequestList(GetScheduleListReq req)
        {
            var query = GetQueryCondition(req);

            if (req.OrderByIDDesc)
            {
                query = query.OrderByDescending((s) => s.Id);
            }
            else
            {
                query = query.OrderBy((s) => s.Id);
            }

            var convertedResult = new List<ScheduleDto>();
            var totalCount = await query.CountAsync();
            var pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;

            if (totalCount == 0)
            {
                req.PageNum = 0;
                return new PageList<ScheduleDto>(convertedResult, req.PageNum, req.PageSize, totalCount);
            }
            else
            {
                if (pageCount < req.PageNum)
                {
                    req.PageNum = pageCount;
                }

                var data = await query.ToPageListAsync(req.PageNum, req.PageSize, totalCount);
                data.ForEach(d => convertedResult.Add(Convert<ScheduleDto>(d)));
                return new PageList<ScheduleDto>(convertedResult, req.PageNum, req.PageSize, totalCount);
            }
        }

        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> ExsistSchedule(GetScheduleListReq req)
        {
            var query = GetQueryCondition(req);

            return await query.AnyAsync();
        }

        private ISugarQueryable<ScheduleHistory> GetQueryCondition(GetScheduleListReq req)
        {
            var query = DBClient.Queryable<ScheduleHistory>();

            query = query.Where(s => s.IsDeleted == 0);
            if (req.Id > 0)
            {
                query = query.Where(s => s.Id == req.Id);
            }
            if (req.RouteCodeList != null && req.RouteCodeList.Any())
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.RouteCode) && req.RouteCodeList.Contains(s.RouteCode));
            }
            if (req.IsAuxiliary.HasValue)
            {
                query = query.Where(s => s.IsAuxiliary == req.IsAuxiliary);
            }
            if (req.HasAgvSetted == true)
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.RequireDeviceId) && s.MasterScheduleId > 0);
            }
            if (req.HasRawItemSetted == true)
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.ItemCode));
            }
            if (req.HasAgvSetted == false)
            {
                query = query.Where(s => string.IsNullOrEmpty(s.RequireDeviceId));
            }
            if (req.IsMaster.HasValue)
            {
                query = query.Where(s => s.IsMaster == req.IsMaster);
            }
            if (req.IsBarcodeOk.HasValue)
            {
                query = query.Where(s => s.IsBarcodeOk == req.IsBarcodeOk);
            }
            if (req.RequestDeviceKindList != null && req.RequestDeviceKindList.Count > 0)
            {
                if (req.RequestDeviceKindList.Count == 1)
                {
                    query = query.Where(s => s.RequestDeviceKind == req.RequestDeviceKindList.FirstOrDefault());
                }
                else
                {
                    query = query.Where(s => req.RequestDeviceKindList.Contains(s.RequestDeviceKind));
                }
            }
            if (req.InteractionSequenceList != null && req.InteractionSequenceList.Count > 0)
            {
                if (req.InteractionSequenceList.Count == 1)
                {
                    query = query.Where(s => req.InteractionSequenceList.FirstOrDefault() == s.InteractionSequence);
                }
                else
                {
                    query = query.Where(s => req.InteractionSequenceList.Contains(s.InteractionSequence));
                }
            }
            if (!string.IsNullOrEmpty(req.TraceId))
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.Code) && s.Code.Equals(req.TraceId));
            }
            if (!string.IsNullOrEmpty(req.RequireDeviceId))
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.RequireDeviceId) && s.RequireDeviceId.ToLower().Contains(req.RequireDeviceId.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.SourceDeviceId))
            {
                query = query.Where(s => (!string.IsNullOrEmpty(s.SourceDeviceId) && s.SourceDeviceId.ToLower().Contains(req.SourceDeviceId.ToLower()))
                        || (!string.IsNullOrEmpty(s.SubDeviceCode) && s.SubDeviceCode.ToLower().Contains(req.SourceDeviceId.ToLower())));
            }
            if (!string.IsNullOrEmpty(req.ItemName))
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.ItemName) && s.ItemName.Contains(req.ItemName));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where(s => (!string.IsNullOrEmpty(s.ItemCode) && s.ItemCode.ToLower().Contains(req.ItemCode.ToLower()))
                 || (!string.IsNullOrEmpty(s.RequestSummaryInfo) && s.RequestSummaryInfo.ToLower().Contains(req.ItemCode.ToLower())));
            }
            if (!string.IsNullOrEmpty(req.TaskId))
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.TaskId) && s.TaskId.ToLower().Contains(req.TaskId.ToLower()));
            }
            if (req.ScheduledTaskStatusList != null && req.ScheduledTaskStatusList.Count > 0)
            {
                if (req.ScheduledTaskStatusList.Count == 1)
                {
                    query = query.Where(s => req.ScheduledTaskStatusList.FirstOrDefault() == s.ScheduledTaskStatus);
                }
                else
                {
                    query = query.Where(s => req.ScheduledTaskStatusList.Contains(s.ScheduledTaskStatus));
                }
            }
            if (req.StartTime != null)
            {
                query = query.Where(s => s.CreateTime >= req.StartTime.Value);
            }
            if (req.EndTime != null)
            {
                query = query.Where(s => s.CreateTime <= req.EndTime.Value);
            }
            if (!string.IsNullOrEmpty(req.RoutingKey))
            {
                query = query.Where(s => s.RoutingKey != null && s.RoutingKey.ToLower().Equals(req.RoutingKey.ToLower()));
            }
            if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
            {
                if (req.RouteCodeList.Count == 1)
                {
                    query = query.Where(s => req.RouteCodeList.FirstOrDefault() == s.RouteCode);
                }
                else
                {
                    query = query.Where(s => req.RouteCodeList.Contains(s.RouteCode));
                }
            }
            if (req.IsUrgent >= 0)
            {
                query = query.Where(s => s.IsUrgent == req.IsUrgent);
            }
            if (req.IsAllPanelSent.HasValue)
            {
                query = query.Where(s => s.IsAllPanelSent == req.IsAllPanelSent);
            }
            if (!string.IsNullOrEmpty(req.SubDeviceCode))
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.SubDeviceCode)! && s.SubDeviceCode.ToLower().Contains(req.SubDeviceCode.ToLower()));
            }
            if (req.RunningStartTime != null)
            {
                query = query.Where(s => s.RunningTime >= req.RunningStartTime.Value);
            }
            if (req.RunningEndTime != null)
            {
                query = query.Where(s => s.RunningTime <= req.RunningEndTime.Value);
            }
            if (!string.IsNullOrEmpty(req.AGVPayloadPanels))
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.AGVPayloadPanels)! && s.AGVPayloadPanels.ToLower().Contains(req.AGVPayloadPanels.ToLower()));
            }
            return query;
        }
        private T Convert<T>(ScheduleHistory schedule) where T : ScheduleDto
        {
            var result = _mapper.Map<T>(schedule);
            return result;
        }

        public async Task<IPageList<ScheduleInfo>> GetFullDataList(GetScheduleListReq req)
        {
            var query = DBClient.Queryable<Schedule, Rack>
               ((s, r) => new object[]
                   {
                        JoinType.Inner, s.SubDeviceCode == r.RelateDeviceCode,
                   });
            query = query.Where((s, r) => s.IsDeleted == 0);
            if (req.Id > 0)
            {
                query = query.Where((s, r) => s.Id == req.Id);
            }
            if (req.RouteCodeList != null && req.RouteCodeList.Any())
            {
                query = query.Where((s, r) => !string.IsNullOrEmpty(s.RouteCode) && req.RouteCodeList.Contains(s.RouteCode));
            }
            if (req.IsAuxiliary.HasValue)
            {
                query = query.Where((s, r) => s.IsAuxiliary == req.IsAuxiliary);
            }
            if (req.HasAgvSetted == true)
            {
                query = query.Where((s, r) => !string.IsNullOrEmpty(s.RequireDeviceId) && s.MasterScheduleId > 0);
            }
            if (req.HasRawItemSetted == true)
            {
                query = query.Where((s, r) => !string.IsNullOrEmpty(s.ItemCode));
            }
            if (req.HasAgvSetted == false)
            {
                query = query.Where((s, r) => string.IsNullOrEmpty(s.RequireDeviceId));
            }
            if (req.IsMaster.HasValue)
            {
                query = query.Where((s, r) => s.IsMaster == req.IsMaster);
            }
            if (req.IsBarcodeOk.HasValue)
            {
                query = query.Where(s => s.IsBarcodeOk == req.IsBarcodeOk);
            }
            if (req.RequestDeviceKindList != null && req.RequestDeviceKindList.Count > 0)
            {
                query = query.Where((s, r) => s.RequestDeviceKind != null && req.RequestDeviceKindList.Contains((DeviceKind)s.RequestDeviceKind));
            }
            if (req.InteractionSequenceList != null && req.InteractionSequenceList.Count > 0)
            {
                query = query.Where((s, r) => s.InteractionSequence != null && req.InteractionSequenceList.Contains((InteractionSequence)s.InteractionSequence));
            }
            if (!string.IsNullOrEmpty(req.TraceId))
            {
                query = query.Where((s, r) => !string.IsNullOrEmpty(s.Code) && s.Code.Equals(req.TraceId));
            }
            if (!string.IsNullOrEmpty(req.RequireDeviceId))
            {
                query = query.Where((s, r) => !string.IsNullOrEmpty(s.RequireDeviceId) && s.RequireDeviceId.ToLower().Contains(req.RequireDeviceId.ToLower()));
            }
            if (!string.IsNullOrEmpty(req.SourceDeviceId))
            {
                query = query.Where((s, r) => (!string.IsNullOrEmpty(s.SourceDeviceId) && s.SourceDeviceId.ToLower().Contains(req.SourceDeviceId.ToLower()))
                || (!string.IsNullOrEmpty(s.SubDeviceCode) && s.SubDeviceCode.ToLower().Contains(req.SourceDeviceId.ToLower())));
            }
            if (!string.IsNullOrEmpty(req.ItemName))
            {
                query = query.Where((s, r) => !string.IsNullOrEmpty(s.ItemName) && s.ItemName.Contains(req.ItemName));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((s, r) => (!string.IsNullOrEmpty(s.ItemCode) && s.ItemCode.ToLower().Contains(req.ItemCode.ToLower()))
                 || (!string.IsNullOrEmpty(s.RequestSummaryInfo) && s.RequestSummaryInfo.ToLower().Contains(req.ItemCode.ToLower())));
            }
            if (!string.IsNullOrEmpty(req.TaskId))
            {
                query = query.Where((s, r) => !string.IsNullOrEmpty(s.TaskId) && s.TaskId.ToLower().Contains(req.TaskId.ToLower()));
            }
            if (req.ScheduledTaskStatusList != null && req.ScheduledTaskStatusList.Count > 0)
            {
                query = query.Where((s, r) => s.ScheduledTaskStatus != null && req.ScheduledTaskStatusList.Contains((ScheduledTaskStatus)s.ScheduledTaskStatus));
            }
            if (req.StartTime != null)
            {
                query = query.Where((s, r) => s.CreateTime >= req.StartTime.Value);
            }
            if (req.EndTime != null)
            {
                query = query.Where((s, r) => s.CreateTime <= req.EndTime.Value);
            }
            if (!string.IsNullOrEmpty(req.RoutingKey))
            {
                query = query.Where((s, r) => s.RoutingKey != null && s.RoutingKey.ToLower().Equals(req.RoutingKey.ToLower()));
            }
            if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
            {
                query = query.Where((s, r) => !string.IsNullOrEmpty(s.RouteCode) && req.RouteCodeList.Contains(s.RouteCode));
            }
            if (req.IsUrgent >= 0)
            {
                query = query.Where((s, r) => s.IsUrgent == req.IsUrgent);
            }
            if (req.IsAllPanelSent.HasValue)
            {
                query = query.Where((s, r) => s.IsAllPanelSent == req.IsAllPanelSent);
            }
            if (!string.IsNullOrEmpty(req.SubDeviceCode))
            {
                query = query.Where((s, r) => !string.IsNullOrEmpty(s.SubDeviceCode)! && s.SubDeviceCode.ToLower().Contains(req.SubDeviceCode.ToLower()));
            }
            if (req.RunningStartTime != null)
            {
                query = query.Where((s, r) => s.RunningTime >= req.RunningStartTime.Value);
            }
            if (req.RunningEndTime != null)
            {
                query = query.Where((s, r) => s.RunningTime <= req.RunningEndTime.Value);
            }
            if (!string.IsNullOrEmpty(req.AGVPayloadPanels))
            {
                query = query.Where((s, r) => !string.IsNullOrEmpty(s.AGVPayloadPanels)! && s.AGVPayloadPanels.ToLower().Contains(req.AGVPayloadPanels.ToLower()));
            }

            if (req.OrderByIDDesc)
            {
                query = query.OrderByDescending((s, r) => s.Id);
            }
            else
            {
                query = query.OrderBy((s, r) => s.Id);
            }

            var convertedResult = new List<ScheduleInfo>();
            var totalCount = await query.CountAsync();
            var pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;

            if (totalCount == 0)
            {
                req.PageNum = 0;
                return new PageList<ScheduleInfo>(convertedResult, req.PageNum, req.PageSize, totalCount);
            }
            else
            {
                if (pageCount < req.PageNum)
                {
                    req.PageNum = pageCount;
                }

                var data = await query.Select((s, r) => new ScheduleInfo
                {
                    Id = s.Id,
                    WareHouseCode = r.WareHouseCode,
                    AGVPayloadPanels = s.AGVPayloadPanels,
                    AllocateTime = s.AllocateTime,
                    IsAllPanelSent = s.IsAllPanelSent,
                    IsAuxiliary = s.IsAuxiliary,
                    CanceledTime = s.CanceledTime,
                    ChangedBehavior = s.ChangedBehavior,
                    ChangedSpindles = s.ChangedSpindles,
                    Code = s.Code,
                    CompletedTime = s.CompletedTime,
                    CreateTime = s.CreateTime,
                    CreatorId = s.CreatorId,
                    EndLocation = s.EndLocation,
                    FailedTime = s.FailedTime,
                    InteractionSequence = s.InteractionSequence,
                    IsMaster = s.IsMaster,
                    IsUrgent = s.IsUrgent,
                    ItemCode = s.ItemCode,
                    ItemId = s.ItemId,
                    ItemName = s.ItemName,
                    MasterScheduleId = s.MasterScheduleId,
                    ModifierId = s.ModifierId,
                    ModifyTime = s.ModifyTime,
                    RequestJson = s.RequestJson,
                    RequestDeviceKind = s.RequestDeviceKind,
                    RoutingKey = s.RoutingKey,
                    Remark = s.Remark,
                    Priority = s.Priority,
                    RequestInteractionBehavior = s.RequestInteractionBehavior,
                    RequestInteractionBehaviorName = s.RequestInteractionBehaviorName,
                    RequestSummaryInfo = s.RequestSummaryInfo,
                    RequireDeviceId = s.RequireDeviceId,
                    RouteCode = s.RouteCode,
                    RouteId = s.RouteId,
                    RouteName = s.RouteName,
                    RunningTime = s.RunningTime,
                    TotalRawCount = s.TotalRawCount,
                    ScheduledTaskStatus = s.ScheduledTaskStatus,
                    SourceDeviceId = s.SourceDeviceId,
                    StartLocation = s.StartLocation,
                    Status = s.Status,
                    SubDeviceCode = s.SubDeviceCode,
                    TaskId = s.TaskId,
                    WarningCode = s.WarningCode,
                    WarningMessage = s.WarningMessage,
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
                return new PageList<ScheduleInfo>(data, req.PageNum, req.PageSize, totalCount);
            }

        }
    }
}
