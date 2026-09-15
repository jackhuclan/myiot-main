using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 设备与工艺路线关联
    /// </summary>
    public class DeviceAndRouteController : BaseController
    {
        private readonly IDeviceAndRouteService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public DeviceAndRouteController(IDeviceAndRouteService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取设备与工艺路线关联列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceInfoAndRouteInfo>>), 200)]
        [PermissionAuthorize("device:deviceAndRoute:list")]
        public async Task<ActionResult> GetList([FromBody] GetDeviceAndRouteListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 根据设备获取工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetInfosByDevice")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceAndRouteDto>>), 200)]
        [PermissionAuthorize("device:deviceAndRoute:list")]
        public async Task<ActionResult> GetInfosByDevice([FromBody] GetDeviceAndRouteListReq req)
        {
            var result = await _mainService.GetInfosByDevice(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取调度配置设备列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceAndRouteList")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceFullDataAndRouteInfo>>), 200)]
        [PermissionAuthorize("device:deviceAndRoute:list")]
        public async Task<ActionResult> GetDeviceAndRouteList([FromBody] GetDeviceFullDataAndRouteInfoReq req)
        {
            var result = await _mainService.GetDeviceAndRouteList(req);
            return Ok(result);
        }

        /// <summary>
        ///获取设备与工艺路线关联
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DeviceAndRouteDto>), 200)]
        [PermissionAuthorize("device:deviceAndRoute:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加设备与工艺路线关联
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:deviceAndRoute:add")]
        public async Task<ActionResult> Post([FromBody] AddDeviceAndRouteListReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改设备与工艺路线关联
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DeviceAndRouteDto>), 200)]
        [PermissionAuthorize("device:deviceAndRoute:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDeviceAndRouteReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备与工艺路线关联
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:deviceAndRoute:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备与工艺路线关联集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:deviceAndRoute:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}
