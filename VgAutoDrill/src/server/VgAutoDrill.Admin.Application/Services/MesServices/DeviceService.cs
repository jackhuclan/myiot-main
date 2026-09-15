using AutoMapper;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Text;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.CentralModels;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using Device = VgAutoDrill.Admin.Model.Entites.Mes.Device;

namespace VgAutoDrill.Admin.Application.Services;

/// <summary>
/// 设备管理
/// </summary>
public class DeviceService : BaseServiceWithTree<Device, DeviceTreeDto, DeviceInfoDto, AddOrUpdateDeviceReq>, IDeviceService
{
    private readonly IDeviceDomainService _deviceDomainService;
    private readonly IDeviceTypeDomainService _deviceTypeDomainService;
    private readonly IWorkstationDomainService _workstationDomainService;
    private readonly IDeviceMaintainService _deviceMaintainService;
    private readonly IRouteDomainService _routeDomainService;
    private readonly IRouteAndProcessDomainService _routeAndProcessDomainService;
    private readonly IRouteProcessAndWorkStationDomainService _routeProcessAndWorkStationDomainService;
    private readonly ITaskDomainService _taskDomainService;
    private readonly IProcessDomainService _processDomainService;
    private readonly IConfiguration _configuration;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ICentralOnlineDevice _centralOnlineDevice;
    private readonly IDeviceAndRouteDomainService _deviceAndRouteDomainService;
    private readonly IDrillPanelDetailService _drillPanelDetailService;
    private readonly IScheduleService _scheduleService;
    private readonly ILogger<DeviceService> logger;
    private readonly IRackDomainService _rackDomainService;
    private readonly IDrillRateFactorService _drillRateFactorService;
    private readonly IDrillRateFactorDomainService _drillRateFactorDomainService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="domainService"></param>
    /// <param name="deviceTypeDomainService"></param>
    /// <param name="workstationDomainService"></param>
    /// <param name="taskDomainService"></param>
    /// <param name="processDomainService"></param>
    /// <param name="mapper"></param>
    /// <param name="deviceMaintainService"></param>
    public DeviceService(IDeviceDomainService domainService,
        IDeviceTypeDomainService deviceTypeDomainService,
        IWorkstationDomainService workstationDomainService,
        ITaskDomainService taskDomainService,
        IProcessDomainService processDomainService,
        IRouteAndProcessDomainService routeAndProcessDomainService,
        IRouteProcessAndWorkStationDomainService routeProcessAndWorkStationDomainService,
        IRouteDomainService routeDomainService,
        IConfiguration configuration,
        ISysConfigManager sysConfigManager,
        IMapper mapper,
        ICentralOnlineDevice centralOnlineDevice,
        IDeviceMaintainService deviceMaintainService,
        IDeviceAndRouteDomainService deviceAndRouteDomainService,
        IDrillPanelDetailService drillPanelDetailService,
        IScheduleService scheduleService,
        IRackDomainService rackDomainService,
        ILoggerFactory loggerFactory,
        IDrillRateFactorService drillRateFactorService,
        IDrillRateFactorDomainService drillRateFactorDomainService)
        : base(domainService, mapper)
    {
        _deviceDomainService = domainService;
        _deviceTypeDomainService = deviceTypeDomainService;
        _workstationDomainService = workstationDomainService;
        _deviceMaintainService = deviceMaintainService;
        _taskDomainService = taskDomainService;
        _processDomainService = processDomainService;
        _configuration = configuration;
        _sysConfigManager = sysConfigManager;
        _centralOnlineDevice = centralOnlineDevice;
        _routeDomainService = routeDomainService;
        _routeAndProcessDomainService = routeAndProcessDomainService;
        _routeProcessAndWorkStationDomainService = routeProcessAndWorkStationDomainService;
        _deviceAndRouteDomainService = deviceAndRouteDomainService;
        _drillPanelDetailService = drillPanelDetailService;
        _scheduleService = scheduleService;
        logger = loggerFactory.CreateLogger<DeviceService>();
        _rackDomainService = rackDomainService;
        _drillRateFactorService = drillRateFactorService;
        _drillRateFactorDomainService = drillRateFactorDomainService;
    }

    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<PageDto<DeviceDto>>> GetEquipmentList(GetDeviceListReq req)
    {
        if (req.PageNum < 1) req.PageNum = 1;
        if (req.PageSize < 1) req.PageSize = 10;

        var result = await _deviceDomainService.PageList(req);
        return Success(result);
    }

    /// <summary>
    /// 获取树形结构数据
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseDto<List<DeviceTreeDto>>> GetEquipmentTreeList()
    {
        var list = await _domainService.QueryAsync(q => q.IsDeleted == 0 && q.Status == 1, q => q.Id, SqlSugar.OrderByType.Asc);
        var result = new ResponseDto<List<DeviceTreeDto>>();
        if (list == null || !list.Any())
        {
            return result;
        }

        var allCodes = AddChildN(list, 0);

        result.Data = allCodes;

        return result;
    }

    /// <summary>
    /// 根据ID获取数据（包括返回设备类型的code和name、工艺路线）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseDto<DeviceFullDataDto>> QueryFullDataByID(long id)
    {
        var entity = await _domainService.QueryByID(id);
        if (entity == null)
        {
            return Fail<DeviceFullDataDto>("信息不存在!");
        }
        if (entity.IsDeleted == 1)
        {
            return Fail<DeviceFullDataDto>("信息错误!");
        }
        var model = entity.Adapt<DeviceFullDataDto>();

        if (entity.DeviceTypeId != null)
        {
            var typeData = await _deviceTypeDomainService.QueryByID(entity.DeviceTypeId);
            if (typeData != null)
            {
                model.DeviceTypeName = typeData.Name;
                model.DeviceTypeCode = typeData.Code;
            }
        }

        if (!string.IsNullOrEmpty(model.DeviceTypeCode))
        {
            if (model.DeviceTypeCode.ToLower().Equals("agv"))
            {
                var agvRoutes = await _deviceAndRouteDomainService.QueryAsync(p => p.DeviceId == model.Id, p => p.RouteCode, OrderByType.Asc);
                if (agvRoutes != null)
                {
                    model.RouteCode = agvRoutes.Select(r => r.RouteCode).ToList();
                }
            }
            else
            {
                var bindRoutes = await _deviceDomainService.GetRPListByDeviceCode(model.Code);
                if (bindRoutes != null)
                {
                    model.RouteCode = bindRoutes.Select(r => r.RouteCode).ToList();
                }
            }
        }

        return Success(model);
    }

    /// <summary>
    /// 根据deviceid获取设备属性
    /// </summary>
    /// <param name="deviceCode"></param>
    /// <returns></returns>
    public async Task<DeviceDto> FindSingle(string deviceCode)
    {
        if (!(await IsExistByDeviceId(deviceCode)))
        {
            return null;
        }

        var entity = await _domainService.FindSingleAsync(device => device.Code.ToLower() == deviceCode.ToLower());

        entity = await _domainService.FindSingleAsync(device => device.Code == deviceCode);

        return _mapper.Map<DeviceDto>(entity);
    }

    public async Task<bool> IsExistByDeviceId(string deviceId)
    {
        return await _domainService.IsExistAsync(d => d.Code.ToLower() == deviceId.ToLower());
    }

    /// <summary>
    /// 新增信息（校验设备状态）
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<string>> AddData(AddOrUpdateDeviceReq req)
    {
        if (req.DeviceStatus != null && (!Enum.IsDefined((DeviceStatus)req.DeviceStatus)) || req.DeviceStatus == DeviceStatus.Unknown)
        {
            return Fail<string>("设备状态填写错误!");
        }

        if (string.IsNullOrEmpty(req.DeviceTypeCode) || req.DeviceTypeId == null || req.DeviceTypeId == 0 || req.DeviceTypeCode == "000")
        {
            return Fail<string>("请选择设备对应的归属产品!");
        }

        if (string.IsNullOrEmpty(req.Code) || string.IsNullOrEmpty(req.Name))
        {
            return Fail<string>("设备编码或设备名称未填写!");
        }

        if (req.MaintainPeriodDays > 0 && req.ProductionTime != null)
        {
            req.LastMaintainTime = req.ProductionTime.Value.AddDays((double)req.MaintainPeriodDays);
        }

        if (!req.InteractionPosition.HasValue)
        {
            req.InteractionPosition = await GetDeviceInteractionPositon();
        }

        req.WorkStationId = await GetWorkStationID(req.Code, req.DeviceTypeCode, req.Name, req.RouteCode);
        var result = await Add(req);

        if (req.DeviceTypeCode.ToLower().Equals("drill") && req.SpindleNum != null && req.SpindleNum != 0 && req.InteractionPosition.HasValue)
        {
            await _drillPanelDetailService.LoadPanelDetailData(new LoadDrillPanelDetailReq
            {
                DeviceCode = req.Code
            });
        }

        return result;
    }

    private async Task<long> GetWorkStationID(string deviceCode, string deviceTypeCode, string deviceName = "", string deviceRouteCode = "")
    {
        long workStationId = 0;
        if (!deviceTypeCode.ToLower().Equals("agv"))
        {
            var workStation = await _workstationDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code)
            && p.Code.ToLower() == deviceCode.ToLower());

            if (workStation != null)
            {
                workStationId = workStation.Id;
            }
            else
            {
                var process = await _processDomainService.FindSingleAsync(p => p.Code.ToLower() == deviceTypeCode.ToLower());
                long processId = 0;
                var processName = "";
                if (process != null)
                {
                    processName = process.Name;
                    processId = process.Id;
                }
                else //add new process
                {
                    processId = await _processDomainService.AddReturnId(new Process
                    {
                        Code = deviceTypeCode.ToLower(),
                        Name = deviceTypeCode.ToLower(),
                        CreateTime = DateTime.Now,
                        CreatorId = UserId,
                        Status = 1
                    });
                }
                //add new work station
                workStationId = await _workstationDomainService.AddReturnId(new WorkStation
                {
                    Code = deviceCode,
                    Name = string.IsNullOrEmpty(deviceName) ? deviceCode : deviceName,
                    ProcessCode = deviceTypeCode.ToLower(),
                    ProcessId = processId,
                    ProcessName = processName,
                    CreateTime = DateTime.Now,
                    CreatorId = UserId,
                    Status = 1
                });
                //set route for work station
                if (!string.IsNullOrEmpty(deviceRouteCode) && processId > 0)
                {
                    var routeCodes = deviceRouteCode.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var routes = await _routeDomainService.QueryAsync(r => r.IsDeleted == 0
                    && routeCodes.Contains(r.Code.ToLower()), r => r.Code, OrderByType.Asc);
                    if (routes != null && routes.Count > 0)
                    {
                        foreach (var route in routes)
                        {
                            if (await _routeAndProcessDomainService.IsExistAsync(rp => rp.RouteId == route.Id && rp.ProcessId == processId))
                            {
                                var routeAndProcess = await _routeAndProcessDomainService.FindSingleAsync(rp =>
                                rp.RouteId == route.Id && rp.ProcessId == processId);
                                if (routeAndProcess == null)
                                {
                                    continue;
                                }
                                //尝试将工作站，绑定到工艺路线的子工序的关联设定中
                                if (!await _routeProcessAndWorkStationDomainService.IsExistAsync(rps =>
                                rps.RouteAndProcessId == routeAndProcess.Id && rps.WorkStationId == workStationId))
                                {
                                    await _routeProcessAndWorkStationDomainService.Add(new RouteProcessAndWorkStation
                                    {
                                        WorkStationId = workStationId,
                                        RouteAndProcessId = routeAndProcess.Id,
                                    });
                                }
                            }
                        }
                    }

                }
            }
        }

        return workStationId;
    }

    public async Task<InteractionPosition> GetDeviceInteractionPositon()
    {
        InteractionPosition interactionPosition = InteractionPosition.Rear;
        var defaultInteractionPosition = await _sysConfigManager.GetIntValue(MESConfigConstants.DEFAULT_INTERACTION_POSITION);
        if (defaultInteractionPosition == 1)
        {
            interactionPosition = InteractionPosition.Front;
        }

        return interactionPosition;
    }

    /// <summary>
    /// 修改信息
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseDto<DeviceInfoDto>> UpdateData(AddOrUpdateDeviceReq req)
    {
        if (req.ParentId > 0)
        {
            var isExist = await _domainService.IsExistAsync(q => q.Id == req.ParentId);
            if (!isExist)
            {
                return Fail<DeviceInfoDto>(" 父对象不存在!");
            }
        }
        var entity = await _domainService.QueryByID(req.Id);
        if (entity == null)
        {
            return Fail<DeviceInfoDto>("信息不存在!");
        }

        if (req.DeviceStatus != null && (!Enum.IsDefined((DeviceStatus)req.DeviceStatus)) || req.DeviceStatus == DeviceStatus.Unknown)
        {
            return Fail<DeviceInfoDto>("设备状态填写错误!");
        }

        if (string.IsNullOrEmpty(req.DeviceTypeCode) || req.DeviceTypeId == null || req.DeviceTypeId == 0 || req.DeviceTypeCode == "000")
        {
            return Fail<DeviceInfoDto>("请选择设备对应的归属产品!");
        }

        if (req.MaintainPeriodDays != null && req.ProductionTime != null)
        {
            DateTime date = DateTime.Now.Date;
            if (entity.ProductionTime == req.ProductionTime)//投产时间不变
            {
                DateTime lastMaintainTime = DateTime.Now.Date;
                if (entity.MaintainPeriodDays == null || entity.LastMaintainTime == null)//如果数据库中没有维护维护周期 或者 投产时间
                {//取此次投产时间 + 维护周期
                    lastMaintainTime = req.ProductionTime.Value.Date.AddDays(req.MaintainPeriodDays.Value);
                }
                else
                {//此次投产时间 + 维护时间差
                    lastMaintainTime = entity.LastMaintainTime.Value.Date.AddDays((double)(req.MaintainPeriodDays - entity.MaintainPeriodDays));
                }
                if (req.MaintainPeriodDays > entity.MaintainPeriodDays)//维护周期变长
                {
                    req.LastMaintainTime = lastMaintainTime;
                }
                else if (req.MaintainPeriodDays < entity.MaintainPeriodDays)//维护周期变短
                {
                    while (DateTime.Compare(lastMaintainTime, date) <= 0)
                    {//计算出来的维护时间 小于今天
                        lastMaintainTime = lastMaintainTime.AddDays(req.MaintainPeriodDays.Value);
                    }
                }

                req.LastMaintainTime = lastMaintainTime;
            }

        }
        //设备参数中固定的几个字段不允许修改
        if (!string.IsNullOrEmpty(entity.Parameters) && !string.IsNullOrEmpty(req.Parameters))
        {
            try
            {
                var oldParameters = JsonSerializer.Deserialize<DeviceParameterDto>(entity.Parameters);
                var reqParameters = JsonSerializer.Deserialize<DeviceParameterDto>(req.Parameters);
                if (oldParameters != null && reqParameters != null)
                {
                    reqParameters.DeviceId = oldParameters.DeviceId;
                    reqParameters.DeviceName = oldParameters.DeviceName;
                    reqParameters.ProductId = oldParameters.ProductId;
                    reqParameters.DeviceClazz = oldParameters.DeviceClazz;

                    string jsonStr = JsonSerializer.Serialize(reqParameters);
                    if (!string.IsNullOrEmpty(jsonStr))
                    {
                        req.Parameters = jsonStr;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        var model = _mapper.Map<Device>(req);
        model.CreateTime = entity.CreateTime;
        model.CreatorId = entity.CreatorId;
        model.ModifierId = UserId;
        model.ModifyTime = DateTime.Now;

        var result = await _domainService.Update(model);
        if (!result)
        {
            return Fail<DeviceInfoDto>("update failed");
        }

        if (req.DeviceTypeCode.ToLower().Equals("drill") && req.SpindleNum != null && req.SpindleNum != 0 && req.InteractionPosition.HasValue)
        {
            await _drillPanelDetailService.LoadPanelDetailData(new LoadDrillPanelDetailReq
            {
                DeviceCode = req.Code
            });
        }

        if (req.WorkStationId != null && req.WorkStationId != 0)
        {
            var workstationData = await _workstationDomainService.QueryByID(req.WorkStationId);
            if (workstationData != null)
            {
                long processId = 0;
                if (!string.IsNullOrEmpty(entity.DeviceTypeCode) && !string.IsNullOrEmpty(req.DeviceTypeCode)
                    && entity.DeviceTypeCode.ToLower() != req.DeviceTypeCode.ToLower())
                {
                    var processName = "";
                    var process = await _processDomainService.FindSingleAsync(p => p.Code.ToLower() == req.DeviceTypeCode.ToLower());
                    if (process != null)
                    {
                        processName = process.Name;
                        processId = process.Id;
                    }
                    else //add new process
                    {
                        processId = await _processDomainService.AddReturnId(new Process
                        {
                            Code = req.DeviceTypeCode.ToLower(),
                            Name = req.DeviceTypeCode.ToLower(),
                            CreateTime = DateTime.Now,
                            CreatorId = UserId,
                            Status = 1
                        });
                    }
                    workstationData.ProcessCode = req.DeviceTypeCode.ToLower();
                    workstationData.ProcessId = processId;
                    workstationData.ProcessName = processName;
                    workstationData.ModifierId = UserId;
                    workstationData.ModifyTime = DateTime.Now;
                    await _workstationDomainService.Update(workstationData);
                }
                else
                {
                    processId = (long)workstationData.ProcessId;
                }

                await _routeProcessAndWorkStationDomainService.DeleteAsync(p => p.WorkStationId == req.WorkStationId);

                //set route for work station
                if (!string.IsNullOrEmpty(req.RouteCode) && processId > 0 && !req.DeviceTypeCode.ToLower().Equals("agv"))
                {
                    var routeCodes = req.RouteCode.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var routes = await _routeDomainService.QueryAsync(r => r.IsDeleted == 0
                    && routeCodes.Contains(r.Code.ToLower()), r => r.Code, OrderByType.Asc);
                    if (routes != null && routes.Count > 0)
                    {
                        foreach (var route in routes)
                        {
                            if (await _routeAndProcessDomainService.IsExistAsync(rp => rp.RouteId == route.Id && rp.ProcessId == processId))
                            {
                                var routeAndProcess = await _routeAndProcessDomainService.FindSingleAsync(rp =>
                                rp.RouteId == route.Id && rp.ProcessId == processId);
                                if (routeAndProcess == null)
                                {
                                    continue;
                                }
                                //尝试将工作站，绑定到工艺路线的子工序的关联设定中
                                if (!await _routeProcessAndWorkStationDomainService.IsExistAsync(rps =>
                                rps.RouteAndProcessId == routeAndProcess.Id && rps.WorkStationId == req.WorkStationId))
                                {
                                    await _routeProcessAndWorkStationDomainService.Add(new RouteProcessAndWorkStation
                                    {
                                        WorkStationId = req.WorkStationId,
                                        RouteAndProcessId = routeAndProcess.Id,
                                    });
                                }
                            }
                        }
                    }

                }
            }
        }

        var entityChanged = await _domainService.QueryByID(req.Id);
        if (entityChanged == null)
        {
            return Fail<DeviceInfoDto>("信息不存在!");
        }

        var dto = _mapper.Map<DeviceInfoDto>(entityChanged);
        return Success(dto);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deviceDescriptor"></param>
    /// <param name="newStatus"></param>
    /// <returns></returns>
    public async Task<ResponseDto<DeviceInfoDto>> UpdateStatus(DeviceDescriptor deviceDescriptor, DeviceStatus newStatus)
    {
        var entity = await _domainService.FindSingleAsync(d => d.Code != null && d.Code.ToLower() == deviceDescriptor.DeviceId.ToLower());
        if (entity == null)
        {
            await AddNewDevice(deviceDescriptor);
        }

        entity = await _domainService.FindSingleAsync(d => d.Code != null && d.Code.ToLower() == deviceDescriptor.DeviceId.ToLower());
        if (deviceDescriptor.Extra.ContainsKey("MachineSize"))
        {
            entity.MachineSize = deviceDescriptor.Extra["MachineSize"].ToStr();
        }
        if (deviceDescriptor.Extra.ContainsKey("IsAuto"))
        {
            entity.IsAuto = deviceDescriptor.Extra["IsAuto"].ToBool();
        }
        if (deviceDescriptor.Extra.ContainsKey("IsDuo"))
        {
            entity.IsDuo = deviceDescriptor.Extra["IsDuo"].ToBool();
        }
        if (deviceDescriptor.Extra.ContainsKey("RawLocationCode") && !string.IsNullOrWhiteSpace(deviceDescriptor.Extra["RawLocationCode"].ToStr()))
        {
            entity.RawLocationCode = deviceDescriptor.Extra["RawLocationCode"].ToStr();
        }
        if (deviceDescriptor.Extra.ContainsKey("ClinkerLocationCode") && !string.IsNullOrWhiteSpace(deviceDescriptor.Extra["ClinkerLocationCode"].ToStr()))
        {
            entity.ClinkerLocationCode = deviceDescriptor.Extra["ClinkerLocationCode"].ToStr();
        }

        entity.Parameters = JsonSerializer.Serialize(deviceDescriptor);

        entity.DeviceStatus = newStatus;
        entity.ModifyTime = DateTime.Now;
        var result = await _domainService.Update(entity);
        if (!result)
        {
            return Fail<DeviceInfoDto>("update failed");
        }

        var entityChanged = await _domainService.QueryByID(entity.Id);
        if (entityChanged == null)
        {
            return Fail<DeviceInfoDto>("信息不存在!");
        }
        var dto = _mapper.Map<DeviceInfoDto>(entityChanged);
        return Success(dto);
    }

    private async System.Threading.Tasks.Task AddNewDevice(DeviceDescriptor deviceDescriptor)
    {
        long deviceTypeId = 1; //默认为类型-全部
        string deviceTypeCode = "000";
        var deviceType = await this._deviceTypeDomainService.FindSingleAsync(d => d.Code != null && d.Code.ToLower() == deviceDescriptor.ProductId.ToLower());
        if (deviceType != null)
        {
            deviceTypeId = deviceType.Id;
            deviceTypeCode = string.IsNullOrEmpty(deviceType.Code) ? "" : deviceType.Code;
        }

        long workStationId = 0;
        if (!deviceTypeCode.ToLower().Equals("agv"))
        {
            var workStation = await _workstationDomainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.Code)
            && p.Code.ToLower() == deviceDescriptor.DeviceId.ToLower());

            if (workStation != null)
            {
                workStationId = workStation.Id;
            }
            else
            {

                var process = await _processDomainService.FindSingleAsync(p => p.Code.ToLower() == deviceDescriptor.ProductId.ToLower());
                long processId = 0;
                var processName = "";
                if (process != null)
                {
                    processName = process.Name;
                    processId = process.Id;
                }
                //add new work station
                workStationId = await _workstationDomainService.AddReturnId(new WorkStation
                {
                    Code = deviceDescriptor.DeviceId,
                    Name = deviceDescriptor.DeviceName,
                    ProcessCode = deviceDescriptor.ProductId.ToLower(),
                    ProcessId = processId,
                    ProcessName = processName,
                    CreateTime = DateTime.Now,
                    CreatorId = UserId,
                    Status = 1
                });
                //set route for work station
                //指向给定的工艺路线，可设置一个或者多个，如"A0001"， 或者 "A0001,B0002"
                if (processId > 0
                    && deviceDescriptor.Extra.ContainsKey("RouteCode")
                    && !string.IsNullOrEmpty(deviceDescriptor.Extra["RouteCode"].ToStr()))
                {
                    var routeCodeValue = deviceDescriptor.Extra["RouteCode"].ToStr().ToLower();
                    var routeCodes = routeCodeValue.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var routes = await _routeDomainService.QueryAsync(r => r.IsDeleted == 0 && routeCodes.Contains(r.Code.ToLower()), r => r.Code, OrderByType.Asc);
                    foreach (var route in routes)
                    {
                        if (await _routeAndProcessDomainService.IsExistAsync(rp => rp.RouteId == route.Id && rp.ProcessId == processId))
                        {
                            var routeAndProcess = await _routeAndProcessDomainService.FindSingleAsync(rp => rp.RouteId == route.Id && rp.ProcessId == processId);
                            if (routeAndProcess == null)
                            {
                                continue;
                            }
                            //尝试将工作站，绑定到工艺路线的子工序的关联设定中
                            if (!await _routeProcessAndWorkStationDomainService.IsExistAsync(rps => rps.RouteAndProcessId == routeAndProcess.Id && rps.WorkStationId == workStationId))
                            {
                                await _routeProcessAndWorkStationDomainService.Add(new RouteProcessAndWorkStation
                                {
                                    WorkStationId = workStationId,
                                    RouteAndProcessId = routeAndProcess.Id,
                                });
                            }
                        }
                    }
                }
            }
        }

        var parameter = JsonSerializer.Serialize(deviceDescriptor);

        InteractionPosition interactionPosition = await GetDeviceInteractionPositon();
        var newDevice = new AddOrUpdateDeviceReq
        {
            Code = deviceDescriptor.DeviceId,
            Name = deviceDescriptor.DeviceName,
            DeviceKind = deviceDescriptor.DeviceKind,
            SpindleNum = deviceDescriptor.SpindleNum,
            InteractionPosition = interactionPosition,
            DeviceTypeId = (int)deviceTypeId,
            DeviceTypeCode = deviceTypeCode,
            Parameters = parameter,
            WorkStationId = workStationId
        };

        if (deviceDescriptor.Extra.ContainsKey("MachineSize"))
        {
            newDevice.MachineSize = deviceDescriptor.Extra["MachineSize"].ToStr();
        }
        if (deviceDescriptor.Extra.ContainsKey("IsAuto"))
        {
            newDevice.IsAuto = deviceDescriptor.Extra["IsAuto"].ToBool();
        }
        if (deviceDescriptor.Extra.ContainsKey("IsDuo"))
        {
            newDevice.IsDuo = deviceDescriptor.Extra["IsDuo"].ToBool();
        }
        if (deviceDescriptor.Extra.ContainsKey("RawLocationCode") && !string.IsNullOrWhiteSpace(deviceDescriptor.Extra["RawLocationCode"].ToStr()))
        {
            newDevice.RawLocationCode = deviceDescriptor.Extra["RawLocationCode"].ToStr();
        }
        if (deviceDescriptor.Extra.ContainsKey("ClinkerLocationCode") && !string.IsNullOrWhiteSpace(deviceDescriptor.Extra["ClinkerLocationCode"].ToStr()))
        {
            newDevice.ClinkerLocationCode = deviceDescriptor.Extra["ClinkerLocationCode"].ToStr();
        }

        await Add(newDevice);

        if (deviceTypeCode.ToLower().Equals("drill") && deviceDescriptor.DeviceKind != 0)
        {
            await _drillPanelDetailService.LoadPanelDetailData(new LoadDrillPanelDetailReq
            {
                DeviceCode = deviceDescriptor.DeviceId
            });
        }
    }

    /// <summary>
    /// 查询首页设备数量
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseDto<DeviceStatusForHomeDto>> GetHomeData()
    {
        DeviceStatusForHomeDto data = new DeviceStatusForHomeDto();
        var result = await _deviceDomainService.QueryAsync(p => p.IsDeleted == 0, p => p.CreateTime, OrderByType.Desc);
        if (result == null || result.Count == 0)
        {
            return Success(data);
        }
        data.AllDeviceCount = result.Count;

        data.RunningCount = result.FindAll(p => p.DeviceStatus == DeviceStatus.Working).ToList().Count;

        data.StandbyCount = result.FindAll(p => p.DeviceStatus == DeviceStatus.Ready).ToList().Count;

        data.WarnningCount = result.FindAll(p => p.DeviceStatus == DeviceStatus.Exception).ToList().Count;

        data.OfflineCount = result.FindAll(p => p.DeviceStatus == DeviceStatus.Offline).ToList().Count;

        data.OnlineCount = result.FindAll(p => p.DeviceStatus == DeviceStatus.Online).ToList().Count;

        data.MaintenanceCount = result.FindAll(p => p.DeviceStatus == DeviceStatus.Maintenance).ToList().Count;

        data.LowBatteryCount = result.FindAll(p => p.DeviceStatus == DeviceStatus.LowBattery).ToList().Count;

        data.ChargingCount = result.FindAll(p => p.DeviceStatus == DeviceStatus.Charging).ToList().Count;

        return Success(data);
    }

    /// <summary>
    /// 自动生成设备维护记录
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseDto<string>> AddDeviceMaintain()
    {
        var where = PredicateBuilder.True<Device>();
        DateTime date = DateTime.Now.Date;
        where = where.And(p => p.IsDeleted == 0 && p.LastMaintainTime == date);
        var result = await _domainService.QueryAsync(where, q => q.Id, SqlSugar.OrderByType.Asc);
        List<DeviceMaintain> list = new List<DeviceMaintain>();
        List<Device> deviceList = new List<Device>();
        foreach (var item in result)
        {
            DeviceMaintain deviceMaintain = new DeviceMaintain();
            deviceMaintain.DeviceId = (int?)item.Id;
            deviceMaintain.DeviceCode = item.Code;
            deviceMaintain.DeviceName = item.Name;
            deviceMaintain.MaintainStatus = "-1";
            deviceMaintain.CreateTime = DateTime.Now;
            deviceMaintain.Status = (int)DataStatusEnum.Enable;
            list.Add(deviceMaintain);
            if (item.LastMaintainTime != null && item.MaintainPeriodDays != null)
            {
                item.LastMaintainTime = item.LastMaintainTime.Value.AddDays((double)item.MaintainPeriodDays);
                deviceList.Add(item);
            }

        }
        //变更机器下次维护时间
        await _domainService.BulkUpdate(deviceList);
        //生成设备维护记录
        await _deviceMaintainService.BulkInsert(list);
        return Success();
    }

    /// <summary>
    /// 删除信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseDto<string>> DeleteData(long id)
    {
        var entity = await _domainService.QueryByID(id);
        if (entity == null)
        {
            return Fail("信息不存在!");
        }

        //判断关联的工作站是否有生产任务，存在生成任务则删除失败；（删除设备同步删除关联的工作站）
        if (entity.WorkStationId != null && entity.WorkStationId != 0)
        {
            var isExistTask = await _taskDomainService.IsExistAsync(p => p.WorkStationId == entity.WorkStationId);
            if (isExistTask)
            {
                return Fail("存在关联的生产任务，建议禁用此设备!");
            }
        }

        var result = await _domainService.DeleteById(id);
        if (result)
        {
            if (entity.WorkStationId != null)
            {
                var isExistWorkStation = await _workstationDomainService.IsExistAsync(p => p.Id == entity.WorkStationId);
                if (isExistWorkStation)
                {
                    await _workstationDomainService.DeleteById((int)entity.WorkStationId);
                }
            }
            return Success("");
        }
        return Fail("删除失败");
    }

    /// <summary>
    /// 删除信息集合
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    public async Task<ResponseDto<string>> DeleteDataList(List<long> idList)
    {
        if (idList != null)
        {
            object[] deleteList = new object[idList.Count];
            List<long> deleteWorkStations = new List<long>();
            for (int i = 0; i < idList.Count; i++)
            {
                var entity = await _domainService.QueryByID(idList[i]);
                if (entity == null)
                {
                    continue;
                }

                //判断关联的工作站是否有生产任务，存在生成任务则删除失败；（删除设备同步删除关联的工作站）
                if (entity.WorkStationId != null && entity.WorkStationId != 0)
                {
                    var isExistTask = await _taskDomainService.IsExistAsync(p => p.WorkStationId == entity.WorkStationId);
                    if (isExistTask)
                    {
                        return Fail(entity.Code + " 存在关联的生产任务，建议禁用此设备!");
                    }

                    var isExistWorkstation = await _workstationDomainService.IsExistAsync(p => p.Id == entity.WorkStationId);
                    if (isExistWorkstation)
                    {
                        deleteWorkStations.Add((long)entity.WorkStationId);
                    }
                }

                deleteList[i] = idList[i];

            }

            var result = await _domainService.DeleteByIds(deleteList);
            if (result)
            {
                await _workstationDomainService.DeleteAsync(p => deleteWorkStations.Contains(p.Id));

                return Success("");
            }
        }
        return Fail("删除失败");
    }

    /// <summary>
    /// 根据设备获取关联的生产线
    /// </summary>
    /// <param name="agvDeviceCode"></param>
    /// <param name="anyDeviceCode"></param>
    /// <returns></returns>
    public async Task<ResponseDto<RouteAndProcessInfoByDeviceDto>> GetRouteAndProcessInfo(string agvDeviceCode, string anyDeviceCode)
    {
        if (string.IsNullOrEmpty(agvDeviceCode) || string.IsNullOrEmpty(anyDeviceCode))
        {
            return Fail<RouteAndProcessInfoByDeviceDto>("agvDeviceCode 、anyDeviceCode 必须填写！");
        }

        var isExsitAGV = await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.Code) && p.Code.ToLower().Equals(agvDeviceCode.ToLower())
        && !string.IsNullOrEmpty(p.DeviceTypeCode) && p.DeviceTypeCode.ToLower().Equals("agv"));
        if (!isExsitAGV)
        {
            return Fail<RouteAndProcessInfoByDeviceDto>("agvDeviceCode 必须填写AGV设备的编码！");
        }

        RouteAndProcessInfoByDeviceDto result = new RouteAndProcessInfoByDeviceDto();

        result.RouteAndProcessLists = await _deviceDomainService.GetRouteAndProcessList(agvDeviceCode, anyDeviceCode);

        if (result.RouteAndProcessLists != null && result.RouteAndProcessLists.Count > 0)
        {
            result.IsMatching = true;
        }
        else
        {
            result.IsMatching = false;
        }

        return Success(result);
    }

    /// <summary>
    /// 批量刷新设备状态
    /// </summary>
    /// <param name="onlineDeviceCodes"></param>
    /// <returns></returns>
    public async Task<int> RefreshDeviceStatus(List<AddOrUpdateDeviceReq> onlineDeviceCodes)
    {
        var lowerCodes = onlineDeviceCodes.Select(x => x.Code.ToLower()).ToList();

        // 限定更新这些状态的设备，其他状态的设备，不做更新
        var deviceStatusList = new List<DeviceStatus?> {
                DeviceStatus.Online,
                DeviceStatus.Exception,
                DeviceStatus.Ready,
                DeviceStatus.Working,
                DeviceStatus.Charging,
                DeviceStatus.LowBattery,
            };

        var updateDevices = await _deviceDomainService.QueryAsync(
                d => deviceStatusList.Contains(d.DeviceStatus) && !string.IsNullOrEmpty(d.Code) && !lowerCodes.Contains(d.Code.ToLower()),
                d => d.Id,
                OrderByType.Asc
            );

        foreach (var device in updateDevices)
        {
            device.DeviceStatus = DeviceStatus.Offline;
            device.ModifierId = 1;
            device.ModifyTime = DateTime.Now;
        }

        var onlineDevices = await _deviceDomainService.QueryAsync(
            d => lowerCodes.Contains(d.Code.ToLower()),
            d => d.Id,
            OrderByType.Asc
            );
        foreach (var device in onlineDevices)
        {
            device.DeviceStatus = onlineDeviceCodes.FirstOrDefault(x => x.Code.ToLower() == device.Code.ToLower()).DeviceStatus;
            device.ModifierId = 1;
            device.ModifyTime = DateTime.Now;
        }
        updateDevices.AddRange(onlineDevices);

        if (await _deviceDomainService.BulkUpdate(updateDevices))
        {
            return updateDevices.Count;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 获取在线设备绑定的工艺路线
    /// </summary>
    /// <param name="devices"></param>
    /// <returns></returns>
    public async Task<List<CentralOnlineDeviceDto>> GetOnlineDeviceInfo(List<CentralOnlineDeviceDto> devices)
    {
        var result = await _deviceDomainService.GetOnlineDeviceInfo(devices);
        return result;
    }

    /// <summary>
    /// 获取中控系统在线设备列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<PageDto<CentralOnlineDeviceDto>>> GetCentralOnlineDevice(CentralOnlineDeviceReq req)
    {
        if (req.PageNum < 1) req.PageNum = 1;
        if (req.PageSize < 1) req.PageSize = 10;
        var pageDto = new PageDto<CentralOnlineDeviceDto>(req.PageNum, req.PageSize);

        try
        {
            //获取中控在线列表，刷新数据状态
            var deviceData = await _centralOnlineDevice.GetOnlineDevicesAndRoute();

            if (deviceData == null)
            {
                return Fail<PageDto<CentralOnlineDeviceDto>>("result, 未查询到结果！");
            }
            else
            {
                var returnData = deviceData;
                if (!string.IsNullOrEmpty(req.DeviceId))
                {
                    returnData = returnData.Where(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower().Contains(req.DeviceId.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(req.TargetDevice))
                {
                    returnData = returnData.Where(p => p.TargetDevice == req.TargetDevice).ToList();
                }
                if (!string.IsNullOrEmpty(req.Status))
                {
                    returnData = returnData.Where(p => p.Status == req.Status).ToList();
                }
                if (req.RequestDeviceKindList != null && req.RequestDeviceKindList.Count > 0)
                {
                    returnData = returnData.Where(p => req.RequestDeviceKindList.Contains(p.Descriptor.DeviceKind)).ToList();
                }
                if (req.IsAuto != null)
                {
                    returnData = returnData.Where(p => p.Descriptor.Extra.ContainsKey("IsAuto")
                    && p.Descriptor.Extra["IsAuto"].ToBool() == req.IsAuto
                    ).ToList();
                }
                returnData = returnData.OrderBy(p => p.DeviceId).ToList();

                pageDto.Total = returnData.Count;
                int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)pageDto.Total / req.PageSize) : 0;
                if (pageCount < req.PageNum)
                {
                    req.PageNum = pageCount;
                }
                returnData = returnData.Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize).ToList();

                pageDto.List = returnData;
            }

            return Success(pageDto);
        }
        catch (Exception)
        {
            return Fail<PageDto<CentralOnlineDeviceDto>>("异常终止！");
        }
    }

    /// <summary>
    /// 根据设备编码获取在线设备详细信息
    /// </summary>
    /// <param name="deviceId"></param>
    /// <returns></returns>
    public async Task<ResponseDto<CentralOnlineDeviceDto>> GetOnlineDeviceInfo(string deviceId)
    {
        try
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return Fail<CentralOnlineDeviceDto>("deviceId未填写！");
            }

            //获取中控在线列表，刷新数据状态
            var deviceData = await _centralOnlineDevice.GetOnlineDevicesAndRoute();

            if (deviceData == null)
            {
                return Fail<CentralOnlineDeviceDto>("result, 未查询到结果！");
            }
            else
            {
                var returnData = deviceData.Where(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower().Equals(deviceId.Trim().ToLower())).ToList();

                if (returnData == null || returnData.Count == 0)
                {
                    return Fail<CentralOnlineDeviceDto>("未查询到数据！");
                }
                return Success(returnData[0]);
            }
        }
        catch (Exception)
        {
            return Fail<CentralOnlineDeviceDto>("异常终止！");
        }
    }

    /// <summary>
    /// 获取钻机实时数据
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<PageDto<DrillDeviceTaskDto>>> GetDrillDeviceTask(GetDrillOrAgvDeviceInfoReq req)
    {
        req.PageNum = 1;
        int shouCount = await _sysConfigManager.GetIntValue(MESConfigConstants.DRILL_DEVICE_TASK_SHOW_COUNT);
        if (shouCount < 1)
        {
            req.PageSize = 10;
        }
        else
        {
            req.PageSize = shouCount;
        }
        var pageDto = new PageDto<DrillDeviceTaskDto>(req.PageNum, req.PageSize);

        try
        {
            //获取中控在线列表，刷新数据状态
            var deviceData = await _centralOnlineDevice.GetOnlineDevicesAndRoute();

            if (deviceData == null)
            {
                return Fail<PageDto<DrillDeviceTaskDto>>("result, 未查询到结果！");
            }
            else
            {
                var returnData = deviceData.Where(p => !string.IsNullOrEmpty(p.ProductId) && p.ProductId.ToLower().Equals("drill")).ToList();

                if (!string.IsNullOrEmpty(req.DeviceCode))
                {
                    returnData = returnData.Where(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower().Contains(req.DeviceCode.ToLower())).ToList();
                }

                if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
                {
                    returnData = returnData.Where(p => !string.IsNullOrEmpty(p.RouteCode) && req.RouteCodeList.Any(r => p.RouteCode.Contains(r))).ToList();
                }

                if (req.DeviceStatuseLists != null && req.DeviceStatuseLists.Count > 0)
                {
                    returnData = returnData.Where(p => Enum.IsDefined(typeof(DeviceStatus), p.DeviceStatus)
                    && req.DeviceStatuseLists.Contains((DeviceStatus)p.DeviceStatus)).ToList();
                }

                returnData = returnData.OrderBy(p => p.DeviceId).ToList();

                pageDto.Total = returnData.Count;
                int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)pageDto.Total / req.PageSize) : 0;
                if (pageCount < req.PageNum)
                {
                    req.PageNum = pageCount;
                }
                returnData = returnData.Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize).ToList();

                var drillDeviceTaskDtos = await _deviceDomainService.GetDrillDeviceTask(returnData);
                drillDeviceTaskDtos = drillDeviceTaskDtos.OrderBy(p => p.DeviceId).ToList();
                pageDto.List = drillDeviceTaskDtos;
            }

            return Success(pageDto);
        }
        catch (Exception)
        {
            return Fail<PageDto<DrillDeviceTaskDto>>("异常终止！");
        }
    }

    /// <summary>
    /// 获取在线AGV板料信息及最近调度记录
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<ResponseDto<PageDto<AGVDeviceSiloInfo>>> GetAGVDeviceSiloInfo(GetDrillOrAgvDeviceInfoReq req)
    {
        req.PageNum = 1;
        req.PageSize = int.MaxValue;
        var pageDto = new PageDto<AGVDeviceSiloInfo>(req.PageNum, req.PageSize);

        try
        {
            //获取中控在线列表，刷新数据状态
            var deviceData = await _centralOnlineDevice.GetOnlineDevicesAndRoute();

            if (deviceData == null)
            {
                return Fail<PageDto<AGVDeviceSiloInfo>>("result, 未查询到结果！");
            }

            var data = deviceData;

            if (req != null && data != null)
            {
                var returnData = data.Where(p => !string.IsNullOrEmpty(p.ProductId) && p.ProductId.ToLower().Equals("agv")).ToList();

                if (!string.IsNullOrEmpty(req.DeviceCode))
                {
                    returnData = returnData.Where(p => !string.IsNullOrEmpty(p.DeviceId) && p.DeviceId.ToLower().Contains(req.DeviceCode.ToLower())).ToList();
                }

                if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
                {
                    returnData = returnData.Where(p => !string.IsNullOrEmpty(p.RouteCode) && req.RouteCodeList.Any(r => p.RouteCode.Contains(r))).ToList();
                }

                if (req.DeviceStatuseLists != null && req.DeviceStatuseLists.Count > 0)
                {
                    returnData = returnData.Where(p => Enum.IsDefined(typeof(DeviceStatus), p.DeviceStatus)
                    && req.DeviceStatuseLists.Contains((DeviceStatus)p.DeviceStatus)).ToList();
                }

                returnData = returnData.OrderBy(p => p.DeviceId).ToList();

                pageDto.Total = returnData.Count;
                int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)pageDto.Total / req.PageSize) : 0;
                if (pageCount < req.PageNum)
                {
                    req.PageNum = pageCount;
                }
                returnData = returnData.Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize).ToList();

                //获取最近调度记录
                List<ScheduleDto> schedules = new List<ScheduleDto>();
                int queryCount = await _sysConfigManager.GetIntValue(MESConfigConstants.AGV_LATEST_SCHEDULE_COUNT);
                if (queryCount > 0)
                {
                    schedules = await _deviceDomainService.GetLatestSchedule(returnData, queryCount);
                }

                List<AGVDeviceSiloInfo> aGVDeviceSiloInfos = new List<AGVDeviceSiloInfo>();
                foreach (var item in returnData)
                {
                    AGVDeviceSiloInfo model = new AGVDeviceSiloInfo();
                    model.DeviceId = item.DeviceId;
                    model.DeviceStatus = item.DeviceStatus;
                    model.RouteCode = item.RouteCode;
                    if (item.Descriptor != null)
                    {
                        model.LayerCount = item.Descriptor.LayerLimit;
                        model.DeviceConsoleAddress = item.Descriptor.HostAddress;
                        model.DeviceKind = item.Descriptor.DeviceKind;
                    }
                    if (item.Properties != null)
                    {
                        model.TargetDevice = item.Properties.ContainsKey("TargetDevice") ? item.Properties["TargetDevice"].ToString() : "";
                        model.TaskCode = item.Properties.ContainsKey("TaskId") ? item.Properties["TaskId"].ToString() : "";
                        model.TaskItemCode = item.Properties.ContainsKey("TaskItemCode") ? item.Properties["TaskItemCode"].ToString() : "";
                        model.RequestInteractionBehaviorName = item.Properties.ContainsKey("RequestInteractionBehaviorName") ?
                            item.Properties["RequestInteractionBehaviorName"].ToString() : "";

                        model.Properties = item.Properties;
                    }

                    model.AgVSiloInfo.AddRange(item.PayloadPanels
                        .Where(p => p.ProductStatus != ProductStatus.EmptyPayload)
                        .GroupBy(p => new { SimpleProductStatus = ProductStatusConstants.SimplifyProductStatus(p.ProductStatus), p.ItemCode, p.SiloCode })
                        .Select(p => new SiloItemSumInfo
                        {
                            ItemCode = p.Key.ItemCode,
                            SimpleProductStatus = p.Key.SimpleProductStatus,
                            ProductStatus = ProductStatusConstants.Finished_PIN.Contains(p.Key.SimpleProductStatus) ? "生料" :
                                (ProductStatusConstants.Finished_DRILL.Contains(p.Key.SimpleProductStatus) ? "熟料" : ""),
                            SiloCount = p.Count(),
                            SiloCode = p.Key.SiloCode
                        })
                        .ToList());

                    model.AgVSiloInfo.AddRange(item.PayloadCutterTrays
                        .Where(p => p.Status != CutterTrayStatus.NoTray)
                        .GroupBy(p => new { p.Status, p.ItemCode, p.SiloCode })
                        .Select(p => new SiloItemSumInfo
                        {
                            ItemCode = p.Key.ItemCode,
                            ProductStatus = p.Key.Status.ToString(),
                            SiloCount = p.Count(),
                            SiloCode = p.Key.SiloCode
                        })
                        .ToList());

                    if (schedules != null)
                    {
                        model.Schedules = schedules.FindAll(p => !string.IsNullOrEmpty(p.RequireDeviceId)
                        && p.RequireDeviceId.ToLower().Equals(item.DeviceId.ToLower()));
                    }

                    model.SiloCode = model.AgVSiloInfo != null && model.AgVSiloInfo.Exists(p => !string.IsNullOrEmpty(p.SiloCode)) ?
                        model.AgVSiloInfo.Where(p => !string.IsNullOrEmpty(p.SiloCode)).Select(p => p.SiloCode).First() : "";

                    aGVDeviceSiloInfos.Add(model);
                }

                pageDto.List = aGVDeviceSiloInfos;
            }
            else
            {
                pageDto.Total = 0;
                pageDto.List = new List<AGVDeviceSiloInfo>();
            }

            return Success(pageDto);
        }
        catch (Exception)
        {
            return Fail<PageDto<AGVDeviceSiloInfo>>("异常终止！");
        }
    }

    /// <summary>
    /// 统计近七天设备调度数量
    /// </summary>
    /// <returns></returns>
    public async Task<ScheduleDeviceByTimesStatsDto> GetScheduleDeviceStats()
    {
        ScheduleDeviceByTimesStatsDto result = new ScheduleDeviceByTimesStatsDto();
        result.StatsTimes = new List<string>();
        for (int i = 6; i >= 0; i--)
        {
            DateTime sTime = DateTime.Today.AddDays(-i);
            result.StatsTimes.Add(string.Format("{0:MM/dd}", sTime));
        }
        result.QuantitysStats = new List<ScheduleStatsDto>();

        var data = await _deviceDomainService.GetSchedules();
        if (data == null || data.Count == 0)
        {
            return result;
        }

        foreach (var item in data.DistinctBy(p => p.RequireDeviceId).ToList())
        {
            if (result.QuantitysStats.Exists(p => p.DeviceCode == item.RequireDeviceId))
            {
                continue;
            }

            result.QuantitysStats.Add(new ScheduleStatsDto
            {
                DeviceCode = item.RequireDeviceId,
            });
        }

        for (int i = 6; i >= 0; i--)
        {
            DateTime sTime = DateTime.Today.AddDays(-i);

            foreach (var deviceItem in result.QuantitysStats)
            {
                if (string.IsNullOrEmpty(deviceItem.DeviceCode))
                {
                    continue;
                }
                var scheduleList = data.Where(p => p.RequireDeviceId == deviceItem.DeviceCode
                && ((p.CompletedTime != null && p.CompletedTime.Value.Date.Equals(sTime))
                || (p.FailedTime != null && p.FailedTime.Value.Date.Equals(sTime)))).ToList();

                if (scheduleList == null || scheduleList.Count == 0)
                {
                    deviceItem.Quantitys.Add("0");
                    continue;
                }

                deviceItem.Quantitys.Add(scheduleList.Count.ToString());
            }
        }

        return result;
    }

    /// <summary>
    /// 统计近七天设备调度数量(完成/异常)
    /// </summary>
    /// <returns></returns>
    public async Task<ScheduleDeviceByTimesStatsDto> GetScheduleDeviceStatusStats()
    {
        ScheduleDeviceByTimesStatsDto result = new ScheduleDeviceByTimesStatsDto();
        result.StatsTimes = new List<string>();
        for (int i = 6; i >= 0; i--)
        {
            DateTime sTime = DateTime.Today.AddDays(-i);
            result.StatsTimes.Add(string.Format("{0:MM/dd}", sTime));
        }
        result.QuantitysStats = new List<ScheduleStatsDto>();

        var completeData = await _deviceDomainService.GetSchedulesByStatus(ScheduledTaskStatus.Completed);
        if (completeData != null && completeData.Count > 0)
        {
            foreach (var item in completeData.DistinctBy(p => p.RequireDeviceId).ToList())
            {
                string label = string.Format("{0}（成功）", item.RequireDeviceId);
                if (result.QuantitysStats.Exists(p => p.DeviceCode == item.RequireDeviceId && p.Lable == label))
                {
                    continue;
                }

                result.QuantitysStats.Add(new ScheduleStatsDto
                {
                    DeviceCode = item.RequireDeviceId,
                    Lable = label
                });
            }
        }

        var failData = await _deviceDomainService.GetSchedulesByStatus(ScheduledTaskStatus.Failed);
        if (failData != null && failData.Count > 0)
        {
            foreach (var item in failData.DistinctBy(p => p.RequireDeviceId).ToList())
            {
                string label = string.Format("{0}（失败）", item.RequireDeviceId);
                if (result.QuantitysStats.Exists(p => p.DeviceCode == item.RequireDeviceId && p.Lable == label))
                {
                    continue;
                }

                result.QuantitysStats.Add(new ScheduleStatsDto
                {
                    DeviceCode = item.RequireDeviceId,
                    Lable = label
                });
            }
        }

        result.QuantitysStats = result.QuantitysStats.OrderBy(p => p.DeviceCode).ToList();

        for (int i = 6; i >= 0; i--)
        {
            DateTime sTime = DateTime.Today.AddDays(-i);

            foreach (var deviceItem in result.QuantitysStats)
            {
                if (string.IsNullOrEmpty(deviceItem.DeviceCode))
                {
                    continue;
                }

                if (deviceItem.Lable.Contains("成功"))
                {
                    var completeScheduleList = completeData.Where(p => p.RequireDeviceId == deviceItem.DeviceCode
                    && p.CompletedTime != null && p.CompletedTime.Value.Date.Equals(sTime)).ToList();
                    if (completeScheduleList == null || completeScheduleList.Count == 0)
                    {
                        deviceItem.Quantitys.Add("0");
                    }
                    else
                    {
                        deviceItem.Quantitys.Add(completeScheduleList.Count.ToString());
                    }
                }
                else if (deviceItem.Lable.Contains("失败"))
                {
                    var failScheduleList = failData.Where(p => p.RequireDeviceId == deviceItem.DeviceCode
                    && p.FailedTime != null && p.FailedTime.Value.Date.Equals(sTime)).ToList();
                    if (failScheduleList == null || failScheduleList.Count == 0)
                    {
                        deviceItem.Quantitys.Add("0");
                    }
                    else
                    {
                        deviceItem.Quantitys.Add(failScheduleList.Count.ToString());
                    }
                }

            }
        }

        return result;
    }

    /// <summary>
    /// 根据调度状态，统计近七天设备调度数量
    /// </summary>
    /// <returns></returns>
    public async Task<ScheduleDeviceByTimesStatsDto> GetSchedulementDeviceStatsByStatus(ScheduledTaskStatus scheduledTaskStatus)
    {
        ScheduleDeviceByTimesStatsDto result = new ScheduleDeviceByTimesStatsDto();
        result.StatsTimes = new List<string>();
        for (int i = 6; i >= 0; i--)
        {
            DateTime sTime = DateTime.Today.AddDays(-i);
            result.StatsTimes.Add(string.Format("{0:MM/dd}", sTime));
        }
        result.QuantitysStats = new List<ScheduleStatsDto>();

        var data = await _deviceDomainService.GetSchedulesByStatus(scheduledTaskStatus);
        if (data == null || data.Count == 0)
        {
            return result;
        }

        foreach (var item in data.DistinctBy(p => p.RequireDeviceId).ToList())
        {
            if (result.QuantitysStats.Exists(p => p.DeviceCode == item.RequireDeviceId))
            {
                continue;
            }

            result.QuantitysStats.Add(new ScheduleStatsDto
            {
                DeviceCode = item.RequireDeviceId,
            });
        }

        for (int i = 6; i >= 0; i--)
        {
            DateTime sTime = DateTime.Today.AddDays(-i);

            foreach (var deviceItem in result.QuantitysStats)
            {
                if (string.IsNullOrEmpty(deviceItem.DeviceCode))
                {
                    continue;
                }
                var scheduleList = data.Where(p => p.RequireDeviceId == deviceItem.DeviceCode
                && ((p.CompletedTime != null && p.CompletedTime.Value.Date.Equals(sTime))
                || (p.FailedTime != null && p.FailedTime.Value.Date.Equals(sTime)))).ToList();

                if (scheduleList == null || scheduleList.Count == 0)
                {
                    deviceItem.Quantitys.Add("0");
                    continue;
                }

                deviceItem.Quantitys.Add(scheduleList.Count.ToString());
            }
        }

        return result;
    }

    public async Task<ResponseDto<string>> SetDeviceStatus(string deviceCode, int status)
    {
        if (string.IsNullOrEmpty(deviceCode))
        {
            return Fail("未识别有效的DeviceCode！");
        }

        var deviceData = await _deviceDomainService.FindSingleAsync(p => p.Code.ToLower() == deviceCode.ToLower());
        if (deviceData == null)
        {
            return Fail("未找到设备！");
        }

        deviceData.Status = status;
        deviceData.ModifierId = UserId;
        deviceData.ModifyTime = DateTime.Now;
        await _deviceDomainService.Update(deviceData);

        logger.LogInformation($"SetDeviceStatus DeviceCode：{deviceCode}，status：{status}.");

        switch (status)
        {
            case 0:
                await _drillRateFactorService.BulkAddOrUpdate(new List<AddOrUpdateDrillRateFactorReq>
                {
                    new AddOrUpdateDrillRateFactorReq
                    {
                        DeviceId = deviceData.Code,
                        StartTime = DateTime.Now,
                        Reason = DrillRateFactorReason.DeviceDisable,
                        LocationCode = deviceData.Code,
                    }
                });
                break;
            case 1:
                var drillRateData = await _drillRateFactorService.GetRateFactorWithoutEndTime(new AddOrUpdateDrillRateFactorReq
                {
                    DeviceId = deviceData.Code,
                    Reason = DrillRateFactorReason.DeviceDisable,
                });

                if (drillRateData != null)
                {
                    drillRateData.EndTime = DateTime.Now;
                    drillRateData.ModifyTime = DateTime.Now;

                    await _drillRateFactorDomainService.Update(drillRateData);
                }
                break;
        }

        return Success();
    }

    public async Task<ResponseDto<string>> BulkInsert(List<DeviceToExcelDto> list)
    {
        if (list == null || list.Count == 0)
        {
            return Fail("未识别有效的数据！");
        }

        bool importStatus = await _sysConfigManager.GetBoolValue(MESConfigConstants.IMPORT_STATUS);

        InteractionPosition interactionPosition = await GetDeviceInteractionPositon();

        int failCount = 0;
        StringBuilder sb = new StringBuilder();
        List<Device> devices = new List<Device>();
        List<DeviceType> deviceTypes = new List<DeviceType>();
        foreach (var item in list)
        {
            if (string.IsNullOrEmpty(item.Code) || string.IsNullOrEmpty(item.Name))
            {
                sb.Append($"编码：{item.Code} 或名称：{item.Name} 为空；{Environment.NewLine}");
                failCount++;
                continue;
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == item.Code);
            if (isExsitCode)
            {
                sb.Append($"编码: {item.Code} 数据库已存在；{Environment.NewLine}");
                failCount++;
                continue;
            }

            if (devices.Exists(p => p.Code == item.Code))
            {
                sb.Append($"编码: {item.Code} 导入列表中已存在；{Environment.NewLine}");
                failCount++;
                continue;
            }

            if (item.SpindleNum < 0)
            {
                sb.Append($"编码: {item.Code} 轴数无法识别；{Environment.NewLine}");
                failCount++;
                continue;
            }

            if (string.IsNullOrEmpty(item.DeviceTypeCode) ||
                !await _deviceTypeDomainService.IsExistAsync(p => p.Code.ToLower() == item.DeviceTypeCode.ToLower()))
            {
                sb.Append($"编码: {item.Code} 设备类别无法识别；{Environment.NewLine}");
                failCount++;
                continue;
            }

            long deviceTypeId = 0;
            if (deviceTypes.Exists(p => p.Code.ToLower() == item.DeviceTypeCode.ToLower()))
            {
                var typeData = deviceTypes.SingleOrDefault(p => p.Code.ToLower() == item.DeviceTypeCode.ToLower());
                if (typeData != null)
                {
                    deviceTypeId = typeData.Id;
                }
            }
            else
            {
                var typeData = await _deviceTypeDomainService.FindSingleAsync(p => p.Code.ToLower() == item.DeviceTypeCode.ToLower());
                if (typeData != null)
                {
                    deviceTypeId = typeData.Id;
                    deviceTypes.Add(typeData);
                }
            }

            Device model = new Device();
            model.Code = item.Code;
            model.Name = item.Name;
            model.ParentId = 0;
            model.Ancestors = "0";
            model.InteractionPosition = interactionPosition;
            model.SpindleNum = item.SpindleNum;
            model.DeviceTypeCode = item.DeviceTypeCode.ToLower();
            model.DeviceTypeId = (int)deviceTypeId;
            model.Status = importStatus ? 1 : 0;
            model.CreatorId = UserId;
            model.CreateTime = DateTime.Now;
            devices.Add(model);
        }
        var result = await _deviceDomainService.BulkInsert(devices);
        if (!result)
        {
            return Fail("导入失败！");
        }

        var deviceCodes = devices.Select(d => d.Code).ToList();
        var deviceDatas = await _deviceDomainService.QueryAsync(p => deviceCodes.Contains(p.Code) && !p.DeviceTypeCode.ToLower().Equals("agv"),
            p => p.Code, OrderByType.Desc);

        if (deviceDatas != null && deviceDatas.Count > 0)
        {
            foreach (var deviceData in deviceDatas)
            {
                deviceData.WorkStationId = (int)await GetWorkStationID(deviceData.Code, deviceData.DeviceTypeCode, deviceData.Name);
            }
            await _deviceDomainService.BulkUpdate(deviceDatas);
        }

        string str = string.Format($"预计导入：{list.Count} 条；成功导入：{list.Count - failCount} 条；失败：{failCount} 条；{Environment.NewLine}");
        str = str + sb.ToString();

        return Success(str);
    }

    public async Task<ResponseDto<string>> AllotsDeviceCommand(DeviceCommandRequest commandRequest)
    {
        if (commandRequest == null)
        {
            return Fail("未识别有效的入参！");
        }
        if (string.IsNullOrEmpty(commandRequest.LocationCode))
        {
            return Fail("未识别有效的LocationCode！");
        }
        if (string.IsNullOrEmpty(commandRequest.Command))
        {
            return Fail("未识别有效的Command！");
        }

        if (commandRequest.Params == null || commandRequest.Params.Count == 0)
        {
            commandRequest.Params = new Dictionary<string, object?> { { "DeviceCode", commandRequest.LocationCode } };
        }

        string deviceCode = string.Empty;
        var rackDatas = await _rackDomainService.QueryAsync(p => p.Code == commandRequest.LocationCode, p => p.CreateTime, OrderByType.Desc);
        if (rackDatas != null && rackDatas.Count > 0)
        {
            deviceCode = rackDatas[0].RelateDeviceCode;
        }

        if (string.IsNullOrEmpty(deviceCode))
        {
            deviceCode = commandRequest.LocationCode;
        }

        DeviceCommandCentralRequest req = new DeviceCommandCentralRequest
        {
            DeviceId = deviceCode,
            Command = commandRequest.Command,
            Params = commandRequest.Params
        };

        var urlAddress = await _sysConfigManager.GetStringValue(MESConfigConstants.CENTRAL_ALLOTS_DEVICE_COMMAND);
        if (string.IsNullOrEmpty(urlAddress))
        {
            return Fail("未识别有效的CentralAllotsDeviceCommand！");
        }

        var commandResult = await _centralOnlineDevice.AllotsDeviceCommand(urlAddress, req);
        if (!string.IsNullOrEmpty(commandResult))
        {
            return Fail($"下发设备指令失败：{commandResult}！请稍后重试！");
        }

        return Success();
    }
}
