using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Fundation.Utils.CNC84;
using VgAutoDrill.Fundation.Utils.CNC84.Model;
using VgAutoDrill.Fundation.Vendor.Vega;

namespace VgAutoDrill.JinLu
{
    public class FrontDrill1ParkingTemp : Device
    {
        private readonly ILogger<FrontDrill1ParkingTemp> logger;
        private readonly ILoggerFactory loggerFactory;
        private readonly IHttpRequestInvoker httpRequestInvoker;
        private readonly int spindleNum;
        private readonly string[] spindleAgvPosition;
        private readonly ICNC84CommandWrapper cNC84CommandWrapper;
        private ICNC84Command cnc84Command;
        private readonly CentralWebOptions centralWebOptions;
        private readonly IMqttClientWrapper mqttClientWrapper;
        private readonly ISet<int> sequenceError = new HashSet<int>() {
            1501, 1502, 1503, 1504, 1505, 1507, 1508, 1509, 1510, 1511,
            1512, 1513, 1514, 1515, 1517, 1518, 1519, 1520, 1521, 1522,
            1523, 1524, 1525, 1526, 1528, 1530, 1531, 1532, 1533, 1534,
            1535, 1536, 1537, 1538, 1539, 1540, 1544, 1545, 1546, 1547,
            1548, 1550, 1551, 1552, 1553, 1554, 1555, 1556, 1557, 1559,
            1560, 1561, 1562, 1563, 1564, 1565, 1566, 1567, 1568, 1569,
            1570, 1571, 1572, 1573, 1574, 1575, 1576, 1577, 1578, 1579,
            1580, 1581, 1582, 1583, 1584, 1585, 1587, 1588, 1590, 1595,
            1596, 1598, 1599, 1600, 1603, 1604, 1605, 1606, 1607, 1608,
            1609, 1610, 1611, 1612, 1613, 1615, 1616, 1617, 1625, 1626,
            1627, 1628, 1629, 1630, 1637 };
        private readonly ISet<int> sequenceAlarm = new HashSet<int>() { 1042, 1102 };
        private bool EndFlag { get; set; }

        public FrontDrill1ParkingTemp(IHttpRequestInvoker httpRequestInvoker,
            IDeviceEngine deviceEngine,
            IMqttClientWrapper mqttClientWrapper,
            DeviceDescriptor deviceDescriptor,
            IOptions<CentralWebOptions> options,
            ILoggerFactory loggerFactory)
            : base(httpRequestInvoker, deviceEngine, mqttClientWrapper, deviceDescriptor, options, loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.cNC84CommandWrapper = this.Engine.DeviceConnector.CNC84CommandWrapper;
            this.logger = loggerFactory.CreateLogger<FrontDrill1ParkingTemp>();
            this.centralWebOptions = options.Value;
            this.httpRequestInvoker = httpRequestInvoker;
            this.spindleNum = deviceDescriptor.SpindleNum;
            this.mqttClientWrapper = mqttClientWrapper;
            this.PayloadPanels = Enumerable.Repeat<Panel?>(null, spindleNum).ToList();

            float panelWidth = 622f;
            if (!float.TryParse(deviceDescriptor.Extra["DefaultPanelWidth"].ToStr(), out panelWidth))
            {
                logger.LogWarning($"未定义DefaultPanelWidth，已使用默认值:{panelWidth}");
            }
            float pinOffset = 0;
            if (!float.TryParse(deviceDescriptor.Extra["DefaultPinOffset"].ToStr(), out pinOffset))
            {
                logger.LogWarning($"未定义DefaultPinOffset，已使用默认值:{pinOffset}");
            }
            //测试阶段
            //TODO:默认加载6个已经钻孔完成的板料
            this.PayloadPanels = new List<Panel?>()
                        {
                             new Panel
                            {
                                PanelCode = $"2023100000001",
                                ItemNo ="item01" ,
                                ProductStatus = ProductStatus.Finished_DRILL,
                                DrillState = PanelDrillState.Drilled,
                                PanelWidth = panelWidth,
                                PinOffset = pinOffset,
                                SiloCode = $"Silo01",
                                Layer = 0,
                                Position = 1,
                                BatchId = "",
                                LotId = "",
                            },
                             new Panel
                            {
                                PanelCode = $"2023100000002",
                                ItemNo ="item01" ,
                                ProductStatus = ProductStatus.Finished_DRILL,
                                DrillState = PanelDrillState.Drilled,
                                PanelWidth = panelWidth,
                                PinOffset = pinOffset,
                                SiloCode = $"Silo01",
                                Layer = 0,
                                Position = 2,
                                BatchId = "",
                                LotId = "",
                            },
                             new Panel
                            {
                                PanelCode = $"2023100000003",
                                ItemNo ="item01" ,
                                ProductStatus = ProductStatus.Finished_DRILL,
                                DrillState = PanelDrillState.Drilled,
                                PanelWidth = panelWidth,
                                PinOffset = pinOffset,
                                SiloCode = $"Silo01",
                                Layer = 0,
                                Position = 3,
                                BatchId = "",
                                LotId = "",
                            },
                             new Panel
                            {
                                PanelCode = $"2023100000004",
                                ItemNo ="item01" ,
                                ProductStatus = ProductStatus.Finished_DRILL,
                                DrillState = PanelDrillState.Drilled,
                                PanelWidth = panelWidth,
                                PinOffset = pinOffset,
                                SiloCode = $"Silo01",
                                Layer = 0,
                                Position = 4,
                                BatchId = "",
                                LotId = "",
                            },
                             new Panel
                            {
                                PanelCode = $"2023100000005",
                                ItemNo ="item01" ,
                                ProductStatus = ProductStatus.Finished_DRILL,
                                DrillState = PanelDrillState.Drilled,
                                PanelWidth = panelWidth,
                                PinOffset = pinOffset,
                                SiloCode = $"Silo01",
                                Layer = 0,
                                Position = 5,
                                BatchId = "",
                                LotId = "",
                            },
                             new Panel
                            {
                                PanelCode = $"2023100000006",
                                ItemNo ="item01" ,
                                ProductStatus = ProductStatus.Finished_DRILL,
                                DrillState = PanelDrillState.Drilled,
                                PanelWidth = panelWidth,
                                PinOffset = pinOffset,
                                SiloCode = $"Silo01",
                                Layer = 0,
                                Position = 6,
                                BatchId = "",
                                LotId = "",
                            }
                        };
            this.spindleAgvPosition = Enumerable.Repeat<string>("null", spindleNum).ToArray();
            var tmpSpindeles = DeviceDescriptor.Extra["Spindles"].ToStr().Split(',', StringSplitOptions.RemoveEmptyEntries);
            Array.Copy(tmpSpindeles, this.spindleAgvPosition, tmpSpindeles.Length);

            AddWatchingProperties();

            this.Connector.ConnectFunc = (deviceDescriptor) =>
             Task.Run(() =>
             {
                 Cnc84Connect();
                 var serialFlag = SerialConnect();
                 var cnc84Flag = cnc84Command != null && cnc84Command.CNC84CommandStatus();
                 return cnc84Flag && serialFlag;
             });

            CollectDataFunc = async (device) =>
            {
                var mergeProperties = RetrieveDataViaPLCAndCnc84();
                SetWatchablePropertiesValue(mergeProperties);

                var mergeInf = WatchingProperties.GetValues();
                await httpRequestInvoker.PostAsJsonAsync<DevicePropertiesReportRequest, DevicePropertiesReportResponse>(
                    centralWebOptions.PropertiesReport,
                    new DevicePropertiesReportRequest()
                    {
                        DeviceId = deviceDescriptor.DeviceId,
                        ProductId = deviceDescriptor.ProductId,
                        Params = mergeInf,
                    }
                 );
            };

            AddWatchingStates();
            AddWatchingEvents();
        }

        public override async Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to ReadProperties!");

            try
            {
                var mergeProperties = RetrieveDataViaPLCAndCnc84();
                SetWatchablePropertiesValue(mergeProperties);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish ReadProperties!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                    .Build(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.FAIL, e.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to WriteProperties!");

            try
            {
                if (deviceServiceInvokeRequest != null && deviceServiceInvokeRequest.Params != null)
                {
                    var param = deviceServiceInvokeRequest.Params.Where(i => WatchingProperties.GetPropertyKeys().Contains(i.Key)).ToDictionary(k => k.Key, v => v.Value);
                    WatchingProperties.SetValues(param);
                }
            }
            catch (Exception e)
            {
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                    .Build(ErrorCodes.Sys.FAIL, e.Message, deviceServiceInvokeRequest.ReplyTopic);
            }

            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish WriteProperties!");
            return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                    .Build(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);

        }

        public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }, "PrepareLoadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }, "InvokeLoadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                if (deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
                {
                    logger.LogDebug($"\r\n整个上下料最后一步 \r\n");

                    bool flag = await LastStep();
                    if (!flag)
                    {
                        return await Response(ErrorCodes.Sys.FAIL, "上下料结束错误", deviceServiceInvokeRequest.ReplyTopic);
                    }
                    logger.LogDebug($"CompleteLoadMaterial 机器【{DeviceDescriptor.DeviceName}】  整个上下料动作结束");
                    return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
                }
                else
                {
                    logger.LogDebug($"\r\n开始降顶升,第 {position} 轴\r\n");
                    await PutUpExtend.ControlSite(position, 0);
                    logger.LogDebug($"\r\n完成降顶升,第 {position} 轴\r\n");
                    await Task.Delay(500);

                    if (deviceServiceInvokeRequest.Params != null && !deviceServiceInvokeRequest.Params.ContainsKey("LoadingPanel"))
                    {
                        logger.LogDebug($"\r\n整个上下料最后一步 未包含 LoadingPanel 参数 \r\n");
                        return await Response(ErrorCodes.Sys.FAIL, "未包含LoadingPanel信息", deviceServiceInvokeRequest.ReplyTopic);
                    }

                    var operationEntity = JsonSerializer.Deserialize<SwapPanel>(deviceServiceInvokeRequest.Params["LoadingPanel"].ToStr()
                        , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (operationEntity != null && operationEntity.PanelList != null && operationEntity.PanelList.Count == 1)
                    {
                        var panel = operationEntity.PanelList[0];
                        if (panel != null)
                        {
                            panel.Layer = 0;
                            panel.DrillState = PanelDrillState.Undrilled;
                        }
                        else
                        {
                            logger.LogDebug($"\r\n panel 为空  不更新PanelList {position - 1} \r\n");
                        }
                        PayloadPanels[position - 1] = panel;
                        logger.LogDebug($"\r\n更新PanelList[ {position - 1}] 为{panel} \r\n");
                    }
                    else
                    {
                        logger.LogDebug($"\r\n LoadingPanel PanelList参数检查，operationEntity != null的值是：{operationEntity != null}");
                        if (operationEntity != null)
                        {
                            logger.LogDebug($"\r\n LoadingPanel PanelList参数检查，operationEntity.PanelList != null的值是：{operationEntity.PanelList != null}");
                            if (operationEntity.PanelList != null)
                            {
                                logger.LogDebug($"\r\n LoadingPanel PanelList参数检查，operationEntity.PanelList.Count的值是：{operationEntity.PanelList.Count}");
                            }
                        }
                        logger.LogDebug($"\r\n LoadingPanel PanelList参数 不正确, 不能更新PanelList[ {position - 1}]列表 \r\n");
                    }
                    return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
                }


            }, "CompleteLoadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                if (!cnc84Command.CNC84CommandStatus())
                {
                    logger.LogDebug($"\r\nCNC84连接断开\r\n");
                    return await Response(ErrorCodes.Sys.FAIL, "CNC84连接断开", deviceServiceInvokeRequest.ReplyTopic);
                }

                if (!await ValidateDoorOpen())
                {
                    logger.LogDebug($"\r\n准备下料过程中验证门未打开状态\r\n");
                    return await Response(ErrorCodes.Sys.FAIL, "钻机门没开", deviceServiceInvokeRequest.ReplyTopic);
                }

                if (deviceServiceInvokeRequest.Params.ContainsKey("IsFirstStep") && deviceServiceInvokeRequest.Params["IsFirstStep"].ToBool())
                {
                    await FirstStep();
                }

                if (EndFlag)
                {
                    logger.LogDebug($"\r\n已经准备好，可以下料\r\n");
                    for (int i = 0; i < PayloadPanels.Count; i++)
                    {
                        var panelInfo = PayloadPanels[i];
                        if (panelInfo != null)
                        {
                            panelInfo.ProductStatus = ProductStatus.Finished_DRILL;
                        }
                    }
                    logger.LogDebug($"\r\n给UnloadingPanel赋值\r\n");
                    deviceServiceInvokeRequest.Params["UnloadingPanel"] = new SwapPanel()
                    {
                        SpindleId = position,
                        PanelList = PayloadPanels
                    };
                    return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.Params, deviceServiceInvokeRequest.ReplyTopic);
                }
                logger.LogDebug($"\r\n未准备好再次执行第一步\r\n");
                await FirstStep();
                if (!EndFlag)
                {
                    logger.LogDebug($"\r\n第一步执行错误不能下料\r\n");
                    return await Response(ErrorCodes.Sys.FAIL, "未开门或者气夹未开或者提升机构操作异常", deviceServiceInvokeRequest.ReplyTopic);
                }

                for (int i = 0; i < PayloadPanels.Count; i++)
                {
                    var panelInfo = PayloadPanels[i];
                    if (panelInfo != null)
                    {
                        panelInfo.ProductStatus = ProductStatus.Finished_DRILL;
                    }
                }
                logger.LogDebug($"\r\n给UnloadingPanel赋值\r\n");
                deviceServiceInvokeRequest.Params["UnloadingPanel"] = new SwapPanel()
                {
                    SpindleId = position,
                    PanelList = PayloadPanels
                };
                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.Params, deviceServiceInvokeRequest.ReplyTopic);

            }, "PrepareUnloadMaterial", false);
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }, "InvokeUnloadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            return await DoService(deviceServiceInvokeRequest, async (request, position) =>
            {
                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, request.Params, deviceServiceInvokeRequest.ReplyTopic);
            }, "CompleteUnloadMaterial");
        }

        public override async Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to Work!");
            try
            {
                if (!cnc84Command.CNC84CommandStatus())
                {
                    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.FAIL, "CNC84断开链接", deviceServiceInvokeRequest.ReplyTopic);
                }

                StartChangeF8();
                logger.LogInformation("切换到F8界面");
                cnc84Command.Start();
                logger.LogInformation("发送打板指令");
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish Work!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.FAIL, e.Message, deviceServiceInvokeRequest.ReplyTopic);
            }

        }

        public override async Task<DeviceServiceInvokeResponse> Standby(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to Standby!");
            try
            {
                if (!cnc84Command.CNC84CommandStatus())
                {
                    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.FAIL, "CNC84断开链接", deviceServiceInvokeRequest.ReplyTopic);
                }

                cnc84Command.Stop();
                logger.LogInformation("发送暂停指令");
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish Standby!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.FAIL, e.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> Shutdown(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to Shutdown!");
            try
            {
                if (!cnc84Command.CNC84CommandStatus())
                {
                    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.FAIL, "CNC84断开链接", deviceServiceInvokeRequest.ReplyTopic);
                }

                cnc84Command.Shutdown();
                logger.LogInformation("发送关闭cnc84指令");
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish Shutdown!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.FAIL, e.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> ScheduleTask(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to ScheduleTask!");
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish ScheduleTask!");
            return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
        }

        public override async Task<DeviceServiceInvokeResponse> GetIsAvailable(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            //检查开门信号，如果未开门，返回fail
            //return await base.GetIsAvailable(deviceServiceInvokeRequest);

            if (!await ValidateDoorOpen())
            {
                return await Response(ErrorCodes.Sys.FAIL, "门没开或者气压异常", deviceServiceInvokeRequest.ReplyTopic);
            }

            return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                    .Build(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
        }

        private void AddWatchingStates()
        {
            WatchingProperties.Properties("Drill_OnNoWork")
               .When(properties =>
               {
                   var isOnNoWork = properties.Property("Drill_OnNoWork").NewValue.ToBool();
                   return !isOnNoWork;
               })
              .TriggerAlways(async () =>
              {
                  Status = DeviceStatus.Ready;
                  var request = MakeStatusRequest(DeviceStatus.Ready);
                  await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, request);
              });

            WatchingProperties.Properties("Drill_OnWork")
                .When(properties =>
                {
                    var isOnWork = properties.Property("Drill_OnWork").NewValue.ToBool();
                    return isOnWork;
                })
               .TriggerAlways(async () =>
               {
                   Status = DeviceStatus.Working;
                   var request = MakeStatusRequest(DeviceStatus.Ready);
                   await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, request);
               });

            WatchingProperties.Properties("Drill_NoError")
                .When(properties =>
                {
                    var isNoErrorOnCnc84 = properties.Property("Drill_NoError").NewValue.ToBool();
                    return !isNoErrorOnCnc84;
                })
                .TriggerAlways(async () =>
                {
                    Status = DeviceStatus.Exception;
                    var request = MakeStatusRequest(DeviceStatus.Exception);

                    await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, request);
                });
        }
        public async Task<bool> ValidateDoorOpen()
        {
            var doorOpen = (cnc84Command.GetIutput(DeviceDescriptor.Extra["DoorOpenFlag"].ToInt()) == "1:1");
            var airClose = (cnc84Command.GetIutput(DeviceDescriptor.Extra["AirCloseFlag"].ToInt()) == "1:2");
            logger.LogDebug($"\r\n门开标识:{doorOpen}\r\n");
            logger.LogDebug($"\r\n气压异常标识:{airClose}\r\n");
            if (!doorOpen || airClose)
            {
                return false;
            }
            await Task.Delay(1000);
            return true;
        }
        private DeviceStatusReportRequest MakeStatusRequest(DeviceStatus newStatus)
        {
            var deviceServiceInvokeRequest = new DeviceStatusReportRequest
            {
                DeviceId = DeviceDescriptor.DeviceId,
                ProductId = DeviceDescriptor.ProductId,
                NewStatus = newStatus
            };

            return deviceServiceInvokeRequest;
        }

        private void AddWatchingProperties()
        {
            WatchingProperties.AddProperty("Drill_ConnectionStatus", false)
                              .AddProperty("Drill_ScreenText", "")
                              .AddProperty("Drill_OnWork", true)
                              .AddProperty("Drill_OnNoWork", false)
                              .AddProperty("Drill_P1FrontParkingPositionFlag", false)
                              .AddProperty("Drill_P1RearParkingPositionFlag", false)
                              .AddProperty("Drill_OnP1ParkingPositionFlag", false)
                              .AddProperty("Drill_NoError", true)
                              .AddProperty("Drill_SpindleAirError", true)
                              .AddProperty("Drill_DoorOpenStatus", "")
                              .AddProperty("Drill_SplineStatus", "")
                              .AddProperty("PrepareLoadOk", false)
                              .AddProperty("InvokeLoadOk", false)
                              .AddProperty("CompleteLoadOk", false)
                              .AddProperty("PrepareUnloadOk", false)
                              .AddProperty("InvokeUnloadOk", false)
                              .AddProperty("CompleteUnload", false)
                              .AddProperty("CompleteAllLoadOrUnloadAction", false)
                              .AddProperty("Drill_CurrentProgramFile", "")
                              .AddProperty("Drill_CurrentParameterFile", "")
                              .AddProperty("Drill_CurrentAtpFile", "");
        }

        private void AddWatchingEvents()
        {
            WatchingProperties.Property("Drill_OnWork")
                .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    logger.LogDebug($"\r\n钻机变成工作状态\r\n");
                    await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                        centralWebOptions.EventReport,
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = this.ClientId,
                            EventId = Events.Drill.DRILL_START_WORK_EVENT,
                            EventName = Events.Drill.DRILL_START_WORK_EVENT_NAME
                        });

                    PayloadPanels.ForEach(panel =>
                    {
                        if (panel != null) panel.DrillState = PanelDrillState.Drilling;
                    });
                });
            WatchingProperties.Property("Drill_OnNoWork")
              .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  try
                  {
                      logger.LogDebug($"\r\n钻机变成非工作状态\r\n");
                      var status = WatchingProperties.Property("Drill_SplineStatus").NewValue.ToStr();
                      var agvPosition = spindleAgvPosition;
                      for (int i = 0; i < status.Length; i++)
                      {
                          if (status[i] == '0')
                          {
                              agvPosition[i] = "null";
                          }
                      }
                      var count = status.Count(c => c == '1');
                      PayloadPanels.ForEach(panel =>
                      {
                          if (panel != null)
                          {
                              panel.DrillState = PanelDrillState.Drilled;
                              panel.ProductStatus = ProductStatus.Finished_DRILL;
                          }
                      });
                      logger.LogDebug($"\r\n钻机发起缺料上报\r\n");
                      var resopnse = await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                          centralWebOptions.EventReport,
                          new DeviceEventReportRequest()
                          {
                              ProductId = DeviceDescriptor.ProductId,
                              DeviceId = DeviceDescriptor.DeviceId,
                              ClientId = this.ClientId,
                              EventId = Events.REQUEST_AGV_UNLOAD_PANEL_THEN_LOAD_PANEL,
                              EventName = Events.Drill.DRILL_UNLOAD_PANEL_THEN_LOAD_PANEL_EVENT_NAME,
                              RequestInteractionBehavior = InteractionBehavior.UnloadPanelThenLoadPanel,
                              Params = new Dictionary<string, object?>()
                              {
                                { "SpindleNum", count },
                                { "Spindles", string.Join(",",agvPosition) }
                              },
                              PayloadPanels = PayloadPanels,
                              RequestInputProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_2,
                              RequestOutputProductStatus = ProductStatus.Finished_DRILL
                          });
                      if (resopnse == null || resopnse.Code != ErrorCodes.Sys.SUCCESS)
                      {
                          logger.LogError($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r --{ProductId}-{DeviceName}-{DeviceId} 上报缺料事件错误 \n\r--");
                      }
                      else
                      {
                          logger.LogInformation($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r --{ProductId}-{DeviceName}-{DeviceId} 上报缺料事件 成功 " +
                            $" code: {resopnse?.Code} message:{resopnse?.Message}  EventId :{resopnse.EventId} {JsonSerializer.Serialize(resopnse)}\n\r--");
                      }
                      var onWork = (cnc84Command.GetOutput(DeviceDescriptor.Extra["WorkFlag"].ToInt()) == "1:1");
                      logger.LogDebug($"\r\n再次确认钻机的工作状态 {onWork} \r\n");
                      if (!onWork)
                      {
                          await FirstStep();

                          await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                         centralWebOptions.EventReport,
                         new DeviceEventReportRequest()
                         {
                             ProductId = DeviceDescriptor.ProductId,
                             DeviceId = DeviceDescriptor.DeviceId,
                             ClientId = this.ClientId,
                             EventId = Events.Drill.DRILL_OPEN_DOOR_EVENT,
                             EventName = Events.Drill.DRILL_OPEN_DOOR_EVENT_NAME,
                             PayloadPanels = PayloadPanels,
                         });
                      }
                      else
                      {
                          logger.LogInformation($"---钻机在工作状态不能执行顶升不能开启门\n\r--");
                      }
                  }
                  catch (Exception ee)
                  {
                      logger.LogDebug($"\r\n钻机在Drill_OnNoWork 事件中发生异常{ee.Message}\r\n");//todo
                  }
              });

            WatchingProperties.Property("Drill_OnP1ParkingPositionFlag")
             .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
             .TriggerAlways(async () =>
             {
                 await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                     centralWebOptions.EventReport,
                     new DeviceEventReportRequest()
                     {
                         ProductId = DeviceDescriptor.ProductId,
                         DeviceId = DeviceDescriptor.DeviceId,
                         ClientId = this.ClientId,
                         EventId = Events.Drill.DRILL_ON_P1_PARKING_POSITION_EVENT,
                         EventName = Events.Drill.DRILL_ON_P1_PARKING_POSITION_EVENT_NAME,
                         PayloadPanels = PayloadPanels,
                     });
             });

        }

        private async Task FirstStep()
        {
            try
            {
                logger.LogDebug($"\r\n开始执行第一步\r\n");
                logger.LogDebug($"\r\n发送开门指令\r\n");
                //开门
                cnc84Command.SetCncComand(DeviceDescriptor.Extra["OpenDoor"].ToStr());
                await Task.Delay(500);
                if (!await ValidateDoorOpen())
                {
                    logger.LogDebug($"\r\n门没开 第一步执行错误\r\n");
                    //EndFlag = false;
                    //return;
                }
                logger.LogDebug($"\r\n发送关气夹指令\r\n");
                //关气夹
                cnc84Command.SetCncComand(DeviceDescriptor.Extra["CloseClamp"].ToStr());
                await Task.Delay(500);
                logger.LogDebug($"\r\n发送开气夹指令\r\n");
                //开气夹
                cnc84Command.SetCncComand(DeviceDescriptor.Extra["OpenClamp"].ToStr());
                await Task.Delay(500);
                //顶升
                logger.LogDebug($"\r\n发送1号轴顶升指令\r\n");
                await PutUpExtend.ControlSite(1, 1);
                await Task.Delay(500);
                logger.LogDebug($"\r\n发送2号轴顶升指令\r\n");
                await PutUpExtend.ControlSite(2, 1);
                await Task.Delay(500);
                logger.LogDebug($"\r\n发送3号轴顶升指令\r\n");
                await PutUpExtend.ControlSite(3, 1);
                await Task.Delay(500);
                logger.LogDebug($"\r\n发送4号轴顶升指令\r\n");
                await PutUpExtend.ControlSite(4, 1);
                await Task.Delay(500);
                logger.LogDebug($"\r\n发送5号轴顶升指令\r\n");
                await PutUpExtend.ControlSite(5, 1);
                await Task.Delay(500);
                logger.LogDebug($"\r\n发送6号轴顶升指令\r\n");
                await PutUpExtend.ControlSite(6, 1);
                await Task.Delay(500);
                EndFlag = true;
            }
            catch (Exception e)
            {
                logger.LogDebug($"\r\n执行第一步发生异常{e.Message}\r\n");
                logger.LogError($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r --{ProductId}-{DeviceName}-{DeviceId} FirstStep 事件错误 {e.Message} \n\r--");

                EndFlag = false;
            }
            logger.LogDebug($"\r\n第一步执行结束\r\n");
        }

        private async Task<bool> LastStep()
        {
            try
            {
                logger.LogDebug($"\r\n开始执行最后一步 \r\n");
                //关门
                //cnc84Command.SetCncComand(DeviceDescriptor.Extra["CloseDoor"].ToStr());
                //await Task.Delay(500);
                //顶升降
                logger.LogDebug($"\r\n 发送降1号轴指令 \r\n");
                await PutUpExtend.ControlSite(1, 0);
                await Task.Delay(500);
                logger.LogDebug($"\r\n 发送降2号轴指令 \r\n");
                await PutUpExtend.ControlSite(2, 0);
                await Task.Delay(500);
                logger.LogDebug($"\r\n 发送降3号轴指令 \r\n");
                await PutUpExtend.ControlSite(3, 0);
                await Task.Delay(500);
                logger.LogDebug($"\r\n 发送降4号轴指令 \r\n");
                await PutUpExtend.ControlSite(4, 0);
                await Task.Delay(500);
                logger.LogDebug($"\r\n 发送降5号轴指令 \r\n");
                await PutUpExtend.ControlSite(5, 0);
                await Task.Delay(500);
                logger.LogDebug($"\r\n 发送降6号轴指令 \r\n");
                await PutUpExtend.ControlSite(6, 0);
                await Task.Delay(500);

                //关气夹
                logger.LogDebug($"\r\n 发送关气夹指令 \r\n");
                cnc84Command.SetCncComand(DeviceDescriptor.Extra["CloseClamp"].ToStr());
                await Task.Delay(500);

                //解析加载文件
                var panel = PayloadPanels.FirstOrDefault(x => x != null);
                if (panel != null)
                {
                    string batchId = panel.ItemNo;
                    logger.LogDebug($"\r\n 加载文件的条码是{batchId} \r\n");
                    //var (drlPath, diaPath) = ParseBatchId(batchId);
                    var (drlPath, diaPath) = DefaultBatchId(batchId);
                    logger.LogDebug($"\r\n 要加载的程序是{drlPath} 要加载的参数文件是{diaPath} \r\n");
                    if (!LoadFile(drlPath, diaPath))
                    {
                        WatchingProperties.SetValues(new Dictionary<string, object?>()
                        {
                             { GLOBAL_EXCEPTION_EVENT_NAME,Events.Drill.DRILL_LOADFILE_FALT_EVENT_NAME },
                             { GLOBAL_EXCEPTION_EVENT_MESSAGE, $"{Events.Drill.DRILL_LOADFILE_FALT_EVENT_NAME } 条码信息 {batchId} 钻带文件 {drlPath}  参数文件 {diaPath}" },
                             { GLOBAL_EXCEPTION_EVENT_ID, Events.Drill.DRILL_LOADFILE_FALT_EVENT },
                        });
                        logger.LogDebug($"\r\n 加载的程序或者参数文件失败 \r\n");
                        return false;
                    }
                    //await Task.Delay(1000);
                    //cnc84Command.SetPcKey(@"\ESC");
                    //StartChangeF8();
                    //logger.LogInformation("切换到F8界面");
                    //cnc84Command.Start();
                    //logger.LogInformation("发送打板指令");

                }
                else
                {
                    logger.LogDebug($"\r\n PayloadPanels为空不执行加载文件 \r\n");
                }
                logger.LogDebug($"\r\n完成最后一步 \r\n");
                return true;
            }
            catch (Exception ee)
            {
                EndFlag = false;
                WatchingProperties.SetValues(new Dictionary<string, object?>()
                {
                    { GLOBAL_EXCEPTION_EVENT_NAME,"Exception"},
                    { GLOBAL_EXCEPTION_EVENT_MESSAGE,ee.Message },
                    { GLOBAL_EXCEPTION_EVENT_ID, $"LastStep_Exception" }
                });
                logger.LogError($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r --{ProductId}-{DeviceName}-{DeviceId} LastStep 事件错误 {ee.Message} \n\r--");
                logger.LogDebug($"\r\n最后一步发生异常{ee.Message} \r\n");
                return false;
            }
        }
        private (string, string) ParseBatchId(string code)
        {
            var drlPath = Path.Combine(DeviceDescriptor.Extra["DrlSearchPath"].ToStr(), code + "." + DeviceDescriptor.Extra["DrlSearchFileExt"].ToStr());
            var diaPath = TryGetDiaFilePath(drlPath);
            return (drlPath, diaPath);
        }
        private (string, string) DefaultBatchId(string code)
        {
            var drlPath = Path.Combine(DeviceDescriptor.Extra["DrlSearchPath"].ToStr(), code + "." + DeviceDescriptor.Extra["DrlSearchFileExt"].ToStr());
            var diaPath = Path.Combine(DeviceDescriptor.Extra["DiaSearchPath"].ToStr(), code + "." + DeviceDescriptor.Extra["DiaSearchFileExt"].ToStr());
            return (drlPath, diaPath);
        }
        private string TryGetDiaFilePath(string srcFileName)
        {
            string diaSearchPath = DeviceDescriptor.Extra["DiaSearchPath"].ToStr();
            string diaSearchFileExt = DeviceDescriptor.Extra["DiaSearchFileExt"].ToStr();

            ISet<string> otherfileName = GetOtherFileName(srcFileName);
            IDictionary<string, string> allDiaFiles = GetDiaFilesPath(diaSearchPath, diaSearchFileExt.Split(';'));

            KeyValuePair<string, string> diaFileInfo = allDiaFiles.FirstOrDefault((diaDic) =>
            {
                if (otherfileName != null)
                {
                    foreach (var item in otherfileName)
                    {
                        if (Path.GetFileNameWithoutExtension(diaDic.Key).Equals(item))
                        {
                            return true;
                        }
                    }
                }
                return false;
            });
            if (diaFileInfo.Key == null)
            {
                return "";
            }
            return diaFileInfo.Value;

        }
        private ISet<string> GetOtherFileName(string filePath)
        {
            string extfileName = Path.GetFileName(filePath);
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(filePath)!);
            FileInfo[] afi = di.GetFiles("*.*");
            ISet<string> onlyFileName = new HashSet<string>(afi.SkipWhile((fi) => { return fi.Name.ToLower().Equals(extfileName.ToLower()); })
                .Select(fi => Path.GetFileNameWithoutExtension(fi.Name)));
            return onlyFileName;
        }
        private IDictionary<string, string> GetDiaFilesPath(string filePath, params string[] exts)
        {
            IDictionary<string, string> filePathDic = new Dictionary<string, string>();
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] afi = di.GetFiles("*.*");
            IEnumerable<FileInfo> afiWithExt = afi.Where((fi) =>
            {
                foreach (var ext in exts)
                {
                    if (Path.GetExtension(fi.Name).Equals(ext, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            });

            foreach (var item in afiWithExt)
            {
                filePathDic.Add(item.Name, item.FullName);
            }
            return filePathDic;
        }

        private bool SerialConnect()
        {
            try
            {
                PutUpExtend.Init(DeviceDescriptor.Extra["Com"].ToStr());
                return PutUpExtend.ConnectStatus();
            }
            catch (Exception e)
            {
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}  SerialConnect {e.Message}!");
            }
            return false;

        }

        private void Cnc84Connect()
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}   begin to Cnc84Connect!");
            this.cnc84Command = this.cNC84CommandWrapper.CreateCNC84Command();
            cnc84Command.Init(DeviceDescriptor.Extra["CNC84Ip"].ToStr(), DeviceDescriptor.Extra["CNC84Port"].ToInt());
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}   finish Cnc84Connect!");
        }

        private Dictionary<string, object> DrillScreenText()
        {
            var screenProperties = new Dictionary<string, object>();
            var vgCNCScreenSaver = cnc84Command.GetScreenText();
            var isErrorOnCNC84 = CNC84Error(vgCNCScreenSaver);

            screenProperties.Add("Drill_ScreenText", vgCNCScreenSaver?.ScreenText);
            screenProperties.Add("Drill_NoError", !isErrorOnCNC84);

            return screenProperties;
        }

        private bool CNC84Error(VgCNCScreenSaver vgCNCScreenSaver)
        {
            if (vgCNCScreenSaver == null) { return false; }
            var color = vgCNCScreenSaver.BackColor;
            var message = vgCNCScreenSaver.ScreenText;
            switch (color)
            {
                case "0000FF":
                    {
                        var messageMath = Regex.Match(message, "\\[([\\d]*)\\]");

                        if (messageMath.Success)
                        {
                            var errorId = int.Parse(messageMath.Groups[1].Value);
                            return errorId != 48;
                        }
                        break;
                    }
                case "FF0000":
                    {
                        var messageMath = Regex.Match(message, "\\[([\\d]*)\\]");

                        if (messageMath.Success)
                        {
                            var errorId = int.Parse(messageMath.Groups[1].Value);
                            return sequenceError.Contains(errorId);
                        }
                        break;
                    }
                case "8000":
                    {
                        var messageMath = Regex.Match(message, "\\[([\\d]*)\\]");

                        if (messageMath.Success)
                        {
                            var errorId = int.Parse(messageMath.Groups[1].Value);
                            return sequenceAlarm.Contains(errorId);
                        }
                        break;

                    }
                default: break;
            }
            return false;
        }
        private Dictionary<string, object> DrillInformation()
        {
            var properties = new Dictionary<string, object>();
            try
            {
                if (cnc84Command.CNC84CommandStatus())
                {
                    properties.Add("Drill_ConnectionStatus", true);
                    properties.Add("Drill_P1FrontParkingPositionFlag", (cnc84Command.GetOutput(DeviceDescriptor.Extra["P1FrontPostion"].ToInt()) == "1:1"));
                    properties.Add("Drill_P1RearParkingPositionFlag", (cnc84Command.GetOutput(DeviceDescriptor.Extra["P1RearPostion"].ToInt()) == "1:0"));

                    if (properties["Drill_P1FrontParkingPositionFlag"].ToBool() && properties["Drill_P1RearParkingPositionFlag"].ToBool())
                    {
                        properties.Add("Drill_OnP1ParkingPositionFlag", true);
                    }
                    else
                    {
                        properties.Add("Drill_OnP1ParkingPositionFlag", false);
                    }

                    var onWork = (cnc84Command.GetOutput(DeviceDescriptor.Extra["WorkFlag"].ToInt()) == "1:1");
                    properties.Add("Drill_OnWork", onWork);
                    properties.Add("Drill_OnNoWork", !onWork);

                    var doorOpen = (cnc84Command.GetIutput(DeviceDescriptor.Extra["DoorOpenFlag"].ToInt()) == "1:2");
                    properties.Add("Drill_DoorOpenStatus", doorOpen);

                    var airClose = (cnc84Command.GetIutput(DeviceDescriptor.Extra["AirCloseFlag"].ToInt()) == "1:2");
                    properties.Add("Drill_SpindleAirError", airClose);

                    var subProperties = DrillScreenText();
                    foreach (var item in subProperties)
                    {
                        properties.Add(item.Key, item.Value);
                    }
                    var programPath = cnc84Command.GetACTProgram();
                    var parameterPath = cnc84Command.GetDiaFileNameWithDialog();
                    var atpPath = cnc84Command.GetAtpFileName();
                    properties.Add("Drill_CurrentProgramFile", programPath);
                    properties.Add("Drill_CurrentParameterFile", parameterPath);
                    properties.Add("Drill_CurrentAtpFile", atpPath);

                    var cncStatus = cnc84Command.GetCncStatus();
                    if (cncStatus != null)
                    {
                        properties.Add("Drill_SplineStatus", string.Join("", cncStatus.SpindleStatus.Skip(cncStatus.SpindleStatus.Length - spindleNum).Reverse()));
                    }

                }
                else
                {
                    properties.Add("Drill_ConnectionStatus", false);
                }

            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

            return properties;

        }

        private bool LoadFile(string drilPath, string? diaPath = "")
        {
            cnc84Command.SetCncComand("CM@@@");
            if (!ValidateCmDrlFile())
            {
                return false;
            }
            if (!string.IsNullOrWhiteSpace(diaPath) && !ValidateDiaFileSame(diaPath))
            {
                cnc84Command.SetLoadFile(diaPath);
                if (!ValidateDiaFile(diaPath))
                {
                    return false;
                }
            }
            cnc84Command.SetLoadFile(drilPath);
            if (!ValidateDrlFile(drilPath))
            {
                return false;
            }
            return true;

        }

        private bool ValidateDrlFile(string drilFilePath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var netProName = string.Empty;
            do
            {
                Thread.Sleep(100);
                netProName = cnc84Command.GetCncStatus()?.ProgramName;
                if (drilFilePath.ToLower().Equals(netProName, StringComparison.CurrentCultureIgnoreCase))
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDrlFileTimeout"].ToLong());
            return false;
        }

        private bool ValidateCmDrlFile()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var netProName = string.Empty;
            do
            {
                Thread.Sleep(1000);
                netProName = cnc84Command.GetACTProgram();
                if (string.IsNullOrEmpty(netProName) || "[NULL]".Equals(netProName))
                {
                    stopwatch.Stop();
                    return true;
                }
                cnc84Command.SetCncComand("CM@@@");
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateCmFileTimeout"].ToLong());
            return false;
        }

        private bool ValidateDiaFile(string diaFilePath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var diaName = string.Empty;
            do
            {
                Thread.Sleep(50);
                diaName = cnc84Command.GetRuntimeString("%S(DiaFileNameWithDialog)");
                if ($"1:{diaFilePath}".Equals(diaName, StringComparison.OrdinalIgnoreCase))
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["ValidateDiaFileTimeout"].ToLong());
            stopwatch.Stop();
            return false;
        }
        private bool ValidateDiaFileSame(string diaFilePath)
        {
            var diaName = cnc84Command.GetRuntimeString("%S(DiaFileNameWithDialog)");
            if ($"1:{diaFilePath}".Equals(diaName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private bool StartPressBoard()
        {
            var toolNum = GetToolNum();
            if (!"0".Equals(toolNum))
            {
                Thread.Sleep(3000);
                cnc84Command.SetCncComand("T");
            }

            if (!"0".Equals(toolNum) && !ValidateRetractToolEnd())
            {
                return false;
            }

            Thread.Sleep(2000);
            PressBoard();
            if (!ValidatePressBoardEnd())
            {
                return false;
            }

            return true;
        }

        private bool ValidateRetractToolEnd()
        {
            Thread.Sleep(2000);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var toolNum = string.Empty;
            var commStatus = string.Empty;

            do
            {
                Thread.Sleep(50);
                toolNum = GetToolNum();
                if ("0".Equals(toolNum) && commStatus != "BUSY:T" && string.IsNullOrEmpty(commStatus))
                {
                    stopwatch.Stop();
                    Thread.Sleep(2000);
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["RetractToolEndTimeout"].ToLong());

            stopwatch.Stop();
            return false;
        }

        private string GetToolNum()
        {
            return cnc84Command.GetToolParameter().ToolNumber;
        }

        private void PressBoard()
        {
            cnc84Command.SetCncComand("M102");
        }

        private bool ValidatePressBoardEnd()
        {
            Thread.Sleep(2000);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                if ("1".Equals(cnc84Command.GetSeqFlag(68), StringComparison.CurrentCultureIgnoreCase))
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["PressBoardEndTimeout"].ToLong());

            return false;
        }

        private bool StartOpenMushroom()
        {
            if (!MushroomCloseLocalTion())
            {
                return false;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            ControlMushroom();

            do
            {
                if (!MushroomCloseLocalTion())
                {
                    stopwatch.Stop();
                    return true;
                }
            } while (stopwatch.ElapsedMilliseconds < DeviceDescriptor.Extra["MushroomEndTimeOut"].ToLong());

            return false;
        }

        private bool MushroomCloseLocalTion()
        {
            var result = cnc84Command.GetOutput(55);

            if ("1:1".Equals(result, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private void ControlMushroom()
        {
            cnc84Command.SetCncComand("M27");
        }

        private void StartDrilBoard()
        {
            Thread.Sleep(2000);
            StartChangeF8();
            Thread.Sleep(2000);
            cnc84Command.SetCncComand("M101");
        }

        private void StartChangeF8()
        {
            cnc84Command.SetChangePage("WORK_WORK");
        }

        private Dictionary<string, object?> RetrieveDataViaPLCAndCnc84()
        {
            var mergeProperties = new Dictionary<string, object?>();
            var drillProperties = DrillInformation();
            foreach (var item in drillProperties)
            {
                mergeProperties.Add(item.Key, item.Value);
            }
            return mergeProperties;
        }

        private void SetWatchablePropertiesValue(Dictionary<string, object?> values)
        {
            WatchingProperties.SetValues(values);
        }

        private async Task<DeviceServiceInvokeResponse> DoService(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
        Func<DeviceServiceInvokeRequest, int, Task<DeviceServiceInvokeResponse>> action, string methodName = "", bool isMonaxialModel = true)
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r --{ProductId}-{DeviceName}-{DeviceId} begin to {methodName}!\n\r--");
            var eventId = "";
            var eventName = "";
            var eventMessage = "";
            try
            {
                if (!PutUpExtend.ConnectStatus())
                {
                    eventId = Events.Drill.DRILL_PUT_UP_EXTEND_UNCONNECT_EVENT;
                    eventName = Events.Drill.DRILL_PUT_UP_EXTEND_UNCONNECT_EVENT_NAME;
                    eventMessage = "提升机构串口连接不成功";
                    return await Response(ErrorCodes.Sys.FAIL, "提升机构串口连接不成功", deviceServiceInvokeRequest.ReplyTopic);
                }
                ushort position = 1;
                if (isMonaxialModel)
                {

                    if (deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("Position"))
                    {
                        eventId = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT;
                        eventName = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT_NAME;
                        eventMessage = $"{methodName} 传入的参数不包含位置信息";
                        return await Response(ErrorCodes.Sys.FAIL, $"{methodName} 传入的参数不包含位置信息", deviceServiceInvokeRequest.ReplyTopic);
                    }

                    position = deviceServiceInvokeRequest.Params["Position"].ToUshort();
                    if ((!(position > 0 && position <= spindleNum)))
                    {
                        eventId = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT;
                        eventName = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT_NAME;
                        eventMessage = $"{methodName} 下发Position参数值{position}不在1和{spindleNum}之间，请下发正确的上料轴信息";
                        logger.LogError(eventMessage);
                        return await Response(ErrorCodes.Sys.FAIL, eventMessage, deviceServiceInvokeRequest.ReplyTopic);
                    }
                }
                var actionResult = await action.Invoke(deviceServiceInvokeRequest, position);
                if (actionResult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    eventId = actionResult.Code;
                    eventName = $"方法：{methodName} 未正常执行";
                    eventMessage = actionResult.Message;
                }
                return actionResult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                eventId = $"{methodName}_Exception";
                eventName = "Exception";
                eventMessage = ex.Message;
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                this.WatchingProperties.SetValues(new Dictionary<string, object?>() {
                        { GLOBAL_EXCEPTION_EVENT_NAME,eventName },
                        { GLOBAL_EXCEPTION_EVENT_MESSAGE, eventMessage },
                        { GLOBAL_EXCEPTION_EVENT_ID, eventId },
                    });
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r -{ProductId}-{DeviceName}-{DeviceId} finish {methodName}!\n\r--");
            }

        }

    }
}
