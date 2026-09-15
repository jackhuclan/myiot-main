using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    public class TransportationTaskController : BaseController
    {
        private readonly ITransportationTaskService _mainService;
        private readonly InnerOptions _innerOptions;
        private readonly IScheduleDomainService _scheduleDomainService;
        private readonly IAPIHelper _apiHelper;

        public TransportationTaskController(IServiceProvider serviceProvider)
        {
            _scheduleDomainService = serviceProvider.GetRequiredService<IScheduleDomainService>();
            _mainService = serviceProvider.GetRequiredService<ITransportationTaskService>();
            _innerOptions = serviceProvider.GetRequiredService<IOptions<InnerOptions>>().Value;
            _apiHelper = serviceProvider.GetRequiredService<IAPIHelper>();
        }

        /// <summary>
        /// 获取料仓任务信息列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<TransferJobDto>>), 200)]
        [AllowAnonymous]
        [PermissionAuthorize("transportationTask:transportationTask:list")]
        public async Task<ActionResult> GetList([FromBody] GetTransferJobListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取料仓任务信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<TransferJobDto>), 200)]
        [PermissionAuthorize("transportationTask:transportationTask:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加料仓任务信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("transportationTask:transportationTask:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateTransferJobReq req)
        {
            if (string.IsNullOrEmpty(req.Code))
            {
                req.Code = Guid.NewGuid().ToString();
            }

            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改料仓任务信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<TransferJobDto>), 200)]
        [PermissionAuthorize("transportationTask:transportationTask:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateTransferJobReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除料仓任务信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("transportationTask:transportationTask:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除料仓任务信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("transportationTask:transportationTask:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        ///获取料仓任务明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTransferJobLog/{id}")]
        [ProducesResponseType(typeof(ResponseDto<TransferJobLogDto>), 200)]
        [PermissionAuthorize("transportationTask:transportationTask:view")]
        public async Task<ActionResult> GetTransferJobLog(long id)
        {
            var result = await _mainService.QueryLogsByID(id);
            return Ok(result);
        }

        [HttpPut]
        [Route("CancelSingle/{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("transportationTask:transportationTask:edit")]
        public async Task<ActionResult> CancelSingle(long id)
        {
            ResponseDto<string> responseDto = new ResponseDto<string>();

            if (string.IsNullOrEmpty(_innerOptions.CancelScheduleUrl))
            {
                responseDto.Code = ResponseCode.Fail;
                responseDto.Message = "没有设置innerOptions.CancelScheduleUrl！";
                return Ok(responseDto);
            }

            var result = await _mainService.CancelSingle(id, true);

            if (string.IsNullOrEmpty(result.Message))
            {
                var job = await _mainService.QueryByID(id);

                if (job != null && job.Data != null)
                {
                    var scheduleCodes = new List<string?>();
                    if (!string.IsNullOrEmpty(job.Data.StartSchedule))
                    {
                        scheduleCodes.Add(job.Data.StartSchedule);
                    }

                    if (!string.IsNullOrEmpty(job.Data.EndSchedule))
                    {
                        scheduleCodes.Add(job.Data.EndSchedule);
                    }

                    if (scheduleCodes.Any())
                    {
                        await CancelScheduleByCentral(responseDto, scheduleCodes);

                        //(bool flowControl, ActionResult? value) = await CancelScheduleByCentral(responseDto, scheduleCodes);
                        //if (!flowControl)
                        //{
                        //    return value;
                        //}
                    }
                }
            }

            return Ok(result);
        }

        private async Task<(bool flowControl, ActionResult? value)> CancelScheduleByCentral(ResponseDto<string> responseDto, List<string?> scheduleCodes)
        {
            foreach (var schedule in scheduleCodes)
            {
                var request = new CancelScheduleTaskRequest
                {
                    TraceId = schedule,
                    Params = new Dictionary<string, object?>
                            {
                                { "CancelReason", "取消料仓任务后，级联取消调度记录" },
                                { "IsForced",true }
                            }
                };

                string response = _apiHelper.RequestData(_innerOptions.CancelScheduleUrl, "post", JsonSerializer.Serialize(request));
                if (string.IsNullOrEmpty(response) || !response.Contains("SUCCESS"))
                {
                    //responseDto.Code = ResponseCode.Fail;
                    //responseDto.Message = response ?? "中控接口没有返回信息";
                    //return (flowControl: false, value: Ok(responseDto));
                }
            }

            return (flowControl: true, value: null);
        }

        /// <summary>
        /// 批量取消
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BulkCancel")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        public async Task<ActionResult> BulkCancel([FromBody] List<long> ids)
        {
            ResponseDto<string> responseDto = new ResponseDto<string>();

            if (string.IsNullOrEmpty(_innerOptions.CancelScheduleUrl))
            {
                responseDto.Code = ResponseCode.Fail;
                responseDto.Message = "没有设置innerOptions.CancelScheduleUrl！";
                return Ok(responseDto);
            }

            var result = await _mainService.BulkCancel(ids, true);

            if (string.IsNullOrEmpty(result.Message))
            {
                var jobs = await _mainService.QueryAsync(x => ids.Contains(x.Id), x => x.Id, SqlSugar.OrderByType.Asc);
                var startSchedules = jobs.Where(x => !string.IsNullOrEmpty(x.StartSchedule)).Select(x => x.StartSchedule).ToList();
                var endSchedules = jobs.Where(x => !string.IsNullOrEmpty(x.EndSchedule)).Select(x => x.EndSchedule).ToList();
                var scheduleCodes = new List<string?>();
                scheduleCodes.AddRange(startSchedules);
                scheduleCodes.AddRange(endSchedules);

                if (scheduleCodes.Any())
                {
                    await CancelScheduleByCentral(responseDto, scheduleCodes);

                    //(bool flowControl, ActionResult? value) = await CancelScheduleByCentral(responseDto, scheduleCodes);
                    //if (!flowControl)
                    //{
                    //    return value;
                    //}
                }
            }
            return Ok(result);
        }

        /// <summary>
        /// 获取料仓历史任务信息列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetHistoryList")]
        [ProducesResponseType(typeof(ResponseDto<List<TransferJobDto>>), 200)]
        public async Task<ActionResult> GetHistoryList([FromBody] GetTransferJobListReq req)
        {
            var result = await _mainService.GetHistoryList(req);
            return Ok(result);
        }

        /// <summary>
        ///获取料仓任务信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetHistory/{id}")]
        [ProducesResponseType(typeof(ResponseDto<TransferJobDto>), 200)]
        public async Task<ActionResult> GetHistory(long id)
        {
            var result = await _mainService.GetHistory(id);
            return Ok(result);
        }

        /// <summary>
        /// 导出料仓任务记录Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        public async Task<ActionResult> DownLoadListAsync([FromBody] GetTransferJobListReq req)
        {
            var list = await _mainService.GetToExcelList(req);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "料仓任务记录.xlsx");
        }

        /// <summary>
        /// 导出料仓任务历史记录Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DownLoadHistoryList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> DownLoadHistoryList([FromBody] GetTransferJobListReq req)
        {
            var list = await _mainService.GetToHistoryExcelList(req);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "料仓任务历史记录.xlsx");
        }
    }
}

