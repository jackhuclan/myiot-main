using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{

    public class ExternalTaskService : BaseService, IExternalTaskService
    {
        private readonly ITaskDomainService _domainService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICentralOnlineDevice _centralOnlineDevice;
        public ExternalTaskService(ITaskDomainService domainService,
            IUnitOfWork unitOfWork,
            ICentralOnlineDevice centralOnlineDevice,
        IMapper mapper)
        {
            _domainService = domainService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _centralOnlineDevice = centralOnlineDevice;
        }

        /// <summary>
        /// 获取钻机信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<ExternalTaskDto>>> GetDeviceList(GetDeviceReq req)
        {
            var db = _unitOfWork.GetDbClient();
            var query = db.Queryable<Device, Model.Entites.Mes.WorkTask>
            ((d, t) => new object[]
              {
                      JoinType.Left,  d.Code == t.WorkStationCode
              });
            query = query.Where((d, t) => d.IsDeleted == 0 && d.DeviceTypeCode == "drill");
            if (!string.IsNullOrEmpty(req.Code) && !string.IsNullOrEmpty(req.Name))
            {
                query = query.Where((d, t) => d.Code == req.Code || d.Name == req.Name);
            }

            if (!string.IsNullOrEmpty(req.Code) && string.IsNullOrEmpty(req.Name))
            {
                query = query.Where((d, t) => d.Code == req.Code);
            }

            if (string.IsNullOrEmpty(req.Code) && !string.IsNullOrEmpty(req.Name))
            {
                query = query.Where((d, t) => d.Name == req.Name);
            }

            if (req.DeviceStatus != null)
            {
                query = query.Where((d, t) => d.DeviceStatus == req.DeviceStatus);
            }

            var result = query.GroupBy((d, t) => d.Code).ToList();

            var drillList = result.Select(t => t.Code).Distinct().ToList();

            List<ExternalTaskDto> list = new List<ExternalTaskDto>();

            if (drillList.Count() > 0)
            {
                var allTaskList = db.Queryable<Model.Entites.Mes.WorkTask>().Where(t => t.IsDeleted == 0 && drillList.Contains(t.WorkStationCode));
                if (!string.IsNullOrEmpty(req.Route))
                {
                    allTaskList = allTaskList.Where(t => t.RouteCode == req.Route);
                }
                var allTask = allTaskList.ToList();
                var onlineDevices = await _centralOnlineDevice.GetOnlineDevices();
                List<TaskStatusEnum> lst = new List<TaskStatusEnum> { TaskStatusEnum.BUFFERED, TaskStatusEnum.SENDING };
                for (int i = 0; i < drillList.Count(); i++)
                {
                    var device = result.Where(t => t.Code == drillList[i]).First();
                    var model = new ExternalTaskDto
                    {
                        Code = drillList[i],
                        Name = device.Name,
                        DeviceStatus = device.DeviceStatus,
                        SpindleNum = device.SpindleNum,
                        DeviceKind = device.DeviceKind,
                        CommitTask = allTask.Where(t => t.WorkStationCode == drillList[i] && t.TaskStatus == TaskStatusEnum.COMMITED).Count(),
                        DraftTask = allTask.Where(t => t.WorkStationCode == drillList[i] && t.TaskStatus == TaskStatusEnum.DRAFT).Count()
                    };

                    var DrillingTaskCode = allTask.Where(t => t.WorkStationCode == drillList[i] && t.TaskStatus == TaskStatusEnum.BEGIN).OrderBy(t => t.RealStartTime);
                    model.DrillingTask = DrillingTaskCode.Count() == 0 ? "" : DrillingTaskCode.FirstOrDefault().Code;
                    var WaitWorkTaskCode = allTask.Where(t => t.WorkStationCode == drillList[i] && lst.Contains((TaskStatusEnum)t.TaskStatus));
                    model.WaitWorkTask = WaitWorkTaskCode.Count() == 0 ? "" : WaitWorkTaskCode.FirstOrDefault().Code;
                    var Route = allTask.Where(t => t.Code == drillList[i]).Select(t => t.RouteCode).ToList();
                    model.Route = Route.Count() > 0 ? Route.FirstOrDefault() : req.Route;
                    var onlineInfo = onlineDevices.FirstOrDefault(t => t.DeviceId.ToLower() == device.Code.ToLower());
                    model.Percentage = onlineInfo == null ? 0 : onlineInfo.Percentage;
                    model.ExistRawPanel = onlineInfo == null ? false : onlineInfo.ExistRawPanel;
                    model.ExistClinkerPanel = onlineInfo == null ? false : onlineInfo.ExistClinkerPanel;
                    model.IsAllVerifyOK = onlineInfo == null ? false : onlineInfo.IsAllVerifyOK;
                    model.IsCompleteUpperPanel = onlineInfo == null ? false : onlineInfo.IsCompleteUpperPanel;
                    model.VerifyFailShafts = onlineInfo == null ? new List<string>() : onlineInfo.VerifyFailShafts;
                    model.UpperPanelProgress = onlineInfo == null ? "0" : onlineInfo.UpperPanelProgress;
                    list.Add(model);
                }
            }
            var rsp = _mapper.Map<List<ExternalTaskDto>>(list);
            return Success(rsp);
        }

        /// <summary>
        /// 获取钻机任务列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<TaskDto>>> GetTaskList(GetTaskReq req)
        {
            var db = _unitOfWork.GetDbClient();
            var query = db.Queryable<Device, Model.Entites.Mes.WorkTask>
            ((d, t) => new object[]
              {
                      JoinType.Inner,  d.Code == t.WorkStationCode
              });
            query = query.Where((d, t) => d.IsDeleted == 0 && t.IsDeleted == 0 && d.DeviceTypeCode == "drill" && t.WorkStationCode == req.DeviceCode);

            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                query = query.Where((d, t) => t.WorkStationCode == req.DeviceCode);
            }

            var result = query.OrderBy((d, t) => t.Code).GroupBy((d, t) => t.Code).ToList();
            var drillList = result.Select(t => t.Code).Distinct().ToList();

            if (drillList.Count() > 0)
            {
                List<TaskStatusEnum> allTaskStatus = await GetDrillSelectStatus(req);
                var allTask = db.Queryable<Model.Entites.Mes.WorkTask, Model.Entites.Mes.WorkOrder>((t, w) => t.WorkOrderCode == w.Code)
                    .Where((t, w) => t.IsDeleted == 0 && drillList.Contains(t.WorkStationCode));
                //&& t.StartTime >= DateTime.Now.AddDays(-30)

                allTask = allTask.Where((t, w) => allTaskStatus.Contains((TaskStatusEnum)t.TaskStatus));
                var allTaskList = allTask.OrderBy((t, w) => t.TaskStatus);
                var data = await allTaskList.Select((t, w) => new TaskDto
                {
                    Code = t.Code,
                    WorkOrderName = t.WorkOrderName,
                    WorkOrderCode = t.WorkOrderCode,
                    WorkStationName = t.WorkStationName,
                    SourceCode = w.SourceCode,
                    WorkStationCode = t.WorkStationCode,
                    ProcessName = t.ProcessName,
                    ProcessCode = t.ProcessCode,
                    ItemName = t.ItemName,
                    ItemCode = t.ItemCode,
                    BatchCode = t.BatchCode,
                    Specification = t.Specification,
                    UnitOfMeasure = t.UnitOfMeasure,
                    Quantity = t.Quantity,
                    QuantityProduced = t.QuantityProduced,
                    QuantityQuanlify = t.QuantityQuanlify,
                    QuantityUnquanlify = t.QuantityUnquanlify,
                    ClientName = t.ClientName,
                    ClientCode = t.ClientCode,
                    StartTime = t.StartTime,
                    Duration = t.Duration,
                    EndTime = t.EndTime,
                    RequestDate = t.RequestDate,
                    TaskStatus = t.TaskStatus,
                    KeyFlag = t.KeyFlag,
                    NowWadCount = t.NowWadCount,
                    PanelCount = t.PanelCount,
                    RealStartTime = t.RealStartTime,
                    RealDuration = t.RealDuration,
                    RealEndTime = t.RealEndTime,
                    RouteCode = t.RouteCode,
                    RouteName = t.RouteName,
                    IsUrgent = t.IsUrgent,
                    SpecGroup = t.SpecGroup,
                }).ToListAsync();
                var rsp = _mapper.Map<List<TaskDto>>(data);
                return Success(rsp);
            }
            else
            {
                return Fail<List<TaskDto>>("");
            }
        }

        private async Task<List<TaskStatusEnum>> GetDrillSelectStatus(GetTaskReq req)
        {
            var allAtatus = await GetDrillAllStatus();

            List<string> tempList = new List<string>();

            if (req.IsContainDraftTask == false)
            {
                allAtatus.Remove(TaskStatusEnum.DRAFT);
            }

            if (req.IsContainCompleteTask == false)
            {
                allAtatus.Remove(TaskStatusEnum.FINISH);
            }

            return allAtatus;
        }

        private async Task<List<TaskStatusEnum>> GetDrillAllStatus()
        {
            List<TaskStatusEnum> lst = new List<TaskStatusEnum>();
            for (int i = 0; i < Enum.GetValues(typeof(TaskStatusEnum)).Length; i++)
            {
                lst.Add((TaskStatusEnum)Enum.GetValues(typeof(TaskStatusEnum)).GetValue(i));
            }

            return lst;
        }

        private async Task<Model.Entites.Mes.WorkTask> SetTimeForTask(Model.Entites.Mes.WorkTask task, int j, int tempQty, DateTime updateTime, DateTime targetStartTime)
        {
            var tempTask = task;
            var tempDuration = task.Duration;
            tempTask.StartTime = targetStartTime.AddMinutes((double)((j - tempQty) * tempDuration));
            tempTask.EndTime = task.StartTime.Value.AddMinutes((double)tempDuration);
            tempTask.ModifyTime = updateTime;
            tempTask.ModifierId = UserId;

            return tempTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adjustTaskInfo"></param>
        /// <param name="StartTime"></param>
        /// <param name="targetTaskInfo"></param>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        private async Task<List<Model.Entites.Mes.WorkTask>> SetAdjustTaskTime(List<Model.Entites.Mes.WorkTask> adjustTaskInfo, DateTime StartTime, List<string> targetTaskInfo, DateTime updateTime)
        {
            for (int i = 0; i < adjustTaskInfo.Count(); i++)
            {
                var tempDuration = adjustTaskInfo[i].Duration.Value;
                adjustTaskInfo[i].WorkStationId = targetTaskInfo[0].ToInt();
                adjustTaskInfo[i].WorkStationName = targetTaskInfo[1];
                adjustTaskInfo[i].WorkStationCode = targetTaskInfo[2];
                adjustTaskInfo[i].StartTime = StartTime.AddMinutes((double)tempDuration * i);
                adjustTaskInfo[i].EndTime = adjustTaskInfo[i].StartTime.Value.AddMinutes((double)tempDuration);
                adjustTaskInfo[i].ModifyTime = updateTime;
                adjustTaskInfo[i].ModifierId = UserId;
            }
            return adjustTaskInfo;
        }

        private async Task<List<Model.Entites.Mes.WorkTask>> GetAdjustTaskInfo(List<string> code, SqlSugarScope db)
        {
            List<TaskStatusEnum> lst = new List<TaskStatusEnum> { TaskStatusEnum.COMMITED, TaskStatusEnum.DRAFT };
            var temp = db.Queryable<Model.Entites.Mes.WorkTask>().Where(t => t.IsDeleted == 0 && code.Contains(t.Code) && lst.Contains((TaskStatusEnum)t.TaskStatus)).ToList();
            return temp;
        }

        /// <summary>
        /// 根据目标任务调整任务顺序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> TaskMoveByTargetTask(TaskMoveByTargetTaskReq req)
        {
            if (req == null || req.Code.Count() == 0)
            {
                return Fail("数据格式错误");
            }

            if (string.IsNullOrEmpty(req.TargetTaskCode))
            {
                return Fail("数据格式错误");
            }

            try
            {
                var updateTime = DateTime.Now;
                var db = _unitOfWork.GetDbClient();
                List<long> IdList = new List<long>();
                List<Model.Entites.Mes.WorkTask> adjustTaskInfo = new List<Model.Entites.Mes.WorkTask>();
                adjustTaskInfo = await GetAdjustTaskInfo(req.Code, db);
                if (adjustTaskInfo.Count() < req.Code.Count)
                {
                    return Fail("选择的任务不能被移动：被移动的任务状态有错误");
                }

                if (adjustTaskInfo.Count() > 0)
                {
                    IdList = adjustTaskInfo.Select(t => t.Id).ToList();
                }

                List<string> drill = new List<string>();
                int tempQty = 0;
                List<Model.Entites.Mes.WorkTask> temps = new List<Model.Entites.Mes.WorkTask>();
                var TargetTaskInfo = db.Queryable<Model.Entites.Mes.WorkTask>().Where(t => t.IsDeleted == 0 && t.Code == req.TargetTaskCode).ToList();

                var tempTime = TargetTaskInfo[0].StartTime.Value > DateTime.Now ? TargetTaskInfo[0].StartTime.Value : DateTime.Now;
                var targetStartTime = tempTime.AddMinutes((double)TargetTaskInfo[0].Duration);
                var targetDrillCode = TargetTaskInfo[0].WorkStationCode;
                adjustTaskInfo = await GetAdjustTaskInfo(req.Code, db);
                if (adjustTaskInfo.Count() < req.Code.Count)
                {
                    return Fail("选择的任务不能被移动：被移动的任务状态有错误");
                }

                drill.Add(TargetTaskInfo[0].Id.ToString());
                drill.Add(TargetTaskInfo[0].WorkStationName);
                drill.Add(TargetTaskInfo[0].WorkStationCode);
                adjustTaskInfo = await SetAdjustTaskTime(adjustTaskInfo, targetStartTime, drill, updateTime);

                var allTask = db.Queryable<Model.Entites.Mes.WorkTask>().Where(t => t.IsDeleted == 0 && t.WorkStationCode == targetDrillCode && t.StartTime >= tempTime && t.Code != req.TargetTaskCode).OrderBy(t => t.StartTime).ToList();
                var allTarGetDate = allTask.DistinctBy(t => new { t.StartTime.Value.Date }).Select(t => t.StartTime.Value.Date).Distinct().ToList();

                for (int i = 0; i < allTarGetDate.Count(); i++)
                {
                    var tempTask = allTask.Where(t => t.StartTime.Value.Date == allTarGetDate[i].Date && t.StartTime >= targetStartTime).OrderBy(t => t.StartTime).ToList();
                    var minStateTime = tempTask.Select(t => t.StartTime).Min();
                    var adjustTaskMaxStartTime = adjustTaskInfo[adjustTaskInfo.Count - 1].EndTime.Value;

                    for (int j = 0; j < tempTask.Count(); j++)
                    {
                        if (tempTask[j].StartTime.Value >= adjustTaskInfo[adjustTaskInfo.Count() - 1].EndTime.Value)
                        {
                            break;
                        }

                        if (IdList.Contains(tempTask[j].Id))
                        {
                            tempQty++;
                            temps.Add(tempTask[j]);
                            continue;
                        }

                        if (j > 0 && tempTask[j].StartTime > tempTask[j - 1].EndTime)
                        {
                            break;
                        }

                        tempTask[j] = await SetTimeForTask(tempTask[j], j, tempQty, updateTime, adjustTaskMaxStartTime);
                        IdList.Add(tempTask[j].Id);
                        adjustTaskInfo.Add(tempTask[j]);
                    }
                }

                _unitOfWork.BeginTran();
                await _domainService.BulkUpdate(adjustTaskInfo);
                _unitOfWork.CommitTran();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }

            return Success();
        }

        /// <summary>
        /// 根据目标钻机和时间调整任务顺序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> TaskMoveByDeviceAndDate(TaskMoveByDeviceAndDateReq req)
        {
            if (req == null || req.Code.Count() == 0)
            {
                return Fail("数据格式错误");
            }

            if (string.IsNullOrEmpty(req.TargetDeviceCode) || req.TargetDate == null)
            {
                return Fail("数据格式错误");
            }
            var updateTime = DateTime.Now;
            var db = _unitOfWork.GetDbClient();
            List<long> IdList = new List<long>();
            List<Model.Entites.Mes.WorkTask> adjustTaskInfo = new List<Model.Entites.Mes.WorkTask>();
            adjustTaskInfo = await GetAdjustTaskInfo(req.Code, db);
            if (adjustTaskInfo.Count() < req.Code.Count)
            {
                return Fail("选择的任务不能被移动：被移动的任务状态有错误");
            }

            if (adjustTaskInfo.Count() > 0)
            {
                IdList = adjustTaskInfo.Select(t => t.Id).ToList();
            }

            List<string> drill = new List<string>();
            var tempDate = DateTime.Now.Date > req.TargetDate.Value.Date ? DateTime.Now.Date : req.TargetDate.Value.Date;
            var allTask = db.Queryable<Model.Entites.Mes.WorkTask>().Where(t => t.IsDeleted == 0 && t.WorkStationCode == req.TargetDeviceCode && t.StartTime.Value.Date >= tempDate).OrderBy(t => t.StartTime).ToList();
            var TargetTaskInfo = db.Queryable<Model.Entites.Mes.WorkTask>().Where(t => t.IsDeleted == 0 && t.WorkStationCode == req.TargetDeviceCode).ToList();

            if (TargetTaskInfo.Count() > 0)
            {
                drill.Add(TargetTaskInfo[0].Id.ToString());
                drill.Add(TargetTaskInfo[0].WorkStationName);
                drill.Add(TargetTaskInfo[0].WorkStationCode);
            }
            else
            {
                var drillInfo = db.Queryable<Device>().Where(t => t.IsDeleted == 0 && t.Code == req.TargetDeviceCode).First();
                drill.Add(drillInfo.Id.ToString());
                drill.Add(drillInfo.Name);
                drill.Add(drillInfo.Code);
            }

            var allTarGetDate = allTask.DistinctBy(t => new { t.StartTime.Value.Date }).Select(t => t.EndTime.Value.Date).Distinct().ToList();
            var startTime = DateTime.Now;
            if (allTarGetDate.Count() > 0)
            {
                for (int i = 0; i < allTarGetDate.Count(); i++)
                {
                    var tempTask = allTask.Where(t => t.StartTime.Value.Date == allTarGetDate[i].Date).OrderBy(t => t.StartTime).ToList();
                    var tempStrtTime = DateTime.Now;

                    if (i == 0)
                    {
                        if (tempTask.Count() > 0)
                        {
                            var tempMaxStartTime = tempTask.Last().StartTime.Value;
                            var duration = tempTask.Last().Duration.Value;

                            startTime = tempMaxStartTime.AddMinutes((double)duration);
                        }
                        else
                        {
                            tempTask = allTask.Where(t => t.StartTime.Value.Date == allTarGetDate[i].Date).OrderBy(t => t.EndTime).ToList();
                            if (tempTask.Count() > 0)
                            {
                                startTime = tempTask.Last().EndTime.Value;
                            }
                        }
                        startTime = startTime > DateTime.Now ? startTime : DateTime.Now;
                        adjustTaskInfo = await SetAdjustTaskTime(adjustTaskInfo, startTime, drill, updateTime);
                    }
                    else
                    {
                        var adjustMaxEndTime = updateTime;
                        for (int j = 0; j < tempTask.Count(); j++)
                        {
                            var tempMinStartTime = tempTask[i].StartTime.Value;
                            if (adjustTaskInfo.Count() > 1)
                            {
                                adjustMaxEndTime = adjustTaskInfo[adjustTaskInfo.Count() - 1].EndTime.Value;
                            }

                            if (IdList.Contains(tempTask[j].Id))
                            {
                                continue;
                            }

                            if (adjustMaxEndTime > tempMinStartTime)
                            {
                                IdList.Add(tempTask[j].Id);
                                tempTask[j].WorkStationId = drill[0].ToInt();
                                tempTask[j].WorkStationName = drill[1];
                                tempTask[j].WorkStationCode = drill[2];

                                tempTask[j] = await SetTimeForTask(tempTask[j], j, 0, updateTime, adjustMaxEndTime);
                                adjustTaskInfo.Add(tempTask[j]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                startTime = startTime > req.TargetDate.Value.Date ? startTime : req.TargetDate.Value.Date;
                adjustTaskInfo = await SetAdjustTaskTime(adjustTaskInfo, startTime, drill, updateTime);
            }
            _unitOfWork.BeginTran();
            await _domainService.BulkUpdate(adjustTaskInfo);
            _unitOfWork.CommitTran();
            return Success();
        }
    }
}
