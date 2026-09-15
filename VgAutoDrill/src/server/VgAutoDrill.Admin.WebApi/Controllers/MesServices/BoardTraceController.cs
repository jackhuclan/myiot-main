using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Panel;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class BoardTraceController : BaseController
    {
        private readonly IPanelService _mainService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="mainService"></param>
        public BoardTraceController(IPanelService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取板料位置列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<PanelDto>>), 200)]
        [PermissionAuthorize("device:boardtrace:list")]
        public async Task<ActionResult> GetList([FromBody] GetPanelListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取板料位置树形列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFullTreeList")]
        [ProducesResponseType(typeof(ResponseDto<List<PanelFullPropertiesTreeDto>>), 200)]
        [PermissionAuthorize("device:boardtrace:list")]
        public async Task<ActionResult> GetFullTreeList([FromBody] GetPanelListReq req)
        {
            var result = await _mainService.GetFullTreeList(req);
            return Ok(result);
        }

        /// <summary>
        ///获取板料位置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<PanelDto>), 200)]
        [PermissionAuthorize("device:boardtrace:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 添加板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:boardtrace:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdatePanelReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }

        /// <summary>
        /// 批量添加板料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GeneratePanels")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:boardtrace:add")]
        public async Task<ActionResult> GeneratePanels([FromBody] BatchInsertPanelReq req)
        {
            var result = await _mainService.GeneratePanels(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改板料位置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<PanelDto>), 200)]
        [PermissionAuthorize("device:boardtrace:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdatePanelReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除板料位置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:boardtrace:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// 删除板料位置集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:boardtrace:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 导出板料追溯Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("device:boardtrace:download")]
        public async Task<ActionResult> DownLoadListAsync([FromBody] GetPanelListReq req)
        {
            List<PanelToExcelDto> list = await _mainService.GetToExcelList(req);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "板料追溯.xlsx");
        }
    }
}