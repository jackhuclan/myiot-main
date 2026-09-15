using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceEvent;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AlarmEventController : BaseController
    {
        private readonly IEventDefineService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public AlarmEventController(IEventDefineService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取事件定义列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<EventDefineDto>>), 200)]
        [PermissionAuthorize("alarm:event:list")]
        public async Task<ActionResult> GetList([FromBody] GetEventDefineListReq req)
        {
            var result = await _mainService.GetDeviceEventList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取事件定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<EventDefineDto>), 200)]
        [PermissionAuthorize("alarm:event:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加事件定义
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("alarm:event:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateEventDefineReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改事件定义
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<EventDefineDto>), 200)]
        [PermissionAuthorize("alarm:event:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateEventDefineReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除事件定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("alarm:event:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除事件定义集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("alarm:event:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 导出事件数据Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("alarm:event:download")]
        public async Task<ActionResult> DownLoadListAsync([FromBody] GetEventDefineListReq req)
        {
            List<EventDefineToExcelDto> list = new List<EventDefineToExcelDto>();

            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 1000;

            var result = await _mainService.GetDeviceEventList(req);

            if (result != null && result.Data.List != null)
            {
                foreach (var item in result.Data.List)
                {
                    EventDefineToExcelDto model = new EventDefineToExcelDto();
                    model.EventLevel = item.EventLevel;
                    model.EventName = item.EventName;
                    model.EventId = item.EventId;
                    model.ParameterJson = item.ParameterJson;

                    list.Add(model);
                }
            }

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "事件数据.xlsx");
        }
    }
}
