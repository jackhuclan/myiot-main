using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.WebApi.Controllers.BigScreenServices
{
    public class DeviceScheduleController : BaseController
    {
        private readonly IDeviceService _deviceService;
        private readonly IScheduleService _scheduleService;
        private readonly ISysConfigService _sysConfigService;
        private readonly ITaskService _taskService;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IWorkOrderService _workOrderService;
        private readonly ILogger<DeviceScheduleController> logger;
        private readonly IDeviceGatewayService _deviceGatewayService;
        private readonly IRackService _rackService;
        public DeviceScheduleController(IDeviceService deviceService,
            IScheduleService scheduleService,
            ISysConfigService sysConfigService,
            ITaskService taskService,
            ILoggerFactory loggerFactory,
            ISysConfigManager sysConfigManager,
            IDeviceGatewayService deviceGateway,
            IWorkOrderService workOrderService,
            IRackService rackService)
        {
            _deviceService = deviceService;
            _scheduleService = scheduleService;
            _sysConfigService = sysConfigService;
            _taskService = taskService;
            _sysConfigManager = sysConfigManager;
            logger = loggerFactory.CreateLogger<DeviceScheduleController>();
            _workOrderService = workOrderService;
            _deviceGatewayService = deviceGateway;
            _rackService = rackService;
        }

        /// <summary>
        /// 获取钻机实时数据
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        [Route("GetDrillDeviceTask")]
        [ProducesResponseType(typeof(ResponseDto<List<DrillDeviceTaskDto>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDrillDeviceTask([FromBody] GetDrillOrAgvDeviceInfoReq req)
        {
            logger.LogInformation($"获取钻机实时数据 BeginTime " + DateTime.Now);
            var result = await _deviceService.GetDrillDeviceTask(req);
            logger.LogInformation($"获取钻机实时数据 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 获取在线AGV板料信息
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        [Route("GetAGVDeviceSiloInfo")]
        [ProducesResponseType(typeof(ResponseDto<List<AGVDeviceSiloInfo>>), 200)]
        [ResponseCache(Duration = 20)]
        [AllowAnonymous]
        public async Task<ActionResult> GetAGVDeviceSiloInfo([FromBody] GetDrillOrAgvDeviceInfoReq req)
        {
            logger.LogInformation($"获取在线AGV板料信息 BeginTime " + DateTime.Now);
            var result = await _deviceService.GetAGVDeviceSiloInfo(req);
            logger.LogInformation($"获取在线AGV板料信息 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 统计近七天AGV设备调度数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetScheduleDeviceStats")]
        [ProducesResponseType(typeof(ScheduleDeviceByTimesStatsDto), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetScheduleDeviceStats()
        {
            logger.LogInformation($"统计近七天AGV设备调度数量 BeginTime " + DateTime.Now);
            var result = await _deviceService.GetScheduleDeviceStats();
            logger.LogInformation($"统计近七天AGV设备调度数量 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 统计近七天AGV设备调度数量（完成/异常）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetScheduleDeviceStatusStats")]
        [ProducesResponseType(typeof(ScheduleDeviceByTimesStatsDto), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetScheduleDeviceStatusStats()
        {
            logger.LogInformation($"统计近七天AGV设备调度数量（完成/异常） BeginTime " + DateTime.Now);
            var result = await _deviceService.GetScheduleDeviceStatusStats();
            logger.LogInformation($"统计近七天AGV设备调度数量（完成/异常） EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        ///根据调度记录ID获取调度记录明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetScheduleLogs/{id}")]
        [ProducesResponseType(typeof(ResponseDto<ScheduleLogsDto>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetScheduleLogs(long id)
        {
            logger.LogInformation($"根据调度记录ID获取调度记录明细 BeginTime " + DateTime.Now);
            var result = await _scheduleService.QueryLogsByID(id);
            logger.LogInformation($"根据调度记录ID获取调度记录明细 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 获取已上报调度记录列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetScheduleList")]
        [ProducesResponseType(typeof(ResponseDto<List<ScheduleDto>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetScheduleList([FromBody] GetScheduleListReq req)
        {
            logger.LogInformation($"获取已上报调度记录列表 BeginTime " + DateTime.Now);
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            req.ScheduledTaskStatusList!.Add(ScheduledTaskStatus.Created);
            req.ScheduledTaskStatusList!.Add(ScheduledTaskStatus.PartCompleted);
            req.OrderByIDDesc = false;

            var result = await _scheduleService.GetList(req);
            logger.LogInformation($"获取已上报调度记录列表 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 获取正在执行调度记录列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRunningScheduleList")]
        [ProducesResponseType(typeof(ResponseDto<List<ScheduleDto>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetRunningScheduleList([FromBody] Page req)
        {
            logger.LogInformation($"获取正在执行调度记录列表 BeginTime " + DateTime.Now);
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _scheduleService.GetList(new GetScheduleListReq
            {
                PageNum = req.PageNum,
                PageSize = req.PageSize,
                IsFromBigScreenWeb = true,
                ScheduledTaskStatusList = new List<ScheduledTaskStatus?>()
                {
                    ScheduledTaskStatus.Running,
                    ScheduledTaskStatus.Allocated
                },
                OrderByIDDesc = false,
            });
            logger.LogInformation($"获取正在执行调度记录列表 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        ///获取告警信息等及时信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSysTimelyInformation")]
        [ProducesResponseType(typeof(ResponseDto<SysTimelyInformationDto>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetSysTimelyInformation()
        {
            logger.LogInformation($"获取告警信息等及时信息 BeginTime " + DateTime.Now);
            var result = await _sysConfigService.GetSysTimelyInformation();
            logger.LogInformation($"获取告警信息等及时信息 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 根据设备编码查询Commit状态的任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTaskByDevice")]
        [ProducesResponseType(typeof(ResponseDto<List<TaskDto>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetTaskByDevice([FromBody] GetDrillOrAgvDeviceInfoReq req)
        {
            logger.LogInformation($"根据设备编码查询Commit状态的任务 BeginTime " + DateTime.Now);
            var result = await _taskService.GetTaskByDevice(req);
            logger.LogInformation($"根据设备编码查询Commit状态的任务 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 获取首页设备数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetHomeDeviceData")]
        [ProducesResponseType(typeof(ResponseDto<DeviceStatusForHomeDto>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetHomeDeviceData()
        {
            logger.LogInformation($"获取首页设备数据 BeginTime " + DateTime.Now);
            var result = await _deviceService.GetHomeData();
            logger.LogInformation($"获取首页设备数据 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceList")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceDto>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDeviceList([FromBody] GetDeviceListReq req)
        {
            logger.LogInformation($"获取设备列表 BeginTime " + DateTime.Now);
            var result = await _deviceService.GetEquipmentList(req);
            logger.LogInformation($"获取设备列表 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 获取生产工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetWorkOrderList")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkOrderDto>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetWorkOrderList([FromBody] GetWorkOrderListReq req)
        {
            logger.LogInformation($"获取生产工单列表 BeginTime " + DateTime.Now);
            var result = await _workOrderService.GetList(req);
            logger.LogInformation($"获取生产工单列表 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 展示系统是否维护状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCentralControlSystemIsMaintaining")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCentralControlSystemIsMaintaining()
        {
            logger.LogInformation($"获取中控是否维护 BeginTime " + DateTime.Now);
            var result = await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN);
            logger.LogInformation($"展示系统是否维护状态 EndTime " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 获取料架详细列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRackFullDatas")]
        [ProducesResponseType(typeof(ResponseDto<List<RackFullData>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetRackFullDatas([FromBody] RackFullQueryReq req)
        {
            var result = await _rackService.GetFullDatas(req);
            return Ok(result);
        }
    }
}
