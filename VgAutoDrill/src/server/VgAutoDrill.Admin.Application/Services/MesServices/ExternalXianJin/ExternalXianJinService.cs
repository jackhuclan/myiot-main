using AutoMapper;
using Newtonsoft.Json;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Application.Interfaces.MesServices.ExternalXianJin;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalXianJin;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices.ExternalXianJin
{
    public class ExternalXianJinService : BaseService, IExternalXianJinService
    {
        private readonly ITaskDomainService _taskDomainService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAPIHelper _apiHelper;
        private readonly ICentralOnlineDevice _centralOnlineDevice;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly SqlSugarScope _sqlSugarScope;
        private readonly ITaskService _taskService;

        public ExternalXianJinService(
            ITaskDomainService taskDomainService,
            IUnitOfWork unitOfWork,
            IAPIHelper apiHelper,
            ICentralOnlineDevice centralOnlineDevice,
            ISysConfigManager sysConfigManager,
            ITaskService taskService,
            IMapper mapper)
        {
            _taskDomainService = taskDomainService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _apiHelper = apiHelper;
            _centralOnlineDevice = centralOnlineDevice;
            _sysConfigManager = sysConfigManager;
            _sqlSugarScope = unitOfWork.GetDbClient();
            _taskService = taskService;
        }

        public async Task<ResponseDto<DrillTaskDto>> QueryTaskByDrill(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return Fail<DrillTaskDto>("未识别有效的DeviceID！");
            }

            DrillTaskDto resultDto = new DrillTaskDto();

            var taskDatas = await _taskDomainService.QueryAsync(p => p.TaskStatus == TaskStatusEnum.COMMITED
            && !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.ToLower() == deviceId.ToLower()
            , p => p.Code, OrderByType.Asc);

            if (taskDatas == null || taskDatas.Count == 0)
            {
                return Fail<DrillTaskDto>("未查询到相关任务！");
            }

            resultDto.DeviceId = deviceId;
            resultDto.DrillTaskInfos = new List<DrillTaskInfo>();
            foreach (var taskInfo in taskDatas)
            {
                if (string.IsNullOrEmpty(taskInfo.Code) || string.IsNullOrEmpty(taskInfo.ItemCode))
                {
                    continue;
                }
                resultDto.DrillTaskInfos.Add(new DrillTaskInfo()
                {
                    TaskCode = taskInfo.Code,
                    ItemCode = taskInfo.ItemCode,
                    BarCode = taskInfo.BarCode,
                    WorkOrderCode = taskInfo.WorkOrderCode,
                });
            }

            return Success(resultDto);
        }

        public async Task<ResponseDto<string>> SetBegin(SetDrillCommandReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的入参！");
            }
            if (string.IsNullOrEmpty(req.TaskCode) || string.IsNullOrEmpty(req.DeviceId) || req.BarCodeList == null || req.BarCodeList.Count == 0)
            {
                return Fail("未识别有效的DeviceId、TackCode、BarCodeList！");
            }

            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.XIANJIN_SET_BEGIN_LOAD_PANEL);
            if (string.IsNullOrEmpty(urlAdress))
            {
                return Fail("无法识别开始上料请求接口地址！");
            }

            try
            {
                var taskData = await _taskDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == req.TaskCode.ToLower()
                && !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.ToLower() == req.DeviceId.ToLower());
                if (taskData == null)
                {
                    return Fail($"未找到设备{req.DeviceId} 相关任务{req.TaskCode}");
                }

                if (taskData.TaskStatus == TaskStatusEnum.DRAFT || taskData.TaskStatus == TaskStatusEnum.FINISH)
                {
                    return Fail($"当前任务状态:{taskData.TaskStatus} 不支持设置开始! ");
                }

                string jsonStr = JsonConvert.SerializeObject(req);
                var result = _apiHelper.RequestData(urlAdress, "post", jsonStr);
                if (!string.IsNullOrEmpty(result))
                {
                    return Fail(result);
                }

                await _taskService.UpdateByOutSide(new UpdateTaskByOutSideReq
                {
                    Code = req.TaskCode,
                    TaskStatus = TaskStatusEnum.SENDING
                });
            }
            catch (Exception ex)
            {
                return Fail($"开始上料请求异常！{ex}");
            }

            return Success();
        }

        public async Task<ResponseDto<List<ExternalTaskDto>>> QueryDrillInfo(List<string> deviceIds)
        {
            if (deviceIds == null || deviceIds.Count == 0)
            {
                return Fail<List<ExternalTaskDto>>("未识别有效的DeviceID！");
            }

            deviceIds = deviceIds.Select(p => p.ToLower()).ToList();

            var db = _unitOfWork.GetDbClient();
            var query = db.Queryable<Device, WorkTask>
            ((d, t) => new object[]
              {
                      JoinType.Left,  d.Code == t.WorkStationCode
              });

            query = query.Where((d, t) => d.IsDeleted == 0 && d.DeviceTypeCode == "drill");

            query = query.Where((d, t) => deviceIds.Contains(d.Code.ToLower()));

            var result = query.GroupBy((d, t) => d.Code).ToList();

            var drillList = result.Select(t => t.Code).Distinct().ToList();

            List<ExternalTaskDto> returnDatas = new List<ExternalTaskDto>();

            if (drillList != null && drillList.Count > 0)
            {
                var allTaskList = db.Queryable<Model.Entites.Mes.WorkTask>().Where(t => t.IsDeleted == 0 && drillList.Contains(t.WorkStationCode));

                var allTask = allTaskList.ToList();
                var onlineDevices = await _centralOnlineDevice.GetOnlineDevices();
                List<TaskStatusEnum> lst = new List<TaskStatusEnum> { TaskStatusEnum.BUFFERED, TaskStatusEnum.SENDING };
                for (int i = 0; i < drillList.Count; i++)
                {
                    var device = result.Where(t => t.Code == drillList[i]).First();
                    var model = new ExternalTaskDto
                    {
                        Code = drillList[i],
                        Name = device.Name,
                        DeviceStatus = device.DeviceStatus,
                        DeviceKind = device.DeviceKind,
                        CommitTask = allTask.Where(t => t.WorkStationCode == drillList[i] && t.TaskStatus == TaskStatusEnum.COMMITED).Count(),
                        DraftTask = allTask.Where(t => t.WorkStationCode == drillList[i] && t.TaskStatus == TaskStatusEnum.DRAFT).Count()
                    };

                    var DrillingTaskCode = allTask.Where(t => t.WorkStationCode == drillList[i] && t.TaskStatus == TaskStatusEnum.BEGIN).OrderBy(t => t.RealStartTime);
                    model.DrillingTask = DrillingTaskCode.Count() == 0 ? "" : DrillingTaskCode.FirstOrDefault().Code;
                    var WaitWorkTaskCode = allTask.Where(t => t.WorkStationCode == drillList[i] && lst.Contains((TaskStatusEnum)t.TaskStatus));
                    model.WaitWorkTask = WaitWorkTaskCode.Count() == 0 ? "" : WaitWorkTaskCode.FirstOrDefault().Code;
                    var Route = allTask.Where(t => t.Code == drillList[i]).Select(t => t.RouteCode).ToList();
                    model.Route = Route.Count > 0 ? Route.FirstOrDefault() : "";

                    var onlineInfo = onlineDevices.FirstOrDefault(t => t.DeviceId.ToLower() == device.Code.ToLower());
                    model.SpindleNum = onlineInfo == null || onlineInfo.Descriptor == null ? device.SpindleNum : onlineInfo.Descriptor.SpindleNum;
                    model.Percentage = onlineInfo == null ? 0 : onlineInfo.Percentage;
                    model.ExistRawPanel = onlineInfo == null ? false : onlineInfo.ExistRawPanel;
                    model.ExistClinkerPanel = onlineInfo == null ? false : onlineInfo.ExistClinkerPanel;
                    model.IsAllVerifyOK = onlineInfo == null ? false : onlineInfo.IsAllVerifyOK;
                    model.IsCompleteUpperPanel = onlineInfo == null ? false : onlineInfo.IsCompleteUpperPanel;
                    model.VerifyFailShafts = onlineInfo == null ? new List<string>() : onlineInfo.VerifyFailShafts;
                    model.UpperPanelProgress = onlineInfo == null ? "0" : onlineInfo.UpperPanelProgress;
                    model.LoadingTask = onlineInfo == null ? "" : onlineInfo.LoadingTask;
                    returnDatas.Add(model);
                }
            }

            return Success(returnDatas);
        }

        public async Task<ResponseDto<List<ExternalTaskDto>>> MockQueryDrillInfo(string deviceId, bool isAllVerifyOK, bool existClinkerPanel)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return Fail<List<ExternalTaskDto>>("未识别有效的DeviceID！");
            }

            var db = _unitOfWork.GetDbClient();
            var query = db.Queryable<Device, WorkTask>
            ((d, t) => new object[]
              {
                      JoinType.Left,  d.Code == t.WorkStationCode
              });

            query = query.Where((d, t) => d.IsDeleted == 0 && d.DeviceTypeCode == "drill");

            query = query.Where((d, t) => deviceId.ToLower().Equals(d.Code.ToLower()));

            var result = query.GroupBy((d, t) => d.Code).ToList();

            var drillList = result.Select(t => t.Code).Distinct().ToList();

            List<ExternalTaskDto> returnDatas = new List<ExternalTaskDto>();

            if (drillList != null && drillList.Count > 0)
            {
                var allTaskList = db.Queryable<Model.Entites.Mes.WorkTask>().Where(t => t.IsDeleted == 0 && drillList.Contains(t.WorkStationCode));

                var allTask = allTaskList.ToList();
                List<TaskStatusEnum> lst = new List<TaskStatusEnum> { TaskStatusEnum.BUFFERED, TaskStatusEnum.SENDING };
                for (int i = 0; i < drillList.Count; i++)
                {
                    var device = result.Where(t => t.Code == drillList[i]).First();
                    var model = new ExternalTaskDto
                    {
                        Code = drillList[i],
                        Name = device.Name,
                        DeviceStatus = device.DeviceStatus,
                        DeviceKind = device.DeviceKind,
                        CommitTask = allTask.Where(t => t.WorkStationCode == drillList[i] && t.TaskStatus == TaskStatusEnum.COMMITED).Count(),
                        DraftTask = allTask.Where(t => t.WorkStationCode == drillList[i] && t.TaskStatus == TaskStatusEnum.DRAFT).Count()
                    };

                    var DrillingTaskCode = allTask.Where(t => t.WorkStationCode == drillList[i] && t.TaskStatus == TaskStatusEnum.BEGIN).OrderBy(t => t.RealStartTime);
                    model.DrillingTask = DrillingTaskCode.Count() == 0 ? "" : DrillingTaskCode.FirstOrDefault().Code;
                    var WaitWorkTaskCode = allTask.Where(t => t.WorkStationCode == drillList[i] && lst.Contains((TaskStatusEnum)t.TaskStatus));
                    model.WaitWorkTask = WaitWorkTaskCode.Count() == 0 ? "" : WaitWorkTaskCode.FirstOrDefault().Code;
                    var Route = allTask.Where(t => t.Code == drillList[i]).Select(t => t.RouteCode).ToList();
                    model.Route = Route.Count > 0 ? Route.FirstOrDefault() : "";

                    model.SpindleNum = device.SpindleNum;
                    if (isAllVerifyOK)
                    {
                        model.Percentage = 100;
                        model.ExistRawPanel = true;
                        model.ExistClinkerPanel = existClinkerPanel;
                        model.IsAllVerifyOK = isAllVerifyOK;
                        model.IsCompleteUpperPanel = isAllVerifyOK;
                        model.VerifyFailShafts = new List<string>();
                    }
                    else
                    {
                        model.Percentage = 30;
                        model.ExistRawPanel = true;
                        model.ExistClinkerPanel = existClinkerPanel;
                        model.IsAllVerifyOK = isAllVerifyOK;
                        model.IsCompleteUpperPanel = isAllVerifyOK;
                        model.VerifyFailShafts = new List<string>() { "1 当前板料2493041J2H，任务需要板料1040_620_1.5_1_3" };
                    }
                    returnDatas.Add(model);
                }
            }

            return Success(returnDatas);
        }

        public async Task<ResponseDto<string>> UnderClinkerPanel(SetDrillCommandReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的入参！");
            }
            if (string.IsNullOrEmpty(req.TaskCode) || string.IsNullOrEmpty(req.DeviceId))
            {
                return Fail("未识别有效的DeviceId、TackCode！");
            }

            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.XIANJIN_UNDER_CLINKER_PANEL);
            if (string.IsNullOrEmpty(urlAdress))
            {
                return Fail("无法识别下料请求接口地址！");
            }

            if (!await _taskDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == req.TaskCode.ToLower()
             && !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.ToLower() == req.DeviceId.ToLower()))
            {
                return Fail($"未找到设备{req.DeviceId} 相关任务{req.TaskCode}");
            }

            try
            {
                urlAdress = urlAdress + req.DeviceId;
                var result = _apiHelper.RequestData(urlAdress, "Get");
                if (!string.IsNullOrEmpty(result))
                {
                    return Fail(result);
                }
            }
            catch (Exception ex)
            {
                return Fail($"下料请求异常！{ex}");
            }

            return Success();
        }

        public async Task<ResponseDto<string>> SetComplete(SetDrillCommandReq req)
        {
            if (req == null)
            {
                return Fail("未识别有效的入参！");
            }
            if (string.IsNullOrEmpty(req.TaskCode) || string.IsNullOrEmpty(req.DeviceId))
            {
                return Fail("未识别有效的DeviceId、TackCode！");
            }

            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.XIANJIN_SET_COMPLETE_UNDER_CLINKER_PANEL);
            if (string.IsNullOrEmpty(urlAdress))
            {
                return Fail("无法识别结束下料请求接口地址！");
            }

            if (!await _taskDomainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower() == req.TaskCode.ToLower()
            && !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.ToLower() == req.DeviceId.ToLower()))
            {
                return Fail($"未找到设备{req.DeviceId} 相关任务{req.TaskCode}");
            }

            try
            {
                urlAdress = urlAdress + req.DeviceId;
                var result = _apiHelper.RequestData(urlAdress, "Get");
                if (!string.IsNullOrEmpty(result))
                {
                    return Fail(result);
                }

                await _taskService.UpdateByOutSide(new UpdateTaskByOutSideReq
                {
                    Code = req.TaskCode,
                    TaskStatus = TaskStatusEnum.FINISH
                });
            }
            catch (Exception ex)
            {
                return Fail($"结束下料请求异常！{ex}");
            }

            return Success();
        }

        public async Task<ResponseDto<List<ExternalWorkTaskDto>>> GetWorkTaskBatch(List<ExternalWorkOrderQueryReq> reqs)
        {
            if (reqs == null || reqs.Count == 0)
            {
                return Fail<List<ExternalWorkTaskDto>>("未识别有效的入参！");
            }

            var souceCodes = reqs.Where(p => !string.IsNullOrEmpty(p.SourceCode)).Select(p => p.SourceCode.ToLower()).ToList();
            var innerCodes = reqs.Where(p => !string.IsNullOrEmpty(p.InnerCode)).Select(p => p.InnerCode.ToLower()).ToList();
            var incodeNumbers = reqs.Where(p => !string.IsNullOrEmpty(p.IncodeNumber)).Select(p => p.IncodeNumber.ToLower()).ToList();

            var workOrderQuery = _sqlSugarScope.Queryable<ExternalWorkOrder, WorkOrder>
                  ((externalwo, innerwo) => new object[]  {
                      JoinType.Left, externalwo.Code == innerwo.Code,
                  }).Where((externalwo, innerwo) =>
                       externalwo.IsDeleted == 0 &&
                  innerwo.IsDeleted == 0);

            if (souceCodes != null && souceCodes.Count > 0)
            {
                workOrderQuery = workOrderQuery.Where((externalwo, innerwo) => souceCodes.Contains(externalwo.SourceCode.ToLower()));
            }

            if (innerCodes != null && innerCodes.Count > 0)
            {
                workOrderQuery = workOrderQuery.Where((externalwo, innerwo) => innerCodes.Contains(innerwo.Code.ToLower()));
            }

            if (incodeNumbers != null && incodeNumbers.Count > 0)
            {
                workOrderQuery = workOrderQuery.Where((externalwo, innerwo) => incodeNumbers.Contains(innerwo.IncodeNumber.ToLower()));
            }

            var workOrderDto = workOrderQuery.Select((externalwo, innerwo) =>
                    new ExternalAndInnerWorkOrderDto()
                    {
                        Code = externalwo.Code,
                        Name = externalwo.Name,
                        SourceCode = externalwo.SourceCode,
                        OrderSource = string.IsNullOrEmpty(innerwo.OrderSource) ? externalwo.OrderSource : innerwo.OrderSource,
                        ManuOrderStatus = innerwo.ManuOrderStatus,
                        Status = externalwo.Status,
                        Remark = externalwo.Remark,
                        SpecGroup = externalwo.SpecGroup,
                    })
                    .ToList();

            var externalWorkOrderTask = _mapper.Map<List<ExternalWorkTaskDto>>(workOrderDto);
            if (externalWorkOrderTask != null && externalWorkOrderTask.Any())
            {
                foreach (var item in externalWorkOrderTask)
                {
                    item.WorkTasks = await GetTaskByWorkOrderId(item.Code);
                }
            }

            return Success(externalWorkOrderTask!);

        }
        private async Task<List<TaskViewDto>> GetTaskByWorkOrderId(string workOrderCode)
        {
            var tasks = await _taskDomainService.QueryAsync(p => p.WorkOrderCode == workOrderCode && p.IsDeleted == 0, p => p.Id, OrderByType.Asc);
            return _mapper.Map<List<TaskViewDto>>(tasks);
        }

        public async Task<ResponseDto<string>> ClearTaskByDevice(string workStationCode)
        {
            var taskdatas = await _taskDomainService.QueryAsync(p => !string.IsNullOrEmpty(p.WorkStationCode)
            && p.WorkStationCode.ToLower() == workStationCode.ToLower() && p.IsDeleted == 0
            && p.TaskStatus != TaskStatusEnum.DRAFT && p.TaskStatus != TaskStatusEnum.FINISH, p => p.Code, OrderByType.Asc);

            if (taskdatas != null && taskdatas.Count > 0)
            {
                foreach (var item in taskdatas)
                {
                    if (item.TaskStatus == TaskStatusEnum.COMMITED)
                    {
                        item.TaskStatus = TaskStatusEnum.DRAFT;
                    }
                    else
                    {
                        item.TaskStatus = TaskStatusEnum.FINISH;
                    }
                    item.ModifyTime = DateTime.Now;
                }

                var result = await _taskDomainService.BulkUpdate(taskdatas);
                if (!result)
                {
                    return Fail("清理数据失败，请联系相关任务界面操作！");
                }
            }

            return Success();
        }

        public async Task<ResponseDto<string>> DeleteTaskBySourceCode(string sourceCode)
        {
            if (string.IsNullOrEmpty(sourceCode))
            {
                return Fail("未识别有效的sourceCode !");
            }

            var tasks = await _taskDomainService.QueryAsync(p => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.ToLower() == sourceCode.ToLower()
            && (p.TaskStatus == TaskStatusEnum.DRAFT || p.TaskStatus == TaskStatusEnum.COMMITED), p => p.Code, OrderByType.Desc);
            if (tasks == null || tasks.Count == 0)
            {
                return Fail("未找到可删除的任务!");
            }

            if (tasks.Count > 1)
            {
                return Fail("任务数量不止一个，不支持删除!");
            }

            var deleteResult = await _taskDomainService.DeleteAsync(p => !string.IsNullOrEmpty(p.WorkOrderCode)
            && p.WorkOrderCode.ToLower() == sourceCode.ToLower()
            && (p.TaskStatus == TaskStatusEnum.DRAFT || p.TaskStatus == TaskStatusEnum.COMMITED));

            if (!deleteResult)
            {
                return Fail("删除失败，请稍后重试 !");
            }

            return Success();
        }
    }
}
