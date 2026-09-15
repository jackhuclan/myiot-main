using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VegaIot.External.Hik;

/// <summary>
/// 中控上报工具类
/// </summary>
public class CentralReporter : DeviceShare<HikTransfer>
{
    private readonly ILogger<CentralReporter> _logger;
    public int uploadTaskStatusQty = 3;

    public CentralReporter(IServiceProvider serviceProvider,
        ILoggerFactory loggerFactory,
        HikTransfer device) : base(serviceProvider, device)
    {
        _logger = loggerFactory.CreateLogger<CentralReporter>();
    }

    /// <summary>
    /// 上报任务状态
    /// </summary>
    /// <param name="deviceServiceInvokeRequest"></param>
    /// <param name="status"></param>
    /// <param name="currentContext"></param>
    /// <returns></returns>
    public async Task<BaseResponse> ReportTaskStatus(ScheduledTaskStatus status, CurrentContext currentContext)
    {
        BaseResponse response = new BaseResponse();
        response.Code = 1;
        response.Message = $"任务状态上传失败";

        try
        {
            ServiceRequest ReportData = new ServiceRequest();
            ReportData.ProductId = "";
            ReportData.DeviceId = currentContext.LocationCode;
            ReportData.TraceId = currentContext.TransferJobCode;
            ReportData.Params["ScheduledStatus"] = status;
            ReportData.Params["TaskCode"] = currentContext.HikTaskCode;
            ReportData.Params["AGVCode"] = currentContext.AGVCode;

            string queryDataUrl = "";

            _logger.LogInformation($"库位{currentContext.LocationCode}的任务currentContext.TransferJobCode={currentContext.TransferJobCode}," +
                $"currentContext.InternalLotNo={currentContext.InternalLotNo}上报中控任务状态{status.ToString()}");

            if (status == ScheduledTaskStatus.Running)
            {
                ReportData.Params["ExternalLotNo"] = currentContext.ExternalLotNo == null ? "" : currentContext.ExternalLotNo;
                queryDataUrl = InteractingDevice.DeviceDescriptor.Extra["BeginReportTask"].ToString();//开始执行任务;
            }
            else if (status == ScheduledTaskStatus.Canceled)
            {
                ReportData.Params["ExternalLotNo"] = currentContext.ExternalLotNo == null ? "" : currentContext.ExternalLotNo;
                queryDataUrl = InteractingDevice.DeviceDescriptor.Extra["CanceleReportTask"].ToString();//取消任务;
            }
            else if (status == ScheduledTaskStatus.Failed)
            {
                _logger.LogInformation($"ReportTaskStatus Fail: 延迟 {InteractingDevice.DeviceDescriptor.Extra["ReportFailInternal"].ToInt()}秒上报");
                //await Task.Delay(InteractingDevice.DeviceDescriptor.Extra["ReportFailInternal"].ToInt() * 1000);

                if (currentContext.HikTaskRequest.Params.ContainsKey("errMsg"))
                {
                    ReportData.Params["errMsg"] = currentContext.HikTaskRequest.Params["errMsg"].ToString();
                }

                queryDataUrl = InteractingDevice.DeviceDescriptor.Extra["FailReportTask"].ToString();//执行任务失败;
            }
            else if (status == ScheduledTaskStatus.Completed)
            {
                if (currentContext.operationType == 4)//上生料
                {
                    ReportData.Params["RawCount"] = currentContext.RawPanelCount;
                    ReportData.Params["ExternalLotNo"] = currentContext.ExternalLotNo;
                }
                ReportData.Params["SiloCode"] = currentContext.SiloCode;
                queryDataUrl = InteractingDevice.DeviceDescriptor.Extra["CompleteReportTask"].ToString();//完成任务;
            }
            else
            {
                response.Code = 1;
                response.Message = "未知的任务的类型";
                return response;
            }

            for (int i = 0; i < uploadTaskStatusQty; i++)
            {
                _logger.LogDebug($"=========================中转位 {currentContext.LocationCode}第{i + 1} 次上传任务状态 {status} 给接口 {queryDataUrl} 参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(ReportData))}=========================");
                var ReportingTask = await HttpRequestInvoker.PostAsJsonAsync<ServiceRequest, ServiceResponse>(queryDataUrl, ReportData);
                _logger.LogDebug($"中转位 中转位 {currentContext.LocationCode}第{i + 1} 次上传任务状态 {status} 给接口 {queryDataUrl} 回传参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(ReportingTask))}");

                if (ReportingTask == null)
                {
                    response.Code = 1;
                    response.Message = $"任务状态上传失败";
                    _logger.LogDebug($"任务状态上传失败");
                    await Task.Delay(2500);
                    continue;
                }
                else
                {
                    if (ReportingTask.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        response.Code = 0;
                        response.Message = "";

                        _logger.LogDebug($"任务状态上传OK: {ReportingTask.Message}");

                        if (status == ScheduledTaskStatus.Completed
                            || status == ScheduledTaskStatus.Failed
                            || status == ScheduledTaskStatus.Canceled)
                        {
                            currentContext.Reset();
                        }

                        return response;
                    }
                    else
                    {
                        response.Code = 1;
                        response.Message = $"任务状态上传失败: {ReportingTask.Message}";
                        _logger.LogDebug($"任务状态上传失败: {ReportingTask.Message}");
                    }
                }

                await Task.Delay(2000);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return response;
        }
    }

    /// <summary>
    /// 上报任务执行log
    /// </summary>
    /// <param name="taskCode"></param>
    /// <param name="Message"></param>
    /// <returns></returns>
    public async Task<string> ReportHikTaskLogToCentral(string? taskCode, string Message)
    {
        var url = DeviceDescriptor.Extra["ReportTaskDetail"].ToString();

        try
        {
            string result = "";
            if (string.IsNullOrWhiteSpace(taskCode))
            {
                result = $"没有收到有效的TaskCode,无法上报进度";
                return result;
            }

            JsonObject json = new JsonObject();
            json["transferJobId"] = taskCode;
            json["message"] = Message;
            //向中控领任务，上下料，上下料仓  FockGroup
            _logger.LogDebug($"++++++中转位 上传任务{taskCode}日志 {url} 参数:{System.Text.RegularExpressions.Regex.Unescape(json.ToString())}++++++");
            JsonObject res = await HttpRequestInvoker.PostAsJsonAsync<JsonObject, JsonObject>(url, json);
            _logger.LogDebug($"++++++中转位 上传任务{taskCode}日志 {url} 回传参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize<JsonObject>(res))}++++++");
            if (res != null)
            {
                result = res["code"].ToString();
                if (result == "SUCCESS")
                {
                    result = $"中转位 上传任务{taskCode}日志{result}";
                }
                else
                {
                    result = $"中转位 上传任务{taskCode}日志{result} : {res["message"]}";
                }
            }
            else
            {
                result = $"Fail,没有收到{taskCode}上传任务任务日志的回传值";
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"上报{url}失败:{ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// 中转位上生料，空料仓上传数据
    /// </summary>
    /// <param name="reportData"></param>
    /// <returns></returns>
    public async Task<bool> UpLoadTaskData(ServiceRequest reportData)
    {
        var taskCode = reportData.Params["TaskCode"].ToString();
        try
        {
            var queryDataUrl = DeviceDescriptor.Extra["ReportAGVInfor"].ToString();//开始执行任务;

            for (int i = 0; i < 3; i++)
            {
                _logger.LogDebug($"=========================中转位 任务{taskCode}第{i + 1} 次上传outbin数据给接口 ReportAGVInfor 参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(reportData))}=========================");
                var ReportingTask = await HttpRequestInvoker.PostAsJsonAsync<ServiceRequest, ServiceResponse>(queryDataUrl, reportData);
                _logger.LogDebug($"中转位 任务{taskCode}第{i + 1} 次上传outbin数据给接口 ReportAGVInfor 回传参数:{System.Text.RegularExpressions.Regex.Unescape(JsonSerializer.Serialize(ReportingTask))}");
                if (ReportingTask.Code != null && ReportingTask.Code == ErrorCodes.Sys.SUCCESS)
                {
                    string Message = $"中转位 任务{taskCode} outbin数据上传OK";
                    _ = await ReportHikTaskLogToCentral(taskCode, Message);
                    _logger.LogDebug(Message);
                    return true;
                }
                else
                {
                    string Message = $"中转位 任务{taskCode} outbin数据第{i + 1} 次上传失败: {ReportingTask.Message}";
                    _ = await ReportHikTaskLogToCentral(taskCode, Message);
                    _logger.LogDebug(Message);
                }

                await Task.Delay(2500);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"中转位 任务{taskCode} outbin数据时上传失败:{ex.Message}");
        }
        return true;
    }
}
