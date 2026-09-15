using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndPanel;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    public class WorkOrderAndPanelController : BaseController
    {
        public readonly IWorkOrderAndPanelService _workOrderAndPanelService;
        public WorkOrderAndPanelController(IWorkOrderAndPanelService workOrderAndPanelService)
        {
            _workOrderAndPanelService = workOrderAndPanelService;
        }

        /// <summary>
        /// 新增工单板材信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddWorkOrderAndPanelList")]
        [ProducesResponseType(typeof(ResponseDto<List<string>>), 200)]
        [PermissionAuthorize("produce:WorkOrderAndPanel:list")]
        public async Task<ActionResult> AddWorkOrderAndPanelList([FromBody] List<AddOrUpdateWorkOrderAndPanelReq> req)
        {
            var result = await _workOrderAndPanelService.AddWorkOrderAndPanelList(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改工单板材信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateWorkOrderAndPanelList")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkOrderAndPanelDto>>), 200)]
        [PermissionAuthorize("produce:WorkOrderAndPanel:list")]
        public async Task<ActionResult> UpdateWorkOrderAndPanelList([FromBody] List<AddOrUpdateWorkOrderAndPanelReq> req)
        {
            var result = await _workOrderAndPanelService.UpdateWorkOrderAndPanelList(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除工单板材信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteList")]
        [ProducesResponseType(typeof(ResponseDto<List<string>>), 200)]
        [PermissionAuthorize("produce:WorkOrderAndPanel:list")]
        public async Task<ActionResult> DeleteList([FromBody] List<string> req)
        {
            var result = await _workOrderAndPanelService.DeleteList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取工单板材信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetWorkOrderAndPanelList")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkOrderAndPanelDto>>), 200)]
        [PermissionAuthorize("produce:WorkOrderAndPanel:list")]
        public async Task<ActionResult> GetWorkOrderAndPanelList([FromBody] GetWorkOrderAndPanelReq req)
        {
            var result = await _workOrderAndPanelService.GetWorkOrderAndPanelList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取工单板材信息List
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetWorkOrderAndPanelInfors")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkOrderAndPanelDto>>), 200)]
        [PermissionAuthorize("produce:WorkOrderAndPanel:list")]
        public async Task<ActionResult> GetWorkOrderAndPanelInfors([FromBody] GetWorkOrderAndPanelInforsReq req)
        {
            var result = await _workOrderAndPanelService.GetWorkOrderAndPanelInfors(req);
            return Ok(result);
        }
    }
}
