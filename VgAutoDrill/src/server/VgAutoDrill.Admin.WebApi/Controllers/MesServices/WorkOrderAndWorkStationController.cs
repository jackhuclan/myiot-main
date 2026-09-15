using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndWorkStation;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 生产工单和工作站关联关系
    /// </summary>
    public class WorkOrderAndWorkStationController : BaseController
    {
        private readonly IWorkOrderAndWorkStationService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public WorkOrderAndWorkStationController(IWorkOrderAndWorkStationService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取生产工单与工作站关系列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkOrderAndWorkStationDto>>), 200)]
        [PermissionAuthorize("produce:workOrderAndWorkStation:list")]
        public async Task<ActionResult> GetList([FromBody] GetWorkOrderAndWorkStationListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取生产工单与工作站关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<WorkOrderAndWorkStationDto>), 200)]
        [PermissionAuthorize("produce:workOrderAndWorkStation:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加/修改生产工单与工作站关系
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workOrderAndWorkStation:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDataReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除生产工单与工作站关系集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workOrderAndWorkStation:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }
    }
}