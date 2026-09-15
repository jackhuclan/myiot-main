using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceGateway;
using VgAutoDrill.Admin.WebApi.Controllers.BigScreenServices;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    public class DeviceGatewayController : BaseController
    {
        private readonly IDeviceGatewayService _deviceGatewayService;
        private readonly ILogger<DeviceScheduleController> logger;
        public DeviceGatewayController(IDeviceGatewayService deviceGatewayService,
             ILoggerFactory loggerFactory)
        {
            _deviceGatewayService = deviceGatewayService;
            logger = loggerFactory.CreateLogger<DeviceScheduleController>();

        }

        /// <summary>
        /// 设备网关查询ById
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceGatewayDto>>), 200)]
        [PermissionAuthorize("produce:DeviceGateway:remove")]
        public async Task<ActionResult> Get(long id)
        {
            logger.LogInformation($"设备网关添加开始 " + DateTime.Now);
            var result = await _deviceGatewayService.QueryByID(id);
            logger.LogInformation($"设备网关添加结束 " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 设备网关查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceGateway")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceGatewayDto>>), 200)]
        [PermissionAuthorize("produce:DeviceGateway:list")]

        public async Task<ActionResult> GetDeviceGateway([FromBody] DeviceGatewayReq req)
        {
            logger.LogInformation($"设备网关查询开始 " + DateTime.Now);
            var result = await _deviceGatewayService.DeviceGatewayList(req);
            logger.LogInformation($"设备网关查询结束 " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 设备网关添加或者更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceGatewayAddOrUpdate")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceGatewayDto>>), 200)]
        [PermissionAuthorize("produce:GetDeviceGatewayAddOrUpdate:list")]

        public async Task<ActionResult> GetDeviceGatewayAddOrUpdate([FromBody] AddOrUpdateDeviceGatewayReq req)
        {
            logger.LogInformation($"设备网关查询开始 " + DateTime.Now);
            var result = await _deviceGatewayService.DeviceGatewayAddOrUpdate(req);
            logger.LogInformation($"设备网关查询结束 " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 设备网关数据添加
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddDeviceGateway")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceGatewayDto>>), 200)]
        [PermissionAuthorize("produce:DeviceGateway:add")]
        public async Task<ActionResult> AddDeviceGateway([FromBody] AddOrUpdateDeviceGatewayReq req)
        {
            logger.LogInformation($"设备网关添加开始 " + DateTime.Now);
            var result = await _deviceGatewayService.AddDeviceGateway(req);
            logger.LogInformation($"设备网关添加结束 " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 设备网关删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceGatewayDto>>), 200)]
        [PermissionAuthorize("produce:DeviceGateway:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            logger.LogInformation($"设备网关删除开始 " + DateTime.Now);
            var result = await _deviceGatewayService.Delete(id);
            logger.LogInformation($"设备网关删除结束 " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 设备网关修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateDeviceGateway")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceGatewayDto>>), 200)]
        [PermissionAuthorize("produce:DeviceGateway:edit")]
        public async Task<ActionResult> UpdateDeviceGateway([FromBody] AddOrUpdateDeviceGatewayReq req)
        {
            logger.LogInformation($"设备网关添加开始 " + DateTime.Now);
            var result = await _deviceGatewayService.UpdateDeviceGateway(req);
            logger.LogInformation($"设备网关添加结束 " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 设备网关版本获取
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVerson")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceGatewayDto>>), 200)]
        [PermissionAuthorize("produce:DeviceGateway:list")]
        public async Task<ActionResult> GetVerson([FromBody] DeviceGatewayReq req)
        {
            logger.LogInformation($"设备网关版本获取 " + DateTime.Now);
            var result = await _deviceGatewayService.GetVerson(req);
            logger.LogInformation($"设备网关版本获取 " + DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        /// 程序更新或者安装
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InstallPackages")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceGatewayDto>>), 200)]
        [PermissionAuthorize("produce:DeviceGateway:list")]
        public async Task<ActionResult> InstallPackages([FromBody] DeviceGatewayReq req)
        {
            logger.LogInformation($"设备网关查询开始 " + DateTime.Now);
            var result = await _deviceGatewayService.InstallPackages(req);
            logger.LogInformation($"设备网关查询结束 " + DateTime.Now);
            return Ok(result);
        }
    }
}
