using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceParameter;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 参数管理
    /// </summary>
    public class DeviceParameterController : BaseController
    {
        private readonly IDeviceParameterService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public DeviceParameterController(IDeviceParameterService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取设备参数列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceParameterDto>>), 200)]
        [PermissionAuthorize("device:deviceParameter:list")]
        public async Task<ActionResult> GetList([FromBody] GetDeviceParameterListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取设备参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DeviceParameterDto>), 200)]
        [PermissionAuthorize("device:deviceParameter:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加设备参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:deviceParameter:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDeviceParameterReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改设备参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DeviceParameterDto>), 200)]
        [PermissionAuthorize("device:deviceParameter:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDeviceParameterReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:deviceParameter:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备参数集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:deviceParameter:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}