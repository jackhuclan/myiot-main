using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.ShenzhenKinwong;

internal class KinwongPanelBarCodeValidator : DefaultPanelBarCodeValidator
{
    private readonly IHttpRequestInvoker _httpRequestInvoker;

    public KinwongPanelBarCodeValidator(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
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
                summaryLog.AppendLine("szkingwong 校验二维码:");
                summaryLog.AppendLine($"检查板料barcode,{Environment.NewLine}正在上料:{schedule.ItemCode}");

                foreach (var panel in rawBufferPanels)
                {
                    summaryLog.AppendLine($"轴:{panel.Position},Item:{panel.ItemCode},Barcode:{panel.Barcode}");
                }

                bool summaryResultWithItem = false;
                var verifyResult = "";
                var IsCheckSZKWSN = await _sysConfigManager.GetIntValue("IsCheckSZKWSN");//深圳景旺校验二维码
                _logger.LogInformation($"中控配置是否校验二维码：{IsCheckSZKWSN} , 1: 校验，2: 不校验，3: 非深圳景旺的逻辑");
                if (IsCheckSZKWSN == 1)
                {
                    string CheckSNUrl = await _sysConfigManager.GetStringValue("IsCheckSZKWSNUrl");
                    var lot = new CheckSZKWSNRequest
                    {
                        ContainerName = rawBufferPanels.Select(t => t.ItemCode).FirstOrDefault()
                    };

                    _logger.LogInformation($"向接口{CheckSNUrl}发送请求，参数为{JsonSerializer.Serialize<CheckSZKWSNRequest>(lot)}");
                    var kwRes = await _httpRequestInvoker.PostAsJsonAsync<CheckSZKWSNRequest, CheckSNResponse>(CheckSNUrl, lot);
                    _logger.LogInformation($"向接口{CheckSNUrl}发送请求，返回消息为{JsonSerializer.Serialize<CheckSNResponse>(kwRes)}");
                    string[] snLst;
                    Dictionary<int, int> checkRes = new Dictionary<int, int>();
                    if (kwRes != null)
                    {
                        if (kwRes.ReadingSwitch == "1" && kwRes.PnlCode != null && kwRes.PnlCode.Length > 0) //kwRes.ReadingSwitch == "1" 需要校验二维码
                        {
                            snLst = kwRes.PnlCode;
                            int Result = 0;// Result : -1: 没有码； 0: 验证失败；1：验证通过；
                            foreach (var panel in rawBufferPanels)
                            {
                                if (!string.IsNullOrEmpty(panel.Barcode))
                                {
                                    Result = Array.Exists(snLst, t => t == panel.Barcode) ? 1 : 0;
                                }
                                else
                                {
                                    Result = -1;
                                }
                                verifyResult = Result + verifyResult;
                                checkRes.Add(panel.Position, Result);
                            }
                        }
                        else if (kwRes.ReadingSwitch == "1" && (kwRes.PnlCode.Length == 0 || kwRes.PnlCode == null))
                        {
                            //_logger.LogInformation($"接口{CheckSNUrl}返回 需要校验二维码但未给出{JsonSerializer.Serialize< CheckSZKWSNRequest>(lot)}的二维码");
                            //校验二维码，景旺返回的二维码为空，认为失败
                            foreach (var panel in rawBufferPanels)
                            {
                                checkRes.Add(panel.Position, 0);
                                verifyResult = "0" + verifyResult;
                            }
                        }
                        else if (kwRes.ReadingSwitch != "1")
                        {
                            //_logger.LogInformation($"接口{CheckSNUrl}返回 不需要校验二维码");
                            //不校验二维码，认为全部没问题,默认 验证通过
                            foreach (var panel in rawBufferPanels)
                            {
                                checkRes.Add(panel.Position, 1);
                                verifyResult = "1" + verifyResult;
                            }
                        }

                        var checkResult = rawBufferPanels.Select(x => new
                        {
                            x.Position,
                            x.ItemCode,
                            x.Barcode,
                            Result = checkRes.ContainsKey(x.Position) ? checkRes[x.Position] : 0
                        });

                        if (checkResult.Where(t => t.Result == 0).ToList().Count() > 0)
                        {
                            summaryResultWithItem = false;
                        }
                        else
                        {
                            summaryResultWithItem = true;
                        }
                        summaryLog.AppendLine($"比对明细：{JsonSerializer.Serialize(checkResult)}");
                    }
                }
                else if (IsCheckSZKWSN == 2)
                {
                    foreach (var panel in rawBufferPanels)
                    {
                        verifyResult = "1" + verifyResult;
                    }

                    //不校验二维码，认为全部没问题
                    summaryLog.AppendLine($"中控端未开启板料二维码验证");
                    _logger.LogInformation(summaryLog.ToString());
                    summaryResultWithItem = true;
                }

                //summaryLog.AppendLine($"比对明细：{JsonSerializer.Serialize(checkResult)}");
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
}
