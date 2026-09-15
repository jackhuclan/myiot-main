using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.BigScreenServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;

namespace VgAutoDrill.Admin.WebApi.Controllers.BigScreenServices
{
    /// <summary>
    /// 大屏接口
    /// </summary>
    public class BigScreenController : BaseController
    {
        private readonly IBigScreenServices _bigScreenServices;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bigScreenServices"></param>
        /// <param name="dataCollectService"></param>
        public BigScreenController(IBigScreenServices bigScreenServices,
            IConfiguration configuration)
        {
            _bigScreenServices = bigScreenServices;
            _configuration = configuration;
        }

        /// <summary>
        ///获取标题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTitle")]
        [ProducesResponseType(typeof(List<string>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetTitle()
        {
            List<string> result = new List<string>();
            if (_configuration["AppConfig:SetTitleLabel1"] != null)
            {
                result.Add(_configuration["AppConfig:SetTitleLabel1"]);
            }
            if (_configuration["AppConfig:SetTitleLabel2"] != null)
            {
                result.Add(_configuration["AppConfig:SetTitleLabel2"]);
            }
            return Ok(result);
        }

        /// <summary>
        ///获取设备统计数据
        ///（设备类型、总数、在线比例）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeviceStats")]
        [ProducesResponseType(typeof(List<DeviceStatsDto>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDeviceStats()
        {
            List<DeviceStatsDto> result = await _bigScreenServices.GetDeviceStats();
            return Ok(result);
        }

        /// <summary>
        ///获取设备告警统计数据
        ///（设备名称、告警日期、告警名称、告警级别）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeviceAlarmStats")]
        [ProducesResponseType(typeof(List<List<string>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDeviceAlarmStats(int deviceId)
        {
            List<List<string>> result = await _bigScreenServices.GetDeviceAlarmStats(deviceId);
            return Ok(result);
        }

        /// <summary>
        ///获取板料追踪统计数据
        ///（板料料号，物料代码，对应设备）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPanelStats")]
        [ProducesResponseType(typeof(List<List<string>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetPanelStats(int deviceId)
        {
            List<List<string>> result = await _bigScreenServices.GetPanelStats(deviceId);
            return Ok(result);
        }

        /// <summary>
        ///获取工单进度统计数据
        ///（工单号，产品名，工单进度）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWorkOrderStats")]
        [ProducesResponseType(typeof(List<List<string>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetWorkOrderStats()
        {
            List<List<string>> result = await _bigScreenServices.GetWorkOrderStats();
            return Ok(result);
        }

        /// <summary>
        ///获取近一周产量统计
        ///（工序、近一周每天产量、合格率）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFeedBackStats")]
        [ProducesResponseType(typeof(FeedBackStatsByTimeDto), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetFeedBackStats()
        {
            FeedBackStatsByTimeDto result = await _bigScreenServices.GetFeedBackStats();
            return Ok(result);
        }

        /// <summary>
        /// 获取生产量统计
        ///（生产目标、实际产量、日进度）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTaskStats")]
        [ProducesResponseType(typeof(TaskStatsByTime), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetTaskStats()
        {
            TaskStatsByTime result = await _bigScreenServices.GetTaskStats();
            return Ok(result);
        }

        /// <summary>
        /// 获取工单统计
        ///（生产增率、工单增率）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWorkOrderRateStats")]
        [ProducesResponseType(typeof(WorkOrderRateStatsDto), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetWorkOrderRateStats()
        {
            WorkOrderRateStatsDto result = await _bigScreenServices.GetWorkOrderRateStats();
            return Ok(result);
        }

        /// <summary>
        /// 获取设备调度日志
        ///（呼叫设备、呼叫时间、响应设备、响应时间）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeviceServiceStatsList")]
        [ProducesResponseType(typeof(List<List<string>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDeviceServiceStatsList()
        {
            List<List<string>> result = new List<List<string>>();
            //await _dataCollectService.GetDeviceServiceStatsList();
            return Ok(result);
        }

        /// <summary>
        /// 获取设备运行情况统计
        ///（运行中、故障中、停用、待机）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeviceStatusStats")]
        [ProducesResponseType(typeof(DeviceStatusStatsDto), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDeviceStatusStats(int deviceId)
        {
            DeviceStatusStatsDto result = await _bigScreenServices.GetDeviceStatusStats(deviceId);
            return Ok(result);
        }

        /// <summary>
        /// 获取设备开机率统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeviceUptimeStats")]
        [ProducesResponseType(typeof(List<List<string>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDeviceUptimeStats(int deviceId)
        {
            List<List<string>> result = await _bigScreenServices.GetDeviceUptimeStats(deviceId);
            return Ok(result);
        }

        /// <summary>
        /// 获取设备稼动率统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeviceMovementStats")]
        [ProducesResponseType(typeof(List<List<string>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDeviceMovementStats(int deviceId)
        {
            List<List<string>> result = await _bigScreenServices.GetDeviceMovementStats(deviceId);
            return Ok(result);
        }

        /// <summary>
        /// 获取设备加工时间统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeviceProcessingStats")]
        [ProducesResponseType(typeof(List<List<string>>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDeviceProcessingStats(int deviceId)
        {
            List<List<string>> result = await _bigScreenServices.GetDeviceProcessingStats(deviceId);
            return Ok(result);
        }

        /// <summary>
        /// 获取设备看板数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeviceDatas")]
        [ProducesResponseType(typeof(List<DeviceDataToScreen>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDeviceDatas()
        {
            var result = await _bigScreenServices.GetDeviceDatas();
            return Ok(result);
        }

        /// <summary>
        /// 获取指定设备的看板数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSpecificDeviceDatas")]
        [ProducesResponseType(typeof(List<DeviceDataToScreen>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetSpecificDeviceDatas(GetDeviceListReq req)
        {
            var result = await _bigScreenServices.GetSpecificDeviceDatas(req);
            return Ok(result);
        }
    }
}
