using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanel;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 设备负载板料
    /// </summary>
    public class DevicePanelController : BaseController
    {
        private readonly IDevicePanelService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public DevicePanelController(IDevicePanelService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取设备负载板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DevicePanelDto>>), 200)]
        [PermissionAuthorize("device:devicePanel:list")]
        public async Task<ActionResult> GetList([FromBody] GetDevicePanelListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取设备负载板料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DevicePanelDto>), 200)]
        [PermissionAuthorize("device:devicePanel:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加设备负载板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:devicePanel:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDevicePanelReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改设备负载板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DevicePanelDto>), 200)]
        [PermissionAuthorize("device:devicePanel:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDevicePanelReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备负载板料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:devicePanel:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备负载板料集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:devicePanel:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}
