using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 料仓板料追溯
    /// </summary>
    public class SiloPanelTraceController : BaseController
    {
        private readonly ISiloPanelTraceService _siloPanelTraceService;

        public SiloPanelTraceController(ISiloPanelTraceService siloPanelTraceService)
        {
            _siloPanelTraceService = siloPanelTraceService;
        }

        /// <summary>
        /// 查询料仓板料追溯记录
        /// </summary>
        /// <param name="req">查询请求参数</param>
        /// <returns>符合条件的追溯记录列表</returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<SiloPanelTraceDto>>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTrace:list")]
        public async Task<ActionResult> GetList([FromBody] GetSiloPanelTraceListReq req)
        {
            var result = await _siloPanelTraceService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取料仓板料追溯记录详情
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>追溯记录详情</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<SiloPanelTraceDto>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTrace:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _siloPanelTraceService.QueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 新增料仓板料追溯记录
        /// </summary>
        /// <param name="req">新增请求参数</param>
        /// <returns>新增结果</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTrace:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateSiloPanelTraceReq req)
        {
            var result = await _siloPanelTraceService.AddAndReturnId(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改料仓板料追溯记录
        /// </summary>
        /// <param name="req">修改请求参数</param>
        /// <returns>修改结果</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTrace:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateSiloPanelTraceReq req)
        {
            var result = await _siloPanelTraceService.Update(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除料仓板料追溯记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>删除结果</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTrace:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _siloPanelTraceService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// 批量删除料仓板料追溯记录
        /// </summary>
        /// <param name="idList">记录ID数组</param>
        /// <returns>删除结果</returns>
        [HttpDelete]
        [Route("DeleteList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTrace:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _siloPanelTraceService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 从PanelList添加料仓板料追溯记录
        /// </summary>
        /// <param name="request">从PanelList添加的请求参数</param>   
        /// <returns>添加结果</returns>
        [HttpPost]
        [Route("AddFromPanelList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTrace:add")]
        public async Task<ActionResult> AddFromPanelList([FromBody] AddSiloPanelTraceFromPanelListReq request)
        {
            // 将请求转换为Panel列表并调用新的方法
            var result = await _siloPanelTraceService.AddFromPanelList(request.Panels, request.Subject, request.ScheduleId, request.TransportationTaskId);
            return Ok(result);
        }

        /// <summary>
        /// 导出料仓板料追溯Excel
        /// </summary>
        /// <param name="req">查询请求参数</param>
        /// <returns>Excel文件</returns>
        [HttpPost]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("warehouse:siloPanelTrace:download")]
        public async Task<ActionResult> DownLoadListAsync([FromBody] GetSiloPanelTraceListReq req)
        {
            List<SiloPanelTraceToExcelDto> list = await _siloPanelTraceService.GetToExcelList(req);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "料仓板料追溯.xlsx");
        }

        /// <summary>
        /// 清理指定位置的缓存
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns>清理结果</returns>
        [HttpPost]
        [Route("ClearCache")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTrace:list")]
        public async Task<ActionResult> ClearCache([FromBody] string location)
        {
            await _siloPanelTraceService.ClearLocationCacheAsync(location);
            var result = new ResponseDto<string>
            {
                Code = ResponseCode.Success,
                Message = "",
                Data = "缓存清理完成"
            };
            return Ok(result);
        }

        /// <summary>
        /// 获取缓存信息
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns>缓存信息</returns>
        [HttpGet]
        [Route("GetCacheInfo")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("warehouse:siloPanelTrace:list")]
        public async Task<ActionResult> GetCacheInfo(string location)
        {
            var cacheInfo = await _siloPanelTraceService.GetCacheInfoAsync(location);
            var result = new ResponseDto<string>
            {
                Code = ResponseCode.Success,
                Message = "",
                Data = cacheInfo
            };
            return Ok(result);
        }

    }
}
