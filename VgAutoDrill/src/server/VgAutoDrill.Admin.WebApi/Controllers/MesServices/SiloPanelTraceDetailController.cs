using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 料仓板料追溯详细
    /// </summary>
    public class SiloPanelTraceDetailController : BaseController
    {
        private readonly ISiloPanelTraceDetailService _siloPanelTraceDetailService;

        public SiloPanelTraceDetailController(ISiloPanelTraceDetailService siloPanelTraceDetailService)
        {
            _siloPanelTraceDetailService = siloPanelTraceDetailService;
        }

        /// <summary>
        /// 获取料仓板料追溯详细记录列表
        /// </summary>
        /// <param name="req">查询请求参数</param>
        /// <returns>详细记录列表</returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<SiloPanelTraceDetailDto>>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTraceDetail:list")]
        public async Task<ActionResult> GetList([FromBody] GetSiloPanelTraceDetailListReq req)
        {
            var result = await _siloPanelTraceDetailService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取料仓板料追溯详细记录详情
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>追溯详细记录详情</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<SiloPanelTraceDetailDto>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTraceDetail:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _siloPanelTraceDetailService.QueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 新增料仓板料追溯详细记录
        /// </summary>
        /// <param name="req">新增请求参数</param>
        /// <returns>新增结果</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTraceDetail:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateSiloPanelTraceDetailReq req)
        {
            var result = await _siloPanelTraceDetailService.Add(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改料仓板料追溯详细记录
        /// </summary>
        /// <param name="req">修改请求参数</param>
        /// <returns>修改结果</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTraceDetail:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateSiloPanelTraceDetailReq req)
        {
            var result = await _siloPanelTraceDetailService.Update(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除料仓板料追溯详细记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>删除结果</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTraceDetail:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _siloPanelTraceDetailService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// 批量删除料仓板料追溯详细记录
        /// </summary>
        /// <param name="idList">记录ID数组</param>
        /// <returns>删除结果</returns>
        [HttpDelete]
        [Route("DeleteList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTraceDetail:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _siloPanelTraceDetailService.DeleteList(idList);
            return Ok(result);
        }


    }
}