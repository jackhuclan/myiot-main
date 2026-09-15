using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

public class DefaultPanelBarCodeValidator : IPanelBarCodeValidator
{
    public readonly IDeviceManager _deviceManager;
    public readonly ISysConfigManager _sysConfigManager;
    public readonly ILogger<ScheduleTaskManager> _logger;
    public readonly IScheduleTaskAdapter _scheduleTaskAdapter;
    public readonly IScheduleService _scheduleService;

    public DefaultPanelBarCodeValidator(IServiceProvider serviceProvider)
    {
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _logger = serviceProvider.GetRequiredService<ILogger<ScheduleTaskManager>>();
        _scheduleTaskAdapter = serviceProvider.GetRequiredService<IScheduleTaskAdapter>();
        _scheduleService = serviceProvider.GetRequiredService<IScheduleService>();
    }

    public virtual async Task<bool> ValidatePanelBarCodeAsync(ScheduleTaskWithRequest? schedule)
    {
        if (schedule == null) return false;

        if (!DeviceKindExtensions.IsDrill(schedule.RequestDeviceKind)
            || schedule.InteractionSequence == InteractionSequence.UnloadOnly
            || string.IsNullOrEmpty(schedule.ItemCode)) return false;

        if (_deviceManager.TryGetOnlineDevice<Drill>(schedule.CallerDeviceId, out var drill) && drill != null)
        {
            var rawBufferPanels = drill.PayloadPanels.Where(p => p.Layer == 0).OrderBy(p => p.Position);
            if (rawBufferPanels.Any())
            {
                var wishItemCode = schedule.ItemCode.ToUpper();
                var summaryLog = new StringBuilder();
                summaryLog.AppendLine("default 校验二维码:");
                summaryLog.AppendLine($"检查板料barcode,{Environment.NewLine}正在上料:{schedule.ItemCode}");

                foreach (var panel in rawBufferPanels)
                {
                    summaryLog.AppendLine($"轴:{panel.Position},Item:{panel.ItemCode},Barcode:{panel.Barcode}");
                }

                bool summaryResultWithItem = false;
                var verifyResult = "";

                // Result : -1: 没有码； 0: 验证失败；1：验证通过；
                var checkResult = rawBufferPanels.Select(x => new
                {
                    x.Position,
                    x.ItemCode,
                    Result = string.IsNullOrEmpty(x.Barcode) ? -1 : (wishItemCode.Contains(x.Barcode.SafeSubstring(1, 9).Trim('0').ToUpper()) ? 1 : 0)
                });

                //var verifyResult = "";
                var withItemResults = checkResult.Where(x => !string.IsNullOrEmpty(x.ItemCode));
                var centralVerifyFunction02 = await _sysConfigManager.GetBoolValue("CentralVerifyFunction02");
                //是否启用扫码容错算法，
                //允许未能识别到码的板子，验证通过；
                //已经识别到错误的码的，仍然判定为false;

                if (centralVerifyFunction02)
                {
                    summaryLog.AppendLine($"已启用容错算法");
                    summaryLog.AppendLine($"-1: 没有码； 0: 验证失败；1：验证通过；");

                    if (withItemResults.Count() <= 1)
                    {
                        summaryResultWithItem = withItemResults.All(x => x.Result == 1) && withItemResults.Any(x => x.Result == 1);
                    }
                    else
                    {
                        summaryResultWithItem = withItemResults.Count(x => x.Result == 1) >= withItemResults.Count() - 1 && !checkResult.Any(x => x.Result == 0);
                    }

                    await _scheduleService.SetBarcodeCheckResult(schedule.Id, summaryResultWithItem);

                    foreach (var item in checkResult.OrderBy(p => p.Position))
                    {
                        if (string.IsNullOrEmpty(item.ItemCode))
                        {
                            verifyResult = "0" + verifyResult;
                        }
                        else if (item.Result == 1)
                        {
                            verifyResult = "1" + verifyResult;
                        }
                        else if (item.Result == -1 && summaryResultWithItem)
                        {
                            verifyResult = "1" + verifyResult;
                        }
                        else
                        {
                            verifyResult = "0" + verifyResult;
                        }
                    }
                }
                else
                {
                    summaryResultWithItem = withItemResults.Any(x => x.Result == 1) && withItemResults.All(x => x.Result == 1);
                    await _scheduleService.SetBarcodeCheckResult(schedule.Id, summaryResultWithItem);

                    foreach (var item in checkResult.OrderBy(p => p.Position))
                    {
                        if (string.IsNullOrEmpty(item.ItemCode))
                        {
                            verifyResult = "0" + verifyResult;
                        }
                        else if (item.Result == 1)
                        {
                            verifyResult = "1" + verifyResult;
                        }
                        else
                        {
                            verifyResult = "0" + verifyResult;
                        }
                    }
                }

                //summaryLog.AppendLine($"比对结果: {summaryResultWithItem}");
                summaryLog.AppendLine($"比对明细：{JsonSerializer.Serialize(checkResult)}");
                summaryLog.AppendLine($"比对结果: {summaryResultWithItem}");
                ////下发barcode 检测结果
                var sendResponse = await SendBarCodeCheckResult(drill, verifyResult);
                summaryLog.AppendLine($"发送结果:{sendResponse}");
                _logger.LogInformation(summaryLog.ToString());
                await _scheduleTaskAdapter.AddScheduleLog(schedule.Id, summaryLog.ToString());
            }
        }

        return true;
    }

    public async Task<string> SendBarCodeCheckResult(Drill drill, string verifyResult)
    {
        var sendResult = "";
        var resultParams = new Dictionary<string, object?>
        {
            { "codeReaderCheck", Convert.ToByte(verifyResult.ToString(), 2) }
        };
        bool isRetry = false;
        int retryCount = 0;

        var invokeRequst = new DeviceServiceInvokeRequest
        {
            DeviceId = drill.DeviceId,
            ProductId = drill.ProductId,
            ClientId = drill.ClientId,
            ServiceId = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
            ServiceName = DeviceCommands.SET_DRILL_CODE_READER_CHECK_COMMAND,
            TargetProductId = drill.ProductId,
            TargetDeviceId = drill.DeviceId,
            TargetClientId = drill.ClientId,
            Params = resultParams
        };

        var result = await drill.InvokeService(invokeRequst);
        if (result == null || result.Code != "SUCCESS")
        {
            isRetry = true;
        }
        else
        {
            return $"检查板料信息，已下发给{drill.DeviceId}";
        }

        while (isRetry && retryCount < 3)
        {
            await Task.Delay(500 * (retryCount + 1));

            invokeRequst = new DeviceServiceInvokeRequest
            {
                DeviceId = drill.DeviceId,
                ProductId = drill.ProductId,
                ClientId = drill.ClientId,
                ServiceId = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
                ServiceName = DeviceCommands.SET_DRILL_CODE_READER_CHECK_COMMAND,
                TargetProductId = drill.ProductId,
                TargetDeviceId = drill.DeviceId,
                TargetClientId = drill.ClientId,
                Params = resultParams
            };

            result = await drill.InvokeService(invokeRequst);
            if (result == null || result.Code != "SUCCESS")
            {
                isRetry = true;
            }
            else
            {
                sendResult = $"检查板料信息，已下发给{drill.DeviceId}";
                isRetry = false;
            }
            retryCount++;
        }

        sendResult = $"检查板料信息{drill.DeviceId}时，下发失败";

        return sendResult;
    }
}
