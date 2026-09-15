using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ScheduleRepository(IUnitOfWork unitOfWork,
            IMapper mapper) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                switch (req.QueryOrderBy)
                {
                    case QueryOrderByEnum.OrderByIdDesc:
                        query = query.OrderByDescending(s => s.Id);
                        break;

                    case QueryOrderByEnum.OrderByIdASC:
                        query = query.OrderBy(s => s.Id);
                        break;

                    case QueryOrderByEnum.OrderByStartTimeDesc:
                        query = query.OrderByDescending(s => s.RunningTime);
                        query = query.OrderByDescending(s => s.Id);
                        break;

                    case QueryOrderByEnum.OrderByStartTimeASC:
                        query = query.OrderBy(s => s.RunningTime);
                        query = query.OrderBy(s => s.Id);
                        break;

                    default:
                        query = query.OrderByDescending(s => s.Id);
                        break;
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
        /// 根据scheduleId更新库位panel信息
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="scheduleLocationPanels"></param>
        /// <returns></returns>
        public async Task<bool> UpdateLocationPanels(long scheduleId, List<ScheduleLocationPanel> scheduleLocationPanels)
        {
            if (scheduleId <= 0)
            {
                return false;
            }
            var schedule = await DBClient.Queryable<Schedule>().Includes(s => s.LocationPanels).FirstAsync(s => s.Id == scheduleId);
            if (schedule == null)
            {
                return false;
            }
            schedule.SetScheduleLocationPanel(scheduleLocationPanels);
            return await DBClient.UpdateNav(schedule
                                            , new UpdateNavRootOptions() { IsDisableUpdateRoot = true }//只更新子表，不更新主表
                                             ).Include(s => s.LocationPanels).ExecuteCommandAsync(); //机制是先删除后插入
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

        private ISugarQueryable<Schedule> GetQueryCondition(GetScheduleListReq req)
        {
            var query = DBClient.Queryable<Schedule>();

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
                query = query.Where(s => s.RequestDeviceKind != null && req.RequestDeviceKindList.Contains((DeviceKind)s.RequestDeviceKind));
            }
            if (req.InteractionSequenceList != null && req.InteractionSequenceList.Count > 0)
            {
                query = query.Where(s => s.InteractionSequence != null && req.InteractionSequenceList.Contains((InteractionSequence)s.InteractionSequence));
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
                query = query.Where(s => s.ScheduledTaskStatus != null && req.ScheduledTaskStatusList.Contains((ScheduledTaskStatus)s.ScheduledTaskStatus));
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
                query = query.Where(s => !string.IsNullOrEmpty(s.RouteCode) && req.RouteCodeList.Contains(s.RouteCode));
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
        private T Convert<T>(Schedule schedule) where T : ScheduleDto
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
