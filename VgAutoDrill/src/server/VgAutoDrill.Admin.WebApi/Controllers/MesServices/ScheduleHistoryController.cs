using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 调度历史记录管理
    /// </summary>
    public class ScheduleHistoryController : BaseController
    {
        private readonly IScheduleService _mainService;

        public ScheduleHistoryController(IScheduleService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取调度记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<ScheduleDto>>), 200)]
        [AllowAnonymous]
        //[PermissionAuthorize("device:schedulement:list")]
        public async Task<ActionResult> GetList([FromBody] GetScheduleListReq req)
        {
            var result = await _mainService.GetHistoryList(req);
            return Ok(result);
        }

        ///// <summary>
        ///// 转移调度历史数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("TransferScheduleHistoryData")]
        //[ProducesResponseType(typeof(ResponseDto<bool>), 200)]
        //[AllowAnonymous]
        //public async Task<ActionResult> TransferScheduleHistoryData(TransferScheduleHistoryDataReq req)
        //{
        //    var result = await _mainService.TransferScheduleHistoryData(req);
        //    return Ok(result);
        //}


        /// <summary>
        ///获取调度记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<ScheduleDto>), 200)]
        //[PermissionAuthorize("device:schedulement:view")]
        [AllowAnonymous]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryHistoryByID(id);
            return Ok(result);
        }

        /// <summary>
        ///获取调度记录明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetScheduleLogs/{id}")]
        [ProducesResponseType(typeof(ResponseDto<ScheduleLogsDto>), 200)]
        [AllowAnonymous]
        //[PermissionAuthorize("device:schedulement:view")]
        public async Task<ActionResult> GetScheduleLogs(long id)
        {
            var result = await _mainService.QueryHistoryLogsByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 导出调度记录Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [AllowAnonymous]
        //[PermissionAuthorize("device:schedulement:download")]
        public async Task<ActionResult> DownLoadListAsync([FromBody] GetScheduleListReq req)
        {
            List<ScheduleToExcelDto> list = await _mainService.GetHisToExcelList(req);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "调度记录.xlsx");
        }
    }
}
