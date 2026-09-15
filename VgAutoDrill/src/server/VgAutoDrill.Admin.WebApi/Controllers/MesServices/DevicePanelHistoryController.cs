using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanelHistory;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 设备负载板料历史
    /// </summary>
    public class DevicePanelHistoryController : BaseController
    {
        private readonly IDevicePanelHistoryService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public DevicePanelHistoryController(IDevicePanelHistoryService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取设备负载板料历史列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DevicePanelHistoryDto>>), 200)]
        [PermissionAuthorize("device:devicePanelHistory:list")]
        public async Task<ActionResult> GetList([FromBody] GetDevicePanelHistoryListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取设备负载板料历史列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListByPanel")]
        [ProducesResponseType(typeof(ResponseDto<List<DevicePanelHistoryDto>>), 200)]
        [PermissionAuthorize("device:devicePanelHistory:list")]
        public async Task<ActionResult> GetListByPanel([FromBody] GetDevicePanelHistoryListReq req)
        {
            var result = await _mainService.GetListByPanel(req);
            return Ok(result);
        }

        /// <summary>
        ///获取设备负载板料历史
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DevicePanelHistoryDto>), 200)]
        [PermissionAuthorize("device:devicePanelHistory:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加设备负载板料历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:devicePanelHistory:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDevicePanelHistoryReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改设备负载板料历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DevicePanelHistoryDto>), 200)]
        [PermissionAuthorize("device:devicePanelHistory:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDevicePanelHistoryReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备负载板料历史
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:devicePanelHistory:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备负载板料历史集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:devicePanelHistory:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}