using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ScheduleHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 调度记录管理
    /// </summary>
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService _mainService;
        private readonly IScheduleDomainService _mainDomainService;
        private readonly IConfiguration _configuration;
        private readonly IAPIHelper _apiHelper;
        private readonly InnerOptions _innerOptions;

        public ScheduleController(IScheduleService mainService,
            IScheduleDomainService mainDomainService,
            IOptions<InnerOptions> options,
            IAPIHelper apiHelper,
            IConfiguration configuration)
        {
            _mainService = mainService;
            _apiHelper = apiHelper;
            _mainDomainService = mainDomainService;
            _configuration = configuration;
            _innerOptions = options.Value;
        }

        /// <summary>
        /// 获取调度记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<ScheduleDto>>), 200)]
        [PermissionAuthorize("device:schedulement:list")]
        public async Task<ActionResult> GetList([FromBody] GetScheduleListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取调度记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<ScheduleDto>), 200)]
        [PermissionAuthorize("device:schedulement:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
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
        [PermissionAuthorize("device:schedulement:view")]
        public async Task<ActionResult> GetScheduleLogs(long id)
        {
            var result = await _mainService.QueryLogsByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 添加调度记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:schedulement:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateScheduleReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改调度记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<ScheduleDto>), 200)]
        [PermissionAuthorize("device:schedulement:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateScheduleReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除调度记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:schedulement:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除调度记录集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:schedulement:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 批量取消调度记录
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("BulkCanceled")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:schedulement:update")]
        public async Task<ActionResult> BulkCanceled(List<long> idList)
        {
            ResponseDto<string> responseDto = new ResponseDto<string>();
            if (string.IsNullOrEmpty(_innerOptions.CancelScheduleUrl))
            {
                responseDto.Code = ResponseCode.Fail;
                responseDto.Message = "没有设置innerOptions.CancelScheduleUrl！";
                return Ok(responseDto);
            }

            var schedules = await _mainDomainService.QueryAsync(x => idList.Contains(x.Id), x => x.Id, SqlSugar.OrderByType.Asc);
            foreach (var schedule in schedules)
            {
                var request = new CancelScheduleTaskRequest
                {
                    TraceId = schedule.Code,
                    Params = new Dictionary<string, object?>
                        {
                            { "CancelReason", "调度记录页面，批量取消" }
                        }
                };
                string result = _apiHelper.RequestData(_innerOptions.CancelScheduleUrl, "post", JsonSerializer.Serialize(request));
                if (string.IsNullOrEmpty(result) || !result.Contains("SUCCESS"))
                {
                    responseDto.Code = ResponseCode.Fail;
                    responseDto.Message = result ?? "中控接口没有返回信息";
                    return Ok(responseDto);
                }
            }

            responseDto.Code = ResponseCode.Success;
            responseDto.Message = string.Empty;
            return Ok(responseDto);
        }

        /// <summary>
        /// 取消调度记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateCentralTask")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:schedulement:update")]
        public async Task<ActionResult> UpdateCentralTask([FromBody] AddOrUpdateScheduleReq req)
        {
            ResponseDto<string> responseDto = new ResponseDto<string>();

            if (string.IsNullOrEmpty(_innerOptions.CancelScheduleUrl))
            {
                responseDto.Code = ResponseCode.Fail;
                responseDto.Message = "没有设置innerOptions.CancelScheduleUrl！";
                return Ok(responseDto);
            }

            var request = new CancelScheduleTaskRequest
            {
                TraceId = req.Code,
                Params = new Dictionary<string, object?>
                    {
                        { "CancelReason", "调度记录页面，单条取消" }
                    }
            };
            string result = _apiHelper.RequestData(_innerOptions.CancelScheduleUrl, "post", JsonSerializer.Serialize(request));
            if (string.IsNullOrEmpty(result) || !result.Contains("SUCCESS"))
            {
                responseDto.Code = ResponseCode.Fail;
                responseDto.Message = result ?? "中控接口没有返回信息";
                return Ok(responseDto);
            }

            responseDto.Code = ResponseCode.Success;
            responseDto.Message = result;
            return Ok(responseDto);
        }

        #region UpdateRepublish
        ///// <summary>
        ///// UpdateRepublish
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("UpdateRepublish")]
        //[ProducesResponseType(typeof(ResponseDto<string>), 200)]
        //[PermissionAuthorize("device:schedulement:updateRepublish")]
        //public async Task<ActionResult> UpdateRepublish([FromBody] AddOrUpdateScheduleReq req)
        //{
        //    if (req == null)
        //    {
        //        return BadRequest();
        //    }
        //    ResponseDto<string> responseDto = new ResponseDto<string>();
        //    if (!await _mainDomainService.IsExistAsync(p => p.Code == req.Code))
        //    {
        //        responseDto.Code = ResponseCode.Fail;
        //        responseDto.Message = "信息不存在！";
        //        return Ok(responseDto);
        //    }
        //    var model = await _mainDomainService.FindSingleAsync(p => p.Code == req.Code);
        //    if (model == null)
        //    {
        //        responseDto.Code = ResponseCode.Fail;
        //        responseDto.Message = "信息不存在！";
        //        return Ok(responseDto);
        //    }

        //    if (model.NeedRepublish == 1)
        //    {
        //        responseDto.Code = ResponseCode.Fail;
        //        responseDto.Message = "任务已发布，不能重复发布！";
        //        return Ok(responseDto);
        //    }

        //    if (string.IsNullOrEmpty(model.RequestJson))
        //    {
        //        responseDto.Code = ResponseCode.Fail;
        //        responseDto.Message = "调度请求数据为空！";
        //        return Ok(responseDto);
        //    }

        //    string urlAddress = _configuration["AppConfig:EventReportAddress"];
        //    if (string.IsNullOrEmpty(urlAddress))
        //    {
        //        responseDto.Code = ResponseCode.Fail;
        //        responseDto.Message = "未配置EventReportAddress！";
        //        return Ok(responseDto);
        //    }

        //    string result = _apiHelper.RequestData(urlAddress, "post", model.RequestJson);

        //    if (string.IsNullOrEmpty(result) || !result.Contains("SUCCESS"))
        //    {
        //        responseDto.Code = ResponseCode.Fail;
        //        responseDto.Message = result;
        //        return Ok(responseDto);
        //    }

        //    model.NeedRepublish = 0;
        //    model.ModifyTime = DateTime.Now;
        //    await _mainDomainService.Update(model);

        //    responseDto.Code = ResponseCode.Success;
        //    responseDto.Message = result;
        //    return Ok(responseDto);
        //}
        #endregion UpdateRepublish

        /// <summary>
        /// 调度记录设置为紧急
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SetScheduleUrgent")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:schedulement:update")]
        public async Task<ActionResult> SetScheduleUrgent(long id)
        {
            ResponseDto<string> responseDto = new ResponseDto<string>();

            if (string.IsNullOrEmpty(_innerOptions.SetScheduleUrgentUrl))
            {
                responseDto.Code = ResponseCode.Fail;
                responseDto.Message = "没有设置innerOptions.SetScheduleUrgentUrl！";
                return Ok(responseDto);
            }

            var result = _apiHelper.RequestData(string.Format(_innerOptions.SetScheduleUrgentUrl, id));

            if (result == null)
            {
                responseDto.Code = ResponseCode.Fail;
                responseDto.Message = "连接中控系统失败！";
                return Ok(responseDto);
            }
            else if (result == string.Empty)
            {
                responseDto.Code = ResponseCode.Success;
                responseDto.Message = result;
                return Ok(responseDto);
            }
            else
            {
                responseDto.Code = ResponseCode.Fail;
                responseDto.Message = $"中控反馈失败：{result}";
                return Ok(responseDto);
            }
        }

        /// <summary>
        /// 导出调度记录Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("device:schedulement:download")]
        public async Task<ActionResult> DownLoadListAsync([FromBody] GetScheduleListReq req)
        {
            List<ScheduleToExcelDto> list = await _mainService.GetToExcelList(req);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "调度记录.xlsx");
        }

        /// <summary>
        /// 转移调度历史数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("TransferScheduleHistoryData")]
        [ProducesResponseType(typeof(ResponseDto<bool>), 200)]
        public async Task<ActionResult> TransferScheduleHistoryData(TransferScheduleHistoryDataReq req)
        {
            var result = await _mainService.TransferScheduleHistoryData(req);
            return Ok(result);
        }
    }
}