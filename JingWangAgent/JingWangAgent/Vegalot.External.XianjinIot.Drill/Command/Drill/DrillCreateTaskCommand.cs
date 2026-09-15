// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using Vegalot.External.XianjinIot.Drill.Models;
using Vegalot.External.XianjinIot.Drill.Models.Ack;
using VgAutoDrill.Fundation.Command;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Drill;

namespace Vegalot.External.XianjinIot.Drill.Command.Drill;

internal class DrillCreateTaskCommand : BaseSimpleCommand
{
    private readonly IMqttClient _mqttClient;
    private readonly ILogger<DrillCreateTaskCommand> _logger;
    private readonly XianJinIotDrillOptions _xianJinIotOptions;
    private ConcurrentDictionary<string, bool> CommandsStatus = new ConcurrentDictionary<string, bool>();//线程安全

    public DrillCreateTaskCommand(
        IMqttClient mqttClient,
        IServiceProvider serviceProvider,
        ILogger<DrillCreateTaskCommand> logger,
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
        //03-102 下发钻孔任务指令 （板子推到钻机上面）
        _logger.LogInformation($"DrillCreateTaskCommand  Invoke  {JsonSerializer.Serialize(deviceServiceInvokeRequest)}");
        var targetResponse = new DeviceServiceInvokeResponse();
        targetResponse.Code = ErrorCodes.Sys.FAIL;
        targetResponse.Message = "暂未开始逻辑";
        string taskCode = string.Empty;
        try
        {

            if (deviceServiceInvokeRequest == null || deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("PayloadSegment") || string.IsNullOrEmpty(deviceServiceInvokeRequest.Params["PayloadSegment"]!.ToString()))
            {
                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"DrillCreateTaskCommand PayloadSegment 参数不正确";
                return targetResponse;
            }
            var options = new JsonSerializerOptions();
            options.Converters.Add(new StringToDecimalConverter());
            options.IgnoreNullValues = true;
            DrillCreateTaskPayload drillCreateTaskPayload = null;
            try
            {
                drillCreateTaskPayload = JsonSerializer.Deserialize<DrillCreateTaskPayload>(deviceServiceInvokeRequest.Params["PayloadSegment"].ToString(), options);
                if (drillCreateTaskPayload == null)
                {
                    targetResponse.Code = ErrorCodes.Sys.FAIL;
                    targetResponse.Message = $"drillCreateTaskPayload 实体转换出错：Null";
                    _logger.LogInformation($"03-102  drillCreateTaskPayload 实体转换出错：Null");
                    return targetResponse;
                }
                var drillIsWorking = await CheckCommandIsWorking(drillCreateTaskPayload.body.taskCode);
                if (drillIsWorking)
                {
                    targetResponse.Code = ErrorCodes.Sys.FAIL;
                    targetResponse.Message = $"钻机正在执行任务中...";
                    _logger.LogInformation($"03-102  钻机正在执行任务中...");
                    return targetResponse;
                }
            }
            catch (Exception ee)
            {

                targetResponse.Code = ErrorCodes.Sys.FAIL;
                targetResponse.Message = $"drillCreateTaskPayload 实体转换出错：{ee.Message}";
                return targetResponse;
            }
            taskCode = drillCreateTaskPayload.body.taskCode;
            CommonModel.UpPanelReportTaskCode = drillCreateTaskPayload.body.taskCode;
            //03-204 钻孔任务指令接收ack
            var drillCreateTaskAckPayload = new DrillCreateTaskAckPayload()
            {
                body = new DrillCreateTaskAckBody()
                {
                    sn = InteractingDevice.DeviceId,
                    taskCode = drillCreateTaskPayload.body.taskCode,
                    code = 200,
                    msg = "指令接收成功",
                }
            };
            drillCreateTaskAckPayload.ModifyHeader();
            _logger.LogInformation($"03-204  钻孔任务指令接收 ack: DrillCreateTaskCommand drillCreateTaskAckPayload  上报的 drillCreateTaskAckPayload {JsonSerializer.Serialize(drillCreateTaskAckPayload, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            })} ");

            await _mqttClient.PublishStringAsyncEnhance(IotTopic.CREATE_TASK_ACK_TOPIC, Base64Convert.FromStringToBase64String(JsonSerializer.Serialize(drillCreateTaskAckPayload, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) })), MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
            await AddTaskCode(drillCreateTaskPayload.body.taskCode);
            //解除

            byte slaveID = (byte)InteractingDevice.DeviceDescriptor.Extra["SlaveID"].ToInt();
            // agv离开和整个上下料结束
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAgvUnLoadingAndLoading"].ToUshort(), 0);
            InteractingDevice.modbusIpMaster.WriteSingleRegister(slaveID, InteractingDevice.DeviceDescriptor.Extra["BufferAllUnloadAndLoadEnd"].ToUshort(), 1);
            // 是否下发验证二维码通过
            targetResponse.Code = ErrorCodes.Sys.SUCCESS;
            targetResponse.Message = "创建完成";
            return targetResponse;
        }
        catch (Exception ee)
        {
            _logger.LogInformation($"DrillCreateTaskCommand  Invoke  异常{ee.Message}");
            targetResponse.Code = ErrorCodes.Sys.FAIL;
            targetResponse.Message = $"DrillCreateTaskCommand {ee.Message}";
            if (!string.IsNullOrEmpty(taskCode))
            {
                await RemoveTaskCodeRecord(taskCode);
            }
            return targetResponse;
        }
    }
}
