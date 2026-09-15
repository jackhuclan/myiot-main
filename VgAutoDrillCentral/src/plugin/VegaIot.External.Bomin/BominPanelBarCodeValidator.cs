using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SqlSugar;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;

namespace VegaIot.External.Bomin;

internal class BominPanelBarCodeValidator : DefaultPanelBarCodeValidator
{
    public BominPanelBarCodeValidator(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    public override async Task<bool> ValidatePanelBarCodeAsync(ScheduleTaskWithRequest? schedule)
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
                summaryLog.AppendLine("boming 校验二维码:");
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
                //var panelBarcodeValidator = _objectFactory.GetOrCreate<DefaultPanelBarCodeValidator>();
                var sendResponse = await SendBarCodeCheckResult(drill, verifyResult);
                summaryLog.AppendLine($"发送结果:{sendResponse}");
                _logger.LogInformation(summaryLog.ToString());
                await _scheduleTaskAdapter.AddScheduleLog(schedule.Id, summaryLog.ToString());
            }
        }

        return true;
    }
}
