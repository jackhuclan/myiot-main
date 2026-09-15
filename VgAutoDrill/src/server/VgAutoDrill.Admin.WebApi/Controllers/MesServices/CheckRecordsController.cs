using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CheckRecords;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 检验记录
    /// </summary>
    public class CheckRecordsController : BaseController
    {
        private readonly ICheckRecordsService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public CheckRecordsController(ICheckRecordsService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取检验记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<CheckRecordsDto>>), 200)]
        [PermissionAuthorize("produce:checkRecords:list")]
        public async Task<ActionResult> GetList([FromBody] GetCheckRecordsListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取检验记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<CheckRecordsDto>), 200)]
        [PermissionAuthorize("produce:checkRecords:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加检验记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:checkRecords:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateCheckRecordsReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改检验记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<CheckRecordsDto>), 200)]
        [PermissionAuthorize("produce:checkRecords:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateCheckRecordsReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改检验记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("PutCheck")]
        [ProducesResponseType(typeof(ResponseDto<CheckRecordsDto>), 200)]
        [PermissionAuthorize("produce:checkRecords:edit")]
        public async Task<ActionResult> PutCheck([FromBody] UpdateCheckRecordsOkReq req)
        {
            var result = await _mainService.UpdateCheck(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除检验记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:checkRecords:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除检验记录集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:checkRecords:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 导出检验记录Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("produce:checkRecords:download")]
        public async Task<ActionResult> DownLoadListAsync()
        {
            List<CheckRecordsToExcelDto> list = new List<CheckRecordsToExcelDto>();

            var result = await _mainService.GetList(new GetCheckRecordsListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            if (result != null && result.Data.List != null)
            {
                foreach (var item in result.Data.List)
                {
                    CheckRecordsToExcelDto model = new CheckRecordsToExcelDto();
                    model.Remark = item.Remark;
                    model.TaskCode = item.TaskCode;
                    model.TaskName = item.TaskName;
                    model.UserName = item.UserName;
                    model.WorkOrderName = item.WorkOrderName;
                    model.WorkOrderCode = item.WorkOrderCode;
                    model.CheckTime = item.CheckTime;
                    model.IsCheckOk = item.IsCheckOk;

                    list.Add(model);
                }
            }

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "检验记录.xlsx");
        }
    }
}
