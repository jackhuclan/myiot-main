using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Modbus.Device;
using VgAutoDrill.Fundation.CNC;
using VgAutoDrill.Fundation.Interaction;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.UnPin.Plc;

namespace VgDeviceGateway.Devices.UnPin;

public class UnPin : Device
{
    private readonly ILogger<UnPin> logger;
    public readonly int spindleNum = 3;
    public readonly int layerLimit = 18;
    public bool shelfIsReady = false;
    public volatile bool allowAllAgv = true;
    private readonly int CallAgvTimeInterval = 0;
    private readonly int AgvEndTimeInterval = 0;
    public string[] spindleAgvPosition;
    public string[] spindleInnerAgvPosition;
    public bool isAgvOnWorkSilo = false;
    public bool isReady = false;
    public string[] transSpindleAgvPosition;
    public string[] transSpindleInnerAgvPosition;
    public string[] transSpindMiddleAgvPosition;
    public string[] transSpindIdeAgvPosition;
    public string[] spindIdePosition;
    public readonly IModbusIpMasterWrapper modbusIpMasterWrapper;
    public IModbusMaster modbusIpMaster;
    public readonly ILoadMaterialInteractionPolicy UnPin_LoadMaterial_InteractionPolicy;
    public readonly IUnloadMaterialInteractionPolicy UnPin_UnloadMaterial_InteractionPolicy;
    public IPlcHandle UnPinPlcHandler;
    public bool plcConnentFlag = false;
    public string ItemNo = "";
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);

    /// <summary>
    /// 获取成品物料代码
    /// </summary>
    public List<MaterialModel> MaterialModels { get; set; } = new List<MaterialModel>();

    public List<string> SiloCodes { get; set; } = new List<string>();
    public Dictionary<string, LocationData<UnPin>> WatchShelfProperty { get; set; }

    public ConcurrentDictionary<string, string> TransactionIds = new ConcurrentDictionary<string, string>();
    public ConcurrentDictionary<string, string> TransactionMessage = new ConcurrentDictionary<string, string>();
    public string TransIds { get; set; }
    public string ShelfIndex { get; set; } = "1";

    public Dictionary<int, DateTime> callAgvTimes;

    public UnPin(DeviceDescriptor deviceDescriptor, IServiceProvider serviceProvider, IDeviceEngine deviceEngine, IObjectFactory objectFactory)
        : base(deviceDescriptor, deviceEngine, serviceProvider)
    {
        logger = LoggerFactory.CreateLogger<UnPin>();
        spindleNum = deviceDescriptor.SpindleNum.ToInt();
        layerLimit = deviceDescriptor.LayerLimit.ToInt();
        CallAgvTimeInterval = deviceDescriptor.Extra["CallAgvTimeInterval"].ToInt();
        AgvEndTimeInterval = deviceDescriptor.Extra["AgvEndTimeInterval"].ToInt();
        UnPin_LoadMaterial_InteractionPolicy = objectFactory.CreateObject<UnPin_LoadMaterial_InteractionPolicy>(this);
        UnPin_UnloadMaterial_InteractionPolicy = objectFactory.CreateObject<UnPin_UnloadMaterial_InteractionPolicy>(this);
        UnPinPlcHandler = objectFactory.CreateObject<UnPinPlcHandler>(this);
        modbusIpMasterWrapper = ((IModbusOperator)Engine.DeviceConnector).ModbusIpMasterWrapper;
        Connector.ConnectFunc = (deviceDescriptor) => Task.Run(() =>
        {
            logger.LogInformation("Connector.ConnectFunc Excute OneMore Times...");
            if (!DeviceDescriptor.Extra["NeedPlc"].ToBool()) return true;

            if (modbusIpMaster == null || !Connector.IsConnected)
            {
                // UnPinPlcHandler.Stop();
                if (PlcConnect())
                {
                    //  return UnPinPlcHandler.Start();
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        });

        Connector.HeartBeatFunc = () => KeepConnect();

        IniAgvPosition();
        InitWatchShelfCallAgvProperty();

        PropertyContainer.AddHandler<UnPinPropertyHandler, UnPin>(this);
        StateContainer.AddHandler<UnPinStateHandler, UnPin>(this);
        EventContainer.AddHandler<UnPinEventHandler, UnPin>(this);
        AlarmContainer.AddHandler<UnPinAlarmHandler, UnPin>(this);
        CollectDataFunc = (d) => PropertyContainer[d.GetType()].CollectPropertyValues();

        _ = GetPanelItemCodeAsync();

        callAgvTimes = new Dictionary<int, DateTime>();
        for (int i = 1; i <= spindleNum; i++)
        {
            callAgvTimes.Add(i, DateTime.Now);
        }

        AddCommand("SetUnPinSiloCommand", (r) => SetUnPinSiloHandler(r));
    }

    public string GetLocationCode(int position)
    {
        return $"{DeviceId}{position.ToStr().PadLeft(3, '0')}";
    }

    private async Task<DeviceServiceInvokeResponse> SetUnPinSiloHandler(DeviceServiceInvokeRequest request)
    {
        return await Task.Run(async () =>
        {
            logger.LogInformation($"SetUnPinSiloCommand SetUnPinSiloHandler request:{System.Text.Json.JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");

            try
            {
                string? deviceCode = request?.Params?["DeviceCode"].ToStr();
                if (string.IsNullOrEmpty(deviceCode)) throw new Exception("参数错误: DeviceCode 为空");

                int position = deviceCode.Substring(deviceCode.Length - 3).ToInt();
                if (position <= 0) throw new Exception("无效的DeviceCode");

                //await PayloadPanels.ChangeListSafely(Task.Run(() =>
                //{
                var payPanels = PayloadPanels.Where(p => p.Position == position).ToList();
                payPanels?.ForEach(p =>
                {
                    var temp = request.PayloadPanels.FirstOrDefault(t => t.Layer == p.Layer);
                    if (temp != null)
                    {
                        p.ItemCode = temp.ItemCode;
                        p.SiloCode = temp.SiloCode;
                        p.PanelCode = temp.PanelCode;
                        p.ProductStatus = temp.ProductStatus;
                    }
                });
                //}));
                await PayloadPanels.RaiseCollectionChangedEvent(GetLocationCode(position));
                await CancelSchdule(position);

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
        });
    }

    protected override async Task OnApplicationStarted()
    {
        SiloCodes.Add("");
        SiloCodes.AddRange(await this.GetSiloCodeAsync());

        await DeviceStore.LoadPayloadPanels(this);
        logger.LogInformation($"PayloadPanels: {System.Text.Json.JsonSerializer.Serialize(PayloadPanels)}");

        LoadPanels();
    }

    public void LoadPanels()
    {
        try
        {
            _autoResetEvent.WaitOne();
            foreach (var location in WatchShelfProperty)
            {
                location.Value?.LoadPanels();
            }
        }
        finally
        {
            _autoResetEvent.Set();
        }
    }

    private bool PlcConnect()
    {
        try
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}   begin to PlcConnect!");
            modbusIpMaster = modbusIpMasterWrapper.CreateIp(DeviceDescriptor.Extra["Ip"].ToStr(), DeviceDescriptor.Extra["Port"].ToInt());
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}   finish  PlcConnect!");

            return modbusIpMaster != null;
        }
        catch (Exception e)
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}  PlcConnect {e.Message}!");
        }

        return false;
    }

    public Task KeepConnect()
    {
        return Task.Run(async () =>
        {
            if (!DeviceDescriptor.Extra["NeedPlc"].ToBool()) return;

            if (modbusIpMaster == null || Connector.IsConnected == false)
            {
                logger.LogInformation($"KeepConnect modbusIpMaster=null,ConnectStatus={Connector.IsConnected}");
                Connector.IsConnected = false;
                return;
            }

            try
            {
                var plcValue = (await modbusIpMaster.ReadHoldingRegistersAsync(1, 0, 1))[0];
                // Connector.IsConnected = true;
            }
            catch (Exception e)
            {
                logger.LogError($"KeepConnect: PLC 通讯异常:{e.Message}");
                Connector.IsConnected = false;
            }
        });
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
        var MaterialModel = (await HttpRequestInvoker.PostAsJsonAsync<GetItemListReq, List<MaterialModel>>(CentralWebOptions.GetItemCode, condition));
        if (!string.IsNullOrEmpty(WatchShelfProperty[ShelfIndex].CurrentContext.MaterialMatchCode))
        {
            MaterialModel = MaterialModel.Where(p => p.Code.Contains(WatchShelfProperty[ShelfIndex].CurrentContext.MaterialMatchCode)).ToList();
        }
        MaterialModels.AddRange(MaterialModel);
        logger.LogWarning("end to GetPanelItemCodeAsync");
    }

    public override Task<DeviceServiceInvokeResponse> CompleteSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        string shelfIndex = deviceServiceInvokeRequest.EventId!.Split('#')[1];
        switch (deviceServiceInvokeRequest.ScheduledStatus)
        {
            case VgAutoDrill.Fundation.Iot.Schedule.ScheduledTaskStatus.Completed:
                logger.LogDebug($"CompleteSchedule --Completed：重置 IsCanCallAgv= true");
                callAgvTimes[shelfIndex.ToInt()] = DateTime.Now;
                WatchShelfProperty[shelfIndex].IsCanCallAgv = true;
                TransactionIds[shelfIndex] = string.Empty;
                break;

            case VgAutoDrill.Fundation.Iot.Schedule.ScheduledTaskStatus.Failed:
                logger.LogError($"CancelSchedule --Failed：人工处理，手动再次呼叫");
                WatchShelfProperty[shelfIndex].IsReady = false;
                break;

            case VgAutoDrill.Fundation.Iot.Schedule.ScheduledTaskStatus.Canceled:
                logger.LogDebug($"CancelSchedule --Canceled：重置 IsCanCallAgv= true,重新呼叫");
                callAgvTimes[shelfIndex.ToInt()] = DateTime.Now;
                TransactionIds[shelfIndex] = string.Empty;
                WatchShelfProperty[shelfIndex].IsCanCallAgv = true;
                break;

            default:
                break;
        }

        return base.CompleteSchedule(deviceServiceInvokeRequest);
    }

    public override Task<DeviceServiceInvokeResponse> CancelSchedule(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        string shelfIndex = deviceServiceInvokeRequest.EventId!.Split('#')[1];
        callAgvTimes[shelfIndex.ToInt()] = DateTime.Now;
        WatchShelfProperty[shelfIndex].IsCanCallAgv = true;
        return base.CancelSchedule(deviceServiceInvokeRequest);
    }

    public bool SiloIsNotExistByPosition(int position)
    {
        var subPanels = PayloadPanels.GetRange((position - 1) * layerLimit, layerLimit);
        return subPanels.All(s => s.ProductStatus == ProductStatus.EmptyPayload);
    }

    public bool SiloIsEmptytByPosition(int position)
    {
        var subPanels = PayloadPanels.GetRange((position - 1) * layerLimit, layerLimit);
        return subPanels.All(s => s.ProductStatus == ProductStatus.EmptySiloBox);
    }

    public bool ExistEmptySilo()
    {
        for (int i = 0; i < spindleNum; i++)
        {
            if (SiloIsNotExistByPosition(i + 1))
            {
                return true;
            }
        }

        return false;
    }

    public void UpdateSiloStatus(Panel panel)
    {
        PayloadPanels[(panel.Position - 1) * layerLimit + panel.Layer - 1] = panel;
    }

    public void UpdateSiloInfoByPosition(int position, List<Panel> panels)
    {
        panels.ForEach(p =>
        {
            p.LocationCode = GetLocationCode(position);
        });

        int panelsIndex = 0;

        for (int i = (position - 1) * layerLimit; i < position * layerLimit; i++)
        {
            if (panelsIndex > layerLimit || panelsIndex >= panels.Count)
            {
                return;
            }

            PayloadPanels[i] = panels[panelsIndex++];
        }

        PayloadPanels.RaiseCollectionChangedEvent(GetLocationCode(position));
    }

    public void UpdateSiloInfoByPositionAndIndex(int position, int index, List<Panel> panels)
    {
        panels.ForEach(p =>
        {
            p.LocationCode = GetLocationCode(position);
        });

        int realIndex = (position - 1) * layerLimit + index;
        for (int i = 0; i < panels.Count; i++)
        {
            PayloadPanels[realIndex++] = panels[i];
        }

        PayloadPanels.RaiseCollectionChangedEvent(GetLocationCode(position));
    }

    public void UpdatePanelStatusFromEmptyPayloadToEmptySiloBox(int position)
    {
        for (int i = (position - 1) * layerLimit; i < position * layerLimit; i++)
        {
            if (PayloadPanels[i].ProductStatus == ProductStatus.EmptyPayload)
            {
                PayloadPanels[i].ProductStatus = ProductStatus.EmptySiloBox;
            }
        }
    }

    public override async Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        var result = await PropertyContainer[GetType()].ReadProperties(deviceServiceInvokeRequest);

        return result;
    }

    public override async Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await PropertyContainer[GetType()].WriteProperties(deviceServiceInvokeRequest);
    }

    public void IniAgvPosition()
    {
        spindleAgvPosition = Enumerable.Repeat("null", spindleNum).ToArray();
        var tmpSpindeles = DeviceDescriptor.Extra["ShelfAgvPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
        Array.Copy(tmpSpindeles, spindleAgvPosition, spindleAgvPosition.Length);
        spindleInnerAgvPosition = Enumerable.Repeat("null", spindleNum).ToArray();
        var tmpInnerSpindeles = DeviceDescriptor.Extra["ShelfInnerPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
        Array.Copy(tmpInnerSpindeles, spindleInnerAgvPosition, spindleInnerAgvPosition.Length);

        transSpindleAgvPosition = Enumerable.Repeat("null", spindleNum).ToArray();
        var transtmpSpindeles = DeviceDescriptor.Extra["TransShelfAgvPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
        Array.Copy(transtmpSpindeles, transSpindleAgvPosition, transSpindleAgvPosition.Length);
        transSpindleInnerAgvPosition = Enumerable.Repeat("null", spindleNum).ToArray();
        var transtmpInnerSpindeles = DeviceDescriptor.Extra["TransShelfInnerPositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
        Array.Copy(transtmpInnerSpindeles, transSpindleInnerAgvPosition, transSpindleInnerAgvPosition.Length);

        transSpindMiddleAgvPosition = Enumerable.Repeat("null", spindleNum).ToArray();
        var transtmpMiddleSpindeles = DeviceDescriptor.Extra["transSpindMiddleAgvPosition"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
        Array.Copy(transtmpMiddleSpindeles, transSpindMiddleAgvPosition, transSpindMiddleAgvPosition.Length);

        spindIdePosition = Enumerable.Repeat("null", spindleNum).ToArray();
        var spindIdePositions = DeviceDescriptor.Extra["ShelfAgvIdlePositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
        Array.Copy(spindIdePositions, spindIdePosition, spindIdePosition.Length);

        transSpindIdeAgvPosition = Enumerable.Repeat("null", spindleNum).ToArray();
        var transSpindIdeAgvPositions = DeviceDescriptor.Extra["TransShelfIdlePositionList"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
        Array.Copy(transSpindIdeAgvPositions, transSpindIdeAgvPosition, transSpindIdeAgvPosition.Length);
    }

    public void InitWatchShelfCallAgvProperty()
    {
        WatchShelfProperty = new Dictionary<string, LocationData<UnPin>>();
        for (int i = 1; i <= spindleNum; i++)
        {
            WatchShelfProperty[$"{i}"] = ObjectFactory.CreateObject<LocationData<UnPin>>(this);
            WatchShelfProperty[$"{i}"].Panels.CollectionChanged += OnPanelsChanged;
            WatchShelfProperty[$"{i}"].Position = i.ToStr();
        }
    }

    protected DeviceStatusReportRequest GetStatusRequest(DeviceStatus newStatus)
    {
        var request = new DeviceStatusReportRequest
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            NewStatus = newStatus,
            OldStatus = this.Status
        };
        request.PayloadPanels = PayloadPanels;

        return request;
    }

    public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await UnPin_LoadMaterial_InteractionPolicy.PrepareLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await UnPin_LoadMaterial_InteractionPolicy.InvokeLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await UnPin_LoadMaterial_InteractionPolicy.CompleteLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await UnPin_UnloadMaterial_InteractionPolicy.PrepareUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await UnPin_UnloadMaterial_InteractionPolicy.InvokeUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await UnPin_UnloadMaterial_InteractionPolicy.CompleteUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    public override async Task<DeviceServiceInvokeResponse> Shutdown(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    public override async Task<DeviceServiceInvokeResponse> Standby(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    public override async Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await ResponseSuccess();
    }

    public async Task<ushort[]> ReadPlc(CurrentContext currentContext)
    {
        switch (currentContext.PlcHandleType)
        {
            case 0:
                return await modbusIpMaster.ReadInputRegistersAsync(1, currentContext.PlcAddr.ToUshort(), currentContext.PlcLength.ToUshort());

            case 1:
                logger.LogInformation($"准备读取PLC。。。:");
                var result = await modbusIpMaster.ReadHoldingRegistersAsync(1, currentContext.PlcAddr.ToUshort(), currentContext.PlcLength.ToUshort());
                logger.LogInformation($"原始值: {System.Text.Json.JsonSerializer.Serialize(result)}");
                //logger.LogInformation($"读取的值位:{code}");
                // logger.LogInformation($"批量读取的字符串:{result.UshortToStrings()}");
                //  return result;
                var Layer = modbusIpMaster.ReadHoldingRegisters(1, 43, 1)[0];
                var barcode = modbusIpMaster.ReadHoldingRegisters(1, 1, 19).UshortToStrings().Replace("\0", "").Replace("\r", "");
                var itemCode = modbusIpMaster.ReadHoldingRegisters(1, 60, 20).UshortToStrings().Replace("\0", "").Replace("\r", "");
                var panelLength = modbusIpMaster.ReadHoldingRegisters(1, 80, 2).UshortToFloat();
                var panelWidth = modbusIpMaster.ReadHoldingRegisters(1, 82, 2).UshortToFloat();
                var PinOffset = modbusIpMaster.ReadHoldingRegisters(1, 84, 2).UshortToFloat();
                var stackCount = modbusIpMaster.ReadHoldingRegisters(1, 86, 2).UshortToFloat();
                var testVal = modbusIpMaster.ReadHoldingRegisters(1, 90, 2).UshortToFloat();
                var thickness = modbusIpMaster.ReadHoldingRegisters(1, 88, 2).UshortToFloat();
                // logger.LogInformation($"HandleLeftPanelFinish: 原始值: panelLength:{JsonSerializer.Serialize(modbusIpMaster.ReadHoldingRegisters(1, 80, 2))},panelWidth:{JsonSerializer.Serialize(modbusIpMaster.ReadHoldingRegisters(1, 82, 2))}, PinOffset:{JsonSerializer.Serialize(modbusIpMaster.ReadHoldingRegisters(1, 84, 2))},StackCount:{modbusIpMaster.ReadHoldingRegisters(1, 86, 2)}");
                logger.LogDebug($"HandleLeftPanelFinish: 条码: {barcode},层号:{Layer},板厚:{thickness}" +
                    $"料号:{itemCode},panelLength:{panelLength},PanelWidth:{panelWidth},PinOffset:{PinOffset},stackCount:{stackCount},testVal:{testVal}");
                return result;

            default:
                return new ushort[] { };
        }
    }

    private int index = 1;

    public float ReadCoilPlc(CurrentContext currentContext)
    {
        ushort[] ud = null;
        if (index == 1)
        {
            ud = new ushort[2] { 6864, 1 }; // 72400
        }
        else if (index == 2)
        {
            ud = new ushort[2] { 54500, 0 };
        }
        else if (index == 3)
        {
            ud = new ushort[2] { 3064, 1 };
        }
        else if (index == 4)
        {
            ud = new ushort[2] { 2, 0 };
        }
        else if (index == 5)
        {
            ud = new ushort[2] { 0, 0 };
        }
        else
        {
            return 0;
        }
        index++;
        var floatVal = ud.UshortToFloat();
        return floatVal;
    }

    public async Task<bool> WritePlc(CurrentContext currentContext)
    {
        switch (currentContext.PlcHandleType)
        {
            case 3:
                await modbusIpMaster.WriteSingleRegisterAsync(1, Convert.ToUInt16(currentContext.PlcAddr), currentContext.PlcValue.ToUshort());
                break;

            case 4:
                var writeVal = currentContext.PlcValue.ToFloat().StringFloatToUshort();
                logger.LogDebug($"WritePlc: {System.Text.Json.JsonSerializer.Serialize(writeVal)}");
                await modbusIpMaster.WriteMultipleRegistersAsync(1, Convert.ToUInt16(currentContext.PlcAddr), writeVal);
                break;

            case 5:
                var value = currentContext.PlcValue.ToFloat().FloatToReal();
                //  ushort[] val = new ushort[2] { 6864, 1 };
                logger.LogDebug($"WritePlc: {System.Text.Json.JsonSerializer.Serialize(value)}");
                await modbusIpMaster.WriteMultipleRegistersAsync(1, Convert.ToUInt16(currentContext.PlcAddr), value);
                //   await modbusIpMaster.WriteMultipleRegistersAsync(1, Convert.ToUInt16(currentContext.PlcAddr), val);
                break;

            default:
                break;
        }

        return true;
    }

    public async Task<bool> ResetSignal(int type)
    {
        switch (type)
        {
            case 1:
                modbusIpMaster?.WriteSingleRegister(1, 46, 1);
                return true;

            case 2:
                modbusIpMaster?.WriteSingleRegister(1, 45, 1);
                return true;

            case 3:
                modbusIpMaster?.WriteSingleRegister(1, 47, 1);
                return true;

            case 4:
                modbusIpMaster?.WriteSingleRegister(1, 56, 1);
                return true;

            case 5:
                modbusIpMaster?.WriteSingleRegister(1, 55, 1);
                return true;

            case 6:
                modbusIpMaster?.WriteSingleRegister(1, 57, 1);
                return true;
        }

        return false;
    }

    public async Task<CancelScheduleTaskResponse?> CancelSchdule(int position)
    {
        var requestParam = new CancelScheduleTaskRequest()
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            Params = new Dictionary<string, object?>()
                       {
                           { "DeviceCode", DeviceDescriptor.DeviceId + position.ToString().PadLeft(3, '0')}
                       }
        };

        logger.LogInformation($"CancelSchdule: {System.Text.Json.JsonSerializer.Serialize(requestParam)}");
        return await HttpRequestInvoker.PostAsJsonAsync<CancelScheduleTaskRequest, CancelScheduleTaskResponse>(DeviceDescriptor.Extra["CancelSchedule"].ToStr(), requestParam);
    }

    public async Task<BaseResponse> CallAgv(int unPinIndex, bool forceCall)
    {
        BaseResponse response = new BaseResponse();

        try
        {
            if (!forceCall)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(callAgvTimes[unPinIndex]);
                if (timeSpan.TotalSeconds < CallAgvTimeInterval)
                {
                    response.Message = $" 小于呼叫间隔不能呼叫";
                    return response;
                }

                callAgvTimes[unPinIndex] = DateTime.Now;

                if (!WatchShelfProperty[$"{unPinIndex}"].IsCanCallAgv)
                {
                    response.Message = $" 呼叫agv信号为 false 不能呼叫";
                    return response;
                }
            }

            if (!MqttClientWrapper.IsConnected)
            {
                response.Message = "呼叫AGV Mqtt 连接断开 不上报消息";
                return response;
            }

            var agvPosition = spindleAgvPosition;
            var agvInnerPosition = spindleInnerAgvPosition;

            string behavior = await GetBeavior(unPinIndex);

            int callAgvSplinesStatus = unPinIndex - 1;
            var unpinItemNo = DeviceDescriptor.Extra["NeedPlc"].ToBool() ? WatchingProperties.Property("PlcItemNo").NewValue.ToStr() : WatchShelfProperty[$"{unPinIndex}"].CurrentContext.ItemCode;
            logger.LogInformation($"上报 信息：  位置{unPinIndex}  动作 {behavior}  forceCall:{forceCall} ");
            if (behavior == "0")  //无料仓呼叫 上料
            {
                var req = new DeviceEventReportRequest()
                {
                    ProductId = DeviceDescriptor.ProductId,
                    DeviceId = DeviceDescriptor.DeviceId,
                    ClientId = ClientId,
                    EventId = Events.REQUEST_AGV_LOAD_SILO_ONLY + "#" + unPinIndex,
                    RequestInputProductStatus = ProductStatus.PRE_UNPIN_TRANSFER_AGV_OUTPUT_1,
                    RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.FRONT_LOAD_SILO_ONLY),
                    RequestDeviceKind = DeviceDescriptor.DeviceKind,
                    RequestMaterialKind = MaterialKind.PanelSilo,
                    RequestInteractionDirection = InteractionPosition.Front,
                    Params = new Dictionary<string, object?>()
                        {
                            { "SpindleNum", 1 },
                            { "ShelfIndex",unPinIndex },
                            { "Spindles",  $"{spindleAgvPosition[callAgvSplinesStatus]}"},
                            { "ShelfInnerPos",  $"{spindleInnerAgvPosition[callAgvSplinesStatus]}"},
                            { "TransSpindles",  $"{transSpindleAgvPosition[callAgvSplinesStatus]}"},
                            { "TransShelfInnerPos",  $"{transSpindleInnerAgvPosition[callAgvSplinesStatus]}"},
                            { "TransActionPos",  $"{transSpindMiddleAgvPosition[callAgvSplinesStatus]}"},
                            { "ShelfAgvIdlePos",  $"{spindIdePosition[callAgvSplinesStatus]}"},
                            { "TransShelfAgvIdlePos",  $"{transSpindIdeAgvPosition[callAgvSplinesStatus]}"},
                            { "SpindleBehavior", behavior },
                            { "DeviceCode",DeviceDescriptor.DeviceId+unPinIndex.ToString().PadLeft(3,'0')},
                            { "SiloCode",""},
                            { "UnpinItemCode",unpinItemNo},
                            {"ForceCall",forceCall},
                            {"AssignAGV",WatchShelfProperty[$"{unPinIndex}"].CurrentContext.AGVCode }
                        },

                    PayloadPanels = PanelList.FromList(PayloadPanels.GetRange((unPinIndex - 1) * layerLimit, layerLimit))
                };

                logger.LogInformation($"{unPinIndex}号工位 上报 上料 请求参数：  {System.Text.Json.JsonSerializer.Serialize(req)}  ");

                var result = await DataExporter.DeviceEventReport(req);

                if (result == null)
                {
                    response.Message = $"{DateTime.Now.ToString()}  ----  和服务器断开连接，呼叫失败";
                }
                else
                {
                    if (result.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        WatchShelfProperty[$"{unPinIndex}"].IsCanCallAgv = false;
                        TransactionIds[$"{unPinIndex}"] = result.TraceId;
                        response.Code = 0;
                        response.Message = $"{DateTime.Now.ToString()}  ----呼叫成功   {result.TraceId}";
                    }
                    else if (result.Code == ErrorCodes.Sys.FAIL)
                    {
                        response.Message = $"{DateTime.Now.ToString()}  ----呼叫失败  {result.Message}";
                    }
                }

                logger.LogInformation($"{unPinIndex}号工位 上报 上料 信息结果：  {System.Text.Json.JsonSerializer.Serialize(result)}  ");

                return response;
            }
            else if (behavior == "1")  //有料仓呼叫 下料
            {
                var req = new DeviceEventReportRequest()
                {
                    ProductId = DeviceDescriptor.ProductId,
                    DeviceId = DeviceDescriptor.DeviceId,
                    ClientId = ClientId,
                    EventId = Events.REQUEST_AGV_UNLOAD_SILO_ONLY + "#" + unPinIndex,
                    RequestOutputProductStatus = ProductStatus.EmptySiloBox,
                    RequestInteractionBehavior = InteractionBehavior.Make(this.DeviceDescriptor.DeviceKind, InteractionBehavior.FRONT_UNLOAD_SILO_ONLY),
                    RequestDeviceKind = DeviceDescriptor.DeviceKind,
                    RequestMaterialKind = MaterialKind.PanelSilo,
                    RequestInteractionDirection = InteractionPosition.Front,
                    Params = new Dictionary<string, object?>()
                      {
                            { "SpindleNum", 1 },
                            { "ShelfIndex",unPinIndex },
                            { "Spindles",  $"{spindleAgvPosition[callAgvSplinesStatus]}"},
                            { "ShelfInnerPos",  $"{spindleInnerAgvPosition[callAgvSplinesStatus]}"},
                            { "TransSpindles",  $"{transSpindleAgvPosition[callAgvSplinesStatus]}"},
                            { "TransShelfInnerPos",  $"{transSpindleInnerAgvPosition[callAgvSplinesStatus]}"},
                            { "TransActionPos",  $"{transSpindMiddleAgvPosition[callAgvSplinesStatus]}"},
                            { "ShelfAgvIdlePos",  $"{spindIdePosition[callAgvSplinesStatus]}"},
                            { "TransShelfAgvIdlePos",  $"{transSpindIdeAgvPosition[callAgvSplinesStatus]}"},
                            { "SpindleBehavior", behavior },
                            { "DeviceCode",DeviceDescriptor.DeviceId+unPinIndex.ToString().PadLeft(3,'0')},
                            { "SiloCode",PayloadPanels.GetRange((unPinIndex - 1) * layerLimit, layerLimit).FirstOrDefault(p=>!string.IsNullOrEmpty(p.SiloCode))?.SiloCode },
                            {"ForceCall",forceCall},
                            {"AssignAGV",WatchShelfProperty[$"{unPinIndex}"].CurrentContext.AGVCode }
                      },

                    PayloadPanels = PanelList.FromList(PayloadPanels.GetRange((unPinIndex - 1) * layerLimit, layerLimit))
                };
                logger.LogInformation($"{unPinIndex}号工位 上报 下料 请求参数：  {System.Text.Json.JsonSerializer.Serialize(req)}  ");
                var result = await DataExporter.DeviceEventReport(req);
                if (result == null)
                {
                    response.Message = $"{DateTime.Now.ToString()}  ----  和服务器断开连接，呼叫失败";
                }
                else
                {
                    if (result.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        WatchShelfProperty[$"{unPinIndex}"].IsCanCallAgv = false;
                        TransactionIds[$"{unPinIndex}"] = result.TraceId;
                        response.Code = 0;
                        response.Message = $"{DateTime.Now.ToString()}  ----呼叫成功   {result.TraceId}";
                    }
                    else if (result.Code == ErrorCodes.Sys.FAIL)
                    {
                        response.Message = $"{DateTime.Now.ToString()}  ----呼叫失败  {result.Message}";
                    }
                }

                logger.LogInformation($" {unPinIndex}号工位 上报 下料 信息结果：  {System.Text.Json.JsonSerializer.Serialize(result)}  ");

                return response;
            }
            else
            {
                response.Message = $"不呼叫AGV ,{unPinIndex}号工位: 请检查料仓信息是否与实际相符";
                return response;
            }
        }
        catch (Exception ee)
        {
            logger.LogError($"{unPinIndex}号工位 呼叫AGV异常 ： {ee.Message} ");
            response.Message = $"呼叫AGV异常 ： {ee.Message} ";
            return response;
        }
    }

    public async Task<string> GetBeavior(int upinIndex)
    {
        if (SiloIsNotExistByPosition(upinIndex))
        {
            return "0";
        }
        else if (SiloIsEmptytByPosition(upinIndex))
        {
            return "1";
        }
        else
        {
            return "-1";
        }
    }

    public async Task<string> ForeCallAgv(int SpindleNum)
    {
        string message = "";

        logger.LogInformation($"ForeCallAgv {SpindleNum}号料架 ");

        WatchShelfProperty[$"{SpindleNum}"].IsReady = false;

        var cancelSchduleResult = await CancelSchdule(SpindleNum);
        if (cancelSchduleResult != null && cancelSchduleResult.Code == ErrorCodes.Sys.SUCCESS)
        {
            logger.LogInformation($"ForeCallAgv： {SpindleNum}号料架 取消后 准备强制呼叫AGV ...");
            await Task.Delay(4 * 1000);
            logger.LogInformation($"ForeCallAgv： {SpindleNum}号料架 取消后 执行强制呼叫AGV ...");
        }
        else
        {
            message = cancelSchduleResult == null ? "接口无返回" : cancelSchduleResult.Message;
            logger.LogInformation($"ForeCallAgv： {SpindleNum}号料架 取消接口返回 {message}");
        }

        var result = await CallAgv(SpindleNum, true);
        if (result != null && result.Code == 0)
        {
            callAgvTimes[SpindleNum] = DateTime.Now;
        }

        message = result?.Message!;
        logger.LogInformation($"ForeCallAgv： {SpindleNum}号料架 强制呼叫结果:{System.Text.Json.JsonSerializer.Serialize(result)}");

        WatchShelfProperty[$"{SpindleNum}"].IsReady = true;
        TransactionMessage[$"{SpindleNum}"] = message;
        WatchingProperties.Property("TranscationChange").SetValue(true);
        return message;
    }

    public async Task<MaterialPanelInfo> GetPanelItemInfoAsync(string itemNo)
    {
        logger.LogInformation($"GetPanelItemInfoAsync: req: {itemNo}");
        var result = await HttpRequestInvoker.GetFromJsonAsync<MaterialPanelInfo>(DeviceDescriptor.Extra["GetItemInfo"].ToStr(), new Dictionary<string, object>() { { "itemCode", itemNo } });
        logger.LogInformation($"GetPanelItemInfoAsync: rsp: {System.Text.Json.JsonSerializer.Serialize(result)}");
        return result;
    }
}
