using AutoMapper;
using Mapster;
using SqlSugar;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.User;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Res;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.res;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 手动呼叫AGV上下料
    /// </summary>
    public class ManualCallAgvLogService : BaseServiceWithoutTree<ManualCallAgvLog, ManualCallAgvLogDto, AddOrUpdateManualCallAgvLogReq>, IManualCallAgvLogService
    {
        private IAPIHelper _apiHelper;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IUserDomainService _userDomainService;
        private readonly ICutterGroupService _cutterGroupService;
        private readonly IManualCallAgvTaskService _manualCallAgvTaskService;
        private readonly ITaskService _taskService;
        private readonly ICentralOnlineDevice _centralOnlineDevice;
        private readonly IManualCallAgvLogDomainService _manualCallAgvLogDomainService;

        public ManualCallAgvLogService(IManualCallAgvLogDomainService domainService,
            IAPIHelper apiHelper,
            ISysConfigManager sysConfigManager,
            ITaskService taskService,
            IManualCallAgvTaskService manualCallAgvTaskService,
            ICutterGroupService cutterGroupService,
            IUserDomainService userDomainService,
            ICentralOnlineDevice centralOnlineDevice,
            IManualCallAgvLogDomainService manualCallAgvLogDomainService,
            IMapper mapper) : base(domainService, mapper)
        {
            _sysConfigManager = sysConfigManager;
            _taskService = taskService;
            _manualCallAgvTaskService = manualCallAgvTaskService;
            _cutterGroupService = cutterGroupService;
            _userDomainService = userDomainService;
            _centralOnlineDevice = centralOnlineDevice;
            _manualCallAgvLogDomainService = manualCallAgvLogDomainService;
            _apiHelper = apiHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ManualCallAgvLogDto>>> List(ListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ManualCallAgvLogDto>(req.PageNum, req.PageSize);
            var where = PredicateBuilder.True<ManualCallAgvLog>();
            where = where.And(p => p.IsDeleted == 0);
            if (!string.IsNullOrWhiteSpace(req.ItemCode))
            {
                where = where.And(p => p.ItemCode.ToLower().Contains(req.ItemCode.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(req.LocationCode))
            {
                where = where.And(p => p.LocationCode.ToLower().Contains(req.LocationCode.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(req.PodCode))
            {
                where = where.And(p => p.PodCode.ToLower().Contains(req.PodCode.ToLower()));
            }
            if (req.AgvOperateType > 0)
            {
                where = where.And(p => p.AgvOperateType == req.AgvOperateType);
            }
            var data = await _manualCallAgvLogDomainService.QueryPageAsync(where, s => s.CreateTime, OrderByType.Desc);
            pageDto.Total = data.TotalCount;
            pageDto.List = data.ToList().Adapt<List<ManualCallAgvLogDto>>();
            if (pageDto.List?.Count > 0)
            {
                var userIds = pageDto.List.Select(s => (int)s.CreatorId).Distinct().ToList();
                if (userIds?.Count > 0)
                {
                    var users = await _userDomainService.GetListByIds(userIds);
                    foreach (var item in pageDto.List)
                    {
                        item.Creator = users.FirstOrDefault(s => s.Id == item.CreatorId)?.RealName;
                    }
                }
            }
            return Success(pageDto);
        }


        /// <summary>
        /// 获取agv运行状态
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        public async Task<ResponseDto<GetAgvRunStatusResponse>> GetAgvRunStatus(string locationCode)
        {
            var response = new GetAgvRunStatusResponse() { LocationCode = locationCode };
            if (string.IsNullOrWhiteSpace(locationCode))
            {
                return Fail("库位编码不能为空", response);
            }
            var url = await _sysConfigManager.GetStringValue(MESConfigConstants.AGV_RUN_STATUS);
            if (string.IsNullOrWhiteSpace(url))
            {
                return Fail("获取agv运行状态的url未配置", response);
            }
            url = string.Format(url, locationCode);
            var data = _apiHelper.RequestData<STDResponse>(url);
            if (data == null)
            {
                return Fail("agv运行状态获取失败，请检查中控", response);
            }
            response.AgvRunStatus = data?.message;
            return Success(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> AgvOperate(AgvOperateReq req)
        {
            var result = new ResponseDto<bool>() { Data = false, Code = ResponseCode.Fail };
            var paramCheck = await ParamCheck(req);
            if (!paramCheck.Success) return Fail(paramCheck.Messgae, false);

            var manualCallAgvTask = await _manualCallAgvTaskService.QueryByLocationCode(req.LocationCode);
            var taskCheck = await ManualCallAgvTaskCheck(req, manualCallAgvTask);
            if (!taskCheck.Success) return Fail(taskCheck.Messgae, false);

            var url = string.Empty;
            var locationType = LocationType.None;
            var postData = new BindSiloBoxReq() { podCode = req.PodCode, podList = [new PodInfoEntity() { lotNo = req.Lot }] };
            var postString = string.Empty;
            switch (req.AgvOperateType)
            {
                case AgvOperateType.UploadRawMaterial:
                    url = await _sysConfigManager.GetStringValue(MESConfigConstants.UP_RAW_MATERIAL);
                    url = !string.IsNullOrWhiteSpace(url) ? string.Format(url, req.Lot, req.LocationCode) : "";
                    locationType = LocationType.RawMaterialLocation;
                    break;
                case AgvOperateType.UnloadEmptyFork:
                    url = await _sysConfigManager.GetStringValue(MESConfigConstants.UNLOAD_EMPTY_FORK);
                    url = !string.IsNullOrWhiteSpace(url) ? string.Format(url, req.PodCode, req.LocationCode) : "";
                    locationType = LocationType.RawMaterialLocation;
                    break;
                case AgvOperateType.Unbind:
                    url = await _sysConfigManager.GetStringValue(MESConfigConstants.BIND_AND_UN_BIND);
                    postData.indBind = "0";
                    postString = JsonSerializer.Serialize(postData);
                    locationType = LocationType.RawMaterialLocation;
                    break;
                case AgvOperateType.UploadEmptyFork:
                    url = await _sysConfigManager.GetStringValue(MESConfigConstants.UPLOAD_EMPTY_FORK);
                    url = !string.IsNullOrWhiteSpace(url) ? string.Format(url, req.LocationCode) : "";
                    locationType = LocationType.ClinkerMaterialLocation;
                    break;
                case AgvOperateType.UnloadClinker:
                    url = await _sysConfigManager.GetStringValue(MESConfigConstants.UNLOAD_CLINKER);
                    url = !string.IsNullOrWhiteSpace(url) ? string.Format(url, req.PodCode, req.Lot, req.ClinkerMaterialNum, req.LocationCode) : "";
                    locationType = LocationType.ClinkerMaterialLocation;
                    //退料之前要先绑定                    
                    postData.indBind = "1";
                    postString = JsonSerializer.Serialize(postData);
                    break;
                case AgvOperateType.Bind:
                    url = await _sysConfigManager.GetStringValue(MESConfigConstants.BIND_AND_UN_BIND);
                    postData.indBind = "1";
                    postString = JsonSerializer.Serialize(postData);
                    locationType = LocationType.ClinkerMaterialLocation;
                    break;
            }
            if (string.IsNullOrWhiteSpace(url))
            {
                return Fail("Mes系统配置项查不到相关agv呼叫的url配置", false);
            }
            //发送agv命令 
            //下料包含绑定和下料请求
            if (req.AgvOperateType == AgvOperateType.UnloadClinker || req.AgvOperateType == AgvOperateType.Unbind || req.AgvOperateType == AgvOperateType.Bind)
            {
                var bindOrUnbindUrl = await _sysConfigManager.GetStringValue(MESConfigConstants.BIND_AND_UN_BIND);
                if (string.IsNullOrWhiteSpace(bindOrUnbindUrl))
                {
                    return Fail("Mes系统配置项查不到相关agv绑定/解绑的url配置", false);
                }
                var operateType = req.AgvOperateType == AgvOperateType.UnloadClinker ? AgvOperateType.Bind : req.AgvOperateType;
                var operateResult = await SendBindOrUnBindOperateCommand(operateType, bindOrUnbindUrl, postString);
                await InsertManualCallAgvLog(req, bindOrUnbindUrl, operateResult.Message, operateType);//插入操作日志
                if (!operateResult.Success)
                {
                    return Fail(operateResult.Message, false);
                }
            }
            if (req.AgvOperateType != AgvOperateType.Unbind && req.AgvOperateType != AgvOperateType.Bind)
            {
                var callBackMessage = await SendAgvOperateCommand(url);
                await InsertManualCallAgvLog(req, url, callBackMessage);//插入操作日志
            }
            if (manualCallAgvTask != null)
            {
                await UpdateManualCallAgvTask(manualCallAgvTask, req);
            }
            else
            {
                var addTaskReq = new AddOrUpdateManualCallAgvTaskReq()
                {
                    DeviceCode = req.DeviceCode,
                    AgvOperateType = req.AgvOperateType,
                    AgvOperateName = req.AgvOperateType.GetDescription(),
                    TaskStatus = ManualCallAgvTaskStatus.Created,
                    TaskStatusDescription = ManualCallAgvTaskStatus.Created.GetDescription(),
                    IsBind = false,
                    LocationCode = req.LocationCode,
                    LocationType = locationType,
                    LocationTypeName = locationType.GetDescription(),
                };
                await _manualCallAgvTaskService.Add(addTaskReq);
            }
            return Success(true, "操作成功");
        }


        /// <summary>
        /// 更新手动呼叫agv任务表
        /// </summary>
        /// <param name="manualCallAgvTask"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private async Task<bool> UpdateManualCallAgvTask(ManualCallAgvTask manualCallAgvTask, AgvOperateReq req)
        {
            if (req.AgvOperateType == AgvOperateType.Unbind || req.AgvOperateType == AgvOperateType.Bind)
            {
                switch (req.AgvOperateType)
                {
                    case AgvOperateType.Unbind:
                        manualCallAgvTask.IsBind = false;
                        break;
                    case AgvOperateType.Bind:
                        manualCallAgvTask.IsBind = true;
                        break;
                }
            }
            else
            {   //绑定解绑是同步操作，不需要更新agv任务状态，其他操作是异步操作，需要更新任务状态
                if (req.AgvOperateType == AgvOperateType.UnloadClinker)//下料包含绑定和下料请求
                {
                    manualCallAgvTask.IsBind = true;
                }
                manualCallAgvTask.TaskStatus = ManualCallAgvTaskStatus.Created;
                manualCallAgvTask.TaskStatusDescription = ManualCallAgvTaskStatus.Created.GetDescription();
            }
            manualCallAgvTask.DeviceCode = req.DeviceCode;
            manualCallAgvTask.AgvOperateType = req.AgvOperateType;
            manualCallAgvTask.AgvOperateName = req.AgvOperateType.GetDescription();
            manualCallAgvTask.ModifyTime = DateTime.Now;
            manualCallAgvTask.ModifierId = UserId;
            await _manualCallAgvTaskService.Update(manualCallAgvTask);
            return true;
        }
        /// <summary>
        /// 插入手动呼叫agv日志信息
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <param name="req"></param>
        /// <param name="url"></param>
        /// <param name="callBackMessage"></param>
        /// <param name="agvOperateType"></param>
        /// <returns></returns>
        private async Task<bool> InsertManualCallAgvLog(AgvOperateReq req, string url, string callBackMessage, AgvOperateType agvOperateType = 0)
        {
            var operateType = agvOperateType > 0 ? agvOperateType : req.AgvOperateType;
            await _manualCallAgvLogDomainService.Add(new ManualCallAgvLog()
            {
                LocationCode = req.LocationCode,
                DeviceCode = req.DeviceCode,
                AgvOperateName = operateType.GetDescription(),
                AgvOperateType = operateType,
                ItemCode = req.Lot,
                PodCode = req.PodCode,
                ClinkerMaterialNum = req.ClinkerMaterialNum,
                CallBackMessage = callBackMessage != null ? JsonSerializer.Serialize(callBackMessage) : string.Empty,
                RequestUrl = url,
                CreateTime = DateTime.Now,
                CreatorId = UserId,
                ModifierId = UserId,
                ModifyTime = DateTime.Now,
            });
            return true;
        }
        /// <summary>
        /// 绑定/解绑命令请求
        /// </summary>
        /// <param name="agvOperateType"></param>
        /// <param name="postString"></param>
        /// <returns></returns>
        private async Task<(bool Success, string Message)> SendBindOrUnBindOperateCommand(AgvOperateType agvOperateType, string url, string postString)
        {
            var calllBackMessage = await SendBindOrUnBindCommand(url, postString);
            if (calllBackMessage == null)
            {
                return (false, $"请求{agvOperateType.GetDescription()}失败");
            }
            if (calllBackMessage?.code != 0)
            {
                return (false, $"请求{agvOperateType.GetDescription()}失败,{JsonSerializer.Serialize(calllBackMessage)}");
            }
            return (true, $"请求{agvOperateType.GetDescription()}成功,{JsonSerializer.Serialize(calllBackMessage)}");

        }
        /// <summary>
        /// 手动呼叫agv任务信息状态检查
        /// </summary>
        /// <param name="req"></param>
        /// <param name="manualCallAgvTask"></param>
        /// <returns></returns>
        private async Task<(bool Success, string Messgae)> ManualCallAgvTaskCheck(AgvOperateReq req, ManualCallAgvTask manualCallAgvTask)
        {
            if (manualCallAgvTask != null)
            {
                //根据原任务数据的信息，做防呆
                switch (req.AgvOperateType)
                {
                    case AgvOperateType.UploadRawMaterial:
                        if (manualCallAgvTask.TaskStatus != ManualCallAgvTaskStatus.Exception && manualCallAgvTask.TaskStatus != ManualCallAgvTaskStatus.Completed)
                        {
                            return (false, "上个呼叫agv任务未完成，暂时不能上料");
                        }
                        if (manualCallAgvTask.IsBind ?? false)
                        {
                            return (false, "该库位lot已绑定，暂时不能上料");
                        }
                        if (!string.IsNullOrWhiteSpace(manualCallAgvTask.PodCode))
                        {
                            return (false, "该库位还存在托盘，暂时不能上料");
                        }
                        break;
                    case AgvOperateType.UnloadEmptyFork:
                        if (manualCallAgvTask.TaskStatus != ManualCallAgvTaskStatus.Exception && manualCallAgvTask.TaskStatus != ManualCallAgvTaskStatus.Completed)
                        {
                            return (false, "上个呼叫agv任务未完成，暂时不能退空盘");
                        }
                        if (manualCallAgvTask.PodCode.ToLower() != req.PodCode.ToLower())
                        {
                            return (false, $"请求的托盘号{req.PodCode}和库位记录的托盘号{manualCallAgvTask.PodCode}不一致，暂时不能退空盘");
                        }
                        break;
                    case AgvOperateType.UploadEmptyFork:
                        if (manualCallAgvTask.TaskStatus != ManualCallAgvTaskStatus.Exception && manualCallAgvTask.TaskStatus != ManualCallAgvTaskStatus.Completed)
                        {
                            return (false, "上个呼叫agv任务未完成，暂时不能叫空盘");
                        }
                        if (!string.IsNullOrWhiteSpace(manualCallAgvTask.PodCode))
                        {
                            return (false, $"当前库位还存在空盘{manualCallAgvTask.PodCode}，暂时不能叫空盘");
                        }
                        break;
                    case AgvOperateType.UnloadClinker:
                        if (manualCallAgvTask.TaskStatus != ManualCallAgvTaskStatus.Exception && manualCallAgvTask.TaskStatus != ManualCallAgvTaskStatus.Completed)
                        {
                            return (false, "上个呼叫agv任务未完成，暂时不能下料");
                        }
                        if (string.IsNullOrWhiteSpace(manualCallAgvTask.PodCode))
                        {
                            return (false, "该库位不存在托盘，暂时不能下料");
                        }
                        if (manualCallAgvTask.PodCode.ToLower() != req.PodCode.ToLower())
                        {
                            return (false, $"请求的托盘号{req.PodCode}和库位记录的托盘号{manualCallAgvTask.PodCode}不一致，暂时不能退空盘");
                        }
                        break;
                }
            }
            else
            {
                if (req.AgvOperateType == AgvOperateType.UnloadEmptyFork || req.AgvOperateType == AgvOperateType.Unbind)
                {
                    return (false, "请先叫料");
                }
                if (req.AgvOperateType == AgvOperateType.UnloadClinker || req.AgvOperateType == AgvOperateType.Bind)
                {
                    return (false, "请先叫空盘");
                }
            }
            return (true, "");
        }
        /// <summary>
        /// 参数检查
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private async Task<(bool Success, string Messgae)> ParamCheck(AgvOperateReq req)
        {
            if (req.AgvOperateType <= 0)
            {
                return (false, "agv操作类型不合规");
            }
            if (string.IsNullOrWhiteSpace(req.DeviceCode))
            {
                return (false, "钻机未选择");
            }
            switch (req.AgvOperateType)
            {
                case AgvOperateType.UploadRawMaterial:
                    if (string.IsNullOrWhiteSpace(req.Lot))
                    {
                        return (false, "料号不能为空");
                    }
                    if (string.IsNullOrWhiteSpace(req.LocationCode))
                    {
                        return (false, "库位号不能为空");
                    }
                    break;
                case AgvOperateType.UnloadEmptyFork:
                    if (string.IsNullOrWhiteSpace(req.LocationCode))
                    {
                        return (false, "库位号不能为空");
                    }
                    if (string.IsNullOrWhiteSpace(req.PodCode))
                    {
                        return (false, "托盘号不能为空");
                    }
                    break;
                case AgvOperateType.UploadEmptyFork:
                    if (string.IsNullOrWhiteSpace(req.LocationCode))
                    {
                        return (false, "库位号不能为空");
                    }
                    break;
                case AgvOperateType.UnloadClinker:
                    if (string.IsNullOrWhiteSpace(req.Lot))
                    {
                        return (false, "料号不能为空");
                    }
                    if (string.IsNullOrWhiteSpace(req.LocationCode))
                    {
                        return (false, "库位号不能为空");
                    }
                    if (string.IsNullOrWhiteSpace(req.PodCode))
                    {
                        return (false, "托盘号不能为空");
                    }
                    break;
            }
            return (true, "");
        }

        /// <summary>
        /// agv操作命令请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private async Task<string> SendAgvOperateCommand(string url, string postData = "")
        {
            try
            {
                return _apiHelper.RequestData(url, string.IsNullOrWhiteSpace(postData) ? "Get" : "Post", postData);
            }
            catch
            {

            }
            return "";
        }
        private async Task<STDResponse> SendBindOrUnBindCommand(string url, string postData)
        {
            try
            {
                return _apiHelper.RequestData<STDResponse>(url, string.IsNullOrWhiteSpace(postData) ? "Get" : "Post", postData);
            }
            catch
            {

            }
            return new STDResponse();
        }
        /// <summary>
        /// 根据钻机code获取任务列表
        /// </summary>
        /// <param name="DeviceCode"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<TaskDto>>> TaskList(GetDrillOrAgvDeviceInfoReq req)
        {
            if (string.IsNullOrWhiteSpace(req.DeviceCode))
            {
                return Fail("钻机code不能为空", new PageDto<TaskDto>(req.PageNum, req.PageSize));
            }
            if (req.PageNum < 1) req.PageNum = 1;
            req.PageSize = int.MaxValue;
            req.IsManualCallAgvRequest = true;
            var data = await _taskService.GetTaskByDevice(req);
            if (data?.Code == ResponseCode.Success && data.Data.List?.Count > 0)
            {
                var cutterGroupNos = data.Data.List.Where(m => !string.IsNullOrWhiteSpace(m.CutterGroupNo)).Select(x => x.CutterGroupNo).ToList();
              //  var cutterGroups = await _cutterGroupService.List(cutterGroupNos!);
                foreach (var item in data.Data.List)
                {
                   
                    item.PNLCount = (int)(item.NowWadCount * item.PanelCount);
                    item.PlannedTime = (int)(item.EndTime - item.StartTime).Value.TotalMinutes;
                    
                }
            }
            return data;
        }

        /// <summary>
        /// 验证钻带文件
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> LoadingDrillFile(LoadingDrillfileReq req)
        {
            if (string.IsNullOrWhiteSpace(req.DeviceCode))
            {
                return Fail("钻机code不能为空", false);
            }
            if (string.IsNullOrWhiteSpace(req.ItemCode))
            {
                return Fail("lot号不能为空", false);
            }
            var url = await _sysConfigManager.GetStringValue(MESConfigConstants.CENTRAL_ALLOTS_DEVICE_COMMAND);
            if (string.IsNullOrWhiteSpace(url))
            {
                return Fail("Mes系统配置项查不到下发设备指令请求路径url配置", false);
            }         
            var result = await _centralOnlineDevice.AllotsDeviceCommand(url, new DeviceCommandCentralRequest()
            {
                Command = "SetScannerLotCommand",
                DeviceId = req.DeviceCode,
                Params = new Dictionary<string, object?>
                    {
                     {"ItemCode", req.ItemCode}
                    }
            });
            if (!string.IsNullOrWhiteSpace(result))
            {
                return Fail(result, false);
            }
            return Success(true);
        }


        /// <summary>
        /// 加载ATP文件
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> LoadingATPFile(LoadingAtpFileReq req)
        {
            if (string.IsNullOrWhiteSpace(req.DeviceCode))
            {
                return Fail("钻机code不能为空", false);
            }
            if (string.IsNullOrWhiteSpace(req.CutterGroupNo))
            {
                return Fail("配刀组计划编码不能为空", false);
            }
            var url = await _sysConfigManager.GetStringValue(MESConfigConstants.CENTRAL_ALLOTS_DEVICE_COMMAND);
            if (string.IsNullOrWhiteSpace(url))
            {
                return Fail("Mes系统配置项查不到下发设备指令请求路径url配置", false);
            }
            var cutterGroup = await _cutterGroupService.QueryByGroupNo(req.CutterGroupNo);
            if (cutterGroup == null)
            {
                return Fail("查不到该配刀组计划", false);
            }
            if(cutterGroup?.CutterGroupStatus< CutterGroupStatusEnum.Locked)
            {
                return Fail("查到该配刀组计划未锁定，不能加载ATP文件", false);
            }

            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.NEED_CHECK_BOXCODES))
            {
                if (cutterGroup.CheckBox != CutterBoxStatusEnum.OK)
                {
                    return Fail("请确认刀盒码是否已校验通过!", false);
                }
            }

            var cutterGroupAtpFile = await _cutterGroupService.GetCutterGroupAtpFile(new GetCutterGroupAtpFileReq()
            {
                DeviceCode = req.DeviceCode,
                GroupNo = req.CutterGroupNo
            });
            if (!(cutterGroupAtpFile?.Data?.IsNeedLoad ?? false))
            {
                return Fail("该钻机已加载ATP文件，无需加载", false);
            }
            var result = await _centralOnlineDevice.AllotsDeviceCommand(url, new DeviceCommandCentralRequest()
            {
                Command = "SetLoadAptFileCommand",
                DeviceId = req.DeviceCode,
                Params = new Dictionary<string, object?>
                    {
                     {"AtpFile", cutterGroupAtpFile.Data.AtpFile},
                     {"Boxs", cutterGroupAtpFile.Data.Boxs},
                     {"GroupNo", cutterGroupAtpFile.Data.GroupNo},
                     {"IsNeedLoad", cutterGroupAtpFile.Data.IsNeedLoad}
                    }
            });
            if (!string.IsNullOrWhiteSpace(result))//加载失败回退配刀组计划状态
            {
                await _cutterGroupService.UpdateCutterGroupStatus(new UpdateCutterGroupStatusReq()
                {
                    GroupStatus = cutterGroup.CutterGroupStatus,
                    GroupNo = req.CutterGroupNo
                });
                return Fail(result, false);
            }
            return Success(true);
        }

        /// <summary>
        /// 更新任务状态(开始和结束)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> UpdateTaskStatus(UpdateTaskByOutSideReq req)
        {
            if (string.IsNullOrWhiteSpace(req.Code))
            {
                return Fail("任务code不能为空", false);
            }
            var task = await _taskService.GetTaskByCode(req.Code);
            if (task == null)
            {
                return Fail("查不到该任务信息", false);
            }
            if (req.TaskStatus == TaskStatusEnum.BEGIN)
            {
                var deviceCode = task?.WorkStationCode;
                var deviceCodeTasks = await _taskService.GetTaskByDevice(new GetDrillOrAgvDeviceInfoReq()
                {
                    DeviceCode = deviceCode,
                    TaskStatusList = new List<TaskStatusEnum>() { TaskStatusEnum.BEGIN, TaskStatusEnum.BUFFERED, TaskStatusEnum.SENDING }
                });
                if (deviceCodeTasks?.Code == ResponseCode.Success && deviceCodeTasks.Data.List?.Count > 0)
                {
                    return Fail("该钻机下有正在进行的任务，请等待任务完成再操作", false);
                }
            }
            if (req.TaskStatus == TaskStatusEnum.FINISH)
            {
                if (task?.TaskStatus == TaskStatusEnum.FINISH)
                {
                    return Fail("该任务已经完成，无需完成操作", false);
                }
            }
            var data = await _taskService.UpdateByOutSide(req);
            if (data?.Code != ResponseCode.Success)
            {
                return Fail(data?.Message, false);
            }
            return Success(true, "操作成功");
        }
    }
}
