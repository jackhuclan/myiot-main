using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.NotificationRecord;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class NotifyController : BaseController
    {
        private readonly INotifyService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public NotifyController(INotifyService mainService)
        {
            _mainService = mainService;
        }


        /// <summary>
        /// 获取通知记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<NotifyDto>>), 200)]
        [PermissionAuthorize("notification:informrecord:list")]
        public async Task<ActionResult> GetList([FromBody] GetNotifyListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取通知记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<NotifyDto>), 200)]
        [PermissionAuthorize("notification:informrecord:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加通知记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("notification:informrecord:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateNotifyReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改通知记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<NotifyDto>), 200)]
        [PermissionAuthorize("notification:informrecord:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateNotifyReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除通知记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("notification:informrecord:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除通知记录集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("notification:informrecord:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 导出通知记录Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("notification:informrecord:download")]
        public async Task<ActionResult> DownLoadListAsync()
        {
            List<NotifyToExcelDto> list = new List<NotifyToExcelDto>();

            var result = await _mainService.GetList(new GetNotifyListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            if (result != null && result.Data.List != null)
            {
                foreach (var item in result.Data.List)
                {
                    NotifyToExcelDto model = new NotifyToExcelDto();
                    model.NotifyCode = item.NotifyCode;
                    model.NotifyWaysName = item.NotifyWaysName;
                    model.NotifyMsg = item.NotifyMsg;
                    model.NotifyTime = item.NotifyTime;
                    model.NotifyName = item.NotifyName;

                    list.Add(model);
                }
            }

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "通知记录.xlsx");
        }
    }
}
