using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Fundation.Vendor.Vega;

namespace VgAutoDrill.JinLu.Mock
{
    public class MockDrill : MockBaseDevice<MockDrill>
    {
        private readonly ILogger<MockDrill> logger;
        private readonly ILoggerFactory loggerFactory;
        private readonly IHttpRequestInvoker httpRequestInvoker;
        private readonly byte slaveID;
        private readonly int spindleNum;
        private DeviceStatus deviceStatus = DeviceStatus.Ready;
        private readonly CentralWebOptions centralWebOptions;
        protected readonly IMqttClientWrapper mqttClientWrapper;

        public MockDrill(IHttpRequestInvoker httpRequestInvoker,
            IDeviceEngine deviceEngine,
            IMqttClientWrapper mqttClientWrapper,
            DeviceDescriptor deviceDescriptor,
            IOptions<CentralWebOptions> options,
            ILoggerFactory loggerFactory)
            : base(httpRequestInvoker, deviceEngine, mqttClientWrapper, deviceDescriptor, options, loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            logger = loggerFactory.CreateLogger<MockDrill>();
            centralWebOptions = options.Value;
            this.httpRequestInvoker = httpRequestInvoker;
            slaveID = (byte)deviceDescriptor.Extra["SlaveID"].ToInt();
            this.mqttClientWrapper = mqttClientWrapper;

            AddWatchingProperties();

            spindleNum = deviceDescriptor.SpindleNum.ToInt();
            var layerLimit = deviceDescriptor.LayerLimit.ToInt();
            this.PayloadPanels = Enumerable.Repeat<Panel?>(null, spindleNum * layerLimit).ToList();

            this.Connector.ConnectFunc = async (deviceDescriptor) =>
            {
                await Task.Run(() =>
                {
                    Cnc84Connect();
                    PlcConnect();
                    return true;
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
                            {"Drill_ScreenText", deviceDescriptor.DeviceId}
                        }
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
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish ReadProperties!");
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                    .Build(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.FAIL, e.Message,
                                   deviceServiceInvokeRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to WriteProperties!");

            try
            {
                if (deviceServiceInvokeRequest != null && deviceServiceInvokeRequest.Params != null)
                {
                    WatchingProperties.SetValues(deviceServiceInvokeRequest.Params);

                    //mock 主动报告状态为Ready
                    if (deviceServiceInvokeRequest.Params["SetReady"].ToStr("0") == "1")
                    {
                        var requestStatus = GetStatusRequest(DeviceStatus.Ready);
                        var responseStatus = await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, requestStatus);
                        if (responseStatus.Code != ErrorCodes.Sys.SUCCESS)
                        {
                            logger.LogWarning($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r - {ProductId} - {DeviceName} - {DeviceId}  fail to reset AGV status!\n\r-----------");
                        }
                        logger.LogWarning($"{DateTime.Now.ToLongTimeString()}\n\r-----------\n\r - {ProductId} - {DeviceName} - {DeviceId}  success to reset AGV status!\n\r-----------");
                    }

                    //Mock 钻机呼叫缺料
                    if (deviceServiceInvokeRequest.Params["SetRequestMaterial02"].ToStr("0") == "1")
                    {
                        //layer 生料、钻孔、熟料
                        //PayloadPanels.ForEach(panel =>
                        //{
                        //    if (panel != null) panel.DrillState = PanelDrillState.Drilled;
                        //});

                        await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
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
                                    { "SpindleNum", 6 },
                                    { "SpindleStatus", "111111" }
                                },
                                PayloadPanels = PayloadPanels,
                                RequestInputProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_2,
                                RequestOutputProductStatus = ProductStatus.Finished_DRILL
                            });

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
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to PrepareLoadMaterial!");

            var response = new DeviceServiceInvokeResponse()
            {
                Params = deviceServiceInvokeRequest.Params,
            };

            try
            {
                //if (response.Params == null || !response.Params.ContainsKey("Position"))
                //{
                //    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                //                    .Build(ErrorCodes.Sys.FAIL, "传入的参数不包含位置信息", deviceServiceInvokeRequest.ReplyTopic);
                //}

                //ushort position = response.Params["Position"].ToUshort();
                //if (!(position > 0 && position <= 6))
                //{
                //    var errorMessage = $" PrepareLoadMaterial 下发Postion参数不正确，请下发正确的上料轴信息";
                //    response.Params.TryAdd("Exception", errorMessage);
                //    response.Params.TryAdd("Result", false);
                //    logger.LogError(errorMessage);

                //    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                //                     .Build(ErrorCodes.Sys.FAIL, errorMessage, response.Params, deviceServiceInvokeRequest.ReplyTopic);
                //}

                //logger.LogInformation($"PrepareLoadMaterial 机器【{DeviceDescriptor.DeviceName}】 轴【{position}】  无生料");
                //logger.LogInformation($"PrepareLoadMaterial 机器【{DeviceDescriptor.DeviceName}】 轴【{position}】  上料已经准备好");

                var dic = new Dictionary<string, object?>();
                dic.Add("PrepareLoadOk", true);
                dic.Add("InvokeLoadOk", false);
                dic.Add("CompleteLoadOk", false);
                WatchingProperties.SetValues(dic);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish PrepareLoadMaterial!");
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.SUCCESS, string.Empty, response.Params, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                response.Params.TryAdd("Result", false);
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.FAIL, e.Message, response.Params, deviceServiceInvokeRequest.ReplyTopic);
            }

        }

        public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to InvokeLoadMaterial!");

            var response = new DeviceServiceInvokeResponse()
            {
                Params = deviceServiceInvokeRequest.Params,
            };

            try
            {
                if (response.Params == null || !response.Params.ContainsKey("Position"))
                {
                    logger.LogInformation($" InvokeLoadMaterial 机器【{DeviceDescriptor.DeviceName}】 {deviceServiceInvokeRequest}  未设置Params");
                    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                    .Build(ErrorCodes.Sys.FAIL, "传入的参数不包含位置信息", deviceServiceInvokeRequest.ReplyTopic);
                }

                ushort position = response.Params["Position"].ToUshort();
                if (!(position > 0 && position <= 6))
                {
                    var errorMessage = $" InvokeLoadMaterial 下发Postion参数不正确，请下发正确的上料轴信息";
                    response.Params.TryAdd("Exception", errorMessage);
                    response.Params.TryAdd("Result", false);
                    logger.LogError(errorMessage);

                    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                     .Build(ErrorCodes.Sys.FAIL, errorMessage, response.Params, deviceServiceInvokeRequest.ReplyTopic);
                }

                logger.LogInformation($"InvokeLoadMaterial 机器【{DeviceDescriptor.DeviceName}】 轴【{position}】  设置上料位置");
                logger.LogInformation($"InvokeLoadMaterial 机器【{DeviceDescriptor.DeviceName}】 轴【{position}】  开始上料");

                WatchingProperties.Property("Buffer_StartLoadRawMaterial").SetValue(true);
                WatchingProperties.Property("Buffer_EndLoadRawMaterial").SetValue(false);

                Task.Delay(2000).Wait();
                var dic = new Dictionary<string, object?>();
                dic.Add("PrepareLoadOk", false);
                dic.Add("InvokeLoadOk", true);
                dic.Add("CompleteLoadOk", false);
                WatchingProperties.SetValues(dic);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish InvokeLoadMaterial!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                   .Build(ErrorCodes.Sys.SUCCESS, string.Empty, response.Params, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                response.Params.TryAdd("Result", false);
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.FAIL, e.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to CompleteLoadMaterial!");

            var response = new DeviceServiceInvokeResponse()
            {
                Params = new Dictionary<string, object?>(),
            };
            try
            {
                Task.Delay(500).Wait();
                logger.LogInformation($"CompleteLoadMaterial 机器【{DeviceDescriptor.DeviceName}】  上料动作完成");
                logger.LogInformation($"CompleteLoadMaterial 机器【{DeviceDescriptor.DeviceName}】  电机停止转动");

                var operationEntity = JsonSerializer.Deserialize<SwapPanel>(deviceServiceInvokeRequest.Params["LoadingPanel"].ToStr(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                WatchingProperties.Property("Buffer_StartLoadRawMaterial").SetValue(false);
                WatchingProperties.Property("Buffer_EndLoadRawMaterial").SetValue(true);

                response.Params.Add("Result", true);
                var dic = new Dictionary<string, object?>();
                dic.Add("PrepareLoadOk", false);
                dic.Add("InvokeLoadOk", false);
                dic.Add("CompleteLoadOk", true);
                WatchingProperties.SetValues(dic);

                if (deviceServiceInvokeRequest.Params != null && deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
                {
                    //WatchingProperties.Property(Events.Drill.BUFFER_LACK_RAW_MATERIAL_EVENT).SetValue(string.Empty);
                    logger.LogInformation($"CompleteLoadMaterial 机器【{DeviceDescriptor.DeviceName}】  整个上下料动作结束");
                }
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish CompleteLoadMaterial!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                    .Build(ErrorCodes.Sys.SUCCESS, string.Empty, response.Params, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                response.Params.TryAdd("Result", false);
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                        .Build(ErrorCodes.Sys.FAIL, string.Empty, response.Params, deviceServiceInvokeRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to PrepareUnloadMaterial!");

            var response = new DeviceServiceInvokeResponse()
            {
                Params = deviceServiceInvokeRequest.Params,
            };

            try
            {
                if (response.Params == null || !response.Params.ContainsKey("Position"))
                {
                    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                    .Build(ErrorCodes.Sys.FAIL, "传入的参数不包含位置信息", deviceServiceInvokeRequest.ReplyTopic);
                }

                ushort position = response.Params["Position"].ToUshort();
                if (!(position > 0 && position <= 6))
                {
                    var errorMessage = $"请下发正确的下料轴信息";
                    response.Params.TryAdd("Exception", errorMessage);
                    logger.LogError($"PrepareUnloadMaterial {errorMessage}");
                    response.Params.TryAdd("Result", false);

                    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                     .Build(ErrorCodes.Sys.FAIL, errorMessage, response.Params, deviceServiceInvokeRequest.ReplyTopic);
                }

                logger.LogInformation($"PrepareLoadMaterial 机器【{DeviceDescriptor.DeviceName}】 轴【{position}】  有熟料");
                logger.LogInformation($"PrepareLoadMaterial 机器【{DeviceDescriptor.DeviceName}】 轴【{position}】  下料已经准备好");

                var dic = new Dictionary<string, object?>();
                dic.Add("PrepareUnloadOk", true);
                dic.Add("InvokeUnloadOk", false);
                dic.Add("CompleteUnload", false);
                WatchingProperties.SetValues(dic);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish PrepareUnloadMaterial!");
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.SUCCESS, string.Empty, response.Params, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                response.Params.TryAdd("Result", false);
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.FAIL, e.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to InvokeUnloadMaterial!");
            var response = new DeviceServiceInvokeResponse()
            {
                Params = deviceServiceInvokeRequest.Params,
            };

            try
            {
                if (response.Params == null || !response.Params.ContainsKey("Position"))
                {
                    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                    .Build(ErrorCodes.Sys.FAIL, "传入的参数不包含位置信息", deviceServiceInvokeRequest.ReplyTopic);
                }

                ushort position = response.Params["Position"].ToUshort();
                if (!(position > 0 && position <= 6))
                {
                    var errorMessage = "下料轴位置设置不正确";
                    response.Params.TryAdd("Exception", errorMessage);
                    logger.LogError($"InvokeUnloadMaterial {errorMessage}");
                    response.Params.TryAdd("Result", false);

                    return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                           .Build(ErrorCodes.Sys.FAIL, errorMessage, response.Params, deviceServiceInvokeRequest.ReplyTopic);
                }

                logger.LogInformation($"InvokeUnloadMaterial 机器【{DeviceDescriptor.DeviceName}】 轴【{position}】  设置下料位置");
                logger.LogInformation($"InvokeUnloadMaterial 机器【{DeviceDescriptor.DeviceName}】 轴【{position}】  开始下料");
                response.Params.TryAdd("Result", true);
                WatchingProperties.Property("Buffer_StartUnloadClinker").SetValue(true);
                WatchingProperties.Property("Buffer_EndUnloadClinker").SetValue(false);

                var dic = new Dictionary<string, object?>();
                dic.Add("PrepareUnloadOk", false);
                dic.Add("InvokeUnloadOk", true);
                dic.Add("CompleteUnload", false);
                WatchingProperties.SetValues(dic);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish InvokeUnloadMaterial!");
                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                         .Build(ErrorCodes.Sys.SUCCESS, $"轴{position}开始下料", response.Params, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                response.Params.TryAdd("Result", false);
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.FAIL, e.Message, response.Params, deviceServiceInvokeRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to CompleteUnloadMaterial!");
            var response = new DeviceServiceInvokeResponse();

            try
            {
                logger.LogInformation($"CompleteUnloadMaterial 机器【{DeviceDescriptor.DeviceName}】  下料动作完成");
                logger.LogInformation($"CompleteUnloadMaterial 机器【{DeviceDescriptor.DeviceName}】  电机停止转动");
                response.Params.Add("Result", true);
                WatchingProperties.Property("Buffer_StartUnloadClinker").SetValue(false);
                WatchingProperties.Property("Buffer_EndUnloadClinker").SetValue(true);

                var dic = new Dictionary<string, object?>();
                dic.Add("PrepareUnloadOk", false);
                dic.Add("InvokeUnloadOk", false);
                dic.Add("CompleteUnload", true);
                WatchingProperties.SetValues(dic);

                if (deviceServiceInvokeRequest.Params != null && deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
                {
                    WatchingProperties.Property(Events.Drill.BUFFER_EXIST_CLINKER_EVENT).SetValue(string.Empty);
                    logger.LogInformation($"CompleteLoadMaterial 机器【{DeviceDescriptor.DeviceName}】  整个上下料动作结束");
                }
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} finish CompleteUnloadMaterial!");

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                .Build(ErrorCodes.Sys.SUCCESS, "下料完成",
                                deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception e)
            {
                response.Params.TryAdd("Result", false);
                logger.LogError(e, e.Message);

                return await DeviceServiceInvokeResponseBuilder.Create(mqttClientWrapper, loggerFactory)
                                  .Build(ErrorCodes.Sys.FAIL, e.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to Work!");
            try
            {

                logger.LogInformation($"Work 机器【{DeviceDescriptor.DeviceName}】  切换到F8界面");
                logger.LogInformation($"Work 机器【{DeviceDescriptor.DeviceName}】  发送打板指令");
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
                logger.LogInformation($"Standby 机器【{DeviceDescriptor.DeviceName}】  发送暂停指令");
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
                logger.LogInformation($"Shutdown 机器【{DeviceDescriptor.DeviceName}】  发送关闭cnc84指令");

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

        protected async override Task Initialize()
        {
            await base.Initialize();
        }

        private void AddWatchingStates()
        {
            WatchingProperties.Properties("Buffer_EnergizeStatus", "Buffer_Automatic", "Buffer_NoError", "Drill_NoError", "Drill_Halt", "Drill_ParkingPosition", "Drill_ExistBoard", "Drill_Mushroom", "Drill_DrillHoleEnd")
                .When(properties =>
                {
                    var isPowerOnBuffer = properties.Property("Buffer_EnergizeStatus").NewValue.ToBool();
                    var isAutomaticOnBuffer = properties.Property("Buffer_Automatic").NewValue.ToBool();
                    var isNoErrorOnBuffer = properties.Property("Buffer_NoError").NewValue.ToBool();
                    var isNoErrorOnCnc84 = properties.Property("Drill_NoError").NewValue.ToBool();
                    var isHaltOnCnc84 = properties.Property("Drill_Halt").NewValue.ToBool();
                    var isParkingPositionOnCnc84 = properties.Property("Drill_ParkingPosition").NewValue.ToBool();
                    var isExistBoardOnCnc84 = properties.Property("Drill_ExistBoard").NewValue.ToBool();
                    var isMushroomOpenOnCnc84 = properties.Property("Drill_Mushroom").NewValue.ToBool();
                    var isDrillHoleEndOnCnc84 = properties.Property("Drill_DrillHoleEnd").NewValue.ToBool();

                    return isPowerOnBuffer
                    && isAutomaticOnBuffer
                    && isNoErrorOnBuffer
                    && isNoErrorOnCnc84
                    && !isHaltOnCnc84
                    && isParkingPositionOnCnc84
                    && !isExistBoardOnCnc84
                    && !isMushroomOpenOnCnc84
                    && isDrillHoleEndOnCnc84;
                })
                .TriggerAlways(async () =>
                {
                    deviceStatus = DeviceStatus.Ready;
                    var request = MakeStatusRequest(DeviceStatus.Ready);
                    await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, request);
                });

            WatchingProperties.Properties("Buffer_Automatic", "Buffer_NoError", "Drill_NoError", "Drill_Halt", "Drill_ExistBoard", "Drill_PinEnd", "Buffer_Poition", "Drill_Mushroom", "Drill_DrillHoleEnd")
                .When(properties =>
                {
                    var isAutomaticOnBuffer = properties.Property("Buffer_Automatic").NewValue.ToBool();
                    var isNoErrorOnBuffer = properties.Property("Buffer_NoError").NewValue.ToBool();
                    var isNoErrorOnCnc84 = properties.Property("Drill_NoError").NewValue.ToBool();
                    var isHaltOnCnc84 = properties.Property("Drill_Halt").NewValue.ToBool();
                    var isExistBoardOnCnc84 = properties.Property("Drill_ExistBoard").NewValue.ToBool();
                    var isPinEndOnCnc84 = properties.Property("Drill_PinEnd").NewValue.ToBool();
                    var isOnAgvPositionOnBuffer = properties.Property("Buffer_Poition").NewValue.ToBool();
                    var isPressBoardEndOnCnc84 = properties.Property("Drill_PressBoardEnd").NewValue.ToBool();
                    var isMushroomOpenOnCnc84 = properties.Property("Drill_Mushroom").NewValue.ToBool();
                    var isDrillHoleEndOnCnc84 = properties.Property("Drill_DrillHoleEnd").NewValue.ToBool();

                    if (deviceStatus == DeviceStatus.Ready)
                    {
                        return isPinEndOnCnc84
                        && isOnAgvPositionOnBuffer
                        && isPressBoardEndOnCnc84
                        && isMushroomOpenOnCnc84
                        && !isDrillHoleEndOnCnc84;
                    }
                    else if (deviceStatus == DeviceStatus.Exception)
                    {
                        return !isHaltOnCnc84
                        && isNoErrorOnBuffer
                        && isNoErrorOnCnc84
                        && isAutomaticOnBuffer
                        && !isDrillHoleEndOnCnc84
                        && isExistBoardOnCnc84;
                    }

                    return false;
                })
                .TriggerAlways(async () =>
                {
                    deviceStatus = DeviceStatus.Working;
                    var request = MakeStatusRequest(DeviceStatus.Working);

                    await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, request);
                });

            WatchingProperties.Properties("Buffer_Automatic", "Buffer_NoError", "Drill_NoError", "Drill_Halt")
                .When(properties =>
                {
                    var isAutomaticOnBuffer = properties.Property("Buffer_Automatic").NewValue.ToBool();
                    var isNoErrorOnBuffer = properties.Property("Buffer_NoError").NewValue.ToBool();
                    var isNoErrorOnCnc84 = properties.Property("Drill_NoError").NewValue.ToBool();
                    var isHaltOnCnc84 = properties.Property("Drill_Halt").NewValue.ToBool();

                    return isHaltOnCnc84
                    || !isNoErrorOnBuffer
                    || !isNoErrorOnCnc84
                    || !isAutomaticOnBuffer;
                })
                .TriggerAlways(async () =>
                {
                    deviceStatus = DeviceStatus.Exception;
                    var request = MakeStatusRequest(DeviceStatus.Exception);

                    await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, request);
                });
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
            WatchingProperties.AddProperty("Drill_ConnectionStatus", true)
                              .AddProperty("Drill_ScreenText", "")
                              .AddProperty("Drill_ParkingPosition", "0")
                              .AddProperty("Drill_BoardPositionStatus", "")
                              .AddProperty("Buffer_RawMaterialLayerBoardStatus", 1)
                              .AddProperty("Buffer_ClinkerLayerBoardStatus", 63)
                              .AddProperty("Buffer_ConnectionStatus", true)
                              .AddProperty("Buffer_EnergizeStatus", true)
                              .AddProperty("Buffer_Automatic", true)
                              .AddProperty("Buffer_NoError", true)
                              .AddProperty("Drill_NoError", true)
                              .AddProperty("Drill_Halt", false)
                              .AddProperty("Drill_Mushroom", true)
                              .AddProperty("Buffer_IsReady", true)
                              .AddProperty("Drill_ExistBoard", true)
                              .AddProperty("Drill_DrillHoleEnd", true)
                              .AddProperty("Drill_PinEnd", true)
                              .AddProperty("Buffer_Poition", true)
                              .AddProperty("Drill_PressBoardEnd", true)
                              .AddProperty("Buffer_StartLoadRawMaterial", false)
                              .AddProperty("Buffer_StartUnloadClinker", false)
                              .AddProperty("Buffer_LoadOrUnloadAxis", "0")
                              .AddProperty("Buffer_EndLoadRawMaterial", false)
                              .AddProperty("Buffer_EndUnloadClinker", false)
                              .AddProperty("PrepareLoadOk", false)
                              .AddProperty("InvokeLoadOk", false)
                              .AddProperty("CompleteLoadOk", false)
                              .AddProperty("PrepareUnloadOk", false)
                              .AddProperty("InvokeUnloadOk", false)
                              .AddProperty("CompleteUnload", false)
                              .AddProperty("CompleteAllLoadOrUnloadAction", false);
        }

        private void AddWatchingEvents()
        {
            WatchingProperties.Property("Drill_Halt")
                .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                        centralWebOptions.EventReport,
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = ClientId,
                            EventId = Events.Drill.DRILL_HALT_EVENT,
                            EventName = Events.Drill.DRILL_HALT_EVENT_NAME,
                            Params = new Dictionary<string, object?>()
                        });

                });

            WatchingProperties.Property("Buffer_StartLoadRawMaterial")
                .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                        centralWebOptions.EventReport,
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = ClientId,
                            EventId = Events.Drill.BUFFER_START_LOAD_RAW_MATERIAL_EVENT,
                            EventName = Events.Drill.BUFFER_START_LOAD_RAW_MATERIAL_EVENT_NAME,
                            Params = new Dictionary<string, object?>()
                        });
                });

            WatchingProperties.Property("Buffer_StartUnloadClinker")
                .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                        centralWebOptions.EventReport,
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = ClientId,
                            EventId = Events.Drill.BUFFER_START_UNLOAD_CLINKER_EVENT,
                            EventName = Events.Drill.BUFFER_START_UNLOAD_CLINKER_EVENT_NAME,
                            Params = new Dictionary<string, object?>()
                        });
                });

            WatchingProperties.Property("Buffer_EndLoadRawMaterial")
                .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                        centralWebOptions.EventReport,
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = ClientId,
                            EventId = Events.Drill.BUFFER_END_LOAD_RAW_MATERIAL_EVENT,
                            EventName = Events.Drill.BUFFER_END_LOAD_RAW_MATERIAL_EVENT_NAME,
                            Params = new Dictionary<string, object?>()
                        });
                });

            WatchingProperties.Property("Buffer_EndUnloadClinker")
                .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                        centralWebOptions.EventReport,
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = ClientId,
                            EventId = Events.Drill.BUFFER_END_UNLOAD_CLINKER_EVENT,
                            EventName = Events.Drill.BUFFER_END_UNLOAD_CLINKER_EVENT_NAME,
                            Params = new Dictionary<string, object?>()
                        });
                });

            WatchingProperties.Property("Drill_Mushroom")
                .PostCondition(p => p.NewValue.ToBool() && p.IsValueChanged)
                .TriggerAlways(async () =>
                {
                    await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                        centralWebOptions.EventReport,
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = ClientId,
                            EventId = Events.Drill.DRILL_START_OPEN_MUSHROOM_EVENT,
                            EventName = Events.Drill.DRILL_START_OPEN_MUSHROOM_EVENT_NAME,
                            Params = new Dictionary<string, object?>()
                        });
                });

            WatchingProperties.Properties("Buffer_RawMaterialLayerBoardStatus", "Buffer_IsReady")
                .When(p =>
                {
                    WatchableProperty boardStatus = p.Property("Buffer_RawMaterialLayerBoardStatus");
                    WatchableProperty isReady = p.Property("Buffer_IsReady");
                    return boardStatus.NewValue.ToInt() == 0 && isReady.NewValue.ToBool() && (boardStatus.IsValueChanged || isReady.IsValueChanged);
                })
                .TriggerAlways(async () =>
                {
                    await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                        centralWebOptions.EventReport,
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = ClientId,
                            EventId = Events.Drill.BUFFER_LACK_RAW_MATERIAL_EVENT,
                            EventName = Events.Drill.BUFFER_LACK_RAW_MATERIAL_EVENT_NAME,
                            Params = new Dictionary<string, object?>()
                        });
                });

            WatchingProperties.Properties("Buffer_ClinkerLayerBoardStatus", "Buffer_IsReady")
                .When(p =>
                {
                    WatchableProperty boardStatus = p.Property("Buffer_ClinkerLayerBoardStatus");
                    WatchableProperty isReady = p.Property("Buffer_IsReady");

                    return boardStatus.NewValue.ToInt() != 0 && isReady.NewValue.ToBool() && (boardStatus.IsValueChanged || isReady.IsValueChanged);
                })
                .TriggerAlways(async () =>
                {
                    await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                        centralWebOptions.EventReport,
                        new DeviceEventReportRequest()
                        {
                            ProductId = DeviceDescriptor.ProductId,
                            DeviceId = DeviceDescriptor.DeviceId,
                            ClientId = ClientId,
                            EventId = Events.Drill.BUFFER_EXIST_CLINKER_EVENT,
                            EventName = Events.Drill.BUFFER_EXIST_CLINKER_EVENT_NAME
                        });
                });

            WatchingProperties.Properties("Drill_PinEnd", "Buffer_Poition")
                .When(p =>
                {
                    WatchableProperty pinEnd = p.Property("Drill_PinEnd");
                    WatchableProperty position = p.Property("Buffer_Poition");
                    return pinEnd.NewValue.ToBool() && position.NewValue.ToBool() && (pinEnd.IsValueChanged || position.IsValueChanged);
                })
                .TriggerAlways(async () =>
                {
                    await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(
                        centralWebOptions.EventReport,
                         new DeviceEventReportRequest()
                         {
                             ProductId = DeviceDescriptor.ProductId,
                             DeviceId = DeviceDescriptor.DeviceId,
                             ClientId = ClientId,
                             EventId = Events.Drill.DRILL_START_LOAD_FILE_EVENT,
                             EventName = Events.Drill.DRILL_START_LOAD_FILE_EVENT_NAME,
                             Params = new Dictionary<string, object?>()
                         });

                    var drilPath = DeviceDescriptor.Extra["DrlPath"].ToStr();
                    var diaPath = DeviceDescriptor.Extra["DiaPath"].ToStr();

                    if (!LoadFile(drilPath, diaPath))
                    {
                        return;
                    }

                    if (!StartPressBoard())
                    {
                        return;
                    }

                    if (!StartOpenMushroom())
                    {
                        return;
                    }

                    StartDrilBoard();
                });
        }

        private void PlcConnect()
        {
            WatchingProperties.Property("Buffer_ConnectionStatus").SetValue(true);
        }

        private void Cnc84Connect()
        {
            WatchingProperties.Property("Drill_ConnectionStatus").SetValue(true);
        }

        private bool LoadFile(string drilPath, string diaPath = "")
        {
            logger.LogInformation($"LoadFile 机器【{DeviceDescriptor.DeviceName}】 发送【CM@@@】指令");

            if (!ValidateCmDrlFile())
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(diaPath))
            {
                logger.LogInformation($"LoadFile 机器【{DeviceDescriptor.DeviceName}】 加载 参数文件{diaPath}");
                if (!ValidateDiaFile(diaPath))
                {
                    return false;
                }
            }

            logger.LogInformation($"LoadFile 机器【{DeviceDescriptor.DeviceName}】 加载 程序文件{drilPath}");
            if (!ValidateDrlFile(drilPath))
            {
                return false;
            }

            return true;

        }

        private bool ValidateDrlFile(string drilFilePath)
        {
            Task.Delay(2000).Wait();
            logger.LogInformation($"LoadFile 机器【{DeviceDescriptor.DeviceName}】 验证 程序文件{drilFilePath} 加载成功");

            return true;
        }

        private bool ValidateCmDrlFile()
        {
            Task.Delay(2000).Wait();
            logger.LogInformation($"ValidateCmDrlFile 机器【{DeviceDescriptor.DeviceName}】 验证【CM@@@】指令");

            return true;
        }

        private bool ValidateDiaFile(string diaFilePath)
        {
            Task.Delay(2000).Wait();
            logger.LogInformation($"ValidateDiaFile 机器【{DeviceDescriptor.DeviceName}】 加载 参数文件{diaFilePath} 成功");

            return true;
        }

        private bool StartPressBoard()
        {
            var toolNum = "0";
            logger.LogInformation($"StartPressBoard 机器【{DeviceDescriptor.DeviceName}】 检测主轴是否无刀 {toolNum == "0"}");

            if (!"0".Equals(toolNum))
            {
                toolNum = "0";
                logger.LogInformation($"StartPressBoard 机器【{DeviceDescriptor.DeviceName}】 发送退刀【T】指令");
                Task.Delay(2000).Wait();
                logger.LogInformation($"StartPressBoard 机器【{DeviceDescriptor.DeviceName}】 检测主轴是否有刀{toolNum == "0"} ");
            }

            Thread.Sleep(2000);
            PressBoard();
            if (!ValidatePressBoardEnd())
            {
                return false;
            }

            return true;
        }

        private string GetToolNum()
        {
            return "0";
        }

        private void PressBoard()
        {
            WatchingProperties.Property("Drill_PressBoardEnd").SetValue(false);
            logger.LogInformation($"PressBoard 机器【{DeviceDescriptor.DeviceName}】 发送压板 【M102】 指令");
            Task.Delay(2000).Wait();
            WatchingProperties.Property("Drill_PressBoardEnd").SetValue(true);
        }

        private bool ValidatePressBoardEnd()
        {
            Thread.Sleep(2000);
            logger.LogInformation($"ValidatePressBoardEnd 机器【{DeviceDescriptor.DeviceName}】 验证压板指令执行结束");
            return true;
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
            logger.LogInformation($"MushroomCloseLocalTion 机器【{DeviceDescriptor.DeviceName}】 蘑菇头进状态");

            return true;
        }

        private bool MushroomCloseLocalTion()
        {
            logger.LogInformation($"MushroomCloseLocalTion 机器【{DeviceDescriptor.DeviceName}】 蘑菇头出状态");
            return true;
        }

        private void ControlMushroom()
        {
            logger.LogInformation($"ControlMushroom 机器【{DeviceDescriptor.DeviceName}】 打开蘑菇头");
            WatchingProperties.Property("Drill_Mushroom").SetValue(false);
            Task.Delay(2000).Wait();
            WatchingProperties.Property("Drill_Mushroom").SetValue(true);
        }

        private void StartDrilBoard()
        {
            logger.LogInformation($"StartDrilBoard 机器【{DeviceDescriptor.DeviceName}】 开始钻板");
        }

    }
}
