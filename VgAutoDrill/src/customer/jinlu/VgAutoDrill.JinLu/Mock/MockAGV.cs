using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Fundation.Vendor.Agvsz;
using VgAutoDrill.Fundation.Vendor.Common;
using VgAutoDrill.Fundation.Vendor.Vega;
using static VgAutoDrill.Fundation.Mqtt.Events;
using static VgAutoDrill.Fundation.Mqtt.Topics;

namespace VgAutoDrill.JinLu.Mock
{
    public class MockAGV : MockBaseVehicle<MockAGV>
    {
        private readonly ILogger<MockAGV> logger;
        private readonly DeviceDescriptor deviceDescriptor;
        private readonly IHttpRequestInvoker httpRequestInvoker;
        private readonly IDeviceProvider deviceProvider;
        private readonly ILoggerFactory loggerFactory;
        private readonly int postAndGetTimeout;
        private readonly IMqttClientWrapper mqttClientWrapper;
        private CentralWebOptions centralWebOptions;
        private Uri Uri { get; set; }

        private byte SlaveId { get; set; }

        private bool IsConnected;

        private bool IsUseable;

        private string lastPosition;
        //private ModbusIpMaster modbusIpMaster;

        private Dictionary<string, object> configExtra;

        private Dictionary<string, object> AxisMap = new Dictionary<string, object>() {
            { "00007",1},
            { "00008",2},
            { "00009",3},
            { "00010",4},
            { "00011",5},
            { "00012",6}
        };

        public MockAGV(IHttpRequestInvoker httpRequestInvoker,
            IDeviceEngine deviceEngine,
            IMqttClientWrapper mqttClientWrapper,
            DeviceDescriptor deviceDescriptor,
            IOptions<CentralWebOptions> options,
            ILoggerFactory loggerFactory)
            : base(httpRequestInvoker, deviceEngine, mqttClientWrapper, deviceDescriptor, options, loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.centralWebOptions = options.Value;
            this.httpRequestInvoker = httpRequestInvoker;
            this.mqttClientWrapper = mqttClientWrapper;
            this.deviceDescriptor = deviceDescriptor;
            this.deviceProvider = deviceProvider;
            this.logger = loggerFactory.CreateLogger<MockAGV>();
            this.configExtra = deviceDescriptor.Extra;
            //this.Uri = new Uri(deviceDescriptor.Extra["ModbusTcpUri"].ToString());
            this.SlaveId = (byte)deviceDescriptor.Extra["ModbusTcpSlaveId"].ToInt();
            this.postAndGetTimeout = configExtra["PostAndGetTimeout"].ToInt();

            var spindleNum = deviceDescriptor.SpindleNum.ToInt();
            var layerLimit = deviceDescriptor.LayerLimit.ToInt();
            this.PayloadPanels = Enumerable.Repeat<Panel?>(null, spindleNum * layerLimit).ToList();

            this.Connector.ConnectFunc = async (device) =>
            {
                await Task.Run(() =>
                {
                    MachineConnect();
                });

                var requestStatus = GetStatusRequest(DeviceStatus.Ready);
                var requestContent = JsonSerializer.Serialize(requestStatus);
                var responseStatus = await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, requestStatus);
                if (responseStatus == null || responseStatus.Code != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogWarning($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r - {ProductId} - {DeviceName} - {DeviceId}  fail to 报告Ready!\n\r{requestContent}\n\r-----------");
                }
                else
                {
                    Status = DeviceStatus.Ready;
                    logger.LogInformation($"\r\n[info]Mock设备 {DeviceId},已上线且状态已设置为Ready\r\n");
                }

                return true;
            };

            CollectDataFunc = async (device) =>
            {
                await httpRequestInvoker.PostAsJsonAsync<DevicePropertiesReportRequest, DevicePropertiesReportResponse>(
                    centralWebOptions.PropertiesReport,
                    new DevicePropertiesReportRequest()
                    {
                        DeviceId = deviceDescriptor.DeviceId,
                        ProductId = deviceDescriptor.ProductId,
                        Params = new Dictionary<string, object?> {
                            {"DeviceId", deviceDescriptor.DeviceId}
                        }
                    }
                 );
            };

            AddWatchingProperties();

            JudgeReadyStatus();
            JudgeWorkingStatus();
            JudgeExceptionStatus();

            ConfigureWatchingProperties();
        }

        public async override Task<bool> CheckStatus(DeviceServiceInvokeRequest request)
        {
            var targetResponse = await httpRequestInvoker.PostAsJsonAsync<DeviceServiceInvokeRequest, DeviceServiceInvokeResponse>(centralWebOptions.CheckStatus, request);
            if (targetResponse == null || targetResponse.Code != ErrorCodes.Sys.SUCCESS)
            {
                return false;
            }
            else
            {
                return true;
            }           
        }

        public override async Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to ReadProperties!");

            if (!IsConnected)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                     .Build(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, request.ReplyTopic);
            }
            if (request.Params == null)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                         .Build(ErrorCodes.Sys.FAIL, "请传入需要获取的字段", request.ReplyTopic);
            }

            try
            {
                var dictionary = CollectRealProperties();
                WatchingProperties.SetValues(dictionary);

                var responseDictionary = new Dictionary<string, object>();
                var allProperties = false;
                foreach (var param in request.Params.Keys)
                {
                    if (param == "AllProperties")
                    {
                        allProperties = true;
                    }
                    else if (dictionary.ContainsKey(param))
                    {
                        responseDictionary.TryAdd(param, dictionary[param]);
                    }
                }

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to ReadProperties!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                      .Build(ErrorCodes.Sys.SUCCESS, string.Empty, allProperties ? dictionary : responseDictionary, request.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, ex.Message, request.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to WriteProperties!");

            WatchingProperties.SetValues(request.Params);

            //mock 加载板料 并报告状态为Ready
            if (request.Params.ContainsKey("SetLoadReady") && request.Params["SetLoadReady"].ToStr("0") == "1")
            {
                //mock 装载了6块板
                for (var i = 0; i < 6; i++)
                {
                    this.PayloadPanels[i] = new Panel
                    {
                        PanelCode = $"Panel10000000{i}",
                        ItemNo = $"Item01",
                        //物料装入第二台后的状态
                        ProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_2,
                        DrillState = PanelDrillState.Undrilled,
                        SiloCode = $"Silo01",
                        Layer = i,
                        Position = 1,//单个料仓 AGV，1
                        BatchId = "",
                        LotId = "",
                    };
                }

                var requestStatus = GetStatusRequest(DeviceStatus.Ready);
                var requestContent = JsonSerializer.Serialize(requestStatus);
                var responseStatus = await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, requestStatus);
                if (responseStatus.Code != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogWarning($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r - AGV装载，{ProductId} - {DeviceName} - {DeviceId}  fail to 状态物料-报告Ready!\n\r{requestContent}\n\r-----------");
                }

                logger.LogWarning($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r - AGV装载，{ProductId} - {DeviceName} - {DeviceId}  success to 状态物料-报告Ready!\n\r{requestContent}\n\r-----------");
            }

            if (request.Params.ContainsKey("SetClearReady") && request.Params["SetClearReady"].ToStr("0") == "1")
            {
                for (var i = 0; i < this.PayloadPanels.Count; i++)
                {
                    PayloadPanels[i] = null;
                }

                var requestStatus = GetStatusRequest(DeviceStatus.Ready);
                var requestContent = JsonSerializer.Serialize(requestStatus);
                var responseStatus = await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, requestStatus);
                if (responseStatus.Code != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogWarning($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r - AGV卸载，{ProductId} - {DeviceName} - {DeviceId}  fail to 状态物料-报告Ready!\n\r{requestContent}\n\r-----------");
                }

                logger.LogWarning($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r - AGV卸载，{ProductId} - {DeviceName} - {DeviceId}  success to 状态物料-报告Ready!\n\r{requestContent}\n\r-----------");
            }

            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to WriteProperties!");
            return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                     .Build(ErrorCodes.Sys.SUCCESS, string.Empty, request.ReplyTopic);
        }

        public async override Task<DeviceStatusReportResponse> ReportStatus()
        {
            var requestStatus = GetStatusRequest(DeviceStatus.Ready);
            var responseStatus = await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, requestStatus);
            return responseStatus;
        }

        public override async Task<DeviceServiceInvokeResponse> Move(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to Move!");

            if (!IsConnected)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                                         .Build(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, request.ReplyTopic);
            }

            try
            {
                //if (request.Params["TaskEventId"] == "AGV_REQUEST_CHARGE")
                //{

                //}
                //else
                //{
                //    logger.LogInformation("Move : CurrentStation  查询到的小车当前位置,   TargetStation" + request.Params["TargetPos"]);//
                //}

                logger.LogInformation($"-{ProductId}-{DeviceName}-{DeviceId} 开始移动!");
                Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()} {DeviceId} - 移动到指定位置");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                              .Build(ErrorCodes.Sys.SUCCESS, string.Empty, request.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, ex.Message, request.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest fromRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {DeviceId}  开始{fromRequest.DeviceId}预上料准备!");
            Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();

            fromRequest.ServiceId = Services.PREPARE_LOAD_MATERIAL_SERVICE_ID;

            if (!IsConnected)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                                         .Build(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, fromRequest.ReplyTopic);
            }
            try
            {
                logger.LogInformation("TargetDevice_DoPre");
                if (fromRequest.ProductId != Products.GEARSHAPING)
                {
                    var targetDeviceId = fromRequest.DeviceId;
                    if (!deviceDescriptor.AutoMode)
                    {
                    }
                    else
                    {
                        await TargetDeviceOperation(fromRequest, DeviceOperationType.PrepareLoadMaterial);
                    }
                }

                logger.LogInformation("LoadSelf_DoPre");
                await PrepareLoadMaterialSelf(fromRequest);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  end to PrepareLoadMaterial!");
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {DeviceId}  完成了{fromRequest.DeviceId}预上料准备!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.SUCCESS, string.Empty, fromRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, ex.Message, fromRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest fromRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {DeviceId}  开始{fromRequest.DeviceId}的上料!");
            Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  begin to InvokeLoadMaterial!");

            fromRequest.ServiceId = Services.INVOKE_LOAD_MATERIAL_SERVICE_ID;

            if (!IsConnected)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                                         .Build(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, fromRequest.ReplyTopic);
            }
            try
            {
                if (fromRequest.ProductId != Products.GEARSHAPING)
                {
                    logger.LogInformation("TargetDevice_DoInvoke");
                    var targetDeviceId = fromRequest.DeviceId.ToString();
                    //request.Params["Axis"] = AxisMap["Position"];//todo AxisMap["Position"] 未定义
                    if (!deviceDescriptor.AutoMode)
                    {
                    }
                    else
                    {
                        await TargetDeviceOperation(fromRequest, DeviceOperationType.InvokeLoadMaterial);
                    }
                }

                logger.LogInformation("LoadSelf_DoInvoke");
                await InvokeLoadMaterialSelf(fromRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, ex.Message, fromRequest.ReplyTopic);
            }

            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  end to InvokeLoadMaterial!");
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {DeviceId}  完成了{fromRequest.DeviceId}上料操作!");

            return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                     .Build(ErrorCodes.Sys.SUCCESS, string.Empty, fromRequest.ReplyTopic);
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest fromRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {DeviceId}  {fromRequest.DeviceId}上料完成!");
            Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  begin to CompleteLoadMaterial!");
            fromRequest.ServiceId = Services.COMPLETE_LOAD_MATERIAL_SERVICE_ID;

            if (!IsConnected)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                                         .Build(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, fromRequest.ReplyTopic);
            }
            try
            {
                var targetDeviceId = fromRequest.DeviceId.ToString();
                if (!deviceDescriptor.AutoMode)
                {
                }
                else
                {
                    if (fromRequest.ProductId != Products.GEARSHAPING)
                    {
                        await TargetDeviceOperation(fromRequest, DeviceOperationType.CompleteLoadMaterial);
                    }
                }

                logger.LogInformation("LoadSelf_DoComplete");
                await CompleteLoadMaterialSelf(fromRequest);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  end to CompleteLoadMaterial!");
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {DeviceId}  {fromRequest.DeviceId} 上料完成!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.SUCCESS, string.Empty, fromRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, ex.Message, fromRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest fromRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  begin to PrepareUnloadMaterial!");

            fromRequest.ServiceId = Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID;

            if (!IsConnected)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                                         .Build(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, fromRequest.ReplyTopic);
            }
            try
            {
                var targetDeviceId = fromRequest.DeviceId.ToString();
                if (!deviceDescriptor.AutoMode)
                {

                }
                else
                {
                    if (fromRequest.ProductId != Products.GEARSHAPING)
                    {
                        await TargetDeviceOperation(fromRequest, DeviceOperationType.PrepareUnloadMaterial);
                    }
                }

                logger.LogInformation("UnloadSelf_DoPre");
                await PrepareUnLoadMaterialSelf(fromRequest);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  end to PrepareUnloadMaterial!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.SUCCESS, string.Empty, fromRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, ex.Message, fromRequest.ReplyTopic);
            }

        }

        public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest fromRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  begin to InvokeUnloadMaterial!");

            fromRequest.ServiceId = Services.INVOKE_UNLOAD_MATERIAL_SERVICE_ID;

            if (!IsConnected)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                                         .Build(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, fromRequest.ReplyTopic);
            }
            try
            {
                if (fromRequest.ProductId != Products.GEARSHAPING)
                {
                    logger.LogInformation("TargetUnload_DoInvoke");
                    var starttime = DateTime.Now;
                    var targetDeviceId = fromRequest.DeviceId.ToString();
                    if (!deviceDescriptor.AutoMode)
                    {
                    }
                    else
                    {
                        await TargetDeviceOperation(fromRequest, DeviceOperationType.InvokeUnloadMaterial);
                    }
                }

                logger.LogInformation("UnloadSelf_DoInvoke");
                await InvokeUnLoadMaterialSelf(fromRequest);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  begin to InvokeUnloadMaterial!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.SUCCESS, string.Empty, fromRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, ex.Message, fromRequest.ReplyTopic);
            }

        }

        public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest fromRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  begin to CompleteUnloadMaterial!");
            fromRequest.ServiceId = Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID;

            if (!IsConnected)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                                         .Build(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, fromRequest.ReplyTopic);
            }
            try
            {
                if (fromRequest.ProductId != Products.GEARSHAPING)
                {
                    logger.LogInformation("TargetUnload_DoComplete");
                    var targetDeviceId = fromRequest.DeviceId.ToString();
                    if (!deviceDescriptor.AutoMode)
                    {
                    }
                    else
                    {
                        await TargetDeviceOperation(fromRequest, DeviceOperationType.CompleteUnloadMaterial);
                    }
                }

                logger.LogInformation("UnloadSelf_DoComplete");
                await CompleteUnLoadMaterialSelf(fromRequest);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  end to CompleteUnloadMaterial!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.SUCCESS, string.Empty, fromRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, ex.Message, fromRequest.ReplyTopic);
            }

        }

        public override async Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            this.Status = DeviceStatus.Working;
            var requestaaa = GetStatusRequest(DeviceStatus.Working);
            var statusReset = await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, requestaaa);
            if (statusReset == null || statusReset.Code != ErrorCodes.Sys.SUCCESS)
            {
                logger.LogWarning($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  fail to set AGV working!");
            }
            else
            {
                //nothing
            }

            var startTime = DateTime.Now;

            logger.LogInformation($"\r{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r{DeviceId} 开始了{deviceServiceInvokeRequest.TargetProductId}-{deviceServiceInvokeRequest.TargetDeviceId}申请的调度任务!\n\r-----------");
            Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();

            if (!IsConnected)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                                         .Build(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, deviceServiceInvokeRequest.ReplyTopic);
            }

            try
            {
                logger.LogInformation("AGV Work");
                int step = 0;

                //获取钻机在用的轴位，停用是用null表示
                //根据业务类型，获取操作列表
                var targetPoslist = deviceServiceInvokeRequest.Params["Spindles"].ToString().Split(new char[] { ',' });
                //根据业务类型，获取操作列表
                var operationList = GetOperationList(deviceServiceInvokeRequest, targetPoslist);

                //仿照页面手动测试
                //1.下料前准备
                deviceServiceInvokeRequest.ServiceId = Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID;
                deviceServiceInvokeRequest.Params["Position"] = 1;
                var precode = (await PrepareUnloadMaterial(deviceServiceInvokeRequest)).Code;
                if (precode != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogInformation("PrepareUnloadMaterial：" + precode);
                }

                //2.6个轴 依次点击上料完成

                foreach (var operation in operationList)
                {
                    step++;

                    lastPosition = operation.AgvPosition;

                    deviceServiceInvokeRequest.Params["LastPos"] = lastPosition;
                    deviceServiceInvokeRequest.Params["MoveTargetPos"] = operation.AgvPosition;
                    deviceServiceInvokeRequest.Params["SpindlePosition"] = operation.SpindleId;
                    deviceServiceInvokeRequest.Params["IsFirstStep"] = (step == 1);

                    //var movecode = (await Move(deviceServiceInvokeRequest)).Code;
                    //if (movecode != ErrorCodes.Sys.SUCCESS)
                    //{
                    //    logger.LogInformation("Move：" + movecode);
                    //    break;
                    //}

                    //Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();
                    //var precode = (await PrepareLoadMaterial(deviceServiceInvokeRequest)).Code;
                    //if (precode != ErrorCodes.Sys.SUCCESS)
                    //{
                    //    logger.LogInformation("PrepareLoadMaterial：" + precode);
                    //    break;
                    //}

                    //Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();
                    //var invokecode = (await InvokeLoadMaterial(deviceServiceInvokeRequest)).Code;
                    //if (invokecode != ErrorCodes.Sys.SUCCESS)
                    //{
                    //    logger.LogInformation("InvokeLoadMaterial：" + invokecode);
                    //    break;
                    //}


                    deviceServiceInvokeRequest.Params["Position"] = operation.SpindleId;
                    deviceServiceInvokeRequest.Params["IsLastStep"] = false;
                    deviceServiceInvokeRequest.Params["LoadingPanel"] = new SwapPanel()
                    {
                        PanelList = new List<Panel?>()
                                 {
                                     new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="1"
                                     },
                                     new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="2"
                                     },
                                      new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="3"
                                     },
                                      new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="4"
                                     },
                                      new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="5"
                                     },
                                      new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="6"
                                     }
                                 }
                    };
                    var comcode = (await CompleteLoadMaterial(deviceServiceInvokeRequest)).Code;
                    if (comcode != ErrorCodes.Sys.SUCCESS)
                    {
                        logger.LogInformation("CompleteLoadMaterial：" + comcode);
                        break;
                    }
                }

                //3.点击全部完成上料
                deviceServiceInvokeRequest.Params["Position"] = operationList.Last().SpindleId;
                deviceServiceInvokeRequest.Params["IsLastStep"] = true;
                deviceServiceInvokeRequest.Params["LoadingPanel"] = new SwapPanel()
                {
                    PanelList = new List<Panel?>()
                                 {
                                     new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="1"
                                     },
                                     new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="2"
                                     },
                                      new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="3"
                                     },
                                      new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="4"
                                     },
                                      new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="5"
                                     },
                                      new Fundation.Iot.Models.Panel()
                                     {
                                         BatchId="p1",
                                         ItemNo="item01",
                                         PanelCode="6"
                                     }
                                 }
                };

                var comcodeAll = (await CompleteLoadMaterial(deviceServiceInvokeRequest)).Code;
                if (comcodeAll != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogInformation("CompleteLoadMaterial comcodeAll：" + comcodeAll);
                }

                //mock remove the drilled panels
                foreach (var operation in operationList.Where(t => t.PanelList.Any(x => x.DrillState == PanelDrillState.Undrilled)))
                {
                    PayloadPanels[PayloadPanels.FindIndex(x => x != null && x.PanelCode == operation.PanelList[0].PanelCode)] = null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }

            requestaaa = GetStatusRequest(DeviceStatus.Ready);
            statusReset = await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, requestaaa);
            if (statusReset.Code != ErrorCodes.Sys.SUCCESS)
            {
                logger.LogWarning($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  fail to set AGV Ready!");
            }
            else
            {
                this.Status = DeviceStatus.Ready;
            }

            logger.LogInformation($"\r{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r{DeviceId} 完成了{deviceServiceInvokeRequest.TargetProductId}-{deviceServiceInvokeRequest.TargetDeviceId}申请的调度任务!\n\r-----{Math.Round(DateTime.Now.Subtract(startTime).TotalSeconds)} seconds------");

            return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                                   .Build(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
        }

        /// <summary>
        /// //获取钻机在用的轴位，停用是用null表示
        /// 根据业务类型，获取操作列表
        /// </summary>
        /// <param name="deviceServiceInvokeRequest"></param>
        /// <param name="targetPoslist"></param>
        /// <returns></returns>
        private List<SwapPanel> GetOperationList(DeviceServiceInvokeRequest deviceServiceInvokeRequest, string[] targetPoslist)
        {
            var operationList = new List<SwapPanel>();
            var splideIndex = 0;
            foreach (var targetPos in targetPoslist)
            {
                splideIndex++;
                if (targetPos.ToLower() == "null" || targetPos == "0")
                {
                    continue;
                }

                switch (deviceServiceInvokeRequest.CallerRequestInteractionBehavior)
                {
                    case InteractionBehavior.LoadPanelOnly:
                    default:
                        break;
                    case InteractionBehavior.UnloadPanelOnly:
                        break;
                    case InteractionBehavior.UnloadPanelThenLoadPanel:
                        //先取下料信息,首次运行时，可能不需要下料
                        var drilledPanel = deviceServiceInvokeRequest.PayloadPanels.FirstOrDefault(p => p != null && p.DrillState == PanelDrillState.Drilled && p.Position == splideIndex);
                        if (drilledPanel != null)
                        {
                            operationList.Add(new SwapPanel
                            {
                                SpindleId = splideIndex,
                                AgvPosition = targetPos,
                                PanelList = new List<Panel?> { drilledPanel }
                            });
                        }

                        //再取上料信息
                        var undrilledPanel = PayloadPanels.FirstOrDefault(x => x != null && x.DrillState == PanelDrillState.Unknown);
                        var swapPanel = new SwapPanel
                        {
                            SpindleId = splideIndex,
                            AgvPosition = targetPos,
                            PanelList = new List<Panel?> { undrilledPanel }
                        };
                        operationList.Add(swapPanel);
                        undrilledPanel.DrillState = PanelDrillState.Undrilled;
                        break;
                }
            }

            return operationList;
        }

        public override async Task<DeviceServiceInvokeResponse> Standby(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  begin to Standby!");

            return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                           .Build(ErrorCodes.Sys.SUCCESS, string.Empty, request.ReplyTopic);
        }

        public event Action OnShutDown;

        public override async Task<DeviceServiceInvokeResponse> Shutdown(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  begin to Shutdown!");
            OnShutDown?.Invoke();

            return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                .Build(ErrorCodes.Sys.SUCCESS, string.Empty, request.ReplyTopic);
        }

        public override async Task<DeviceServiceInvokeResponse> Charge(DeviceServiceInvokeRequest request)
        {
            logger.LogInformation($"\r{DateTime.Now.ToLongTimeString()}\n\r---------------\n\r{ProductId} - {DeviceName} - {DeviceId} 开始充电!\n\r---------------");

            if (!IsConnected)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                                         .Build(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, request.ReplyTopic);
            }

            var response = await Move(request);
            if (response.Code != ErrorCodes.Sys.SUCCESS)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, "移动失败", request.ReplyTopic);
            }
            try
            {
                logger.LogInformation("charge to 40%");
                await Task.Delay(500);
                WatchingProperties.Property("Battery").SetValue(40);
                logger.LogInformation("charge to 60%");
                await Task.Delay(500);
                WatchingProperties.Property("Battery").SetValue(60);
                logger.LogInformation("charge to 80%");
                await Task.Delay(500);
                WatchingProperties.Property("Battery").SetValue(80);
                logger.LogInformation("charge to 100%");
                await Task.Delay(500);
                WatchingProperties.Property("Battery").SetValue(100);

                var requestaaa = GetStatusRequest(DeviceStatus.Ready);
                var statusReset = await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, requestaaa);
                if (statusReset.Code != ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogWarning($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  fail to reset AGV status!");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                         .Build(ErrorCodes.Sys.FAIL, ex.Message, request.ReplyTopic);
            }

            logger.LogInformation($"\r{DateTime.Now.ToLongTimeString()}\n\r----------------\n\r{ProductId} - {DeviceName} - {DeviceId} 已完成充电!\n\r---------------");

            return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                         .Build(ErrorCodes.Sys.SUCCESS, string.Empty, request.ReplyTopic);
        }

        protected async override Task Initialize()
        {
            await base.Initialize();
        }

        private Dictionary<string, object> CollectRealProperties()
        {
            var realProperties = new Dictionary<string, object>();

            //Dictionary<string, string> carInfo = GetCarInfo();
            var carInfo = new CarModel();// GetCarInfo();
            realProperties.TryAdd("CurrentPos", carInfo.CurrentStation);
            realProperties.TryAdd("TargetPos", carInfo.TargetStation);
            realProperties.TryAdd("Battery", 80);
            realProperties.TryAdd("Offset_X", carInfo.X);
            realProperties.TryAdd("Offset_Z", carInfo.Y);

            //realProperties.TryAdd("CurrentPos", carInfo["CurrentStation"]);
            //realProperties.TryAdd("TargetPos", carInfo["TargetStation"]);
            //realProperties.TryAdd("Battery", carInfo["Battery"]);
            //realProperties.TryAdd("Offset_X", carInfo["X"]);
            //realProperties.TryAdd("Offset_Z", carInfo["Y"]);

            Dictionary<string, object> plcInfo = GetPLCInfo();
            realProperties.TryAdd("SiloInfo", plcInfo["SiloInfo"]);
            realProperties.TryAdd("IsReady", plcInfo["IsReady"]);
            realProperties.TryAdd("IsAuto", plcInfo["IsAuto"]);
            realProperties.TryAdd("IsError", plcInfo["IsError"]);
            realProperties.TryAdd("IsHalt", plcInfo["IsHalt"]);

            realProperties.TryAdd("DeviceId", DeviceId);
            realProperties.TryAdd("IsUseable", IsUseable);
            realProperties.TryAdd("IsConnected", IsConnected);
            return realProperties;
        }

        private Dictionary<string, object> GetPLCInfo()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            //ushort[] WareHouseInfo = modbusIpMaster.ReadHoldingRegisters(SlaveId, 4030, 24);
            //dictionary.TryAdd("SiloInfo", WareHouseInfo.UshortToString());

            //ushort[] status = modbusIpMaster.ReadHoldingRegisters(SlaveId, 4001, 4);
            //dictionary.TryAdd("IsReady", status[0] == 1);
            //dictionary.TryAdd("IsAuto", status[1] == 2);
            //dictionary.TryAdd("IsError", status[2] == 1);
            //dictionary.TryAdd("IsHalt", status[3] == 1);

            dictionary.TryAdd("SiloInfo", "");
            dictionary.TryAdd("IsReady", 1);
            dictionary.TryAdd("IsAuto", 2);
            dictionary.TryAdd("IsError", 1);
            dictionary.TryAdd("IsHalt", 1);

            logger.LogInformation("设置 获取PLC信息");
            return dictionary;
        }

        private CarModel GetCarInfo()
        {
            object CarsInfo = null;
            try
            {
                CarsInfo = httpRequestInvoker.GetFromJsonAsync<object>(configExtra["CarAllInfo"].ToString());

                object? Result = CarsInfo.GetType().GetProperty("Result").GetValue(CarsInfo);
                var carlist = JsonSerializer.Deserialize<CarState>(Result.ToString());
                foreach (var item in carlist.Result)
                {
                    if (item.Name.Equals(DeviceId))
                    {
                        return item;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new CarModel();
            }
            return new CarModel();
        }

        private bool JudgeOfflineStatus()
        {
            return !IsConnected;
        }

        private bool JudgeExceptionStatus()
        {
            bool IsReady = WatchingProperties.Property("IsReady").NewValue.ObjToBool();
            bool IsAuto = WatchingProperties.Property("IsAuto").NewValue.ObjToBool();
            bool IsConnected = WatchingProperties.Property("IsConnected").NewValue.ObjToBool();
            bool IsError = WatchingProperties.Property("IsError").NewValue.ObjToBool();
            bool IsHalt = WatchingProperties.Property("IsHalt").NewValue.ObjToBool();

            WatchingProperties.Properties("IsReady", "IsAuto", "IsConnected", "IsError", "IsHalt")
             .When(p => IsConnected && (!IsReady || !IsAuto || IsError || IsHalt)
             )
             .TriggerAlways(async () =>
             {
                 var request = GetStatusRequest(DeviceStatus.Exception);
                 await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, request);
             });

            return IsConnected && (!IsReady || !IsAuto || IsError || IsHalt);

        }

        private async Task<bool> JudgeWorkingStatus()
        {
            //this.Status = DeviceStatus.Ready;
            //var requestaaa = GetStatusRequest(DeviceStatus.Ready);
            //var statusReset = await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, requestaaa);
            //if (statusReset.Code != ErrorCodes.Sys.SUCCESS)
            //{
            //    logger.LogWarning($"{DateTime.Now.ToLongTimeString()} - {ProductId} - {DeviceName} - {DeviceId}  fail to set AGV Ready!");
            //}

            return true;
        }

        private bool JudgeReadyStatus()
        {
            return true;
        }

        private bool JudgeOnlineStatus()
        {
            return IsConnected;
        }

        private async Task ConfigureWatchingProperties()
        {
            WatchingProperties.Property("Battery")
              .When(properties => properties.NewValue.ToFloat() < 30 && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  var request = new DeviceServiceInvokeRequest()
                  {

                      DeviceId = DeviceDescriptor.DeviceId,
                      ProductId = DeviceDescriptor.ProductId,
                      ClientId = ClientId,
                      ServiceId = Services.SCHEDULE_TASK_SERVICE_ID,//todo 这个填什么？？
                      Params = new Dictionary<string, object?>
                      {
                          { Tasks.PARAMS_TASK_EVENT_ID,AGV.AGV_CHARGE_EVENT },
                          {Tasks.PARAMS_TASK_STATUS, TaskStatus.Created },
                          {"TargetPos","00002"}
                      }
                  };

                  await ScheduleTask(request);

                  var eventRequest = new DeviceEventReportRequest()
                  {
                      EventId = AGV.AGV_CHARGE_EVENT,
                      EventName = AGV.AGV_CHARGE_NAME,
                      DeviceId = DeviceDescriptor.DeviceId,
                      ProductId = DeviceDescriptor.ProductId,
                      Params = new Dictionary<string, object?>
                      {
                          { Tasks.PARAMS_TASK_EVENT_ID,AGV.AGV_CHARGE_EVENT },
                          {Tasks.PARAMS_TASK_STATUS, TaskStatus.Created },
                          {"TargetPos","00002"}
                      }
                  };

                  await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, eventRequest);
              });
        }

        private async Task SetStatusValue(Device device, DeviceStatus status)
        {
            var request = new DeviceServiceInvokeRequest()
            {
                DeviceId = DeviceDescriptor.DeviceId,
                ProductId = DeviceDescriptor.ProductId,
                Params = new Dictionary<string, object>()
                {
                    {"PreStatus", device.Status },
                    {"PostStatus", status }
                }
            };
            Status = status;
            await httpRequestInvoker.PostAsJsonAsync<DeviceServiceInvokeRequest, DeviceServiceInvokeResponse>(centralWebOptions.StatusReport, request);
        }

        private Task AGVReset()
        {
            return Task.Run(() =>
            {
                logger.LogInformation("重置AGV PLC点位");
            });
        }

        private bool MachineConnect()
        {
            try
            {
                //Initialize Modbus TCP connection
                //TcpClient tcpClient = new TcpClient();
                //tcpClient.Connect(Uri.Host, Uri.Port);
                //modbusIpMaster = ModbusIpMaster.CreateIp(tcpClient);
                IsConnected = true;

                return true;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                return false;
            }
        }

        private bool IsTimeout(DateTime starttime, int timeout)
        {
            if (DateTime.Now > starttime.AddMilliseconds(timeout))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Task PrepareLoadForGearshaping(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareLoadForGearshaping");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task PrepareLoadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareLoadForProcessedTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task PrepareLoadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareLoadForRawTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task PrepareLoadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareLoadForUnPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task PrepareLoadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareLoadForPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task PrepareLoadForDrill(DeviceServiceInvokeRequest request)
        {
            return Task.Run(() =>
            {
                try
                {
                    logger.LogInformation("设置PLC信息 PrepareLoadForDrill");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
            });
        }

        private Task InvokeLoadForGearshaping(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeLoadForGearshaping");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task InvokeLoadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeLoadForProcessedTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task InvokeLoadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeLoadForRawTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task InvokeLoadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeLoadForUnPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task InvokeLoadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeLoadForPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task InvokeLoadForDrill(DeviceServiceInvokeRequest request)
        {
            return Task.Run(() =>
            {
                try
                {
                    logger.LogInformation("设置PLC信息 InvokeLoadForDrill");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
            });
        }

        private async void CompleteLoadForGearshaping(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteLoadForGearshaping");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        private Task CompleteLoadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteLoadForProcessedTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task CompleteLoadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteLoadForRawTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task CompleteLoadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteLoadForUnPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task CompleteLoadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteLoadForPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task CompleteLoadForDrill(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteLoadForDrill");
                AGVReset();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return Task.CompletedTask;
        }

        private Task PrepareUnloadForGearshaping(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareUnloadForGearshaping");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task PrepareUnloadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareUnloadForProcessedTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task PrepareUnloadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareUnloadForRawTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task PrepareUnloadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareUnloadForUnPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task PrepareUnloadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareUnloadForPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task PrepareUnloadForDrill(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 PrepareUnloadForDrill");
            }
            catch (Exception)
            {

                throw;
            }
            return Task.CompletedTask;
        }
        private Task InvokeUnloadForGearshaping(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeUnloadForGearshaping");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task InvokeUnloadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeUnloadForProcessedTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task InvokeUnloadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeUnloadForRawTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task InvokeUnloadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeUnloadForUnPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task InvokeUnloadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeUnloadForPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task InvokeUnloadForDrill(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 InvokeUnloadForDrill");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private async void CompleteUnloadForGearshaping(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteUnloadForGearshaping");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        private Task CompleteUnloadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteUnloadForProcessedTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task CompleteUnloadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteUnloadForRawTagingDesk");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task CompleteUnloadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteUnloadForUnPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task CompleteUnloadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteUnloadForPin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private Task CompleteUnloadForDrill(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("设置PLC信息 CompleteUnloadForDrill");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
        }

        private void AddWatchingProperties()
        {
            WatchingProperties.AddProperty("ConnectionStatus", "")
            .AddProperty("MachineStatus", "")
            .AddProperty("CurrentPos", "")
            .AddProperty("TargetPos", "")
            .AddProperty("Battery", "80")
            .AddProperty("Offset_X", "")
            .AddProperty("Offset_Z", "")
            .AddProperty("SiloInfo", "")
            .AddProperty("IsReady", false)
            .AddProperty("IsAuto", false)
            .AddProperty("IsError", false)
            .AddProperty("IsHalt", false)
            .AddProperty("DeviceId", DeviceId)
            .AddProperty("IsUseable", IsUseable)
            .AddProperty("IsConnected", IsConnected)
            .AddProperty("PrepareLoadOk", false)
            .AddProperty("InvokeLoadOk", false)
            .AddProperty("CompleteLoadOk", false)
            .AddProperty("PrepareUnloadOk", false)
            .AddProperty("InvokeUnloadOk", false)
            .AddProperty("CompleteUnloadOk", false);
        }

        private async Task<DeviceServiceInvokeResponse> TargetDeviceOperation(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            var targetResult = new DeviceServiceInvokeResponse();
            var centralWebOptionsType = "";
            if (!deviceDescriptor.AutoMode)
            {
            }
            else
            {
                switch (deviceOperation)
                {
                    case DeviceOperationType.PrepareLoadMaterial:
                        deviceServiceInvokeRequest.ServiceId = Topics.Services.PREPARE_LOAD_MATERIAL_SERVICE_ID;
                        centralWebOptionsType = centralWebOptions.ServicePrepare ?? "";
                        break;
                    case DeviceOperationType.InvokeLoadMaterial:
                        deviceServiceInvokeRequest.ServiceId = Topics.Services.INVOKE_LOAD_MATERIAL_SERVICE_ID;
                        centralWebOptionsType = centralWebOptions.ServiceInvoke ?? "";
                        break;
                    case DeviceOperationType.CompleteLoadMaterial:
                        deviceServiceInvokeRequest.ServiceId = Topics.Services.COMPLETE_LOAD_MATERIAL_SERVICE_ID;
                        centralWebOptionsType = centralWebOptions.ServiceComplete ?? "";
                        break;
                    case DeviceOperationType.PrepareUnloadMaterial:
                        deviceServiceInvokeRequest.ServiceId = Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID;
                        centralWebOptionsType = centralWebOptions.ServicePrepare ?? "";
                        break;
                    case DeviceOperationType.InvokeUnloadMaterial:
                        deviceServiceInvokeRequest.ServiceId = Topics.Services.INVOKE_UNLOAD_MATERIAL_SERVICE_ID;
                        centralWebOptionsType = centralWebOptions.ServiceInvoke ?? "";
                        break;
                    case DeviceOperationType.CompleteUnloadMaterial:
                        deviceServiceInvokeRequest.ServiceId = Topics.Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID;
                        centralWebOptionsType = centralWebOptions.ServiceComplete ?? "";
                        break;
                    default:
                        targetResult = new DeviceServiceInvokeResponse() { Code = ErrorCodes.Sys.FAIL, Message = "deviceOperation 不正确" };
                        break;
                }
                targetResult = await AutoModelTargetDeviceOperation(deviceServiceInvokeRequest, centralWebOptionsType);
            }

            return targetResult;
        }

        private async Task<DeviceServiceInvokeResponse> AutoModelTargetDeviceOperation(DeviceServiceInvokeRequest request, string operationType)
        {
            var response = new DeviceServiceInvokeResponse()
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = string.Empty,
            };
            try
            {
                var startTime = DateTime.Now;
                var postAndGetTimeout = configExtra["PostAndGetTimeout"].ToInt();
                while (true)
                {
                    var targetResponse = await httpRequestInvoker.PostAsJsonAsync<DeviceServiceInvokeRequest, DeviceServiceInvokeResponse>(operationType, request);
                    if (targetResponse == null)
                    {
                        response.Code = ErrorCodes.Sys.WEBAPI_RETURNNULL_CODE;
                        response.Message = "返回值为NULL";
                        //errorInfo = new Tuple<string, string, string>("AGV_AutoModelTargetDeviceOperation_PostApiNoSuccess", "PostApiNoSuccess", response.Message);
                        return response;
                    }
                    if (targetResponse.Code == ErrorCodes.Sys.SUCCESS || IsTimeout(startTime, postAndGetTimeout))
                    {
                        break;
                    }
                }

                if (IsTimeout(startTime, postAndGetTimeout))
                {
                    response.Code = ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_CODE;
                    response.Message = ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_MESSAGE;
                    //errorInfo = new Tuple<string, string, string>("AGV_AutoModelTargetDeviceOperation_PostApiOutTime", "PostApiOutTime", response.Message);
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Code = ErrorCodes.Sys.EXCEPTION_CODE;
                response.Message = ex.Message;
                //errorInfo = new Tuple<string, string, string>("AGV_AutoModelTargetDeviceOperation_Exception", "Exception", response.Message);
                return response;
            }
            finally
            {
                //AddWatchingErrorInfo(errorInfo);
            }
            return response;
        }

        private Task PrepareLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            switch (request.ProductId)
            {
                case Products.DRILL:
                    PrepareLoadForDrill(request);
                    break;
                case Products.PIN:
                    PrepareLoadForPin(request);
                    break;
                case Products.UNPIN:
                    PrepareLoadForUnPin(request);
                    break;
                case Products.RAWSTAGINGDESK:
                    PrepareLoadForRawTagingDesk(request);
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    PrepareLoadForProcessedTagingDesk(request);
                    break;
                case Products.GEARSHAPING:
                    PrepareLoadForGearshaping(request);
                    break;
            }
            return Task.CompletedTask;
        }

        private Task InvokeLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            switch (request.ProductId)
            {
                case Products.DRILL:
                    InvokeLoadForDrill(request);
                    break;
                case Products.PIN:
                    InvokeLoadForPin(request);
                    break;
                case Products.UNPIN:
                    InvokeLoadForUnPin(request);
                    break;
                case Products.RAWSTAGINGDESK:
                    InvokeLoadForRawTagingDesk(request);
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    InvokeLoadForProcessedTagingDesk(request);
                    break;
                case Products.GEARSHAPING:
                    InvokeLoadForGearshaping(request);
                    break;
            }
            return Task.CompletedTask;
        }

        private Task CompleteLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            switch (request.ProductId)
            {
                case Products.DRILL:
                    CompleteLoadForDrill(request);
                    break;
                case Products.PIN:
                    CompleteLoadForPin(request);
                    break;
                case Products.UNPIN:
                    CompleteLoadForUnPin(request);
                    break;
                case Products.RAWSTAGINGDESK:
                    CompleteLoadForRawTagingDesk(request);
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    CompleteLoadForProcessedTagingDesk(request);
                    break;
                case Products.GEARSHAPING:
                    CompleteLoadForGearshaping(request);
                    break;
            }
            return Task.CompletedTask;
        }

        private Task PrepareUnLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            switch (request.ProductId)
            {
                case Products.DRILL:
                    PrepareUnloadForDrill(request);
                    break;
                case Products.PIN:
                    PrepareUnloadForPin(request);
                    break;
                case Products.UNPIN:
                    PrepareUnloadForUnPin(request);
                    break;
                case Products.RAWSTAGINGDESK:
                    PrepareUnloadForRawTagingDesk(request);
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    PrepareUnloadForProcessedTagingDesk(request);
                    break;
                case Products.GEARSHAPING:
                    PrepareUnloadForGearshaping(request);
                    break;
            }
            return Task.CompletedTask;
        }

        private Task InvokeUnLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            switch (request.ProductId)
            {
                case Products.DRILL:
                    InvokeUnloadForDrill(request);
                    break;
                case Products.PIN:
                    InvokeUnloadForPin(request);
                    break;
                case Products.UNPIN:
                    InvokeUnloadForUnPin(request);
                    break;
                case Products.RAWSTAGINGDESK:
                    InvokeUnloadForRawTagingDesk(request);
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    InvokeUnloadForProcessedTagingDesk(request);
                    break;
                case Products.GEARSHAPING:
                    InvokeUnloadForGearshaping(request);
                    break;
            }
            return Task.CompletedTask;
        }

        private Task CompleteUnLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            switch (request.ProductId)
            {
                case Products.DRILL:
                    CompleteUnloadForDrill(request);
                    break;
                case Products.PIN:
                    CompleteUnloadForPin(request);
                    break;
                case Products.UNPIN:
                    CompleteUnloadForUnPin(request);
                    break;
                case Products.RAWSTAGINGDESK:
                    CompleteUnloadForRawTagingDesk(request);
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    CompleteUnloadForProcessedTagingDesk(request);
                    break;
                case Products.GEARSHAPING:
                    CompleteUnloadForGearshaping(request);
                    break;
            }

            //mock add the drilled panels
            var unloadPanelCount = request.Params.ContainsKey("UnloadPanelCount") ? request.Params["UnloadPanelCount"].ToInt() : 6;

            for (var i = 0; i <= PayloadPanels.Count && unloadPanelCount > 0; i++)
            {
                var panel = PayloadPanels[i];
                if (panel == null)
                {
                    --unloadPanelCount;

                    PayloadPanels[i] = new Panel
                    {
                        PanelCode = $"202310000000{i + 1}",
                        ItemNo = "DrilledItemCode",
                        //物料已钻孔完成
                        ProductStatus = ProductStatus.Finished_DRILL,
                        DrillState = PanelDrillState.Drilled,
                        SiloCode = $"",
                        Layer = i,
                        Position = 1,//单个料仓 AGV，1
                        BatchId = "10025",
                        LotId = "",
                    };

                }
            }

            return Task.CompletedTask;
        }
    }
}