// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using FluentFTP;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Vegalot.External.XianjinIot.Drill.Models;
using Vegalot.External.XianjinIot.Drill.Models.Ack;
using Vegalot.External.XianjinIot.Drill.Models.Report;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Drill;

namespace Vegalot.External.XianjinIot.Drill.Command.Drill;

internal class DrillLoadDrillingFileCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly ILogger<DrillCreateTaskCommand> _logger;
    private readonly XianJinIotDrillOptions _xianJinIotOptions;

    public DrillLoadDrillingFileCommand(
        IMqttClient mqttClient,
        ILogger<DrillCreateTaskCommand> logger,
        IServiceProvider serviceProvider,
        IOptions<XianJinIotDrillOptions> options,
        DefaultDrill device,
        CommandDescriptor commandDescriptor)
        : base(serviceProvider, device, commandDescriptor)
    {
        _mqttClient = mqttClient;
        _logger = logger;
        _xianJinIotOptions = options.Value;
    }

    public override async Task<DeviceServiceInvokeResponse> Invoke(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        //03-103 下发调取钻孔资料命令
        _logger.LogInformation($"DrillLoadDrillingFileCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");
        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Code = ErrorCodes.Sys.FAIL;
        targetResponse.Message = "暂未开始逻辑";
        string taskCode = string.Empty;
        try
        {



            if (deviceServiceInvokeRequest == null || deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment") || string.IsNullOrEmpty(deviceServiceInvokeRequest.Params["PayloadSegment"]!.ToString()))
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"DrillLoadDrillingFileCommand PayloadSegment 参数不正确";
                return targetResponse;
            }

            var drillLoadDrillingFilePayload = JsonSerializer.Deserialize<DrillLoadDrillingFilePayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToString());
            if (drillLoadDrillingFilePayload == null)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"drillLoadDrillingFilePayload 实体转换出错：Null";
                return targetResponse;
            }
            var drillIsWorking = await CheckCommandIsWorking(drillLoadDrillingFilePayload.body.taskCode);
            if (drillIsWorking)
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"钻机正在执行任务中...";
                _logger.LogInformation($"03-103   钻机正在执行任务中...");
                return targetResponse;
            }
            //03-205 上报调资料命令接收ack
            await Ack(drillLoadDrillingFilePayload);
            taskCode = drillLoadDrillingFilePayload.body.taskCode;
            await AddTaskCode(drillLoadDrillingFilePayload.body.taskCode);
            LoadFileAndReport(drillLoadDrillingFilePayload);

            //todo 上报调资料完成

            targetResponse.Code = ErrorCodes.Sys.SUCCESS;
            targetResponse.Message = "创建完成";
            return targetResponse;
        }
        catch (Exception ee)
        {
            _logger.LogInformation($"DrillLoadDrillingFileCommand  Invoke  异常{ee.Message}");
            targetResponse.Message = $"DrillLoadDrillingFileCommand {ee.Message}";
            if (!string.IsNullOrEmpty(taskCode))
            {
                await RemoveTaskCodeRecord(taskCode);
            }
            return targetResponse;
        }
    }

    private async Task Ack(DrillLoadDrillingFilePayload? drillLoadDrillingFilePayload)
    {
        var drillLoadDrillingFileAckPayload = new DrillLoadDrillingFileAckPayload()
        {
            body = new DrillLoadDrillingFileAckBody()
            {
                sn = InteractingDevice.DeviceId,
                taskCode = drillLoadDrillingFilePayload.body.taskCode,
                code = 200,
                msg = "指令接收成功",
            }
        };
        drillLoadDrillingFileAckPayload.ModifyHeader();
        _logger.LogInformation($"03-205  上报调资料指令ack : DrillLoadDrillingFileCommand Ack  上报的 drillLoadDrillingFileAckPayload {JsonSerializer.Serialize(drillLoadDrillingFileAckPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })} ");

        await _mqttClient.PublishStringAsyncEnhance(IotTopic.LOAD_DRILLING_FILE_ACK_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillLoadDrillingFileAckPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
    }

    private async Task LoadFileAndReport(DrillLoadDrillingFilePayload? drillLoadDrillingFilePayload)
    {
        int state = 0;
        string message = "未知异常";
        try
        {
            string filePath = drillLoadDrillingFilePayload.body.filePath;
            //todo 调用加载
            if (string.IsNullOrEmpty(filePath))
            {
                try
                {
                    filePath = GetFilePathFormFtp(drillLoadDrillingFilePayload.body.incodeNumber);
                }
                catch (Exception ee)
                {
                    state = 2;
                    // dsp 显示
                    message = ee.Message;
                    return;
                }
            }
            _logger.LogInformation($"将要加载到cnc84 的路径是{filePath}");
            // 加载文件
            if (!CheckDrlFile(filePath))
            {
                state = 2;
                message = $"文件不存在";
                return;
            }
            if (!LoadFile(filePath))
            {
                state = 3;
                message = $"文件加载失败";
                return;
            }
            state = 1;
            message = $"完成";
            return;
            //验证成功 上报结果
        }
        catch (Exception ee)
        {
            _logger.LogInformation($"未知异常 {ee.Message}");
        }
        finally
        {
            var drillLoadDrillingFileReportPayload = new DrillLoadDrillingFileReportPayload()
            {
                body = new DrillLoadDrillingFileReportBody()
                {
                    sn = InteractingDevice.DeviceId,
                    taskCode = drillLoadDrillingFilePayload.body.taskCode,
                    msg = message,
                    status = state
                }
            };
            drillLoadDrillingFilePayload.ModifyHeader();
            _logger.LogInformation($"03-206  上报调资料完成 : DrillLoadDrillingFileCommand LoadFileAndReport  上报的 drillLoadDrillingFileReportPayload {JsonSerializer.Serialize(drillLoadDrillingFileReportPayload, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            })} ");
            if (state != 1)
            {
                _logger.LogInformation($"03-206  异常解除锁命令 {drillLoadDrillingFilePayload.body.taskCode}");
                await RemoveTaskCodeRecord(drillLoadDrillingFilePayload.body.taskCode);
            }

            await _mqttClient.PublishStringAsyncEnhance(IotTopic.LOAD_DRILLING_FILE_REPORT_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillLoadDrillingFileReportPayload, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
        }
    }

    public bool CheckDrlFile(string drlFilePath)
    {
        string DrlFilePath_tmp = drlFilePath;
        _logger.LogDebug("钻带程序路径" + DrlFilePath_tmp);

        if (!File.Exists(DrlFilePath_tmp))
        {
            _logger.LogDebug("请检查是否有生产程序文件!");
            return false;
        }
        return true;
    }

    public bool LoadFile(string drilPath)
    {
        InteractingDevice.cnc84Command.SetCncComand("CM@@@");
        if (!ValidateCmDrlFile())
        {
            return false;
        }

        InteractingDevice.cnc84Command.SetLoadFile(drilPath);
        if (!ValidateDrlFile(drilPath))
        {
            return false;
        }
        _logger.LogDebug($"ValidateDrlFile  成功");
        if (!ValidateDrlImg())
        {
            return false;
        }
        _logger.LogDebug($"ValidateDrlImg  成功");
        return true;
    }

    private bool ValidateCmDrlFile()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var netProName = string.Empty;
        do
        {
            Thread.Sleep(1000);
            netProName = InteractingDevice.cnc84Command.GetACTProgram();
            if (string.IsNullOrEmpty(netProName) || "[NULL]".Equals(netProName))
            {
                stopwatch.Stop();
                return true;
            }
            InteractingDevice.cnc84Command.SetCncComand("CM@@@");
        } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateCmFileTimeout"].ToLong());
        return false;
    }

    private bool ValidateDrlFile(string drilFilePath)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var netProName = string.Empty;
        do
        {
            Thread.Sleep(100);
            netProName = InteractingDevice.cnc84Command.GetCncStatus()?.ProgramName.ToLower();
            if (drilFilePath.ToLower().Equals(netProName, StringComparison.CurrentCultureIgnoreCase))
            {
                stopwatch.Stop();
                return true;
            }
        } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDrlFileTimeout"].ToLong());
        return false;
    }

    private bool ValidateDrlImg()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        int count = 0;
        do
        {
            Thread.Sleep(1000);
            var screenSaver = InteractingDevice.cnc84Command.GetScreenSaver();
            _logger.LogDebug($"ValidateDrlImg  {screenSaver?.ScreenText}");
            if (!string.IsNullOrEmpty(screenSaver?.ScreenText) && screenSaver.ScreenText.Contains("[2000]"))
            {
                count++;
                _logger.LogDebug($"ValidateDrlImg  count={count} 有值");
                if (_xianJinIotOptions.ValidateDrlImgCountOnOff)
                {
                    if (count >= 2)
                    {
                        _logger.LogDebug($"ValidateDrlImg  xianJinIotOptions.ValidateDrlImgCountOnOff count={count}");
                        stopwatch.Stop();
                        return true;
                    }
                }
                else
                {
                    stopwatch.Stop();
                    return true;
                }
            }
            else
            {
                count = 0;
                _logger.LogDebug($"ValidateDrlImg  count={count} 无值");
            }
        } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateCmFileTimeout"].ToLong());
        return false;
    }

    /// <summary>
    /// ftp 服务器
    ///
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public string GetFilePathFormFtp(string itemCode)
    {
        _logger.LogInformation($"ftp  GetFilePath");
        try
        {
            using (FtpClient ftp = new FtpClient(_xianJinIotOptions.FtpHost, _xianJinIotOptions.FtpUsername, _xianJinIotOptions.FtpPassword, _xianJinIotOptions.FtpPort)) //TODO
            /// // using (FtpClient ftp = new FtpClient(_xianJinIotOptions.FtpHost, _xianJinIotOptions.FtpPort))
            {
                ftp.Connect();
                _logger.LogInformation($"ftp 搜索的路径:{_xianJinIotOptions.FtpPath}");
                var list = ftp.GetListing(_xianJinIotOptions.FtpPath).Where(s => s.FullName.ToLower().Contains(itemCode.ToLower()));
                Console.WriteLine(list.Count());
                if (list == null || list.Count() <= 0)
                {
                    _logger.LogError($"ftp 未找到文件");

                    throw new Exception("ftp 未找到文件");
                }
                foreach (var item in list)
                {
                    _logger.LogInformation($"ftp 搜索的路径文件有:{item.FullName}");
                }
                var remotePath = list.FirstOrDefault().FullName;
                _logger.LogInformation($"ftp 服务器地址{remotePath}");
                var localFilePath = Path.Combine(_xianJinIotOptions.LocalDirectory, Path.GetFileName(remotePath));
                _logger.LogInformation($"ftp 本地地址{localFilePath}");
                ftp.DownloadFile(localFilePath, remotePath, FtpLocalExists.Overwrite);
                string drlPth = localFilePath;
                return drlPth;
            }
        }
        catch (Exception ee)
        {
            _logger.LogError($"和ftp服务器交互失败 {ee.Message}");
            throw new Exception($"和ftp服务器交互失败 {ee.Message}");
        }
    }
}
