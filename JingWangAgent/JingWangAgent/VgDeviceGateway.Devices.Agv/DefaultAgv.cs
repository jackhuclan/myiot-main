using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modbus.Device;
using Org.BouncyCastle.Asn1.Ocsp;
using VgAutoDrill.Fundation.CNC;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Agv.AgvDevice;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Common.Agv;
using static VgDeviceGateway.Devices.Common.Agv.Hik.HikCarStatus;

namespace VgDeviceGateway.Devices.Agv;

public class DefaultAgv : Vehicle
{
    private readonly IModbusIpMasterWrapper modbusIpMasterWrapper;
    public readonly ILogger<DefaultAgv> logger;
    public IConfiguration configuration;
    private readonly int postAndGetTimeOut;
    private readonly int reportRunStatusTimeOut;
    public volatile int moveTimeout;
    public readonly int readCodeLength;
    private readonly Uri plcUri;
    public volatile MaterialKind materialType = MaterialKind.Panel;
    public string CurrentRoutingKey = string.Empty;
    public string targetProductId = string.Empty;
    public string targetDeviceId = string.Empty;
    public int currentLoadedCount = 0;
    public int currentUnloadedCount = 0;
    public List<MaterialModel> MaterialModels { get; set; } = new List<MaterialModel>();
    public List<string> SiloCodes { get; set; } = new List<string>();
    public WatchableProperty IsWorkingProperty { get; private set; }
    public readonly byte slaveId;
    public readonly Dictionary<string, object> configExtra;
    public Dictionary<string, object> plcWaringInfo = new Dictionary<string, object>();
    public volatile Dictionary<string, object> CarInfoExtra = new Dictionary<string, object>();
    public readonly int waitPlcSignalTimeout;
    public volatile bool isMoving = false;
    public volatile bool isWorking = false;
    public volatile bool isCharging = false;
    public volatile bool isAgvWorkFail = false;
    public volatile string currentEventTraceId = string.Empty;
    public IModbusMaster modbusIpMaster;
    public volatile bool agvIsReady = false;
    public volatile int loadAndUnLoadLayer = -1;
    public volatile bool isFullSilo = false;
    public volatile string spindlePosition = "";
    public volatile string loadAndUnloadDirection = "";
    public Tuple<string, string, string> errorInfo;
    public readonly AGVToDevice_LoadMaterial_Agv_InteractionPolicy agvToDeviceAgvLoadPolicy;
    public readonly AGVToDevice_UnloadMaterial_Agv_InteractionPolicy agvToDeviceAgvUnloadPolicy;
    public DefaultAgvChassis defaultAgvChassis;
    public IAgvDevice agvDevice;
    public volatile bool sendCancelBeforeArrivedDevice = true;
    public volatile string publicMoveId = "";
    public volatile string currentStepMsg = "";
    public volatile int heartValue = 0;
    public volatile float deviationValue = 0;//X轴偏差值
    public volatile bool IsCheckMovePoint = true;
    public volatile string failTaskCode = "";
    public volatile int errorSignal = 0;
    public volatile string errorsMsg = "";

    public volatile bool checkmoveArrived = false;
    public volatile bool checkPrepareSuccess = false;

    public volatile string AgvTaskId = "";
    public volatile string AgvMoveStartTime = "";
    public volatile string AgvMoveArrivedTime = "";

    public volatile bool AgvCanLeave = true;
    public volatile bool AgvCanLeaveCallBack = false;
    public readonly int askLeaveTimeOut;
    public volatile string panelBarCode = "";
    public volatile string StdGroupNo = "";

    public Dictionary<string, LocationData<DefaultAgv>> Locations { get; set; }
    public string ShelfIndex { get; set; } = "1";

    public DefaultAgv(DeviceDescriptor deviceDescriptor,
        IDeviceEngine deviceEngine,
        IServiceProvider serviceProvider)
        : base(deviceDescriptor, deviceEngine, serviceProvider)
    {
        configuration = ApplicationServices.GetRequiredService<IConfiguration>(); ;

        agvToDeviceAgvLoadPolicy = this.ObjectFactory.CreateObject<AGVToDevice_LoadMaterial_Agv_InteractionPolicy>(this);
        agvToDeviceAgvUnloadPolicy = this.ObjectFactory.CreateObject<AGVToDevice_UnloadMaterial_Agv_InteractionPolicy>(this);

        this.modbusIpMasterWrapper = ((IModbusOperator)Engine.DeviceConnector).ModbusIpMasterWrapper;
        logger = LoggerFactory.CreateLogger<DefaultAgv>();
        configExtra = deviceDescriptor.Extra;

        if (!string.IsNullOrEmpty(deviceDescriptor.Extra["ModbusTcpUri"].ToStr()))
        {
            plcUri = new Uri(deviceDescriptor.Extra["ModbusTcpUri"].ToStr());
        }
        slaveId = deviceDescriptor.Extra["ModbusTcpSlaveId"].ToByte();
        postAndGetTimeOut = configExtra["PostAndGetTimeout"].ToInt();
        waitPlcSignalTimeout = configExtra["WaitPlcSignalTimeout"].ToInt();
        moveTimeout = configExtra["MoveTimeout"].ToInt();
        askLeaveTimeOut = configExtra.GetConfig("AskLeaveTimeOut").ToInt();
        reportRunStatusTimeOut = configExtra["ReportRunStatusTimeOut"].ToInt();
        readCodeLength = configExtra["ReadCodeLength"].ToInt();
        AgvScanResult = true;
        errorInfo = new Tuple<string, string, string>("", "", "");
        agvDevice = InteractionAgvFactory.CreatetAgvDevice(this);

        //初始化空料仓，有料仓没有料
        //InitSiloNoMaterial();
        GetPlcWarningMessage();
        Connector.ConnectFunc = (device) => Task.Run(async () =>
        {
            return await MachineConnect();
        });

        Connector.HeartBeatFunc = () => KeepPlcHeart();

        CollectDataFunc = (d) => PropertyContainer[d.GetType()].CollectPropertyValues();

        PropertyContainer.AddHandler<DefaultAgvPropertyHandler, DefaultAgv>(this);
        StateContainer.AddHandler<DefaultAgvStateHandler, DefaultAgv>(this);
        EventContainer.AddHandler<DefaultAgvEventHandler, DefaultAgv>(this);

        AddCommand("SetAgvCommand", (r) => SetAgvHandler(r));
        AddCommand("AgvMoveCommand", (r) => SetAgvMoveHandler(r));
    }

    protected override async Task Initialize()
    {
        SiloCodes.Add("");

        var gsca = await this.GetSiloCodeAsync();
        if (gsca != null && gsca.Any())
        {
            SiloCodes.AddRange(gsca);
        }
        _ = GetPanelItemCodeAsync();

        defaultAgvChassis = this.ObjectFactory.CreateObject<DefaultAgvChassis>(this);
        PeriodicTimers["5s"]!.OnTick += async () =>
        {
            await this.PayloadPanels.RaiseCollectionChangedEvent(this.DeviceId);
            //上报状态
            var request = GetStatusRequest(this.Status);
            var response = await DataExporter.DeviceStatusReport(request);
            logger.LogInformation($"每5秒上报状态:{this.Status}_上报结果：{response.Code}_msg:{response.Message}");
        };
        await base.Initialize();
    }

    private async Task<DeviceServiceInvokeResponse> SetAgvMoveHandler(DeviceServiceInvokeRequest request)
    {
        return await Task.Run(async () =>
        {
            logger.LogDebug($"AgvMoveCommand SetAgvMoveHandler request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");

            try
            {
                if (!request.Params.ContainsKey("CheckMovePoint"))
                {
                    request.Params["CheckMovePoint"] = configExtra["CheckMovePoint"];
                }
                return await defaultAgvChassis.MoveLocal(this, request);
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

    private async Task<DeviceServiceInvokeResponse> SetAgvHandler(DeviceServiceInvokeRequest request)
    {
        return await Task.Run(async () =>
        {
            logger.LogDebug($"SetAgvCommand SetAgvHandler request:{JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })}");

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
                if (request.PayloadPanels.Any(panel => panel.Position - 1 + panel.Layer < 0))
                {
                    return new DeviceServiceInvokeResponse
                    {
                        Code = ErrorCodes.Sys.FAIL,
                        Message = "panel的属性Position或者Layer，设置不对。导致了panel.Position - 1 + panel.Layer < 0",
                    };
                }

                await PayloadPanels.ChangeListSafely(DeviceId, Task.Run(() =>
                {
                    PayloadPanels.ForEach(p =>
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
                }));

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

    private Task KeepPlcHeart()
    {
        return Task.Run(() =>
        {
            try
            {
                if (string.IsNullOrEmpty(DeviceDescriptor.Extra["ModbusTcpUri"].ToStr()))
                {
                    return;
                }
                if (modbusIpMaster == null)
                {
                    logger.LogInformation($"给PLC链接失败，modbusIpMaster 是null");
                    Connector.IsConnected = false;
                    return;
                }
                SetPlcHeart();
                Connector.IsConnected = true;
            }
            catch (Exception ex)
            {
                errorInfo = new Tuple<string, string, string>("AGV_UnloadMaterialBehavior_Exception", "Exception", "异常：" + ex.Message);
                logger.LogWarning($"{DeviceDescriptor.DeviceId} cannot connect to plc.");
                Connector.IsConnected = false;
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        });
    }

    private void SetPlcHeart()
    {
        agvDevice.SetPlcHeartLocal();
    }

    public override async Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await PropertyContainer[GetType()].ReadProperties(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await PropertyContainer[GetType()].WriteProperties(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> Move(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await defaultAgvChassis.MoveLocal(this, deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvToDeviceAgvLoadPolicy.PrepareLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvToDeviceAgvLoadPolicy.InvokeLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvToDeviceAgvLoadPolicy.CompleteLoadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvToDeviceAgvUnloadPolicy.PrepareUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvToDeviceAgvUnloadPolicy.InvokeUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvToDeviceAgvUnloadPolicy.CompleteUnloadMaterial(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvDevice.WorkLocal(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> Standby(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvDevice.StandbyLocal(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> Shutdown(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvDevice.ShutdownLocal(deviceServiceInvokeRequest);
    }

    public override async Task<DeviceServiceInvokeResponse> Charge(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvDevice.ChargeLocal(deviceServiceInvokeRequest);
    }

    public async Task GetPanelItemCodeAsync(QueryOrderByEnum queryOrder = QueryOrderByEnum.OrderByCreateTimeDesc, string materialMatchCode = "")
    {
        if (MaterialModels == null)
        {
            MaterialModels = new List<MaterialModel>() { };
        }
        var CurrentContext = new CurrentContext();
        MaterialModels.Clear();
        logger.LogWarning("begin to GetPanelItemCodeAsync");
        var condition = new GetItemListReq
        {
            QueryOrderBy = queryOrder,
            Code = materialMatchCode,
        };
        var MaterialModel = await HttpRequestInvoker.PostAsJsonAsync<GetItemListReq, List<MaterialModel>>(CentralWebOptions.GetItemCode, condition);
        if (!string.IsNullOrEmpty(materialMatchCode))
        {
            MaterialModel = MaterialModel.Where(p => p.Code.Contains(materialMatchCode)).ToList();
        }
        MaterialModels.AddRange(MaterialModel);
        logger.LogWarning("end to GetPanelItemCodeAsync");
    }

    public void InitSiloNoMaterial()
    {
        agvDevice.InitSiloNoMaterialLocal();
        //PayloadPanels.Clear();
        //PayloadPanels.AddRange(Panel.SiloNoPanelSpindleFirst(DeviceDescriptor.SpindleNum.ToInt() * DeviceDescriptor.LayerLimit.ToInt(), DeviceDescriptor.LayerLimit.ToInt()));
    }

    //初始化没有料仓
    public void InitNoSilo()
    {
        agvDevice.InitNoSiloLocal();
        //PayloadPanels.Clear();
        //PayloadPanels.AddRange(Panel.NoSiloSpindleFirst(DeviceDescriptor.SpindleNum.ToInt() * DeviceDescriptor.LayerLimit.ToInt(), DeviceDescriptor.LayerLimit.ToInt()));
    }

    /// <summary>
    /// agv能否继续下面的步骤，
    /// 比如设备状态异常或者任务被手动终止就不能继续
    /// </summary>
    /// <param name="stopForcedly">手动终止</param>
    /// <returns></returns>
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
            var message = "CanProceedNextStep: 结束任务：监控到 Status = DeviceStatus.Exception \r\n ";
            logger.LogDebug($"{message} ");
            logger.LogDebug($"\r\n 监控到异常：\r\n" +
                $"CurrentEventTraceId：{currentEventTraceId}\r\n" +
                $"IsConnected：{WatchingProperties.Property("IsConnected").NewValue.ToBool()}\r\n" +
                $"PlcIsReady：{WatchingProperties.Property("PlcIsReady").NewValue.ToBool()}\r\n" +
                $"IsAuto：{WatchingProperties.Property("IsAuto").NewValue.ToBool()}\r\n" +
                $"IsError：{WatchingProperties.Property("IsError").NewValue.ToBool()}\r\n" +
                $"WarningCode：{WatchingProperties.Property("WarningCode").NewValue.ToStr()}\r\n" +
                $"ErrorMessage：{errorsMsg}\r\n" +
                $"IsHalt：{WatchingProperties.Property("IsHalt").NewValue.ToBool()}\r\n" +
                $"CanDispatch：{WatchingProperties.Property("CanDispatch").NewValue.ToBool()}\r\n" +
                $"IsAgvWorkFail：{WatchingProperties.Property("IsAgvWorkFail").NewValue.ToBool()} \r\n");

            ReportingProcess(message, AlarmLevel.Severe);
            return true;
        }

        return result;
    }

    public async Task<DeviceStatusReportResponse> ReportStatus()
    {
        var requestStatus = GetStatusRequest(DeviceStatus.Online);
        var responseStatus = await HttpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(CentralWebOptions.StatusReport, requestStatus);
        return responseStatus;
    }

    public Tuple<bool, string> ReadMaterialCode(int startindex)
    {
        return agvDevice.ReadMaterialCodeLocal(startindex);
    }

    public string ScannigConvert(string ScannigData)
    {
        return agvDevice.ScannigConvertL(ScannigData);
    }

    public async Task<Tuple<bool, string>> LoadExternalMaterial(AgvPageEntity agvPageEntity)
    {
        return await agvDevice.LoadExternalMaterialLocal(agvPageEntity);
    }

    public string QueryExternalMaterial()
    {
        return agvDevice.QueryExternalMaterialLocal();
    }

    public string ClearExternalMaterial()
    {
        return agvDevice.ClearExternalMaterialLocal();
    }

    public Tuple<bool, string> ReadErrorCode()
    {
        var code = "";

        return new Tuple<bool, string>(false, code);
    }

    public Tuple<bool, string> AgvToReady()
    {
        return agvDevice.AgvToReadyLocal();
    }

    public async Task setNewPanelLog(AgvPageEntity agvPageEntity)
    {
        if (string.IsNullOrEmpty(agvPageEntity.SiloCode))
        {
            logger.LogWarning($"AGV料仓数据已经更新为 无料仓");
        }
        else
        {
            logger.LogWarning($"AGV料仓{agvPageEntity.SiloCode}数据已经更新");
        }
    }

    public async Task<Tuple<bool, string>> ResetDeviceStatus()
    {
        SchedulingTasks.Clear();
        WatchingProperties.Property("IsWorking").SetValue(false);
        WatchingProperties.Property("IsMoving").SetValue(false);
        WatchingProperties.Property("AgvIsReady").SetValue(false);
        Status = DeviceStatus.Exception;
        MoveActionStatus = AgvMoveActionStatus.ToDo;
        AgvScanResult = true;
        isWorking = false;
        isMoving = false;
        agvIsReady = false;
        isAgvWorkFail = false;
        spindlePosition = "";
        currentLoadedCount = 0;
        currentUnloadedCount = 0;
        sendCancelBeforeArrivedDevice = true;
        IsCheckMovePoint = true;
        currentEventTraceId = "";
        errorSignal = 0;
        errorsMsg = "";
        AgvCanLeave = true;
        AgvCanLeaveCallBack = false;
        var resetmsg = "";

        if (!string.IsNullOrEmpty(failTaskCode))
        {
            DeviceServiceInvokeRequest request = new DeviceServiceInvokeRequest();
            request.Params["taskCode"] = failTaskCode;
            logger.LogDebug($"\r\n+++++++++++++++++++++++++++++++++AGV取消任务{failTaskCode}+++++++++++++++++++++++++++++++++\r\n");
            var newmoveresult = await defaultAgvChassis.CancelLocal(this, request);
            resetmsg = $"，取消任务：{failTaskCode}_结果：{newmoveresult.Code}_{newmoveresult.Message}";
            logger.LogDebug(resetmsg);
            await Task.Delay(500);
        }

        if (configExtra["AgvChassisSupplier"].ToStr() == "StdRobot")
        {
            logger.LogDebug($"StdRobot_publicMoveId: {publicMoveId}");
            if (!string.IsNullOrWhiteSpace(publicMoveId))
            {
                logger.LogDebug($"\r\n+++++++++++++++++++++++++++++++++AGV取消任务 {publicMoveId}+++++++++++++++++++++++++++++++++\r\n");
                DeviceServiceInvokeRequest request = new DeviceServiceInvokeRequest();
                request.Params["taskCode"] = publicMoveId;
                var newmoveresult = await defaultAgvChassis.CancelLocal(this, request);
                resetmsg = $"StdRobot_CancelTask：{publicMoveId}_结果：{newmoveresult.Code}_{newmoveresult.Message}";
                if (newmoveresult.Code == "0") publicMoveId = "";
                logger.LogDebug(resetmsg);
            }

            if (!string.IsNullOrWhiteSpace(StdGroupNo))
            {
                logger.LogDebug($"\r\n+++++++++++++++++++++++++++++++++AGV完成组任务ID: {StdGroupNo}+++++++++++++++++++++++++++++++++\r\n");
                var groupResult = await defaultAgvChassis.ReleaseLocal(this, null);
                if (groupResult.Code == "0") StdGroupNo = "";
                logger.LogDebug(groupResult.ToJson());
            }
        }

        if (configExtra["AgvChassisSupplier"].ToStr() == "HikRobot")
        {
            DeviceServiceInvokeRequest request = new DeviceServiceInvokeRequest();
            var taskCode = DateTime.Now.ToString("yyyyMMddHHmmss");
            request.Params["Unlock"] = true;
            request.Params["taskCode"] = taskCode;
            request.Params["reqCode"] = taskCode + "_" + "Unlock";
            string restingPoints = configExtra["RestingPoints"].ToStr();
            //var carCurrentPos = WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
            //request.Params["MoveTargetPos"] = string.IsNullOrEmpty(carCurrentPos) ? restingPoints : carCurrentPos;
            request.Params["MoveTargetPos"] = restingPoints;
            request.Params["count"] = "0";

            request.Params["CheckMovePoint"] = configExtra["CheckMovePoint"].ToStr();
            logger.LogDebug("\r\n 手动解锁小车");

            var response = await defaultAgvChassis.MoveLocal(this, request);
            resetmsg += $"，解锁{request.Params["MoveTargetPos"].ToStr()}：{taskCode}_结果：{response.Code}_{response.Message}";
            logger.LogDebug(resetmsg);
        }

        ReportingProcess(resetmsg);
        //通知PLC
        modbusIpMaster.WriteSingleRegister(slaveId, 5058, 1);
        logger.LogDebug("\r\n 通知PLC：重置设备状态");
        return new Tuple<bool, string>(true, $"重置信号,{failTaskCode}_{resetmsg}");
    }

    public async Task<Tuple<bool, string>> AgvToException()
    {
        Status = DeviceStatus.Exception;
        isAgvWorkFail = true;
        var request = GetStatusRequest(DeviceStatus.Exception);
        var errreport = await DataExporter.DeviceStatusReport(request);
        logger.LogDebug($"AgvToException_（手动终止）上报异常:{errreport.Code}_{errreport.Message}_ {JsonSerializer.Serialize(errreport)}");

        DeviceServiceInvokeRequest requestfail = new DeviceServiceInvokeRequest();
        requestfail.Params = new Dictionary<string, object?>();
        var reportfail = await ReportFail(requestfail);
        logger.LogDebug($"AgvToException_（手动终止）上报fail结果: {reportfail.Code}_{reportfail.Message}");
        return new Tuple<bool, string>(errreport.Code == ErrorCodes.Sys.SUCCESS, errreport.Message);
    }

    public Tuple<bool, string> CheckCanMove()
    {
        var onlycheck = agvDevice.CheckCanMoveLocal();
        return new Tuple<bool, string>(onlycheck.Item1, onlycheck.Item2);
    }

    public bool IsTimeout(DateTime startTime, int timeout)
    {
        var dateNow = DateTime.Now;
        var dateCompare = startTime.AddSeconds(timeout);
        if (dateNow > dateCompare)
        {
            logger.LogDebug($"IsTimeout= true, Now:{dateNow.ToString("yyyy-MM-dd HH:mm:ss")},CompareTiem:{dateCompare.ToString("yyyy-MM-dd HH:mm:ss")}");
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> CheckCanExcuteNextTask()
    {
        DateTime dtBegin = DateTime.Now;
        while ((DateTime.Now - dtBegin).TotalSeconds < askLeaveTimeOut)
        {
            if (AgvCanLeaveCallBack)
            {
                AgvCanLeaveCallBack = false;
                return true;
            }

            await Task.Delay(500);
        }

        return false;
    }

    public async Task<DeviceServiceInvokeResponse> TargetDeviceOperation(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
    {
        try
        {
            if (DeviceDescriptor.SoloMode)
            {
                logger.LogDebug("\r\n 单机模式：TargetDeviceOperation直接返回true \r\n ");
                return await Response(ErrorCodes.Sys.SUCCESS, "");
            }

            logger.LogDebug($"\r\n TargetDeviceOperation :{deviceOperation} \r\n ");
            var targetDeviceId = deviceServiceInvokeRequest.TargetDeviceId;
            var startTime = DateTime.Now;
            var targetResult = new DeviceServiceInvokeResponse();
            var CentralWebOptionsType = "";
            switch (deviceOperation)
            {
                case DeviceOperationType.PrepareLoadMaterial:
                    deviceServiceInvokeRequest.ServiceId = Topics.Services.PREPARE_LOAD_MATERIAL_SERVICE_ID;
                    CentralWebOptionsType = CentralWebOptions.ServicePrepare ?? "";
                    break;

                case DeviceOperationType.InvokeLoadMaterial:
                    deviceServiceInvokeRequest.ServiceId = Topics.Services.INVOKE_LOAD_MATERIAL_SERVICE_ID;
                    CentralWebOptionsType = CentralWebOptions.ServiceInvoke ?? "";
                    break;

                case DeviceOperationType.CompleteLoadMaterial:
                    deviceServiceInvokeRequest.ServiceId = Topics.Services.COMPLETE_LOAD_MATERIAL_SERVICE_ID;
                    CentralWebOptionsType = CentralWebOptions.ServiceComplete ?? "";
                    break;

                case DeviceOperationType.PrepareUnloadMaterial:
                    deviceServiceInvokeRequest.ServiceId = Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID;
                    CentralWebOptionsType = CentralWebOptions.ServicePrepare ?? "";
                    break;

                case DeviceOperationType.InvokeUnloadMaterial:
                    deviceServiceInvokeRequest.ServiceId = Topics.Services.INVOKE_UNLOAD_MATERIAL_SERVICE_ID;
                    CentralWebOptionsType = CentralWebOptions.ServiceInvoke ?? "";
                    break;

                case DeviceOperationType.CompleteUnloadMaterial:
                    deviceServiceInvokeRequest.ServiceId = Topics.Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID;
                    CentralWebOptionsType = CentralWebOptions.ServiceComplete ?? "";
                    break;

                default:
                    targetResult = new DeviceServiceInvokeResponse() { Code = ErrorCodes.Sys.FAIL, Message = "deviceOperation 不正确" };
                    break;
            }

            var automsg = "";
            if (!DeviceDescriptor.AutoMode)
            {
                #region 手动模式

                var targetDevice = DeviceProvider.GetDevice(targetDeviceId);
                if (targetDevice == null)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_TargetDeviceOperation_UnAutoMode_NoTargetDevice", "NoTargetDevice", string.Format($"未找到目标设备:{targetDeviceId}"));
                    return await Response(ErrorCodes.Sys.NOT_FIND_DEVICE_CODE, ErrorCodes.Sys.NOT_FIND_DEVICE_CODE);
                }
                var isTimeOut = false;
                var isFinishedWork = false;
                while (true)
                {
                    logger.LogDebug($"\r\n 等待 deviceOperation:{deviceOperation}\r\n");
                    switch (deviceOperation)
                    {
                        case DeviceOperationType.PrepareLoadMaterial:
                            targetResult = await targetDevice.PrepareLoadMaterial(deviceServiceInvokeRequest);
                            break;

                        case DeviceOperationType.InvokeLoadMaterial:
                            targetResult = await targetDevice.InvokeLoadMaterial(deviceServiceInvokeRequest);
                            break;

                        case DeviceOperationType.CompleteLoadMaterial:
                            targetResult = await targetDevice.CompleteLoadMaterial(deviceServiceInvokeRequest);

                            break;

                        case DeviceOperationType.PrepareUnloadMaterial:
                            targetResult = await targetDevice.PrepareUnloadMaterial(deviceServiceInvokeRequest);
                            break;

                        case DeviceOperationType.InvokeUnloadMaterial:
                            targetResult = await targetDevice.InvokeUnloadMaterial(deviceServiceInvokeRequest);
                            break;

                        case DeviceOperationType.CompleteUnloadMaterial:
                            targetResult = await targetDevice.CompleteUnloadMaterial(deviceServiceInvokeRequest);
                            break;

                        default:
                            targetResult = new DeviceServiceInvokeResponse() { Code = ErrorCodes.Sys.FAIL, Message = "deviceOperation 不正确" };
                            break;
                    }

                    if (targetResult.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        break;
                    }
                    isTimeOut = IsTimeout(startTime, postAndGetTimeOut);
                    if (isTimeOut)//超时或者 异常
                    {
                        Status = DeviceStatus.Exception;
                        logger.LogDebug($"\r\n 等待 deviceOperation:{deviceOperation} 信号超时\r\n");
                        break;
                    }
                    isFinishedWork = CanProceedNextStep();
                    if (isFinishedWork)// 异常
                    {
                        logger.LogDebug($"\r\n 程序异常跳出循环deviceOperation:{deviceOperation}\r\n");
                        break;
                    }
                    await Task.Delay(50);
                }
                if (isTimeOut || isFinishedWork)
                {
                    Status = DeviceStatus.Exception;
                    errorInfo = new Tuple<string, string, string>("AGV_TargetDeviceOperation_UnAutoModeTargetDeviceOperationTimeOut", "UnAutoModeTargetDeviceOperationTimeOut", string.Format($"目标设备操作超时:{targetDeviceId}"));
                    return await Response(ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_CODE, $"{ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_MESSAGE}_{targetResult.Message}");
                }

                #endregion 手动模式
            }
            else
            {
                await ReportingProcess($"\r\n 程序调用 AutoModelTargetDeviceOperation");
                targetResult = await AutoModelTargetDeviceOperation(deviceServiceInvokeRequest, CentralWebOptionsType);
                automsg = $"\r\n 程序调用 AutoModelTargetDeviceOperation 结果：{targetResult.Code}_{targetResult.Message}\r\n ";
                await ReportingProcess(automsg);
                logger.LogDebug(automsg);
            }

            if (targetResult.Code != ErrorCodes.Sys.SUCCESS)
            {
                errorInfo = new Tuple<string, string, string>("AGV_TargetDeviceOperation_AutoModeTargetDeviceOperationNoSuccess", "AutoModeTargetDeviceOperationNoSuccess", string.Format($"目标设备操作不成功:{targetDeviceId}"));
                return await Response(targetResult.Code, targetResult.Message);
            }
            return await Response(ErrorCodes.Sys.SUCCESS, "", targetResult.Params);
        }
        catch (Exception ex)
        {
            errorInfo = new Tuple<string, string, string>("AGV_AutoModelTargetDeviceOperation_Exception", "Exception", ex.Message);
            return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message);
        }
        finally
        {
            AddWatchingErrorInfo(errorInfo);
        }
    }

    public void AddWatchingErrorInfo(Tuple<string, string, string> errorinfo)
    {
        if (errorinfo == null)
        {
            return;
        }
        if (string.IsNullOrEmpty(errorinfo.Item1))
        {
            return;
        }
        var errorDic = new Dictionary<string, object?>()
                {
                    { GLOBAL_EXCEPTION_EVENT_ID,errorInfo.Item1},
                    { GLOBAL_EXCEPTION_EVENT_NAME,errorInfo.Item2},
                    { GLOBAL_EXCEPTION_EVENT_MESSAGE,errorInfo.Item3}
                };
        WatchingProperties.SetValues(errorDic);
        errorsMsg = errorInfo.Item3;
        logger.LogError($"{GLOBAL_EXCEPTION_EVENT_ID}:{errorInfo.Item1}{Environment.NewLine}{GLOBAL_EXCEPTION_EVENT_NAME}:{errorInfo.Item2}{Environment.NewLine}{GLOBAL_EXCEPTION_EVENT_MESSAGE}:{errorInfo.Item3}");
        errorInfo = new Tuple<string, string, string>("", "", "");
    }

    private void GetPlcWarningMessage()
    {
        Type type = typeof(ErrorCodes.AGV);
        foreach (var p in type.GetFields())
        {
            var nameStr = p.Name;
            var value = p.GetValue(null);
            var list = nameStr.Split('_');
            if (list.Length != 5)
            {
                continue;
            }
            if (list[4] != "MESSAGE")
            {
                continue;
            }
            plcWaringInfo.Add(list[3], value.ToStr());
        }
    }

    public async Task<StartScheduleTaskResponse> ReportStart()
    {
        var request = new StartScheduleTaskRequest
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            TraceId = currentEventTraceId,
        };

        request.Params["CarCurrentPos"] = WatchingProperties.Property("CarCurrentPos").NewValue.ToStr(); //CarCurrentPos
        request.Params["routingKey"] = CurrentRoutingKey;
        request.Params["TargetProductId"] = targetProductId;
        request.Params["TargetDeviceId"] = targetDeviceId;

        return await HttpRequestInvoker.PostAsJsonAsync<StartScheduleTaskRequest, StartScheduleTaskResponse>(CentralWebOptions.ScheduleStart, request);
    }

    public async Task<CompleteScheduleTaskResponse> ReportComplete(DeviceServiceInvokeRequest deviceServiceInvoke)
    {
        var request = new CompleteScheduleTaskRequest
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            TraceId = currentEventTraceId,
        };
        request.Params = deviceServiceInvoke.Params;
        request.Params["CarCurrentPos"] = WatchingProperties.Property("CarCurrentPos").NewValue.ToStr(); //CarCurrentPos
        request.Params["routingKey"] = CurrentRoutingKey;
        request.Params["TargetProductId"] = targetProductId;
        request.Params["TargetDeviceId"] = targetDeviceId;
        request.Params["CurrentLoadedCount"] = currentLoadedCount;
        request.Params["CurrentUnloadedCount"] = currentUnloadedCount;
        return await HttpRequestInvoker.PostAsJsonAsync<CompleteScheduleTaskRequest, CompleteScheduleTaskResponse>(CentralWebOptions.ScheduleComplete, request) ?? new CompleteScheduleTaskResponse()
        {
            Code = ErrorCodes.Sys.FAIL,
            Message = "中控通信断开"
        };
    }

    public async Task<FailScheduleTaskResponse> ReportFail(DeviceServiceInvokeRequest deviceServiceInvoke)
    {
        var request = new FailScheduleTaskRequest
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            TraceId = currentEventTraceId,
        };
        request.Params = deviceServiceInvoke.Params;
        request.Params["CarCurrentPos"] = WatchingProperties.Property("CarCurrentPos").NewValue.ToStr(); //CarCurrentPos
        request.Params["routingKey"] = CurrentRoutingKey;
        request.Params["TargetProductId"] = targetProductId;
        request.Params["TargetDeviceId"] = targetDeviceId;
        var warningCode = WatchingProperties.Property("WarningCode").NewValue.ToStr();
        request.Params["WarningCode"] = warningCode;
        request.Params["WarningMessage"] = GetWarningMessage(warningCode);
        request.Params["CurrentLoadedCount"] = currentLoadedCount;
        request.Params["CurrentUnloadedCount"] = currentUnloadedCount;
        request.Params["SendCancelBeforeArrivedDevice"] = sendCancelBeforeArrivedDevice;
        return await HttpRequestInvoker.PostAsJsonAsync<FailScheduleTaskRequest, FailScheduleTaskResponse>(CentralWebOptions.ScheduleFail, request) ?? new FailScheduleTaskResponse() { Code = ErrorCodes.Sys.FAIL, Message = "中控通信断开" };
    }

    internal string GetWarningMessage(string code)
    {
        if (!string.IsNullOrEmpty(code))
        {
            return "";
        }
        if (plcWaringInfo.ContainsKey(code))
        {
            return plcWaringInfo[code].ToStr();
        }
        return "";
    }

    protected override bool CanAcceptScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        logger.LogInformation($"CanAcceptScheduleTask: {deviceServiceInvokeRequest.ToString()}");
        var tracidbool = deviceServiceInvokeRequest.Params.ContainsKey(ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID);
        var traceid = "";
        if (tracidbool)
        {
            traceid = deviceServiceInvokeRequest.Params[ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID].ToStr();
        }

        var result = this.Status == DeviceStatus.Ready && SchedulingTasks.Count == 0;

        logger.LogInformation($"是否进入WORK:Status:{this.Status}_SchedulingTasksCount：{SchedulingTasks.Count}，traceid：{traceid},result:{result}");

        return result;
    }

    public override Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest request)
    {
        logger.LogInformation($"ScheduleTask Receive request:{request.ToString()}");
        return base.ScheduleTask(request);
    }

    private Task<bool> MachineConnect() => Task.Run(() =>
    {
        try
        {
            if (string.IsNullOrEmpty(DeviceDescriptor.Extra["ModbusTcpUri"].ToStr()))
            {
                return true;
            }
            if (modbusIpMaster == null)
            {
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}   begin to PlcConnect!");
                modbusIpMaster = modbusIpMasterWrapper.CreateIp(plcUri.Host, plcUri.Port);
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}   finish  PlcConnect!");
            }

            if (modbusIpMaster == null)
            {
                logger.LogDebug("PLC链接失败 modbusIpMaster == null");
                errorInfo = new Tuple<string, string, string>("AGV_MachineConnect_modbusIpMasterNull", "modbusIpMasterNull", "PLC链接失败");
                return false;
            }

            SetPlcHeart();
            return true;
        }
        catch (Exception ex)
        {
            modbusIpMaster = null;
            logger.LogError(ex, ex.Message);
            errorInfo = new Tuple<string, string, string>("AGV_MachineConnect_Exception", "Exception", ex.Message);
            return false;
        }
        finally
        {
            AddWatchingErrorInfo(errorInfo);
        }
    });

    public DeviceStatusReportRequest GetStatusRequest(DeviceStatus newStatus)
    {
        var request = new DeviceStatusReportRequest
        {
            DeviceId = DeviceDescriptor.DeviceId,
            ProductId = DeviceDescriptor.ProductId,
            NewStatus = newStatus,
            OldStatus = this.Status
        };
        request.Params["CarCurrentPos"] = WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
        request.Params["IsFullSilo"] = WatchingProperties.Property("IsFullSilo").NewValue.ToBool();
        request.Params["WarningCode"] = WatchingProperties.Property("WarningCode").NewValue.ToStr();
        request.Params["WarningMessage"] = GetWarningMessage(WatchingProperties.Property("WarningCode").NewValue.ToStr());
        request.Params["NewStatus"] = newStatus;
        request.Params["OldStatus"] = this.Status;
        request.PayloadPanels = PayloadPanels;
        return request;
    }

    private async Task<DeviceServiceInvokeResponse> AutoModelTargetDeviceOperation(DeviceServiceInvokeRequest request, string operationType)
    {
        var response = new DeviceServiceInvokeResponse()
        {
            Code = ErrorCodes.Sys.FAIL,
            Message = string.Empty
        };

        try
        {
            var startTime = DateTime.Now;
            var isTimeOut = false;
            var isFinishedWork = false;
            var errorMsg = "";
            var postAndGetTimeout = configExtra["PostAndGetTimeout"].ToInt();
            if (request.Params.ContainsKey("IsLastStep") && request.Params["IsLastStep"].ToBool())
            {
                logger.LogDebug($"\r\n IsLastStep调用 {operationType} 接口一次\r\n");
                var targetResponse = await DataExporter.DeviceServiceReport(request);
                if (targetResponse == null)
                {
                    errorMsg = $"\r\n IsLastStep调用 {operationType} 接口一次,结果:Null\r\n";
                    logger.LogDebug(errorMsg);
                }
                else
                {
                    errorMsg = $"\r\n IsLastStep调用 {operationType} 接口一次,结果:{targetResponse.Code}_返回信息:{targetResponse.Message}\r\n";
                    logger.LogDebug(errorMsg);
                }
            }
            else
            {
                while (true)
                {
                    var targetResponse = await DataExporter.DeviceServiceReport(request);
                    response.Params = targetResponse.Params;
                    errorMsg = targetResponse.Message;
                    if (targetResponse == null)
                    {
                        logger.LogDebug($"\r\n 调用{request.TargetProductId}_{request.TargetDeviceId}_{operationType}接口:Response=null");
                        response.Code = ErrorCodes.Sys.WEBAPI_RETURNNULL_CODE;
                        response.Message = "返回值为NULL";
                        errorMsg = "返回值为NULL";
                        errorInfo = new Tuple<string, string, string>("AGV_AutoModelTargetDeviceOperation_PostApiNoSuccess", "PostApiNoSuccess", response.Message);
                        return response;
                    }
                    var message = targetResponse.Code == ErrorCodes.Sys.SUCCESS ? "" : targetResponse.Message;
                    await ReportingProcess($"\r\n 自动调用：{request.TargetProductId}_{request.TargetDeviceId}_{operationType}接口:Response:{targetResponse.Code}_{message}");
                    logger.LogDebug($"\r\n 自动调用：{request.TargetProductId}_{request.TargetDeviceId}_{operationType}接口:Response:{targetResponse.Code}_{targetResponse.Message}");

                    var isexist = targetResponse.Params.ContainsKey("DeviceIsException");
                    if (isexist && targetResponse.Params["DeviceIsException"].ToBool())
                    {
                        Status = DeviceStatus.Exception;
                        errorMsg = $"\r\n 等待 设备存在异常结束isexist:{isexist},{targetResponse.Message} \r\n";
                        logger.LogDebug(errorMsg);
                        await ReportingProcess(errorMsg, AlarmLevel.Severe, "AES10023");
                        response.Code = ErrorCodes.Sys.FAIL;
                        response.Message = errorMsg;
                        return response; ;
                    }

                    if (targetResponse.Code == ErrorCodes.Sys.SUCCESS)
                    {
                        break;
                    }
                    isTimeOut = IsTimeout(startTime, postAndGetTimeout);
                    if (isTimeOut)//超时或者 异常
                    {
                        Status = DeviceStatus.Exception;
                        logger.LogDebug($"\r\n 等待 postAndGet 信号超时：{response.Message}\r\n");
                        errorMsg = $"\r\n 等待 postAndGet 信号超时：{response.Message}\r\n";
                        break;
                    }
                    isFinishedWork = CanProceedNextStep();
                    if (isFinishedWork)// 异常
                    {
                        errorMsg = $"\r\n 程序异常跳出循环postAndGet：{response.Message} \r\n";
                        logger.LogDebug($"\r\n 程序异常跳出循环postAndGet：{response.Message} \r\n");
                        break;
                    }
                    await Task.Delay(1000);
                }
            }
            if (isTimeOut || isFinishedWork)
            {
                await ReportingProcess($"\r\n 调用设备返回失败:isTimeOut{isTimeOut}_isFinishedWork：{isFinishedWork}", AlarmLevel.Severe, "AES10017");
                Status = DeviceStatus.Exception;
                response.Code = ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_CODE;
                response.Message = $"{ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_MESSAGE}_{errorMsg}";
                errorInfo = new Tuple<string, string, string>("AGV_AutoModelTargetDeviceOperation_PostApiOutTime", "PostApiOutTime", response.Message);
                return response;
            }
            response.Code = ErrorCodes.Sys.SUCCESS;
            return response;
        }
        catch (Exception ex)
        {
            response.Code = ErrorCodes.Sys.EXCEPTION_CODE;
            response.Message = ex.Message;
            errorInfo = new Tuple<string, string, string>("AGV_AutoModelTargetDeviceOperation_Exception", "Exception", response.Message);
            return response;
        }
        finally
        {
            AddWatchingErrorInfo(errorInfo);
        }
    }

    public Task ReportingProcess(string processMessage, AlarmLevel information = AlarmLevel.Information, string errorCode = "")
    {
        var request = new AddScheduleLogRequest
        {
            DeviceId = DeviceDescriptor.DeviceId,
            TraceId = currentEventTraceId,
            Message = processMessage
        };

        HttpRequestInvoker.PostAsJsonAsync<AddScheduleLogRequest, AddScheduleLogResponse>(CentralWebOptions.ProcessReportSchedule, request);

        DataExporter.DeviceAlarmReport(new DeviceAlarmReportRequest
        {
            ProductId = DeviceDescriptor.ProductId,
            DeviceId = DeviceDescriptor.DeviceId,
            RequestDeviceKind = DeviceDescriptor.DeviceKind,
            AlarmCode = errorCode,
            AlarmContent = processMessage,
            AlarmLevel = information,
            AlarmTime = DateTime.Now,
            AlarmKind = AlarmKind.Unknown,
        });

        return Task.CompletedTask;
    }

    public async Task<Tuple<bool, string>> MonitoringSignal(int signal, string signalMessage)
    {
        var isTimeOut = false;
        var startTime = DateTime.Now;
        var isFinishedWork = false;
        logger.LogDebug($"等待_{signalMessage}");
        while (true)
        {
            var status = modbusIpMaster.ReadHoldingRegisters(slaveId, (ushort)signal, 1);

            if (status[0] == 1)
            {
                logger.LogDebug($"\r\n 收到_{signalMessage}\r\n");
                break;
            }

            isTimeOut = IsTimeout(startTime, waitPlcSignalTimeout);
            if (isTimeOut)
            {
                Status = DeviceStatus.Exception;
                logger.LogDebug($"\r\n 等待 {signal} 信号超时\r\n");
                break;
            }
            isFinishedWork = CanProceedNextStep();
            if (isFinishedWork)
            {
                logger.LogDebug($"\r\n 程序异常跳出循环 {signal}\r\n");
                break;
            }
            await Task.Delay(50);
        }
        if (isTimeOut || isFinishedWork)
        {
            errorSignal = signal;
            this.Status = DeviceStatus.Exception;
            return new Tuple<bool, string>(false, $"等待_{signalMessage}_超时，或异常_{Status}");
        }
        return new Tuple<bool, string>(true, "");
    }

    public async Task<DeviceServiceInvokeResponse> ReportRunStatusStart(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        var startTime = DateTime.Now;
        var isTimeOut = false;
        var isFinishedWork = false;
        var retries = 1;

        while (true)
        {
            logger.LogDebug($"{DateTime.Now} ReportStart - {retries++} times");
            var reportStartPonse = await ReportStart();
            if (reportStartPonse != null)
            {
                logger.LogDebug($"上报开始ReportStart 中控返回信息code:{reportStartPonse.Code}_Message:{reportStartPonse.Message}");
                if (reportStartPonse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    return await Response(ErrorCodes.Sys.FAIL, reportStartPonse.Message);
                }
                else
                {
                    return await Response(ErrorCodes.Sys.SUCCESS, "");
                }
            }

            isTimeOut = IsTimeout(startTime, reportRunStatusTimeOut);
            if (isTimeOut)
            {
                Status = DeviceStatus.Exception;
                logger.LogDebug("\r\n 上报运行开始状态超时\r\n");
                break;
            }

            isFinishedWork = CanProceedNextStep();
            if (isFinishedWork)
            {
                logger.LogDebug("\r\n 程序异常\r\n");
                break;
            }

            if (configExtra.ContainsKey("ReportRunStatusTimes"))
            {
                await Task.Delay(configExtra["ReportRunStatusTimes"].ToInt());
            }
            else
            {
                await Task.Delay(1000);
            }
        }

        if (isTimeOut || isFinishedWork)
        {
            Status = DeviceStatus.Exception;
            return await Response(ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_CODE, "上报运行开始状态 超时或者异常");
        }

        return await Response(ErrorCodes.Sys.FAIL, "ReportRunStatusStart 发生错误");
    }

    protected override async Task OnApplicationStarted()
    {
        logger.LogDebug($"ApplicationStarted1:PayloadPanels:{JsonSerializer.Serialize(PayloadPanels)}");
        await base.OnApplicationStarted();
        if (PayloadPanels.Count() == 0)
        {
            PayloadPanels.AddRange(agvDevice.InitPayloadPanels());
            PayloadPanels.SetLocationCode(DeviceId);
            PayloadPanels.SetSiloCode(DeviceId);
        }

        await PayloadPanels.RaiseCollectionChangedEvent(DeviceId);

        logger.LogDebug($"ApplicationStarted:PayloadPanels2:{JsonSerializer.Serialize(PayloadPanels)}");
    }

    public string SearchTask()
    {
        var task = SchedulingTasks.UnorderedItems;

        return Newtonsoft.Json.JsonConvert.SerializeObject(task);
    }

    public async Task<DeviceServiceInvokeResponse> PlcOpertaion(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        return await agvDevice.PlcOpertaionLocal(deviceServiceInvokeRequest);
    }

    public string ReadPlc(ushort[] plcData)
    {
        //  var strMsg = plcData.UshortToStrings();
        var strMsg = plcData.ConvertUshortArrayToStr();
        return $"读取的原始值为：{JsonSerializer.Serialize(plcData)},最终:{string.Join(' ', plcData)} \r\n  转换后的值为: {strMsg}";
    }
}
