using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AlarmController : BaseController
    {
        private readonly IAlarmService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public AlarmController(IAlarmService mainService)
        {
            _mainService = mainService;
        }


        /// <summary>
        /// 获取告警记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<AlarmDto>>), 200)]
        [PermissionAuthorize("alarm:alarmrecord:list")]
        public async Task<ActionResult> GetList([FromBody] GetAlarmListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取告警记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<AlarmDto>), 200)]
        [PermissionAuthorize("alarm:alarmrecord:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加告警记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("alarm:alarmrecord:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateAlarmReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改告警记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<AlarmDto>), 200)]
        [PermissionAuthorize("alarm:alarmrecord:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateAlarmReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除告警记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("alarm:alarmrecord:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除告警记录集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("alarm:alarmrecord:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 手动触发S开头外部工单告警检查（用于测试）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ManualCheckSampleOrderAlarms")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("alarm:alarmrecord:add")] // 恢复权限控制
        public async Task<ActionResult> ManualCheckSampleOrderAlarms()
        {
            try
            {
                // 检查是否启用SampleOrder告警功能
                var innerOptions = HttpContext.RequestServices.GetRequiredService<IOptions<VgAutoDrill.Admin.Common.Configuration.InnerOptions>>();
                if (!innerOptions.Value.EnableSampleOrderAlarm)
                {
                    return Ok(new ResponseDto<string>
                    {
                        Code = ResponseCode.Success,
                        Message = "SampleOrder告警功能已禁用，跳过检查",
                        Data = $"执行时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}, 功能状态: 已禁用"
                    });
                }

                int newAlarmCount = await _mainService.CheckSampleOrderAlarms();

                string message = newAlarmCount > 0
                    ? $"S开头外部工单告警检查执行成功，新增了 {newAlarmCount} 条告警记录"
                    : "S开头外部工单告警检查执行成功，没有新增告警记录";

                return Ok(new ResponseDto<string>
                {
                    Code = ResponseCode.Success,
                    Message = message,
                    Data = $"执行时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}, 新增告警数量: {newAlarmCount}"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseDto<string>
                {
                    Code = ResponseCode.Fail,
                    Message = $"告警检查执行失败: {ex.Message}",
                    Data = null
                });
            }
        }

        /// <summary>
        /// 导出告警记录Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("alarm:alarmrecord:download")]
        public async Task<ActionResult> DownLoadListAsync([FromBody] GetAlarmListReq req)
        {
            List<AlarmToExcelDto> list = await _mainService.GetToExcelList(req);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "告警记录.xlsx");
        }
    }
}
