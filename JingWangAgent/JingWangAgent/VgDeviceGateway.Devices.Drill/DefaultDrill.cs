// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO.Ports;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Modbus.Device;
using VgAutoDrill.Fundation.CNC;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Drill.DrillDevice;
using VgDeviceGateway.Devices.Drill.Other.CodeReader;
using VgDeviceGateway.Devices.Drill.Other.Front;

namespace VgDeviceGateway.Devices.Drill;

public class DefaultDrill : Device
{
    public static SerialPort port = new SerialPort();
    public readonly ILogger<DefaultDrill> logger;
    public readonly int spindleNum;
    public readonly int region;
    public readonly IModbusIpMasterWrapper modbusIpMasterWrapper;
    public IModbusMaster modbusIpMaster;
    public readonly ICNCCommandWrapper cNC84CommandWrapper;
    public ICNCCommand cnc84Command;
    private string transactionId = "";

    public string TransactionId
    {
        get
        {
            return transactionId;
        }
        set
        {
            transactionId = value;
            CallAgvMessageColor = !string.IsNullOrEmpty(transactionId) && transactionId.Contains("呼叫失败");
        }
    }

    public string NewTranscationId = "";
    public string NewTranscationIdTemp = "";
    public string DrillTransactionId = "";
    public string OldTransactionId = "";
    public string callResult = "";
    public string callTimeResult = "";
    public volatile int percentage = 0;
    public readonly AGVToDrill_LoadMaterial_Drill_InteractionPolicy AGVToDrillDrilllLoadPolicy;
    public readonly AGVToDrill_UnloadMaterial_Drill_InteractionPolicy AGVToDrilllDrillUnloadPolicy;
    public AutoResetEvent escRestEvent = new AutoResetEvent(false);
    public string CodeMessage { get; set; } = string.Empty;
    public bool LoadFileFlag { get; set; } = false;
    public bool EscFlag { get; set; } = true;
    public readonly CancellationTokenSource cancellationTokenSource = new();
    public string[] spindleAgvPosition;
    public bool scanGunOnOff;
    public readonly bool middleMushroomExist;
    public readonly bool threeColorLightsExistsOnBuffer;
    public string SplineNoBrokenStatus { get; set; }
    public string MaterialMatchCode { get; set; } = string.Empty;
    public bool isCallAgvUnload = false;
    public volatile bool allowAllAgv = true;
    public DateTime CallAgvTime = DateTime.Now;
    public DateTime AgvEndTime = DateTime.Now;
    public int CallAgvTimeInterval = 0;
    public int AgvEndTimeInterval = 0;
    public string QueryScheduleUrl = string.Empty;
    public string CallCondition = string.Empty;
    public volatile bool plcConnentFlag = false;
    private readonly int agvOperationTypes;
    public IDrillDevice realDrillDevice;

    //0 agv没在给钻机上下料  1 agv正在给钻机上下料
    public int AgvOnWorkDrillStatus = 0;

    // 0 是false 1是true
    public int MaterialEnsureFlag = 0;

    //0 不是最后一步 开始降顶升  1 最后一步 开始降顶升
    private int isLastStep = 0;

    public volatile ushort bufferAllUnloadAndLoadEnd = 0;
    public volatile bool AgvIsWorkOnBuffer = false;
    public volatile bool existRawPanel = false;
    public volatile bool isAvailbleForAgv = false;
    public volatile InteractionSequence suggestInteractionSequence = InteractionSequence.None;
    public volatile bool CallAgvMessageColor = false;
    public readonly List<int> plcAlarmNotNotifyDrillErrorIds;
    private static readonly object lockModify = new object();//
    public CodeReaderBaseSetting codeReaderBaseSetting;

    public bool codeReaderOnOff;

    public volatile bool loadMaterialVale = true;
    public volatile bool mushroomValue = true;

    public static readonly object lockObject = new object();
    public static volatile float CollectCleanTime = 0;
    public static DateTime CollectCleanStartTime = DateTime.Now;
    public static DateTime CollectCleanEndTime = DateTime.Now;

    public volatile ushort[] material = new ushort[2];

    public int IsLastStep
    {
        get { return isLastStep; }
        set
        {
            Interlocked.Exchange(ref isLastStep, value);
            // WatchingProperties.AddProperty("IsLastStep", value);
        }
    }

    public List<MaterialModel> MaterialModels { get; set; } = new List<MaterialModel>();
    public FrontExtendDevice frontExtendDevice;
    public volatile bool isOnDoingScript = false;
    public volatile bool isExistShowDialogText = false;
    public volatile string? ItemCode = string.Empty;

    public DefaultDrill(DeviceDescriptor deviceDescriptor, IServiceProvider serviceProvider, IDeviceEngine deviceEngine, IObjectFactory objectFactory)
        : base(deviceDescriptor, deviceEngine, serviceProvider)
    {
        cNC84CommandWrapper = ((IModbusOperator)Engine.DeviceConnector).CNCCommandWrapper;
        modbusIpMasterWrapper = ((IModbusOperator)Engine.DeviceConnector).ModbusIpMasterWrapper;
        logger = LoggerFactory.CreateLogger<DefaultDrill>();
        spindleNum = deviceDescriptor.SpindleNum;
        scanGunOnOff = DeviceDescriptor.Extra["ScanGunOnOff"].ToBool();
        codeReaderOnOff = DeviceDescriptor.Extra["CodeReaderOnOff"].ToBool();
        agvOperationTypes = DeviceDescriptor.Extra["AgvOperationTypes"].ToInt();
        if (agvOperationTypes == 3)
        {
            region = DeviceDescriptor.Extra["Region"].ToInt();
        }
        else if (agvOperationTypes == 1)
        {
            frontExtendDevice = objectFactory.CreateObject<FrontExtendDevice>();
        }
        realDrillDevice = InteractionFactory.CreatetDrillDevice(objectFactory, this, agvOperationTypes);
        // if (DeviceDescriptor.AutoMode && agvOperationTypes != 10)
        {
            AGVToDrillDrilllLoadPolicy = objectFactory.CreateObject<AGVToDrill_LoadMaterial_Drill_InteractionPolicy>(this);
            AGVToDrilllDrillUnloadPolicy = objectFactory.CreateObject<AGVToDrill_UnloadMaterial_Drill_InteractionPolicy>(this);
        }

        threeColorLightsExistsOnBuffer = DeviceDescriptor.Extra["ThreeColorLightsExistsOnBuffer"].ToBool();
        SplineNoBrokenStatus = new string(Enumerable.Repeat('1', spindleNum).ToArray());
        middleMushroomExist = DeviceDescriptor.Extra["MiddleMushroomExist"].ToBool();
        CallAgvTimeInterval = deviceDescriptor.Extra["CallAgvTimeInterval"].ToInt();
        AgvEndTimeInterval = deviceDescriptor.Extra["AgvEndTimeInterval"].ToInt();
        QueryScheduleUrl = deviceDescriptor.Extra["QuerySchedule"].ToStr();
        plcAlarmNotNotifyDrillErrorIds = deviceDescriptor.Extra["PlcAlarmNotNotifyDrillErrorIds"].ToStr().Split(",").Select(s => int.Parse(s)).ToList();
        IniAgvPosition();
        Connector.ConnectFunc = (deviceDescriptor) => Task.Run(() =>
        {
            return realDrillDevice.ConnectToCncAndOtherDevice();
        });
        PropertyContainer.AddHandler<DefaultDrillPropertyHandler, DefaultDrill>(this);
        StateContainer.AddHandler<DefaultDrillStateHandler, DefaultDrill>(this);
        EventContainer.AddHandler<DefaultDrillEventHandler, DefaultDrill>(this);
        CollectDataFunc = (d) => PropertyContainer[d.GetType()].CollectPropertyValues();

        AddCommand("SetDrillCommand", (r) => SetDrillHandler(r));
        AddCommand("SetDrillCodeReaderCheckCommand", (r) => SetDrillCodeReaderCheckHandler(r));
        AddCommand("SetDrillCodeReaderCheckSingleSplindleCommand", (r) => SetDrillCodeReaderCheckSingleSplindleHandler(r));
        {
            // CollectCleanTime 从文件中读取
        }

        AddCommand("SetScannerLotCommand", SetScannerLot);
        AddCommand("SetLoadAptFileCommand", LoadAptFile);
        AddCommand("SetCodeReaderOnOffCommand", CodeReaderOnOffToCnc);

        MqttClientWrapper.OnConnected += MqttClientWrapper_OnConnected;
    }

    private async Task<DeviceServiceInvokeResponse> CodeReaderOnOffToCnc(DeviceServiceInvokeRequest request)
    {
        var rest = await realDrillDevice.CodeReaderOnOffToCnc(request);
        return rest;
    }

    private async Task<DeviceServiceInvokeResponse> SetScannerLot(DeviceServiceInvokeRequest request)
    {
        var rest = await realDrillDevice.SetScannerLot(request);
        return rest;
    }

    private async Task<DeviceServiceInvokeResponse> LoadAptFile(DeviceServiceInvokeRequest request)
    {
        logger.LogInformation($"LoadAptFile 开始了");
        var rest = await realDrillDevice.LoadAtpFile(request);
        logger.LogInformation($"LoadAptFile 返回结束 {JsonSerializer.Serialize(rest)}");
        return rest;
    }

    private async Task MqttClientWrapper_OnConnected()
    {
        try
        {
            logger.LogInformation($"MqttClientWrapper_OnConnected  上报板材");
            if (DeviceDescriptor.AutoMode)
            {
                await PayloadPanels.RaiseCollectionChangedEvent(DeviceDescriptor.DeviceId);
                logger.LogInformation($"MqttClientWrapper_OnConnected  上报板材 {JsonSerializer.Serialize(PayloadPanels)}");
            }
        }
        catch (Exception e)
        {
            logger.LogInformation($"MqttClientWrapper_OnConnected  上报板材异常 {e.Message}");
        }
    }

    //

    private async Task<DeviceServiceInvokeResponse> SetDrillCodeReaderCheckSingleSplindleHandler(DeviceServiceInvokeRequest request)
    {
        logger.LogInformation($"SetDrillCodeReaderCheckSingleSplindleHandler   【 codeReaderCheck 】  request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
        try
        {
            if (request.Params == null || !request.Params.ContainsKey("codeReaderCheck") || !request.Params.ContainsKey("singleSplindleNum"))
            {
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = "未下发codeReaderCheck数据 或者没有下发 singleSplindleNum",
                };
            }
            if (!Int32.TryParse(request.Params["codeReaderCheck"].ToStr(), out int result) || !(result == 0 || result == 1))
            {
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = "codeReaderCheck数据 不是整数或者数据不在0和1之间",
                };
            }

            if (!Int32.TryParse(request.Params["singleSplindleNum"].ToStr(), out int splindle) || splindle <= 0 || splindle > spindleNum)
            {
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"singleSplindleNum 不是整数 或者轴不在1和{spindleNum}之间",
                };
            }

            realDrillDevice.CodeReaderCheck(result);
            logger.LogInformation($"SetDrillCodeReaderCheckSingleSplindleHandler 已修改【 codeReaderCheck 】");

            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = string.Empty,
            };
        }
        catch (Exception ex)
        {
            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = ex.Message,
            };
        }
    }

    private async Task<DeviceServiceInvokeResponse> SetDrillCodeReaderCheckHandler(DeviceServiceInvokeRequest request)
    {
        logger.LogInformation($"SetDrillCodeReaderCheckHandler   【 codeReaderCheck 】  request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
        try
        {
            if (request.Params == null || !request.Params.ContainsKey("codeReaderCheck"))
            {
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = "未下发codeReaderCheck数据",
                };
            }
            if (!Int32.TryParse(request.Params["codeReaderCheck"].ToStr(), out int result))
            {
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = "codeReaderCheck数据 不是整数",
                };
            }

            realDrillDevice.CodeReaderCheck(result);
            logger.LogInformation($"SetDrillCodeReaderCheckHandler 已修改【 codeReaderCheck 】");

            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = string.Empty,
            };
        }
        catch (Exception ex)
        {
            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = ex.Message,
            };
        }
    }

    private async Task<DeviceServiceInvokeResponse> SetDrillHandler(DeviceServiceInvokeRequest request)
    {
        logger.LogInformation($"SetDrillCommand SetDrillHandler request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
        try
        {
            if (request.PayloadPanels == null || request.PayloadPanels.Count == 0)
            {
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = "未下发PayloadPanels数据",
                };
            }
            if (request.PayloadPanels.Any(panel => panel.Position - 1 + panel.Layer * spindleNum < 0))
            {
                return new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = "panel的属性Position或者Layer，设置不对。导致了panel.Position - 1 + panel.Layer * spindleNum < 0",
                };
            }
            logger.LogInformation($"SetDrillHandler中控下发 未修改【 PayloadPanels 】{JsonSerializer.Serialize(PayloadPanels, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
            await UpdatePayloadPanels(request.PayloadPanels);
            logger.LogInformation($"SetDrillHandler中控下发 已修改【 PayloadPanels 】{JsonSerializer.Serialize(PayloadPanels, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");
            var panel = PayloadPanels.Take(spindleNum).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.ItemCode));
            ItemCode = string.Empty; if (panel != null) { ItemCode = panel.ItemCode; }

            logger.LogInformation($"SetDrillHandler中控下发 获取buffer上层的{ItemCode}");

            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = string.Empty,
            };
        }
        catch (Exception ex)
        {
            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = ex.Message,
            };
        }
    }

    private void InitPayloadCutterTrays()
    {
        for (int z = 0; z < DeviceDescriptor.LayerLimit; z++)
        {
            for (int i = 0; i < spindleNum; i++)
            {
                for (int j = 0; j < region; j++)
                {
                    PayloadCutterTrays.Add(

                        new CutterTray()
                        {
                            X = i * region + j,
                            Y = z,
                            Z = 0,
                        }
                  );
                }
            }
        }
    }

    public async Task UpdatePayloadPanels(List<Panel> panels)
    {
        if (panels == null || panels.Count == 0)
        {
            return;
        }
        logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  界面或者中控 未修改【 PayloadPanels 】 {JsonSerializer.Serialize(PayloadPanels)} ");
        await PayloadPanels.ChangeListSafely(DeviceId, Task.Run(() =>
        {
            foreach (Panel panel in panels)
            {
                panel.SiloCode = DeviceId;
                panel.LocationCode = DeviceId;
                var index = panel.Position - 1 + panel.Layer * spindleNum;
                if (index >= 0 && index < 3 * spindleNum)
                {
                    try
                    {
                        PayloadPanels[panel.Position - 1 + panel.Layer * spindleNum] = panel;
                    }
                    catch (Exception ee)
                    {

                        logger.LogError($"{DateTime.Now.ToShortTimeString()}  界面或者中控  PayloadPanels 异常{ee.Message}");

                    }

                }

            }
        }));
        logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  界面或者中控 已修改【 PayloadPanels 】 {JsonSerializer.Serialize(PayloadPanels)} ");
    }

    public override Task<DeviceServiceInvokeResponse> CancelSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        realDrillDevice.CanceledScheduleLocal(deviceServiceInvokeRequest.ScheduledStatus);
        return base.CancelSchedule(deviceServiceInvokeRequest);
    }

    public override Task<DeviceServiceInvokeResponse> CompleteSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        switch (deviceServiceInvokeRequest.ScheduledStatus)
        {
            case ScheduledTaskStatus.Canceled:
                realDrillDevice.CanceledScheduleLocal(deviceServiceInvokeRequest.ScheduledStatus);
                break;

            case ScheduledTaskStatus.Completed:
                realDrillDevice.CompleteScheduleLocal(deviceServiceInvokeRequest.ScheduledStatus);
                break;

            case ScheduledTaskStatus.Failed:
                realDrillDevice.FailScheduleLocal();
                break;

            default:
                break;
        }

        return base.CompleteSchedule(deviceServiceInvokeRequest);
    }

    protected override Task Initialize()
    {
        try
        {
            if (DeviceDescriptor.AutoMode) _ = GetPanelItemCodeAsync();
        }
        catch (Exception ee)
        {
            logger.LogWarning($"{DateTime.Now.ToShortTimeString()}  GetPanelItemCodeAsync {ee.Message} ");
        }

        realDrillDevice.InitializeLocal();
        return base.Initialize();
    }

    public override async Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await PropertyContainer[GetType()].ReadProperties(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await PropertyContainer[GetType()].WriteProperties(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToDrillDrilllLoadPolicy.PrepareLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToDrillDrilllLoadPolicy.InvokeLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToDrillDrilllLoadPolicy.CompleteLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToDrilllDrillUnloadPolicy.PrepareUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToDrilllDrillUnloadPolicy.InvokeUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await AGVToDrilllDrillUnloadPolicy.CompleteUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await realDrillDevice.WorkLocal();
    }

    public override async Task<DeviceServiceInvokeResponse> Standby(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await realDrillDevice.StandbyLocal();
    }

    public override async Task<DeviceServiceInvokeResponse> Shutdown(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await realDrillDevice.ShutdownLocal();
    }

    public override async Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await realDrillDevice.ScheduleTaskLocal();
    }

    public string CompleteAllEnd()
    {
        return realDrillDevice.CompleteAllEndLocal();
    }

    public bool ChangeUnloadClinkerNoloadRawStatus()
    {
        try
        {
            logger.LogInformation($"只下熟料不上生料  {loadMaterialVale}");
            modbusIpMaster.WriteSingleCoil(1, DeviceDescriptor.Extra["NoLoadMaterialOnOffWritePlc"].ToUshort(), loadMaterialVale);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void IniAgvPosition()
    {
        spindleAgvPosition = Enumerable.Repeat("null", spindleNum).ToArray();
        var tmpSpindeles = DeviceDescriptor.Extra["Spindles"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
        Array.Copy(tmpSpindeles, spindleAgvPosition, spindleAgvPosition.Length);
    }

    protected override async Task OnApplicationStarted()
    {
        await base.OnApplicationStarted();
        if (PayloadPanels.Count() == 0)
        {
            PayloadPanels.AddRange(realDrillDevice.InitPayloadPanels());
            PayloadPanels.SetLocationCode(DeviceId);
        }
        if (DeviceDescriptor.AutoMode) await PayloadPanels.RaiseCollectionChangedEvent(DeviceId);
        if (agvOperationTypes == 3 && PayloadCutterTrays.Count() == 0)
        {
            InitPayloadCutterTrays();
            if (DeviceDescriptor.AutoMode) await PayloadCutterTrays.RaiseCollectionChangedEvent(DeviceId);
        }
    }

    protected override Task OnApplicationStopping()
    {
        if (codeReaderOnOff)
        {
            string statistics = $"从{CodeReaderBaseSetting.CodeReaderStartTime}到{DateTime.Now} 触发{CodeReaderBaseSetting.triggerNum} 成功{CodeReaderBaseSetting.successNum}\r\n";
            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "codeReader.txt"), statistics);
        }
        cancellationTokenSource.Cancel();
        if (cnc84Command != null)
        {
            //cnc84Command.Stop();
        }

        if (agvOperationTypes == 1)
        {
            frontExtendDevice.CloseDevice();
        }
        return base.OnApplicationStopping();
    }

    public async Task GetPanelItemCodeAsync(QueryOrderByEnum queryOrder = QueryOrderByEnum.OrderByCreateTimeDesc, string matchItemCode = "")
    {
        if (MaterialModels == null)
        {
            MaterialModels = new List<MaterialModel>() { };
        }
        MaterialModels.Clear();
        MaterialModels.Add(new MaterialModel() { Name = "", Code = "" });
        logger.LogWarning("begin to GetPanelItemCodeAsync");
        var condition = new GetItemListReq { QueryOrderBy = queryOrder, Code = matchItemCode };
        try
        {
            var MaterialModel = (await HttpRequestInvoker.PostAsJsonAsync<GetItemListReq, List<MaterialModel>>(CentralWebOptions.GetItemCode, condition));
            if (MaterialModel != null && !string.IsNullOrWhiteSpace(MaterialMatchCode))
            {
                MaterialModel = MaterialModel.Where(s => s.Code.ToLower().Contains(MaterialMatchCode.ToLower())).ToList();
            }

            MaterialModels.AddRange(MaterialModel);
        }
        catch (Exception ee)
        {
            logger.LogError($"获取板材信息异常 {ee.Message}");
        }

        logger.LogWarning("end to GetPanelItemCodeAsync");
    }

    public void LoadFileFromCenter()
    {
        logger.LogDebug("Drill page 界面 加载文件--压板--蘑菇头按钮被点击了");
        var eventHandler = EventContainer[typeof(DefaultDrill)] as DefaultDrillEventHandler;
        eventHandler?.drillEventHandler.LoadFileFromCenter();
    }

    public bool IsTimeout(DateTime startTime, int timeout)
    {
        if (DateTime.Now > startTime.AddSeconds(timeout))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanProceedNextStep(bool stopForcedly = false)
    {
        var result = false;
        if (stopForcedly)
        {
            logger.LogDebug("手动停止");
            return true;
        }

        if (Status == DeviceStatus.Exception)
        {
            var message = "结束任务：监控到 Status = DeviceStatus.Exception 请检查PLC机构异常或程序异常";
            logger.LogDebug(message);
            ReportingProcess(message, AlarmLevel.Severe);
            return true;
        }

        return result;
    }

    public Task ReportingProcess(string processMessage, AlarmLevel information = AlarmLevel.Information)
    {
        var request = new AddScheduleLogRequest
        {
            DeviceId = DeviceDescriptor.DeviceId,
            TraceId = NewTranscationIdTemp,
            Message = processMessage
        };

        HttpRequestInvoker.PostAsJsonAsync<AddScheduleLogRequest, AddScheduleLogResponse>(CentralWebOptions.ProcessReportSchedule, request);

        DataExporter.DeviceAlarmReport(new DeviceAlarmReportRequest
        {
            ProductId = DeviceDescriptor.ProductId,
            DeviceId = DeviceDescriptor.DeviceId,
            RequestDeviceKind = DeviceDescriptor.DeviceKind,
            AlarmCode = "",
            AlarmContent = processMessage,
            AlarmLevel = information,
            AlarmTime = DateTime.Now,
            AlarmKind = AlarmKind.Unknown,
        });

        return Task.CompletedTask;
    }
}
