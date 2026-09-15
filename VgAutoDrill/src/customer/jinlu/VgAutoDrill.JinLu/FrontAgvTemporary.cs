using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Modbus.Device;
using System.Text.Json;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Fundation.Vendor.Common;
using VgAutoDrill.Fundation.Vendor.STD;
using VgAutoDrill.Fundation.Vendor.Vega;
using static VgAutoDrill.Fundation.Mqtt.Events;

namespace VgAutoDrill.JinLu
{
    public class FrontAgvTemporary : Vehicle
    {
        private readonly IModbusIpMasterWrapper modbusIpMasterWrapper;
        private IModbusMaster modbusIpMaster;
        private readonly IHttpRequestInvoker httpRequestInvoker;
        private readonly DeviceDescriptor deviceDescriptor;
        private readonly IDeviceProvider deviceProvider;
        private readonly ILogger<FrontAgvTemporary> logger;
        private readonly int postAndGetTimeOut;
        private readonly int waitPlcSignalTimeout;
        private readonly int defaultTimeout;
        private readonly int moveTimeout;
        private readonly byte slaveId;
        private readonly Dictionary<string, object> configExtra;
        private readonly CentralWebOptions centralWebOptions;
        private readonly Uri plcUri;
        private bool isbusy = false;

        private bool IsMoving = false;
        private bool IsWorking = false;

        /// <summary>
        /// 当前执行的任务的code
        /// </summary>
        private string CurrentEventTraceId = string.Empty;

        /// <summary>
        /// EVENT_ID
        /// EVENT_NAME
        /// EVENT_MESSAGE
        /// </summary>
        private Tuple<string, string, string> errorInfo;
        public FrontAgvTemporary(IHttpRequestInvoker httpRequestInvoker,
            IDeviceEngine deviceEngine,
            IMqttClientWrapper mqttClientWrapper,
            DeviceDescriptor deviceDescriptor,
            IOptions<CentralWebOptions> options,
            ILoggerFactory loggerFactory,
            IDeviceProvider deviceProvider)
            : base(httpRequestInvoker, deviceEngine, mqttClientWrapper, deviceDescriptor, options, loggerFactory)
        {
            this.modbusIpMasterWrapper = this.Engine.DeviceConnector.ModbusIpMasterWrapper;
            this.centralWebOptions = options.Value;
            this.httpRequestInvoker = httpRequestInvoker;
            this.deviceDescriptor = deviceDescriptor;
            this.deviceProvider = deviceProvider;
            this.logger = loggerFactory.CreateLogger<FrontAgvTemporary>();
            this.configExtra = deviceDescriptor.Extra;
            this.plcUri = new Uri(deviceDescriptor.Extra["ModbusTcpUri"].ToStr());
            this.slaveId = deviceDescriptor.Extra["ModbusTcpSlaveId"].ToByte();
            this.postAndGetTimeOut = configExtra["PostAndGetTimeout"].ToInt();
            this.waitPlcSignalTimeout = configExtra["WaitPlcSignalTimeout"].ToInt();
            this.defaultTimeout = configExtra["DefaultTimeout"].ToInt();
            this.moveTimeout = configExtra["MoveTimeout"].ToInt();
            var spindleNum = deviceDescriptor.SpindleNum.ToInt();
            var layerLimit = deviceDescriptor.LayerLimit.ToInt();
            this.PayloadPanels = Enumerable.Repeat<Panel?>(null, spindleNum * layerLimit).ToList();
            errorInfo = new Tuple<string, string, string>("", "", "");

            this.Connector.ConnectFunc = (device) => Task.Run(async () =>
            {
                if (!CanWritePlc()) return await MachineConnect();
                return true;
            });

            CollectDataFunc = async (device) =>
            {
                if (!isbusy)
                {
                    isbusy = true;
                    var realProperties = await CollectRealProperties();
                    WatchingProperties.SetValues(realProperties);

                    var mergeInf = WatchingProperties.GetValues();
                    await httpRequestInvoker.PostAsJsonAsync<DevicePropertiesReportRequest, DevicePropertiesReportResponse>(
                    centralWebOptions.PropertiesReport,
                     new DevicePropertiesReportRequest()
                     {
                         DeviceId = deviceDescriptor.DeviceId,
                         ProductId = deviceDescriptor.ProductId,
                         Params = mergeInf,
                     });
                    isbusy = false;
                }
            };

            AddWatchingProperties();
            AddWatchingStates();
            AddWatchingEvent();
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

        public override async Task<DeviceServiceInvokeResponse> ReadProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to ReadProperties!");

            try
            {
                if (!this.Engine.DeviceConnector.IsConnected)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_ReadProperties_PlcUnConnected", "PlcUnConnected", "PLC未连接");
                    return await Response(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, deviceServiceInvokeRequest.ReplyTopic);
                }

                var dictionary = await CollectRealProperties();
                WatchingProperties.SetValues(dictionary);

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to ReadProperties!");

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, dictionary, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_ReadProperties_Exception", "异常", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> WriteProperties(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to WriteProperties!");

            try
            {
                WatchingProperties.SetValues(deviceServiceInvokeRequest.Params);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_ReadProperties_Exception", "异常", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }

            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to WriteProperties!");
            return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
        }

        public override async Task<DeviceServiceInvokeResponse> Move(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to Move!");

            try
            {
                var movecheck = await MoveCheck(deviceServiceInvokeRequest);
                if (movecheck.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_Move_MoveCheckError", "MoveCheckError", movecheck.Message);
                    return await Response(movecheck.Code, movecheck.Message, deviceServiceInvokeRequest.ReplyTopic);
                }
                var agvTargetPos = deviceServiceInvokeRequest.Params["MoveTargetPos"].ToStr();
                var carCurrentPos = this.WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();

                var mapId = configExtra["AGVMapId"].ToInt();
                logger.LogInformation("Move : CurrentStation" + carCurrentPos + ",   TargetStation" + agvTargetPos);

                var order = new OrderRequestEntity()
                {
                    vehicle_id = configExtra["AGVId"].ToInt(),
                    priority = 10,
                    mission = new List<RequestMission>()
                    {
                        new RequestMission()
                        {
                           type= "move",
                           destination= agvTargetPos.ToInt(),
                           map_id= mapId
                        },
                         new RequestMission()//精调
                        {
                            type="act",                //动作任务
                            action_id= 7,            //自定义动作
                            action_param1= 1,         //表示操作光栅的功能
                            action_param2= 0               //是动作任务
                        },
                        new RequestMission()
                        {
                            type="act",                //动作任务
                            action_id= -20,            //自定义动作
                            action_param1= 1,         //表示操作光栅的功能
                            action_param2= 1               //是动作任务
                        }
                    }
                };

                var agvToken = configExtra["AgvToken"].ToStr();
                var moveresult = await httpRequestInvoker.PostAsJsonAsync<OrderRequestEntity, Order>(configExtra["AGVMoveStart"].ToStr(), order, agvToken);

                if (moveresult == null)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_Move_MoveApiError", "MoveApiError", "");
                    return await Response(ErrorCodes.Sys.FAIL, "移动失败", deviceServiceInvokeRequest.ReplyTopic);
                }

                var taskId = moveresult?.id.ToInt();

                //车辆状态变成移动中···
                var update = new Dictionary<string, object?>
                {
                   { "IsMoving", true },
                   { "IsArrived",false },
                   { "AgvReturnTaskId", taskId }
                };
                WatchingProperties.SetValues(update);
                this.IsMoving = true;

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to Move!");

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_Move_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to PrepareLoadMaterial!");
            this.WatchingProperties.Property("PrepareLoadOk").SetValue(false);

            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.PrepareLoadMaterial,
                       async (deviceOperationType) =>
                       {
                           var checkresult = CheckStatus(deviceOperationType);
                           if (!checkresult.Item1)
                           {
                               errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_CheckStatusError", "CheckStatusError", checkresult.Item3);
                           }
                           return await Response(checkresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, checkresult.Item3, deviceServiceInvokeRequest.ReplyTopic);
                       },
                       async (deviceServiceInvokeRequest, deviceOperationType) =>
                       {
                           var result = await TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperationType);
                           if (result.Code != ErrorCodes.Sys.SUCCESS)
                           {
                               errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_TargetDeviceOperationError", "TargetDeviceOperationError", result.Message);
                           }
                           return await Response(result.Code, result.Message, deviceServiceInvokeRequest.ReplyTopic);
                       },
                      async (deviceServiceInvokeRequest) =>
                      {
                          var selfresult = PrepareLoadMaterialSelf(deviceServiceInvokeRequest);
                          if (!selfresult.Item1)
                          {
                              errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_PrepareLoadMaterialSelfError", "PrepareLoadMaterialSelfError", selfresult.Item2);
                          }
                          return await Response(selfresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, selfresult.Item2, deviceServiceInvokeRequest.ReplyTopic);
                      }
                      );

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to PrepareLoadMaterial!");
                if (serviceCallResponse.Code == ErrorCodes.Sys.SUCCESS)
                {
                    this.WatchingProperties.Property("PrepareLoadOk").SetValue(true);
                }

                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to InvokeLoadMaterial!");

            this.WatchingProperties.Property("InvokeLoadOk").SetValue(false);
            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.InvokeLoadMaterial,
                  async (deviceOperationType) =>
                  {
                      var checkresult = CheckStatus(deviceOperationType);
                      if (!checkresult.Item1)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_CheckStatusError", "CheckStatusError", checkresult.Item3);
                      }
                      return await Response(checkresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, checkresult.Item3, deviceServiceInvokeRequest.ReplyTopic);
                  },
                  async (deviceServiceInvokeRequest, deviceOperationType) =>
                  {
                      var result = await TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperationType);
                      if (result.Code != ErrorCodes.Sys.SUCCESS)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_InvokeLoadMaterial_TargetDeviceOperationError", "TargetDeviceOperationError", result.Message);
                      }
                      return await Response(result.Code, result.Message, deviceServiceInvokeRequest.ReplyTopic);
                  },
                  async (deviceServiceInvokeRequest) =>
                  {
                      var selfresult = InvokeLoadMaterialSelf(deviceServiceInvokeRequest);
                      if (!selfresult.Item1)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_InvokeLoadMaterial_PrepareLoadMaterialSelfError", "PrepareLoadMaterialSelfError", selfresult.Item2);
                      }
                      return await Response(selfresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, selfresult.Item2, deviceServiceInvokeRequest.ReplyTopic);
                  }
                  );

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to InvokeLoadMaterial!");
                if (serviceCallResponse.Code == ErrorCodes.Sys.SUCCESS)
                {
                    this.WatchingProperties.Property("InvokeLoadOk").SetValue(true);
                }

                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_InvokeLoadMaterial_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteLoadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to CompleteLoadMaterial!");

            this.WatchingProperties.Property("CompleteLoadOk").SetValue(false);
            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.CompleteLoadMaterial,
                  async (deviceOperationType) =>
                  {
                      var checkresult = CheckStatus(deviceOperationType);
                      if (!checkresult.Item1)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_CheckStatusError", "CheckStatusError", checkresult.Item3);
                      }
                      return await Response(checkresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, checkresult.Item3, deviceServiceInvokeRequest.ReplyTopic);
                  },
                  async (deviceServiceInvokeRequest, deviceOperationType) =>
                  {
                      var result = await TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperationType);
                      if (result.Code != ErrorCodes.Sys.SUCCESS)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_CompleteLoadMaterial_TargetDeviceOperationError", "TargetDeviceOperationError", result.Message);
                      }
                      return await Response(result.Code, result.Message, deviceServiceInvokeRequest.ReplyTopic);
                  },
                  async (deviceServiceInvokeRequest) =>
                  {
                      var selfresult = CompleteLoadMaterialSelf(deviceServiceInvokeRequest);
                      if (!selfresult.Item1)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_CompleteLoadMaterial_PrepareLoadMaterialSelfError", "PrepareLoadMaterialSelfError", selfresult.Item2);
                      }
                      return await Response(selfresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, selfresult.Item2, deviceServiceInvokeRequest.ReplyTopic);
                  }
                  );

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to CompleteLoadMaterial!");
                if (serviceCallResponse.Code == ErrorCodes.Sys.SUCCESS)
                {
                    this.WatchingProperties.Property("CompleteLoadOk").SetValue(true);
                }

                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_CompleteLoadMaterial_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to PrepareUnloadMaterial!");

            this.WatchingProperties.Property("PrepareUnloadOk").SetValue(false);
            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.PrepareUnloadMaterial,
                  async (deviceOperationType) =>
                  {
                      var checkresult = CheckStatus(deviceOperationType);
                      if (!checkresult.Item1)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_CheckStatusError", "CheckStatusError", checkresult.Item3);
                      }
                      return await Response(checkresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, checkresult.Item3, deviceServiceInvokeRequest.ReplyTopic);
                  },
                  async (deviceServiceInvokeRequest, deviceOperationType) =>
                  {
                      var result = await TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperationType);
                      if (result.Code != ErrorCodes.Sys.SUCCESS)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_PrepareUnloadMaterial_TargetDeviceOperationError", "TargetDeviceOperationError", result.Message);
                      }
                      return await Response(result.Code, result.Message, deviceServiceInvokeRequest.ReplyTopic);
                  },
                  async (deviceServiceInvokeRequest) =>
                  {
                      var selfresult = PrepareUnloadMaterialSelf(deviceServiceInvokeRequest);
                      if (!selfresult.Item1)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_PrepareUnloadMaterial_PrepareLoadMaterialSelfError", "PrepareLoadMaterialSelfError", selfresult.Item2);
                      }
                      return await Response(selfresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, selfresult.Item2, deviceServiceInvokeRequest.ReplyTopic);
                  }
                  );

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to PrepareUnloadMaterial!");
                if (serviceCallResponse.Code == ErrorCodes.Sys.SUCCESS)
                {
                    this.WatchingProperties.Property("PrepareUnloadOk").SetValue(true);
                }

                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_PrepareUnloadMaterial_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to InvokeUnloadMaterial!");

            this.WatchingProperties.Property("InvokeUnloadOk").SetValue(false);
            try
            {
                var serviceCallResponse = await DoServiceCallDeviceFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.InvokeUnloadMaterial,
                  async (deviceOperationType) =>
                  {
                      var checkresult = CheckStatus(deviceOperationType);
                      if (!checkresult.Item1)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadMaterial_CheckStatusError", "CheckStatusError", checkresult.Item3);
                      }
                      return await Response(checkresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, checkresult.Item3, deviceServiceInvokeRequest.ReplyTopic);
                  },
                  async (deviceServiceInvokeRequest, deviceOperationType) =>
                  {
                      var result = await TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperationType);
                      if (result.Code != ErrorCodes.Sys.SUCCESS)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_InvokeUnloadMaterial_TargetDeviceOperationError", "TargetDeviceOperationError", result.Message);
                      }
                      return await Response(result.Code, result.Message, deviceServiceInvokeRequest.ReplyTopic);
                  },
                  async (deviceServiceInvokeRequest) =>
                  {
                      var selfresult = InvokeUnloadMaterialSelf(deviceServiceInvokeRequest);
                      if (!selfresult.Item1)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_InvokeUnloadMaterial_PrepareLoadMaterialSelfError", "PrepareLoadMaterialSelfError", selfresult.Item2);
                      }
                      return await Response(selfresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, selfresult.Item2, deviceServiceInvokeRequest.ReplyTopic);
                  }
                  );

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to InvokeUnloadMaterial!");
                if (serviceCallResponse.Code == ErrorCodes.Sys.SUCCESS)
                {
                    this.WatchingProperties.Property("InvokeUnloadOk").SetValue(true);
                }

                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_InvokeUnloadMaterial_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterial(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to CompleteUnloadMaterial!");

            this.WatchingProperties.Property("CompleteUnloadOk").SetValue(false);
            try
            {
                var serviceCallResponse = await DoServiceCallSelfFirstComplete(deviceServiceInvokeRequest, DeviceOperationType.CompleteUnloadMaterial,
                  async (deviceOperationType) =>
                  {
                      var checkresult = CheckStatus(deviceOperationType);

                      if (!checkresult.Item1)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_CompleteUnloadMaterial_CheckStatusError", "CheckStatusError", checkresult.Item3);
                      }
                      return await Response(checkresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, checkresult.Item3, deviceServiceInvokeRequest.ReplyTopic);
                  },
                  async (deviceServiceInvokeRequest) =>
                  {
                      var selfresult = CompleteUnloadMaterialSelf(deviceServiceInvokeRequest);

                      if (!selfresult.Item1)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_CompleteUnloadMaterial_CompleteUnloadMaterialSelfError", "CompleteUnloadMaterialSelfError", selfresult.Item2);
                      }
                      return await Response(selfresult.Item1 ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL, selfresult.Item2, deviceServiceInvokeRequest.ReplyTopic);
                  },
                  async (deviceServiceInvokeRequest, deviceOperationType) =>
                  {
                      var result = await TargetDeviceOperation(deviceServiceInvokeRequest, deviceOperationType);

                      if (result.Code != ErrorCodes.Sys.SUCCESS)
                      {
                          errorInfo = new Tuple<string, string, string>("AGV_CompleteUnloadMaterial_TargetDeviceOperationError", "TargetDeviceOperationError", result.Message);
                      }

                      return await Response(result.Code, result.Message, deviceServiceInvokeRequest.ReplyTopic);
                  }
                  );

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to CompleteUnloadMaterial!");
                if (serviceCallResponse.Code == ErrorCodes.Sys.SUCCESS)
                {
                    this.WatchingProperties.Property("CompleteUnloadOk").SetValue(true);
                }

                return serviceCallResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_CompleteUnloadMaterial_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> Work(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} begin to Work!");
            var isAllowedNextOperation = true;
            CurrentEventTraceId = deviceServiceInvokeRequest.Params.ContainsKey(Tasks.PARAMS_TASK_EVENT_TRACE_ID)
                                        ? deviceServiceInvokeRequest.Params[Tasks.PARAMS_TASK_EVENT_TRACE_ID].ToStr()
                                        : string.Empty;

            try
            {
                this.WatchingProperties.Property("IsWorking").SetValue(true);
                this.Status = DeviceStatus.Working;
                this.IsWorking = true;
                if (!this.Engine.DeviceConnector.IsConnected)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_Work_PlcUnConnected", "PlcUnConnected", "PLC未连接");
                    return await Response(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, deviceServiceInvokeRequest.ReplyTopic);
                }

                //获取钻机在用的轴位，停用是用null表示
                var targetPoslist = deviceServiceInvokeRequest.Params["Spindles"].ToStr().Split(',');
                if (targetPoslist == null || targetPoslist.Length <= 0)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_Work_RequestParamsError", "RequestParamsError", "没有Spindles信息");
                    return await Response(ErrorCodes.Sys.FAIL, "没有Spindles信息", deviceServiceInvokeRequest.ReplyTopic);
                }

                this.ActionStatus = ActionStatus.ToDo;
                var actionList = GetActionList(deviceServiceInvokeRequest, targetPoslist);

                var lastPosition = "";
                var deviceOperationResponse = new DeviceServiceInvokeResponse();
                var errormsg = "";

                int step = 0;
                for (int i = 0; i < actionList.Count; i++)
                {
                    logger.LogDebug("\r\n 动作之前重置4210 \r\n");
                    modbusIpMaster.WriteSingleRegister(slaveId, 4210, 0);
                    logger.LogDebug("\r\n 动作之前重置4211 \r\n");
                    modbusIpMaster.WriteSingleRegister(slaveId, 4211, 0);

                    //等待机械臂在原点
                    logger.LogDebug("\r\n 等待机械臂在原点 \r\n");
                    var startTime = DateTime.Now;
                    var isTimeOut = false;
                    while (true)
                    {
                        logger.LogDebug("\r\n 等待4110 \r\n");
                        var status = modbusIpMaster.ReadHoldingRegisters(slaveId, 4110, 1);
                        if (status[0] == 1)
                        {
                            logger.LogDebug("\r\n收到机械臂在原点\r\n");
                            break;
                        }
                        isTimeOut = IsTimeout(startTime, waitPlcSignalTimeout);
                        if (isTimeOut)
                        {
                            logger.LogDebug("\r\n 等待 4110 信号超时\r\n");
                            break;
                        }
                        if (this.Status == DeviceStatus.Exception)
                        {
                            logger.LogDebug("\r\n 程序异常跳出循环4110\r\n");
                            break;
                        }
                        await Task.Delay(50);
                    }
                    if (isTimeOut || this.Status == DeviceStatus.Exception)//超时或者 异常
                    {
                        return await Response(ErrorCodes.Sys.FAIL, "4110超时或者异常", deviceServiceInvokeRequest.ReplyTopic);
                    }

                    //确保回调成功
                    logger.LogDebug("\r\n等待ActionStatus车辆到位回调成功 todo\r\n");

                    startTime = DateTime.Now;
                    while (true)
                    {
                        logger.LogDebug("\r\n 等待todo \r\n");
                        if (this.ActionStatus == ActionStatus.ToDo)
                        {
                            logger.LogDebug("\r\n收到ActionStatus todo\r\n");
                            break;
                        }
                        isTimeOut = IsTimeout(startTime, defaultTimeout);
                        if (isTimeOut)
                        {
                            logger.LogDebug("\r\n 等待 ActionStatus todo 信号超时\r\n");
                            break;
                        }
                        if (this.Status == DeviceStatus.Exception)
                        {
                            logger.LogDebug("\r\n 程序异常跳出循环ActionStatus todo\r\n");
                            break;
                        }
                        await Task.Delay(50);
                    }
                    if (isTimeOut || this.Status == DeviceStatus.Exception)//超时或者 异常
                    {
                        return await Response(ErrorCodes.Sys.FAIL, "ActionStatus todo 超时或者异常", deviceServiceInvokeRequest.ReplyTopic);
                    }

                    deviceServiceInvokeRequest.Params["MoveTargetPos"] = actionList[i].AgvPosition;

                    //给PLC 发送预运动操作 提高CT
                    logger.LogDebug("\r\n 给PLC发送预运动操作 \r\n");
                    SetPlcPerAction(deviceServiceInvokeRequest, actionList, i);

                    logger.LogDebug("\r\n AGV移动··· \r\n");
                    deviceOperationResponse = await Move(deviceServiceInvokeRequest);
                    errormsg = string.Format($"Move：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                    logger.LogInformation(errormsg);
                    if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        isAllowedNextOperation = false;
                        errorInfo = new Tuple<string, string, string>("AGV_Work_MoveNoSuccess", "MoveNoSuccess", errormsg);
                        break;
                    }

                    logger.LogDebug("\r\nActionStatus  改为Doing\r\n");
                    this.ActionStatus = ActionStatus.Doing;

                    //---------------------------------------------------------------------
                    //deviceOperationResponse = await Arrived(deviceServiceInvokeRequest);
                    //errormsg = string.Format($"Arrived：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                    //if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    //{
                    //    isAllowedNextOperation = false;
                    //    errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                    //    break;
                    //}


                    //即开始执行预备动作
                    logger.LogDebug("\r\n 开始执行预备动作 \r\n");
                    modbusIpMaster.WriteSingleRegister(slaveId, 4210, 1);

                    foreach (var item in actionList[i].OperationList)
                    {
                        if (!isAllowedNextOperation)
                        {
                            logger.LogDebug("\r\n 任务失败··· \r\n");
                            return await Response(ErrorCodes.Sys.FAIL, "任务失败:" + item.SpindleId, deviceServiceInvokeRequest.ReplyTopic);
                        }
                        step++;
                        lastPosition = this.WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
                        deviceServiceInvokeRequest.Params["LastPos"] = lastPosition;
                        deviceServiceInvokeRequest.Params["SpindlePosition"] = item.SpindleId;
                        deviceServiceInvokeRequest.Params["IsFirstStep"] = (step == 1);
                        deviceServiceInvokeRequest.Params["Position"] = item.SpindleId;
                        deviceServiceInvokeRequest.Params["CarCurrentPos"] = item.AgvPosition;
                        //设置PLC单轴信息
                        logger.LogDebug($"\r\n 设置PLC {item.SpindleId} 单轴信息 \r\n");
                        SetPlcSingleSpindle(deviceServiceInvokeRequest, item);

                        logger.LogDebug("\r\n 等待车辆到位 \r\n");
                        deviceOperationResponse = await Arrived(deviceServiceInvokeRequest);
                        errormsg = string.Format($"Arrived：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                        if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                        {
                            isAllowedNextOperation = false;
                            errorInfo = new Tuple<string, string, string>("AGV_Work_ArrivedNoSuccess", "ArrivedNoSuccess", errormsg);
                            break;
                        }

                        var agvposition = deviceServiceInvokeRequest.Params["CarCurrentPos"].ToInt();
                        modbusIpMaster.WriteSingleRegister(slaveId, 4212, (ushort)agvposition);
                        logger.LogDebug($"\r\n 车辆到达告知PLC当前的车辆位置{agvposition} \r\n");

                        //为兼容4210 和4211 延迟0.5s
                        await Task.Delay(500);

                        logger.LogDebug("\r\n 开始执行动作\r\n");
                        modbusIpMaster.WriteSingleRegister(slaveId, 4211, 1);

                        switch (deviceServiceInvokeRequest.CallerRequestInteractionBehavior)
                        {
                            case InteractionBehavior.LoadPanelOnly:
                            case InteractionBehavior.LoadSiloOnly:
                                isAllowedNextOperation = false;
                                deviceOperationResponse.Code = ErrorCodes.Sys.FAIL;
                                errormsg = "OnlyLoad：Error," + deviceServiceInvokeRequest.CallerRequestInteractionBehavior;
                                logger.LogInformation(errormsg);
                                break;
                            case InteractionBehavior.UnloadPanelOnly:
                            case InteractionBehavior.UnloadSiloOnly:
                                isAllowedNextOperation = false;
                                deviceOperationResponse.Code = ErrorCodes.Sys.FAIL;
                                errormsg = "OnlyUnload：Error," + deviceServiceInvokeRequest.CallerRequestInteractionBehavior;
                                logger.LogInformation(errormsg);
                                break;
                            case InteractionBehavior.LoadPanelThenUnloadPanel:
                            case InteractionBehavior.LoadSiloThenUnloadSilo:
                                isAllowedNextOperation = false;
                                deviceOperationResponse.Code = ErrorCodes.Sys.FAIL;
                                errormsg = "LoadAndUnload：Error," + deviceServiceInvokeRequest.CallerRequestInteractionBehavior;
                                logger.LogInformation(errormsg);
                                break;
                            case InteractionBehavior.UnloadPanelThenLoadPanel:
                            case InteractionBehavior.UnloadSiloThenLoadSilo:
                                if (deviceServiceInvokeRequest.PayloadPanels.Any(x => x != null))
                                {
                                    var requestPanle = deviceServiceInvokeRequest.PayloadPanels[item.SpindleId - 1];
                                    if (requestPanle != null)
                                    {
                                        deviceOperationResponse = await UnloadMaterialBehavior(deviceServiceInvokeRequest, item);
                                        errormsg = string.Format($"Work_UnloadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                        logger.LogInformation(errormsg);
                                        if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                        {
                                            isAllowedNextOperation = false;
                                            errorInfo = new Tuple<string, string, string>("AGV_Work_UnloadMaterialBehaviorNoSuccess", "UnloadMaterialBehaviorNoSuccess", errormsg);
                                            break;
                                        }
                                    }
                                }
                                deviceOperationResponse = await LoadMaterialBehavior(deviceServiceInvokeRequest, item);
                                errormsg = string.Format($"Work_LoadMaterialBehavior：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                                logger.LogDebug(errormsg);
                                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                                {
                                    isAllowedNextOperation = false;
                                    errorInfo = new Tuple<string, string, string>("AGV_Work_LoadMaterialBehaviorNoSuccess", "LoadMaterialBehaviorNoSuccess", errormsg);
                                    break;
                                }

                                break;
                            default:
                                isAllowedNextOperation = false;
                                deviceOperationResponse.Code = ErrorCodes.Sys.FAIL;
                                errormsg = "UnknowBehavior：Error," + deviceServiceInvokeRequest.CallerRequestInteractionBehavior;
                                logger.LogInformation(errormsg);
                                break;
                        }

                        logger.LogDebug($"\r\n 料仓信息更改_下料 \r\n");
                        UpdateUnloadingSiloInfo(deviceServiceInvokeRequest);

                        logger.LogDebug("等待单轴完成信号:" + item.SpindleId);
                        startTime = DateTime.Now;
                        while (true)
                        {
                            logger.LogDebug("\r\n 等待4111 \r\n");
                            var status = modbusIpMaster.ReadHoldingRegisters(slaveId, 4111, 1);
                            if (status[0] == 1)
                            {
                                logger.LogDebug("收到单轴完成信号:" + item.SpindleId);

                                logger.LogDebug("重置信号：4211 开始执行动作");
                                modbusIpMaster.WriteSingleRegister(slaveId, 4211, 0);
                                logger.LogDebug("重置信号：4212 车辆位置");
                                modbusIpMaster.WriteSingleRegister(slaveId, 4212, 0);
                                logger.LogDebug("重置信号：4218 动作详情");
                                modbusIpMaster.WriteSingleRegister(slaveId, 4218, 0);
                                logger.LogDebug("重置信号：4225 顶升下降完成");
                                modbusIpMaster.WriteSingleRegister(slaveId, 4225, 0);
                                logger.LogDebug("重置信号：4111 单轴完成信号");
                                modbusIpMaster.WriteSingleRegister(slaveId, 4111, 0);
                                break;
                            }
                            isTimeOut = IsTimeout(startTime, waitPlcSignalTimeout);
                            if (isTimeOut)//超时或者 异常
                            {
                                logger.LogDebug("\r\n 等待 4111 信号超时\r\n");
                                break;
                            }
                            if (this.Status == DeviceStatus.Exception)// 异常
                            {
                                logger.LogDebug("\r\n 程序异常跳出循环4111\r\n");
                                break;
                            }
                            await Task.Delay(50);
                        }
                        if (isTimeOut || this.Status == DeviceStatus.Exception)//超时或者 异常
                        {
                            return await Response(ErrorCodes.Sys.FAIL, "4111超时或者异常", deviceServiceInvokeRequest.ReplyTopic);
                        }
                    }
                    if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        isAllowedNextOperation = false;
                        return await Response(deviceOperationResponse.Code, errormsg, deviceServiceInvokeRequest.ReplyTopic);
                    }

                    logger.LogDebug("重置信号：4210 开始预准备动作");
                    modbusIpMaster.WriteSingleRegister(slaveId, 4210, 0);
                    //重置信号4214-4216 123轴动作情况
                    logger.LogDebug("\r\n 重置信号4214-4216 123轴动作情况 \r\n");
                    var splinedOperation = new ushort[] { 0, 0, 0 };
                    modbusIpMaster.WriteMultipleRegisters(slaveId, 4214, splinedOperation);

                    logger.LogDebug("\r\n 重置信号4219-4224 夹爪打开范围 \r\n");
                    var jiavalue = new ushort[] { 0, 0, 0, 0, 0, 0 };
                    modbusIpMaster.WriteMultipleRegisters(slaveId, 4219, jiavalue);

                    logger.LogDebug("\r\n 重置信号4233-4238 销钉偏移量 \r\n");
                    var offsetvalue = new ushort[] { 0, 0, 0, 0, 0, 0 };
                    modbusIpMaster.WriteMultipleRegisters(slaveId, 4233, offsetvalue);

                    logger.LogDebug($"\r\n {actionList[i].AgvPosition}站点工作完成 等待 机械臂在原点\r\n");

                    startTime = DateTime.Now;
                    isTimeOut = false;
                    while (true)
                    {
                        logger.LogDebug("\r\n 工作完成等待4110 \r\n");
                        var status = modbusIpMaster.ReadHoldingRegisters(slaveId, 4110, 1);
                        if (status[0] == 1)
                        {
                            logger.LogDebug($"\r\n {actionList[i].AgvPosition}站点工作完成 收到 机械臂在原点\r\n");
                            //agv回调
                            logger.LogDebug("\r\n ActionStatus改为Done 结束AGV订单任务AGV可以继续执行下个站点任务\r\n");
                            this.ActionStatus = ActionStatus.Done;
                            break;
                        }
                        isTimeOut = IsTimeout(startTime, waitPlcSignalTimeout);
                        if (isTimeOut)
                        {
                            logger.LogDebug("\r\n 等待 4110 信号超时\r\n");
                            break;
                        }
                        if (this.Status == DeviceStatus.Exception)// 异常
                        {
                            logger.LogDebug("\r\n 程序异常\r\n");
                            break;
                        }
                        await Task.Delay(50);
                    }
                    if (isTimeOut || this.Status == DeviceStatus.Exception)//超时或者 异常
                    {
                        return await Response(ErrorCodes.Sys.FAIL, "4110超时或者异常", deviceServiceInvokeRequest.ReplyTopic);
                    }
                }

                //因为没有传感器所以需要一个信号用于 结束信号
                if (step == actionList.Sum(x => x.OperationList.Count))
                {
                    logger.LogDebug("\r\n调用设备IsLastStep\r\n");
                    deviceServiceInvokeRequest.Params["IsLastStep"] = true;
                    var end = await TargetDeviceOperation(deviceServiceInvokeRequest, DeviceOperationType.CompleteLoadMaterial);
                    logger.LogDebug($"\r\n 调用设备IsLastStep 结果：{end.Code}_{end.Message}\r\n");
                    if (end.Code != ErrorCodes.Sys.SUCCESS)
                    {
                        isAllowedNextOperation = false;
                        return await Response(end.Code, "", deviceServiceInvokeRequest.ReplyTopic);
                    }

                    logger.LogDebug("\r\n IsWorking 改为false \r\n");
                    this.WatchingProperties.Property("IsWorking").SetValue(false);
                    this.IsWorking = false;
                }

                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId} end to Work!");

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogDebug($"\r\n 程序发生异常：{ex} \r\n");
                this.Status = DeviceStatus.Exception;
                isAllowedNextOperation = false;
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_Work_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        public override async Task<DeviceServiceInvokeResponse> Standby(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            errorInfo = new Tuple<string, string, string>("", "", "");
            return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
        }

        public override async Task<DeviceServiceInvokeResponse> Shutdown(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            errorInfo = new Tuple<string, string, string>("", "", "");
            return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
        }

        public override async Task<DeviceServiceInvokeResponse> Charge(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            try
            {
                if (!this.Engine.DeviceConnector.IsConnected)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_Charge_PlcUnConnected", "PlcUnConnected", "PLC未连接");
                    return await Response(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, deviceServiceInvokeRequest.ReplyTopic);
                }

                var agvMapId = configExtra["AGVMapId"].ToInt();
                var agvChargePosition = configExtra["AGVChargePosition"].ToInt();
                var order = new OrderRequestEntity()
                {
                    vehicle_id = configExtra["AGVId"].ToInt(),
                    priority = 10,
                    mission = new List<RequestMission>()
                    {
                        new RequestMission()
                        {
                           type= "move",
                           destination= agvChargePosition,
                           map_id= agvMapId
                        },
                        new RequestMission()
                        {
                            type="act",                //动作任务
                            action_id= 78,            //自定义动作
                            action_param1= 1,         //表示操作光栅的功能
                            action_param2= 0               //是动作任务
                        }
                    }
                };

                var agvToken = configExtra["AgvToken"].ToStr();
                var moveresult = await httpRequestInvoker.PostAsJsonAsync<OrderRequestEntity, Order>(configExtra["AGVMoveStart"].ToStr(), order, agvToken);

                if (moveresult == null)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_Charge_ChargrApiReponseNull", "ChargrApiReponseNull", "调用充电接口返回null");
                    return await Response(ErrorCodes.Sys.FAIL, "move：返回NULL", deviceServiceInvokeRequest.ReplyTopic);
                }

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_Charge_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }


        }

        public async Task<DeviceStatusReportResponse> ReportStatus()
        {
            var requestStatus = GetStatusRequest(DeviceStatus.Ready);

            var responseStatus = await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, requestStatus);
            return responseStatus;
        }

        private async Task<Dictionary<string, object?>> CollectRealProperties()
        {
            var realProperties = new Dictionary<string, object?>();
            var carInfo = await GetCarInfo();
            realProperties["CarCurrentPos"] = carInfo.cur_station_no.ToStr();
            realProperties["Battery"] = carInfo.battery.ToInt();
            realProperties["IsLowBattery"] = carInfo.battery < configExtra["LowBattery"].ToInt();
            realProperties["AgvStatus"] = carInfo.sys_state.ToStr();
            realProperties["CanDispatch"] = CanDispatch(carInfo);
            realProperties["IsConnected"] = this.Engine.DeviceConnector.IsConnected;
            realProperties["IsMoving"] = this.IsMoving;
            realProperties["IsWorking"] = this.IsWorking;
            var arrivedinfo = await ArrivedInfo();
            if (arrivedinfo != null)
            {
                realProperties["AgvTaskId"] = arrivedinfo.order_id.ToStr();
                realProperties["IsArrived"] = CheckIsArrived(arrivedinfo);
            }
            else
            {
                logger.LogError($"CollectRealProperties_ArrivedInfo_Null");
            }
            if (this.Engine.DeviceConnector.IsConnected)
            {
                Dictionary<string, object> plcInfo = GetPLCInfo();
                if (plcInfo != null)
                {
                    realProperties["PlcIsReady"] = plcInfo["PlcIsReady"];
                    realProperties["IsAuto"] = plcInfo["IsAuto"];
                    realProperties["IsError"] = plcInfo["IsError"];
                    realProperties["IsHalt"] = plcInfo["IsHalt"];
                }
                else
                {
                    logger.LogError($"CollectRealProperties_GetPLCInfo_NULL");
                }
            }
            else
            {
                logger.LogError($"CollectRealProperties_PLC_UnConnected");
            }
            return realProperties;
        }

        private bool CheckIsArrived(CallBackEntity arrivedinfo)
        {
            var returnTaskId = this.WatchingProperties.Property("AgvReturnTaskId").NewValue.ToStr();
            var agvTaskId = this.WatchingProperties.Property("AgvTaskId").NewValue.ToStr();
            var isSuccess = arrivedinfo.order_id > 0;
            if (returnTaskId == agvTaskId && isSuccess)
            {
                return true;
            }
            return false;
        }

        private async Task<CallBackEntity?> ArrivedInfo()
        {
            try
            {
                var deviceOnline = configExtra["DeviceOnline"].ToStr();
                var toMoveTaskId = this.WatchingProperties.Property("AgvReturnTaskId").NewValue.ToStr();
                logger.LogInformation("ArrivedInfo:" + toMoveTaskId);
                var arrivedInfo = await this.httpRequestInvoker.GetFromJsonAsync<CallBackEntity>(deviceOnline, new Dictionary<string, object> { { "orderid", toMoveTaskId } });
                if (arrivedInfo == null)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_ArrivedInfo_DeviceOnlineApiFail", "DeviceOnlineApiFail", "调用车辆接口null");
                }
                return arrivedInfo;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_ArrivedInfo_Exception", "Exception", ex.Message);
                return null;
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        private bool CanDispatch(VehicleInfo carInfo)
        {
            var lowBattery = configExtra["LowBattery"].ToInt();
            var isLowBattery = carInfo.battery.ToInt() < lowBattery;//是否低电量
            var canDispatch = carInfo.sys_state.ToStr() == "IDLE";//是否可接受任务
            if (!isLowBattery && canDispatch)
            {
                return true;
            }
            return false;
        }

        private Dictionary<string, object> GetPLCInfo()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            try
            {
                ushort[] status = modbusIpMaster.ReadHoldingRegisters(slaveId, 4101, 4);
                dictionary["PlcIsReady"] = status[0] == 1;
                dictionary["IsAuto"] = status[1] == 2;
                dictionary["IsError"] = status[2] == 1;
                dictionary["IsHalt"] = status[3] == 1;
                return dictionary;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_GetPLCInfo_Exception", "Exception", ex.Message);
                return null;
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        private async Task<VehicleInfo> GetCarInfo()
        {
            try
            {
                var carAllInfostr = configExtra["CarAllInfo"].ToStr();
                var agvToken = configExtra["AgvToken"].ToStr();
                var carlist = await httpRequestInvoker.GetFromJsonAsync<VehiclesResponse>(carAllInfostr, agvToken);
                if (carlist != null)
                {
                    var carinfo = carlist.vehicles.FirstOrDefault(x => x.nickname == DeviceId);
                    return carinfo == null ? new VehicleInfo() : carinfo;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new VehicleInfo();
            }
            return new VehicleInfo();
        }

        private void AddWatchingEvent()
        {
            WatchingProperties.Property("IsAuto")
               .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
               .TriggerAlways(async () =>
               {
                   //更新PLC的料仓信息
                   UpdatePLcSiloInfo();

                   await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, new DeviceEventReportRequest()
                   {
                       EventId = Events.AGV.AGV_UPDATE_PLCSILOINFO_EVENT,
                       EventName = Events.AGV.AGV_UPDATE_PLCSILOINFO_NAME,
                       DeviceId = this.DeviceId,
                       ProductId = this.ProductId,
                       ClientId = this.ClientId,
                       PayloadPanels = this.PayloadPanels
                   });
               });

            AgvStatusReportEvent();
            LoadMaterialOkReportEvent();
            UnLoadMaterialOkReportEvent();
        }

        private void AgvStatusReportEvent()
        {
            WatchingProperties.Property("IsArrived")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, new DeviceEventReportRequest()
                  {
                      EventId = Events.AGV.AGV_MOVE_ISARRIVED_EVENT,
                      EventName = Events.AGV.AGV_MOVE_ISARRIVED_NAME,
                      DeviceId = this.DeviceId,
                      ProductId = this.ProductId,
                      ClientId = this.ClientId,
                      PayloadPanels = this.PayloadPanels
                  });
              });

            WatchingProperties.Property("IsMoving")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, new DeviceEventReportRequest()
                  {
                      EventId = Events.AGV.AGV_MOVE_ISMOVING_EVENT,
                      EventName = Events.AGV.AGV_MOVE_ISMOVING_NAME,
                      DeviceId = this.DeviceId,
                      ProductId = this.ProductId,
                      ClientId = this.ClientId,
                      PayloadPanels = this.PayloadPanels
                  });
              });

            WatchingProperties.Property("AgvStatus")
              .When(properties => properties.NewValue.ToStr() == StdAgvStatus.CHARGING && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  this.Status = DeviceStatus.Charging;
                  await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, new DeviceEventReportRequest()
                  {
                      EventId = Events.AGV.AGV_CHARGE_EVENT,
                      EventName = Events.AGV.AGV_CHARGE_NAME,
                      DeviceId = this.DeviceId,
                      ProductId = this.ProductId,
                      ClientId = this.ClientId,
                      PayloadPanels = this.PayloadPanels
                  });
              });
        }

        private void UnLoadMaterialOkReportEvent()
        {
            WatchingProperties.Property("PrepareUnloadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {

                  await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, new DeviceEventReportRequest()
                  {
                      EventId = Events.AGV.AGV_PREPAREUNLOADOK_EVENT,
                      EventName = Events.AGV.AGV_PREPAREUNLOADOK_NAME,
                      DeviceId = this.DeviceId,
                      ProductId = this.ProductId,
                      ClientId = this.ClientId,
                      PayloadPanels = this.PayloadPanels
                  });
              });

            WatchingProperties.Property("InvokeUnloadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {

                  await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, new DeviceEventReportRequest()
                  {
                      EventId = Events.AGV.AGV_INVOKEUNLOADOK_EVENT,
                      EventName = Events.AGV.AGV_INVOKEUNLOADOK_NAME,
                      DeviceId = this.DeviceId,
                      ProductId = this.ProductId,
                      ClientId = this.ClientId,
                      PayloadPanels = this.PayloadPanels
                  });
              });

            WatchingProperties.Property("CompleteUnloadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {

                  await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, new DeviceEventReportRequest()
                  {
                      EventId = Events.AGV.AGV_COMPLETEUNLOADOK_EVENT,
                      EventName = Events.AGV.AGV_COMPLETEUNLOADOK_NAME,
                      DeviceId = this.DeviceId,
                      ProductId = this.ProductId,
                      ClientId = this.ClientId,
                      PayloadPanels = this.PayloadPanels
                  });
              });
        }

        private void LoadMaterialOkReportEvent()
        {
            WatchingProperties.Property("PrepareLoadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {

                  await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, new DeviceEventReportRequest()
                  {
                      EventId = Events.AGV.AGV_PREPARELOADOK_EVENT,
                      EventName = Events.AGV.AGV_PREPARELOADOK_NAME,
                      DeviceId = this.DeviceId,
                      ProductId = this.ProductId,
                      ClientId = this.ClientId,
                      PayloadPanels = this.PayloadPanels
                  });
              });

            WatchingProperties.Property("InvokeLoadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, new DeviceEventReportRequest()
                  {
                      EventId = Events.AGV.AGV_INVOKELOADOK_EVENT,
                      EventName = Events.AGV.AGV_INVOKELOADOK_NAME,
                      DeviceId = this.DeviceId,
                      ProductId = this.ProductId,
                      ClientId = this.ClientId,
                      PayloadPanels = this.PayloadPanels
                  });
              });

            WatchingProperties.Property("CompleteLoadOk")
              .When(properties => properties.NewValue.ToBool() && properties.IsValueChanged)
              .TriggerAlways(async () =>
              {
                  await httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(centralWebOptions.EventReport, new DeviceEventReportRequest()
                  {
                      EventId = Events.AGV.AGV_COMPLETELOADOK_EVENT,
                      EventName = Events.AGV.AGV_COMPLETELOADOK_NAME,
                      DeviceId = this.DeviceId,
                      ProductId = this.ProductId,
                      ClientId = this.ClientId,
                      PayloadPanels = this.PayloadPanels
                  });
              });
        }

        private void AGVReset()
        {
            try
            {
                modbusIpMaster.WriteMultipleRegisters(slaveId, 5010, new ushort[5] { 0, 0, 0, 0, 0 });
                modbusIpMaster.WriteSingleRegister(slaveId, 5016, 0);
                modbusIpMaster.WriteSingleRegister(slaveId, 5019, 0);
                modbusIpMaster.WriteSingleRegister(slaveId, 5021, 0);
                modbusIpMaster.WriteMultipleRegisters(slaveId, 4022, new ushort[3] { 0, 0, 0 });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_AGVReset_Exception", "Exception", ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        private Task<bool> MachineConnect() => Task.Run(() =>
        {
            try
            {
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}   begin to PlcConnect!");
                modbusIpMaster = this.modbusIpMasterWrapper.CreateIp(plcUri.Host, plcUri.Port);
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}   finish  PlcConnect!");

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_MachineConnect_Exception", "Exception", ex.Message);
                return false;
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        });

        private bool IsTimeout(DateTime startTime, int timeout)
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

        private Tuple<bool, string> PrepareLoadForSiloshelves(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareLoadForSiloshelves" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> PrepareLoadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareLoadForProcessedTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> PrepareLoadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareLoadForRawTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> PrepareLoadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareLoadForUnPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> PrepareLoadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareLoadForPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> PrepareLoadForDrill(DeviceServiceInvokeRequest request)
        {
            try
            {
                //ushort[] wData = new ushort[4] { (ushort)request.Params["Mode"].ToInt(), 1, 3, 1 };
                ////Write the silo data to the corresponding Position according to the silo type
                //ushort[] silos = "silosStrFromDB".StringToUshort();
                //modbusIpMaster.WriteMultipleRegisters(slaveId, 5100, silos);
                //modbusIpMaster.WriteMultipleRegisters(slaveId, 5010, wData);
                return new Tuple<bool, string>(true, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_PrepareLoadForDrill_Exception", "Exception", ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }

        }

        private Tuple<bool, string> InvokeLoadForSiloshelves(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeLoadForSiloshelves" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeLoadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeLoadForProcessedTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeLoadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeLoadForRawTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeLoadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeLoadForUnPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeLoadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeLoadForPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeLoadForDrill(DeviceServiceInvokeRequest request)
        {
            try
            {
                ////AGV deviation fine adjustment
                //modbusIpMaster.WriteMultipleRegisters(slaveId, 5019, request.Params["deviation_x"].ToFloat().SetReal());
                //modbusIpMaster.WriteMultipleRegisters(slaveId, 5021, request.Params["deviation_z"].ToFloat().SetReal());
                //Task.Delay(5);
                ////AGV reaches the designated Position and starts LoadMaterial
                //modbusIpMaster.WriteMultipleRegisters(slaveId, 5013, new ushort[2] { 0, 1 });
                //modbusIpMaster.WriteSingleRegister(slaveId, 5016, 1);
                return new Tuple<bool, string>(true, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_InvokeLoadForDrill_Exception", "Exception", ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }

        }

        private async Task<Tuple<bool, string>> CompleteLoadForSiloshelves(DeviceServiceInvokeRequest request)
        {
            try
            {
                modbusIpMaster.WriteSingleRegister(slaveId, 4062, 1);

                var startTime = DateTime.Now;
                var isTimeOut = false;
                while (true)
                {
                    ushort[] ushorts = modbusIpMaster.ReadHoldingRegisters(slaveId, 4063, 1);
                    if (ushorts[0].ToInt() == 1 || IsTimeout(startTime, postAndGetTimeOut))
                    {
                        logger.LogInformation("AGV可以离开叉齿区");
                        break;
                    }
                    isTimeOut = IsTimeout(startTime, waitPlcSignalTimeout);
                    if (isTimeOut)//超时或者 异常
                    {
                        logger.LogDebug("\r\n 等待 4063 信号超时\r\n");
                        break;
                    }
                    if (this.Status == DeviceStatus.Exception)// 异常
                    {
                        logger.LogDebug("\r\n 程序异常跳出循环\r\n");
                        break;
                    }
                }
                if (isTimeOut || this.Status == DeviceStatus.Exception)//超时或者 异常
                {
                    return new Tuple<bool, string>(false, "AGV可以离开叉齿区超时");
                }

                var outresult = await Move(request);
                if (outresult.Code == ErrorCodes.Sys.SUCCESS)
                {
                    modbusIpMaster.WriteSingleRegister(slaveId, 5061, 1);
                    logger.LogInformation("AGV已离开叉齿区");
                }

                while (true)
                {
                    ushort[] loadfinished = modbusIpMaster.ReadHoldingRegisters(slaveId, 4061, 1);
                    if (loadfinished[0].ToInt() == 1 || IsTimeout(startTime, postAndGetTimeOut))
                    {
                        logger.LogInformation("上料仓动作结束");
                        break;
                    }
                }
                if (IsTimeout(startTime, postAndGetTimeOut))
                {
                    return new Tuple<bool, string>(false, "上料动作超时");
                }
                modbusIpMaster.WriteSingleRegister(slaveId, 4062, 0);
                modbusIpMaster.WriteSingleRegister(slaveId, 5061, 0);
                logger.LogInformation("重置点位");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_CompleteLoadForSiloshelves_Exception", "Exception", ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
            return new Tuple<bool, string>(true, "");

        }

        private Tuple<bool, string> CompleteLoadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:CompleteLoadForProcessedTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> CompleteLoadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:CompleteLoadForRawTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> CompleteLoadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:CompleteLoadForUnPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> CompleteLoadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:CompleteLoadForPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> CompleteLoadForDrill(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation(request.DeviceId);
                ////BUFFER已收到生料
                //modbusIpMaster.WriteSingleRegister(slaveId, 5027, 1);
                //AGVReset();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_CompleteLoadForDrill_Exception", "Exception", ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> PrepareUnloadForSiloshelves(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareUnloadForSiloshelves" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> PrepareUnloadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareUnloadForProcessedTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> PrepareUnloadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareUnloadForRawTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> PrepareUnloadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareUnloadForUnPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> PrepareUnloadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareUnloadForPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        /// <summary>
        /// //PLC逻辑,对应单轴
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Tuple<bool, string> PrepareUnloadForDrill(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:PrepareUnloadForDrill:" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_PrepareUnloadForDrill_Exception", "Exception", ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeUnloadForSiloshelves(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeUnloadForSiloshelves:" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeUnloadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeUnloadForProcessedTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeUnloadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeUnloadForRawTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeUnloadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeUnloadForUnPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeUnloadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeUnloadForPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> InvokeUnloadForDrill(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:InvokeUnloadForDrill" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_InvokeUnloadForDrill_Exception", "Exception", ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
            return new Tuple<bool, string>(true, "");
        }

        private async Task<Tuple<bool, string>> CompleteUnloadForSiloshelves(DeviceServiceInvokeRequest request)
        {
            try
            {
                modbusIpMaster.WriteSingleRegister(slaveId, 5062, 1);
                logger.LogInformation("料仓出AGV");

                var postAndGetTimeout = configExtra["PostAndGetTimeout"].ToInt();
                var startTime = DateTime.Now;
                //AGV可以进入叉齿区
                while (true)
                {
                    ushort[] ushorts = modbusIpMaster.ReadHoldingRegisters(slaveId, 4062, 1);
                    if (ushorts[0].ToInt() == 1)
                    {
                        logger.LogInformation("AGV可以进入叉齿区");
                        break;
                    }
                    await Task.Delay(50);
                }

                var inresult = await Move(request);
                //AGV已到达叉齿区
                if (inresult.Code == ErrorCodes.Sys.SUCCESS)
                {
                    logger.LogInformation("AGV已到达叉齿区");
                    modbusIpMaster.WriteSingleRegister(slaveId, 5063, 1);
                }

                //AGV可以离开叉齿区
                while (true)
                {
                    ushort[] ushorts = modbusIpMaster.ReadHoldingRegisters(slaveId, 4063, 1);
                    if (ushorts[0].ToInt() == 1 || IsTimeout(startTime, postAndGetTimeout))
                    {
                        logger.LogInformation("AGV可以离开叉齿区");
                        break;
                    }

                    await Task.Delay(50);
                }
                if (IsTimeout(startTime, postAndGetTimeout))
                {
                    logger.LogInformation("离开叉齿区超时");
                    return new Tuple<bool, string>(false, "离开叉齿区超时");
                }

                var outresult = await Move(request);
                if (outresult.Code == ErrorCodes.Sys.SUCCESS)
                {
                    modbusIpMaster.WriteSingleRegister(slaveId, 5061, 1);
                    logger.LogInformation("AGV已离开叉齿区");
                }

                while (true)
                {
                    ushort[] unloadfinished = modbusIpMaster.ReadHoldingRegisters(slaveId, 4061, 1);
                    if (unloadfinished[0].ToInt() == 1 || IsTimeout(startTime, postAndGetTimeout))
                    {
                        logger.LogInformation("下料仓动作结束");
                        break;
                    }
                }
                if (IsTimeout(startTime, postAndGetTimeout))
                {
                    logger.LogInformation("超时");
                    return new Tuple<bool, string>(false, "料架下料超时");
                }

                modbusIpMaster.WriteSingleRegister(slaveId, 5061, 0);
                modbusIpMaster.WriteSingleRegister(slaveId, 5062, 0);
                modbusIpMaster.WriteSingleRegister(slaveId, 5063, 0);
                logger.LogInformation("重置点位");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_CompleteUnloadForSiloshelves_Exception", "Exception", ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> CompleteUnloadForProcessedTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:CompleteUnloadForProcessedTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> CompleteUnloadForRawTagingDesk(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:CompleteUnloadForRawTagingDesk" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> CompleteUnloadForUnPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:CompleteUnloadForUnPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> CompleteUnloadForPin(DeviceServiceInvokeRequest request)
        {
            try
            {
                logger.LogInformation("todo:CompleteUnloadForPin" + request.DeviceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return new Tuple<bool, string>(true, "");
        }

        private Tuple<bool, string> CompleteUnloadForDrill(DeviceServiceInvokeRequest request)
        {
            var result = false;
            var message = "";
            try
            {
                logger.LogInformation("todo:CompleteUnloadForDrill" + request.DeviceId);

                return new Tuple<bool, string>(true, message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_CompleteUnloadForDrill_Exception", "Exception", ex.Message);
                return new Tuple<bool, string>(false, ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }

        }

        private DeviceStatusReportRequest GetStatusRequest(DeviceStatus newStatus)
        {
            var request = new DeviceStatusReportRequest
            {
                DeviceId = DeviceDescriptor.DeviceId,
                ProductId = DeviceDescriptor.ProductId,
                NewStatus = newStatus,
                PayloadPanels = this.PayloadPanels
            };
            request.Params[Tasks.PARAMS_TASK_EVENT_TRACE_ID] = CurrentEventTraceId;
            request.Params["CarCurrentPos"] = this.WatchingProperties.Property("CarCurrentPos").NewValue.ToStr(); //CarCurrentPos
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
                var postAndGetTimeout = configExtra["PostAndGetTimeout"].ToInt();

                if (request.Params.ContainsKey("IsLastStep") && request.Params["IsLastStep"].ToBool())
                {
                    logger.LogDebug($"\r\n IsLastStep调用 {operationType} 接口一次\r\n");
                    var targetResponse = await httpRequestInvoker.PostAsJsonAsync<DeviceServiceInvokeRequest, DeviceServiceInvokeResponse>(operationType, request);
                    if (targetResponse == null)
                    {
                        logger.LogDebug($"\r\n IsLastStep调用 {operationType} 接口一次,结果:Null\r\n");
                    }
                    else
                    {
                        logger.LogDebug($"\r\n IsLastStep调用 {operationType} 接口一次,结果:{targetResponse.Code}\r\n");
                    }
                }
                else
                {
                    var isTimeOut = false;
                    while (true)
                    {
                        logger.LogDebug($"\r\n 调用{request.TargetProductId}_{request.TargetDeviceId}_{operationType}接口");
                        var targetResponse = await httpRequestInvoker.PostAsJsonAsync<DeviceServiceInvokeRequest, DeviceServiceInvokeResponse>(operationType, request);
                        if (targetResponse == null)
                        {
                            response.Code = ErrorCodes.Sys.WEBAPI_RETURNNULL_CODE;
                            response.Message = "返回值为NULL";
                            errorInfo = new Tuple<string, string, string>("AGV_AutoModelTargetDeviceOperation_PostApiNoSuccess", "PostApiNoSuccess", response.Message);
                            return response;
                        }
                        if (targetResponse.Code == ErrorCodes.Sys.SUCCESS)
                        {
                            break;
                        }
                        isTimeOut = IsTimeout(startTime, postAndGetTimeout);
                        if (isTimeOut)//超时或者 异常
                        {
                            logger.LogDebug("\r\n 等待 postAndGet 信号超时\r\n");
                            break;
                        }
                        if (this.Status == DeviceStatus.Exception)// 异常
                        {
                            logger.LogDebug("\r\n 程序异常跳出循环postAndGet\r\n");
                            break;
                        }
                        await Task.Delay(500);
                    }
                }
                if (IsTimeout(startTime, postAndGetTimeout) || this.Status == DeviceStatus.Exception)
                {
                    response.Code = ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_CODE;
                    response.Message = ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_MESSAGE;
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

        private Tuple<bool, string> PrepareLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            var result = false;
            var message = "";
            switch (request.TargetProductId)
            {
                case Products.DRILL:
                    var drill = PrepareLoadForDrill(request);
                    result = drill.Item1;
                    message = drill.Item2;
                    break;
                case Products.PIN:
                    var pin = PrepareLoadForPin(request);
                    result = pin.Item1;
                    message = pin.Item2;
                    break;
                case Products.UNPIN:
                    var unpin = PrepareLoadForUnPin(request);
                    result = unpin.Item1;
                    message = unpin.Item2;
                    break;
                case Products.RAWSTAGINGDESK:
                    var raw = PrepareLoadForRawTagingDesk(request);
                    result = raw.Item1;
                    message = raw.Item2;
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    var process = PrepareLoadForProcessedTagingDesk(request);
                    result = process.Item1;
                    message = process.Item2;
                    break;
                case Products.SILOSHELVES:
                    var SILOSHELVES = PrepareLoadForSiloshelves(request);
                    result = SILOSHELVES.Item1;
                    message = SILOSHELVES.Item2;
                    break;
                default:
                    result = false;
                    message = "PrepareLoadMaterialSelf：设备不正确" + request.ProductId;
                    break;
            }
            return new Tuple<bool, string>(result, message);
        }

        private Tuple<bool, string> InvokeLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            var result = false;
            var message = "";
            switch (request.TargetProductId)
            {
                case Products.DRILL:
                    var drill = InvokeLoadForDrill(request);
                    result = drill.Item1;
                    message = drill.Item2;
                    break;
                case Products.PIN:
                    var pin = InvokeLoadForPin(request);
                    result = pin.Item1;
                    message = pin.Item2;
                    break;
                case Products.UNPIN:
                    var unpin = InvokeLoadForUnPin(request);
                    result = unpin.Item1;
                    message = unpin.Item2;
                    break;
                case Products.RAWSTAGINGDESK:
                    var raw = InvokeLoadForRawTagingDesk(request);
                    result = raw.Item1;
                    message = raw.Item2;
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    var process = InvokeLoadForProcessedTagingDesk(request);
                    result = process.Item1;
                    message = process.Item2;
                    break;
                case Products.SILOSHELVES:
                    var siloshelves = InvokeLoadForSiloshelves(request);
                    result = siloshelves.Item1;
                    message = siloshelves.Item2;
                    break;
                default:
                    result = false;
                    message = "InvokeLoadMaterialSelf：设备不正确" + request.ProductId;
                    break;
            }
            return new Tuple<bool, string>(result, message);
        }

        private Tuple<bool, string> CompleteLoadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            var result = false;
            var message = "";
            switch (request.TargetProductId)
            {
                case Products.DRILL:
                    var drill = CompleteLoadForDrill(request);
                    result = drill.Item1;
                    message = drill.Item2;
                    break;
                case Products.PIN:
                    var pin = CompleteLoadForPin(request);
                    result = pin.Item1;
                    message = pin.Item2;
                    break;
                case Products.UNPIN:
                    var unpin = CompleteLoadForUnPin(request);
                    result = unpin.Item1;
                    message = unpin.Item2;
                    break;
                case Products.RAWSTAGINGDESK:
                    var raw = CompleteLoadForRawTagingDesk(request);
                    result = raw.Item1;
                    message = raw.Item2;
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    var process = CompleteLoadForProcessedTagingDesk(request);
                    result = process.Item1;
                    message = process.Item2;
                    break;
                case Products.SILOSHELVES:
                    var siloshelves = CompleteLoadForSiloshelves(request).Result;
                    result = siloshelves.Item1;
                    message = siloshelves.Item2;
                    break;
                default:
                    result = false;
                    message = "CompleteLoadMaterialSelf：设备不正确" + request.ProductId;
                    break;
            }
            return new Tuple<bool, string>(result, message);
        }

        private Tuple<bool, string> PrepareUnloadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            var result = false;
            var message = "";
            switch (request.TargetProductId)
            {
                case Products.DRILL:
                    var drill = PrepareUnloadForDrill(request);
                    result = drill.Item1;
                    message = drill.Item2;
                    break;
                case Products.PIN:
                    var pin = PrepareUnloadForPin(request);
                    result = pin.Item1;
                    message = pin.Item2;
                    break;
                case Products.UNPIN:
                    var unpin = PrepareUnloadForUnPin(request);
                    result = unpin.Item1;
                    message = unpin.Item2;
                    break;
                case Products.RAWSTAGINGDESK:
                    var raw = PrepareUnloadForRawTagingDesk(request);
                    result = raw.Item1;
                    message = raw.Item2;
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    var process = PrepareUnloadForProcessedTagingDesk(request);
                    result = process.Item1;
                    message = process.Item2;
                    break;
                case Products.SILOSHELVES:
                    var siloshelves = PrepareUnloadForSiloshelves(request);
                    result = siloshelves.Item1;
                    message = siloshelves.Item2;
                    break;
                default:
                    result = false;
                    message = "PrepareUnloadMaterialSelf：设备不正确" + request.ProductId;
                    break;
            }
            return new Tuple<bool, string>(result, message);
        }

        private Tuple<bool, string> InvokeUnloadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            var result = false;
            var message = "";
            switch (request.TargetProductId)
            {
                case Products.DRILL:
                    var drill = InvokeUnloadForDrill(request);
                    result = drill.Item1;
                    message = drill.Item2;
                    break;
                case Products.PIN:
                    var pin = InvokeUnloadForPin(request);
                    result = pin.Item1;
                    message = pin.Item2;
                    break;
                case Products.UNPIN:
                    var unpin = InvokeUnloadForUnPin(request);
                    result = unpin.Item1;
                    message = unpin.Item2;
                    break;
                case Products.RAWSTAGINGDESK:
                    var raw = InvokeUnloadForRawTagingDesk(request);
                    result = raw.Item1;
                    message = raw.Item2;
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    var process = InvokeUnloadForProcessedTagingDesk(request);
                    result = process.Item1;
                    message = process.Item2;
                    break;
                case Products.SILOSHELVES:
                    var siloshelves = InvokeUnloadForSiloshelves(request);
                    result = siloshelves.Item1;
                    message = siloshelves.Item2;
                    break;
                default:
                    result = false;
                    message = "InvokeUnloadMaterialSelf：设备不正确" + request.ProductId;
                    break;
            }
            return new Tuple<bool, string>(result, message);
        }

        private Tuple<bool, string> CompleteUnloadMaterialSelf(DeviceServiceInvokeRequest request)
        {
            var result = false;
            var message = "";
            switch (request.TargetProductId)
            {
                case Products.DRILL:
                    var drill = CompleteUnloadForDrill(request);
                    result = drill.Item1;
                    message = drill.Item2;
                    break;
                case Products.PIN:
                    var pin = CompleteUnloadForPin(request);
                    result = pin.Item1;
                    message = pin.Item2;
                    break;
                case Products.UNPIN:
                    var unpin = CompleteUnloadForUnPin(request);
                    result = unpin.Item1;
                    message = unpin.Item2;
                    break;
                case Products.RAWSTAGINGDESK:
                    var raw = CompleteUnloadForRawTagingDesk(request);
                    result = raw.Item1;
                    message = raw.Item2;
                    break;
                case Products.PROCESSEDSTAGINGDESK:
                    var process = CompleteUnloadForProcessedTagingDesk(request);
                    result = process.Item1;
                    message = process.Item2;
                    break;
                case Products.SILOSHELVES:
                    var siloshelves = CompleteUnloadForSiloshelves(request).Result;
                    result = siloshelves.Item1;
                    message = siloshelves.Item2;
                    break;
                default:
                    result = false;
                    message = "CompleteUnloadMaterialSelf：设备不正确" + request.ProductId;
                    break;
            }
            return new Tuple<bool, string>(result, message);
        }

        private async void AddWatchingStates()
        {
            WatchingProperties.Properties("IsConnected", "IsAuto", "PlcIsReady", "IsError", "IsHalt", "CanDispatch", "IsMoving", "IsWorking")
                  .When(properties => properties.Property("IsConnected").NewValue.ToBool()
                  && properties.Property("IsAuto").NewValue.ToBool()
                  && properties.Property("PlcIsReady").NewValue.ToBool()
                  && !properties.Property("IsError").NewValue.ToBool()
                  && !properties.Property("IsHalt").NewValue.ToBool()
                  && properties.Property("CanDispatch").NewValue.ToBool()
                  && !properties.Property("IsMoving").NewValue.ToBool()
                  && !properties.Property("IsWorking").NewValue.ToBool())
                  .TriggerAlways(async () =>
                  {
                      Console.WriteLine("Ready");
                      this.Status = DeviceStatus.Ready;
                      var request = GetStatusRequest(DeviceStatus.Ready);
                      await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, request);
                  });


            WatchingProperties.Properties("IsConnected", "IsAuto", "PlcIsReady", "IsError", "IsHalt", "CanDispatch", "IsWorking")
                  .When(properties => properties.Property("IsConnected").NewValue.ToBool()
                  && properties.Property("IsAuto").NewValue.ToBool()
                  && properties.Property("PlcIsReady").NewValue.ToBool()
                  && !properties.Property("IsError").NewValue.ToBool()
                  && !properties.Property("IsHalt").NewValue.ToBool()
                  && properties.Property("CanDispatch").NewValue.ToBool()
                  && properties.Property("IsWorking").NewValue.ToBool())
                  .TriggerAlways(async () =>
                  {
                      Console.WriteLine("Working");
                      this.Status = DeviceStatus.Working;
                      var request = GetStatusRequest(DeviceStatus.Working);
                      await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, request);
                  });

            WatchingProperties.Properties("IsAuto", "PlcIsReady", "IsConnected", "IsError", "IsHalt")
                  .When(properties => !properties.Property("IsConnected").NewValue.ToBool()
                  || !properties.Property("PlcIsReady").NewValue.ToBool()
                  || !properties.Property("IsAuto").NewValue.ToBool()
                  || properties.Property("IsError").NewValue.ToBool()
                  || properties.Property("IsHalt").NewValue.ToBool())
                 .TriggerAlways(async () =>
                 {
                     logger.LogDebug($"\r\n 监控到异常：\r\n" +
                         $"IsConnected：{WatchingProperties.Property("IsConnected").NewValue.ToBool()}\r\n" +
                         $"PlcIsReady：{WatchingProperties.Property("PlcIsReady").NewValue.ToBool()}\r\n" +
                         $"IsAuto：{WatchingProperties.Property("IsAuto").NewValue.ToBool()}\r\n" +
                         $"IsError：{WatchingProperties.Property("IsError").NewValue.ToBool()}\r\n" +
                         $"IsHalt：{WatchingProperties.Property("IsHalt").NewValue.ToBool()} \r\n");
                     this.Status = DeviceStatus.Exception;
                     var request = GetStatusRequest(DeviceStatus.Exception);
                     await httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(centralWebOptions.StatusReport, request);
                 });
        }

        private async Task<DeviceServiceInvokeResponse> TargetDeviceOperation(DeviceServiceInvokeRequest deviceServiceInvokeRequest, DeviceOperationType deviceOperation)
        {
            try
            {
                if (deviceDescriptor.SoloMode)
                {
                    logger.LogDebug("\r\n 单机模式：TargetDeviceOperation直接返回true \r\n ");
                    return await Response(ErrorCodes.Sys.SUCCESS, "", deviceServiceInvokeRequest.ReplyTopic);
                }

                logger.LogDebug($"\r\n TargetDeviceOperation :{deviceOperation} \r\n ");
                var targetDeviceId = deviceServiceInvokeRequest.TargetDeviceId;
                var startTime = DateTime.Now;
                var targetResult = new DeviceServiceInvokeResponse();
                var centralWebOptionsType = "";
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
                if (!deviceDescriptor.AutoMode)
                {
                    var targetDevice = deviceProvider.GetDevice(targetDeviceId);
                    if (targetDevice == null)
                    {
                        errorInfo = new Tuple<string, string, string>("AGV_TargetDeviceOperation_UnAutoMode_NoTargetDevice", "NoTargetDevice", string.Format($"未找到目标设备:{targetDeviceId}"));
                        return await Response(ErrorCodes.Sys.NOT_FIND_DEVICE_CODE, ErrorCodes.Sys.NOT_FIND_DEVICE_CODE, deviceServiceInvokeRequest.ReplyTopic);
                    }
                    var isTimeOut = false;
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
                                targetResult = await targetDevice.InvokeLoadMaterial(deviceServiceInvokeRequest);

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
                            logger.LogDebug($"\r\n 等待 deviceOperation:{deviceOperation} 信号超时\r\n");
                            break;
                        }
                        if (this.Status == DeviceStatus.Exception)// 异常
                        {
                            logger.LogDebug($"\r\n 程序异常跳出循环deviceOperation:{deviceOperation}\r\n");
                            break;
                        }
                        await Task.Delay(50);
                    }
                    if (IsTimeout(startTime, postAndGetTimeOut) || this.Status == DeviceStatus.Exception)
                    {
                        errorInfo = new Tuple<string, string, string>("AGV_TargetDeviceOperation_UnAutoModeTargetDeviceOperationTimeOut", "UnAutoModeTargetDeviceOperationTimeOut", string.Format($"目标设备操作超时:{targetDeviceId}"));
                        return await Response(ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_CODE, ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_MESSAGE, deviceServiceInvokeRequest.ReplyTopic);
                    }
                }
                else
                {
                    logger.LogDebug($"\r\n 程序调用 AutoModelTargetDeviceOperation");
                    targetResult = await AutoModelTargetDeviceOperation(deviceServiceInvokeRequest, centralWebOptionsType);
                    logger.LogDebug($"\r\n 程序调用 AutoModelTargetDeviceOperation 结果：{targetResult.Code}_{targetResult.Message}\r\n ");
                }

                if (targetResult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_TargetDeviceOperation_AutoModeTargetDeviceOperationNoSuccess", "AutoModeTargetDeviceOperationNoSuccess", string.Format($"目标设备操作不成功:{targetDeviceId}"));
                    return await Response(targetResult.Code, targetResult.Message, deviceServiceInvokeRequest.ReplyTopic);
                }
                return await Response(ErrorCodes.Sys.SUCCESS, "", deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                errorInfo = new Tuple<string, string, string>("AGV_AutoModelTargetDeviceOperation_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        private void UpdateSiloInfo(DeviceServiceInvokeRequest request)
        {
            try
            {
                //var operationEntity = JsonSerializer.Deserialize<SwapPanel>(request.Params["LoadingPanel"].ToStr(),
                //                                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var operationEntity = request.Params["LoadingPanel"] as SwapPanel;
                logger.LogDebug($"\r\n UpdateSiloInfo_operationEntity：{JsonSerializer.Serialize(operationEntity)}\r\n ");
                if (operationEntity == null || operationEntity.PanelList == null)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_UpdateSiloInfoo_UnExistUnloadingPanel", "UnExistUnloadingPanel", "operationEntity:null");
                    return;
                }

                logger.LogDebug($"\r\n UpdateSiloInfo_CallerRequestInteractionBehavior：{request.CallerRequestInteractionBehavior}\r\n ");
                if (request.CallerRequestInteractionBehavior == InteractionBehavior.UnloadPanelOnly
                    || request.CallerRequestInteractionBehavior == InteractionBehavior.LoadPanelThenUnloadPanel
                    || request.CallerRequestInteractionBehavior == InteractionBehavior.UnloadPanelThenLoadPanel)
                {
                    var plcSiloLayer = request.Params["UnloadPanelLayer"].ToInt();
                    PayloadPanels[plcSiloLayer - 1] = operationEntity.PanelList[0];
                    logger.LogDebug($"更新{plcSiloLayer}层下料信息：{JsonSerializer.Serialize(operationEntity.PanelList[0])}\r\n ");
                    logger.LogDebug($"\r\n 下料时整体料仓信息：{JsonSerializer.Serialize(PayloadPanels)}\r\n ");
                }
                else
                {
                    PayloadPanels = operationEntity.PanelList;
                }
                //给PLC更新料仓信息
                UpdatePLcSiloInfo();

            }
            catch (Exception ex)
            {
                errorInfo = new Tuple<string, string, string>("AGV_UpdateUnloadingSiloInfo_Exception", "Exception", ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }
        private void UpdateUnloadingSiloInfo(DeviceServiceInvokeRequest request)
        {
            try
            {
                //var operationEntity = JsonSerializer.Deserialize<SwapPanel>(request.Params["UnloadingPanel"].ToStr(),
                //                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var operationEntity = request.Params["UnloadingPanel"] as SwapPanel;
                logger.LogDebug($"\r\n UpdateUnloadingSiloInfo_operationEntity：{JsonSerializer.Serialize(operationEntity)}\r\n ");
                if (operationEntity == null || operationEntity.PanelList == null)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_UpdateUnloadingSiloInfo_UnExistUnloadingPanel", "UnExistUnloadingPanel", "operationEntity:null");
                    return;
                }

                logger.LogDebug($"\r\n UpdateUnloadingSiloInfo_CallerRequestInteractionBehavior：{request.CallerRequestInteractionBehavior}\r\n ");
                if (request.CallerRequestInteractionBehavior == InteractionBehavior.UnloadPanelOnly
                    || request.CallerRequestInteractionBehavior == InteractionBehavior.LoadPanelThenUnloadPanel
                    || request.CallerRequestInteractionBehavior == InteractionBehavior.UnloadPanelThenLoadPanel)
                {
                    var plcSiloLayer = request.Params["UnloadPanelLayer"].ToInt();
                    operationEntity.PanelList[0].DrillState = PanelDrillState.Drilled;
                    operationEntity.PanelList[0].Layer = plcSiloLayer;
                    operationEntity.PanelList[0].Position = 1;
                    PayloadPanels[plcSiloLayer - 1] = operationEntity.PanelList[0];
                    logger.LogDebug($"更新{plcSiloLayer}层下料信息：{JsonSerializer.Serialize(operationEntity.PanelList[0])}\r\n ");
                    logger.LogDebug($"\r\n 下料时整体料仓信息：{JsonSerializer.Serialize(PayloadPanels)}\r\n ");
                }
                else
                {
                    for (int i = 0; i < operationEntity.PanelList.Count; i++)
                    {
                        var panel = operationEntity.PanelList[i];
                        if (panel == null)
                        {
                            continue;
                        }
                        panel.DrillState = PanelDrillState.Drilled;
                        panel.Layer = (i + 1);
                        panel.Position = 1;
                    }
                    PayloadPanels = operationEntity.PanelList;
                }
                //给PLC更新料仓信息
                UpdatePLcSiloInfo();

            }
            catch (Exception ex)
            {
                errorInfo = new Tuple<string, string, string>("AGV_UpdateUnloadingSiloInfo_Exception", "Exception", ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        private void UpdatePLcSiloInfo()
        {
            logger.LogDebug("\r\n 给PLC更新料仓信息 \r\n ");
            var siloInfo = "";
            for (int i = 0; i < PayloadPanels.Count; i++)
            {
                //PLC料仓信息
                var panelType = 0;
                var panelinfo = PayloadPanels[i];
                if (panelinfo == null)
                {
                    panelType = 0;
                    siloInfo += $"【{i},{panelType}】";
                    modbusIpMaster.WriteSingleRegister(slaveId, (4250 + i).ToUshort(), 0);
                    //料仓板宽信息
                    float defaultvalue = 0;
                    modbusIpMaster.WriteMultipleRegisters(slaveId, (4300 + i * 2).ToUshort(), defaultvalue.FloatToReal());
                    //销钉距中心偏移量
                    modbusIpMaster.WriteMultipleRegisters(slaveId, (4350 + i * 2).ToUshort(), defaultvalue.FloatToReal());
                    continue;
                }
                if (PayloadPanels[i] != null && panelinfo.DrillState == PanelDrillState.Drilled)
                {
                    panelType = 2;
                }
                else
                {
                    panelType = 1;
                }
                modbusIpMaster.WriteSingleRegister(slaveId, (4250 + i).ToUshort(), panelType.ToUshort());
                //料仓板宽信息
                var panelWidth = panelinfo.PanelWidth.FloatToReal();
                modbusIpMaster.WriteMultipleRegisters(slaveId, (4300 + i * 2).ToUshort(), panelWidth);
                //销钉距中心偏移量
                var pinOffset = panelinfo.PinOffset.FloatToReal();
                modbusIpMaster.WriteMultipleRegisters(slaveId, (4350 + i * 2).ToUshort(), pinOffset);

                siloInfo += $"【{i},{panelType}】";
            }
            logger.LogDebug("给PLC料仓的详细信息：" + siloInfo);
        }

        private void UpdateLoadingSiloInfo(DeviceServiceInvokeRequest request)
        {
            try
            {
                //料仓信息，更改
                //var operationEntity = JsonSerializer.Deserialize<SwapPanel>(request.Params["LoadingPanel"].ToStr(),
                //                                                     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var operationEntity = request.Params["LoadingPanel"] as SwapPanel;

                if (operationEntity == null)
                {
                    logger.LogDebug("operationEntity 为NULL");
                    errorInfo = new Tuple<string, string, string>("AGV_UpdateLoadingSiloInfo_UnExistLoadingPanel", "UnExistLoadingPanel", "operationEntity:null");
                    return;
                }
                if (operationEntity.PanelList.Count <= 0)
                {
                    logger.LogDebug("operationEntity Count<= 0");
                    errorInfo = new Tuple<string, string, string>("AGV_UpdateLoadingSiloInfo_UnExistLoadingPanel", "UnExistLoadingPanel", "更新料仓信息失败 operationEntity.PanelList:0");
                    return;
                }

                if (operationEntity.PanelList[0].Layer < 0 || operationEntity.PanelList[0].Layer > PayloadPanels.Count - 1)
                {
                    var message = $"PayloadPanels count{PayloadPanels.Count} perationEntity.PanelList[0].Layer:{operationEntity.PanelList[0].Layer}"; errorInfo = new Tuple<string, string, string>("AGV_UpdateLoadingSiloInfo_PayloadPanelsCountError", "PayloadPanelsCountError", message);
                    logger.LogDebug(message);
                }
                else
                {
                    PayloadPanels[operationEntity.PanelList[0].Layer] = null;
                }

                //给PLC更新料仓信息
                UpdatePLcSiloInfo();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_UpdateLoadingSiloInfo_Exception", "Exception", ex.Message);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        private Tuple<bool, string, string> CheckStatus(DeviceOperationType deviceOperation)
        {
            if (!this.Engine.DeviceConnector.IsConnected)
            {
                errorInfo = new Tuple<string, string, string>("AGV_CheckStatus_PlcUnConnected", "PlcUnConnected", "PLC未连接");
                return new Tuple<bool, string, string>(false, ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE);
            }
            var checkresult = true;
            switch (deviceOperation)
            {
                case DeviceOperationType.PrepareLoadMaterial:
                    if (this.Status != DeviceStatus.Working) checkresult = false;
                    break;
                case DeviceOperationType.InvokeLoadMaterial:
                    if (this.Status == DeviceStatus.Exception || this.Status != DeviceStatus.Working) checkresult = false;
                    break;
                case DeviceOperationType.CompleteLoadMaterial:
                    if (this.Status == DeviceStatus.Exception || this.Status != DeviceStatus.Working) checkresult = false;
                    break;
                case DeviceOperationType.PrepareUnloadMaterial:
                    if (this.Status != DeviceStatus.Working) checkresult = false;
                    break;
                case DeviceOperationType.InvokeUnloadMaterial:
                    if (this.Status == DeviceStatus.Exception || this.Status != DeviceStatus.Working) checkresult = false;
                    break;
                case DeviceOperationType.CompleteUnloadMaterial:
                    if (this.Status == DeviceStatus.Exception || this.Status != DeviceStatus.Working) checkresult = false;
                    break;
                default:
                    checkresult = false;
                    break;
            }
            if (!checkresult)
            {
                errorInfo = new Tuple<string, string, string>("AGV_CheckStatus_DeviceStatusError", "DeviceStatusError", string.Format($"{deviceOperation},设备状态不正确：{this.Status}"));
                return new Tuple<bool, string, string>(false, ErrorCodes.Sys.FAIL, string.Format($"{deviceOperation},设备状态不正确：{this.Status}"));
            }
            return new Tuple<bool, string, string>(true, ErrorCodes.Sys.SUCCESS, "");
        }

        private async Task<DeviceServiceInvokeResponse> DoServiceCallDeviceFirstComplete(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
            DeviceOperationType deviceOperationType,
            Func<DeviceOperationType, Task<DeviceServiceInvokeResponse>> checkDeviceStatusAction,
            Func<DeviceServiceInvokeRequest, DeviceOperationType, Task<DeviceServiceInvokeResponse>> deviceAction,
            Func<DeviceServiceInvokeRequest, Task<DeviceServiceInvokeResponse>> selfAction)
        {
            try
            {
                logger.LogDebug($"\r\n 开始DoServiceCallDeviceFirstComplete_{deviceOperationType}_checkDeviceStatusAction \r\n");
                var checkStatusResponse = await checkDeviceStatusAction.Invoke(deviceOperationType);
                var message = $"DoServiceCallDeviceFirstComplete_{deviceOperationType}_checkDeviceStatusAction:{checkStatusResponse.Code}_{checkStatusResponse.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug($"\r\n {message} \r\n");
                if (checkStatusResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_CheckDeviceStatusActionError", "CheckDeviceStatusActionError", checkStatusResponse.Message);
                    return await Response(ErrorCodes.Sys.FAIL, checkStatusResponse.Message, deviceServiceInvokeRequest.ReplyTopic);
                }

                logger.LogDebug($"\r\n 开始DoServiceCallDeviceFirstComplete_{deviceOperationType}_deviceAction \r\n");
                var deviceResult = await deviceAction.Invoke(deviceServiceInvokeRequest, deviceOperationType);
                var deviceResultmessage = $"DoServiceCallDeviceFirstComplete_{deviceOperationType}_deviceAction:{deviceResult.Code}_{deviceResult.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug($"\r\n {deviceResultmessage} \r\n");
                if (deviceResult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_DeviceActionError", "DeviceActionError", deviceResult.Message);
                    return await Response(ErrorCodes.Sys.FAIL, deviceResult.Message, deviceServiceInvokeRequest.ReplyTopic);
                }

                logger.LogDebug($"\r\n 开始DoServiceCallDeviceFirstComplete_{deviceOperationType}_selfAction \r\n");
                var selfResult = await selfAction.Invoke(deviceServiceInvokeRequest);
                var selfResultmessage = $"DoServiceCallDeviceFirstComplete_{deviceOperationType}_selfAction:{selfResult.Code}_{selfResult.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug($"\r\n {selfResultmessage} \r\n");
                if (selfResult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_SelfActionError", "SelfActionError", selfResult.Message);
                    return await Response(ErrorCodes.Sys.FAIL, selfResult.Message, deviceServiceInvokeRequest.ReplyTopic);
                }

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> DoServiceCallSelfFirstComplete(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
            DeviceOperationType deviceOperationType,
            Func<DeviceOperationType, Task<DeviceServiceInvokeResponse>> checkDeviceStatusAction,
            Func<DeviceServiceInvokeRequest, Task<DeviceServiceInvokeResponse>> selfAction,
                Func<DeviceServiceInvokeRequest, DeviceOperationType, Task<DeviceServiceInvokeResponse>> deviceAction)
        {
            try
            {
                var checkStatusResponse = await checkDeviceStatusAction.Invoke(deviceOperationType);
                var message = $"DoServiceCallSelfFirstComplete_{deviceOperationType}_checkDeviceStatusAction:{checkStatusResponse.Code}_{checkStatusResponse.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug(message);
                if (checkStatusResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_DoServiceCallSelfFirstComplete_checkDeviceStatusActionError", "checkDeviceStatusActionError", checkStatusResponse.Message);
                    return await Response(ErrorCodes.Sys.FAIL, checkStatusResponse.Message, deviceServiceInvokeRequest.ReplyTopic);
                }

                var selfresult = await selfAction.Invoke(deviceServiceInvokeRequest);
                var selfresultmessage = $"DoServiceCallSelfFirstComplete_{deviceOperationType}_selfAction:{selfresult.Code}_{selfresult.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug(selfresultmessage);
                if (selfresult.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_DoServiceCallSelfFirstComplete_SelfActionNoSuccess", "SelfActionNoSuccess", selfresult.Message);
                    return await Response(ErrorCodes.Sys.FAIL, selfresult.Message, deviceServiceInvokeRequest.ReplyTopic);
                }

                var result = await deviceAction.Invoke(deviceServiceInvokeRequest, deviceOperationType);
                var resultmessage = $"DoServiceCallSelfFirstComplete_{deviceOperationType}_deviceAction:{selfresult.Code}_{selfresult.Message}_Request:{JsonSerializer.Serialize(deviceServiceInvokeRequest)}";
                logger.LogDebug(resultmessage);
                if (result.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_DoServiceCallSelfFirstComplete_DeviceActionNoSuccess", "DeviceActionNoSuccess", result.Message);
                    return await Response(ErrorCodes.Sys.FAIL, result.Message, deviceServiceInvokeRequest.ReplyTopic);
                }

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty, deviceServiceInvokeRequest.ReplyTopic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                errorInfo = new Tuple<string, string, string>("AGV_DoServiceCall_Exception", "Exception", ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ex.Message, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        private void AddWatchingErrorInfo(Tuple<string, string, string> errorinfo)
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
            logger.LogError($"{GLOBAL_EXCEPTION_EVENT_ID}:{errorInfo.Item1}{Environment.NewLine}{GLOBAL_EXCEPTION_EVENT_NAME}:{errorInfo.Item2}{Environment.NewLine}{GLOBAL_EXCEPTION_EVENT_MESSAGE}:{errorInfo.Item3}");
            errorInfo = new Tuple<string, string, string>("", "", "");

        }

        private bool SetLoadingPanel(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            bool result = true;
            //判断是否是钻机(是否是单片)
            if (deviceServiceInvokeRequest.CallerRequestInteractionBehavior == InteractionBehavior.LoadPanelOnly
                 || deviceServiceInvokeRequest.CallerRequestInteractionBehavior == InteractionBehavior.LoadPanelThenUnloadPanel
                 || deviceServiceInvokeRequest.CallerRequestInteractionBehavior == InteractionBehavior.UnloadPanelThenLoadPanel)
            {
                ///PLC操作的板料位置
                var readspindleId = item.SpindleId;
                if (item.SpindleId > 3)
                {
                    readspindleId = item.SpindleId - 3;
                }

                var plcSiloLayer = new ushort[1];
                if (readspindleId == 1)
                {
                    plcSiloLayer = modbusIpMaster.ReadHoldingRegisters(slaveId, 4112, 1);
                }
                else if (readspindleId == 2)
                {
                    plcSiloLayer = modbusIpMaster.ReadHoldingRegisters(slaveId, 4114, 1);
                }
                else if (readspindleId == 3)
                {
                    plcSiloLayer = modbusIpMaster.ReadHoldingRegisters(slaveId, 4116, 1);
                }

                logger.LogDebug($"{item.SpindleId}轴上料的层数是{plcSiloLayer[0]}:" + JsonSerializer.Serialize(PayloadPanels));
                var onepanel = PayloadPanels.FirstOrDefault(x => x != null && x.Layer == (plcSiloLayer[0] - 1));

                if (onepanel != null)
                {
                    logger.LogDebug($"SetLoadingPanel onepanel 1111");
                    onepanel.ProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_2;
                    onepanel.Position = item.SpindleId;
                }
                else
                {
                    logger.LogDebug($"SetLoadingPanel onepanel null");
                    result = false;
                    return result;
                }

                logger.LogDebug($"SetLoadingPanel onepanel 2222:" + JsonSerializer.Serialize(onepanel));
                var loadingPanel = new SwapPanel()
                {
                    AgvPosition = item.AgvPosition,
                    SpindleId = item.SpindleId,

                    PanelList = new List<Panel>() { onepanel }
                };
                deviceServiceInvokeRequest.Params["LoadingPanel"] = loadingPanel;
                logger.LogDebug($"SetLoadingPanel onepanel 3333");
            }
            else
            {
                foreach (var panelItem in PayloadPanels)
                {
                    if (panelItem != null)
                    {
                        panelItem.ProductStatus = ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1;
                    }
                }
                var loadingPanel = new SwapPanel()
                {
                    AgvPosition = item.AgvPosition,
                    SpindleId = item.SpindleId,
                    PanelList = PayloadPanels
                };
                deviceServiceInvokeRequest.Params["LoadingPanel"] = loadingPanel;
                if (PayloadPanels.Count <= 0)
                {
                    result = false;
                }
            }
            return result;
        }

        private static void SetUnloadingPanel(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            deviceServiceInvokeRequest.Params["UnloadingPanel"] = item;
        }
        private int SetPlcSiloLayer(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            var plcSiloLayer = new ushort[1];

            if (deviceServiceInvokeRequest.TargetProductId != null && deviceServiceInvokeRequest.TargetProductId.ToLower().Contains("drill"))
            {
                var readspindleId = item.SpindleId;
                if (item.SpindleId > 3)
                {
                    readspindleId = item.SpindleId - 3;
                }


                if (readspindleId == 1)
                {
                    plcSiloLayer = modbusIpMaster.ReadHoldingRegisters(slaveId, 4113, 1);
                }
                else if (readspindleId == 2)
                {
                    plcSiloLayer = modbusIpMaster.ReadHoldingRegisters(slaveId, 4115, 1);
                }
                else if (readspindleId == 3)
                {
                    plcSiloLayer = modbusIpMaster.ReadHoldingRegisters(slaveId, 4117, 1);
                }

                deviceServiceInvokeRequest.Params["UnloadPanelLayer"] = plcSiloLayer[0];
            }
            return plcSiloLayer[0];
        }

        private async Task<DeviceServiceInvokeResponse> LoadMaterialBehavior(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            try
            {
                logger.LogDebug($"\r\n 开始PrepareLoadMaterial \r\n");
                var deviceOperationResponse = await PrepareLoadMaterial(deviceServiceInvokeRequest);
                var errormsg = string.Format($"PrepareLoadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_LoadMaterialBehavior_PrepareLoadMaterialNoSuccess", "PrepareLoadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                logger.LogDebug($"\r\n 开始InvokeLoadMaterial \r\n");
                deviceOperationResponse = await InvokeLoadMaterial(deviceServiceInvokeRequest);
                errormsg = string.Format($"InvokeLoadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_LoadMaterialBehavior_InvokeLoadMaterialNoSuccess", "InvokeLoadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                var result = SetLoadingPanel(deviceServiceInvokeRequest, item);
                if (!result)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_SetLoadingPanel_SetLoadingPanelError", "SetLoadingPanelError", "SetLoadingPanel false");
                    deviceOperationResponse.Code = ErrorCodes.Sys.FAIL;
                    deviceOperationResponse.Message = "SetLoadingPanelError";
                    return deviceOperationResponse;
                }

                //等待顶升下降信号
                logger.LogDebug("等待顶升下降信号：4118");
                var startTime = DateTime.Now;
                var waitPlcSignalTime = configExtra["WaitPlcSignalTimeout"].ToInt();
                var isTimeOut = false;
                while (true)
                {
                    logger.LogDebug("等待4118");
                    var needdown = modbusIpMaster.ReadHoldingRegisters(slaveId, 4118, 1);
                    if (needdown[0] == 1)
                    {
                        logger.LogDebug("收到顶升下降信号：4118");
                        break;
                    }
                    isTimeOut = IsTimeout(startTime, waitPlcSignalTime);
                    if (isTimeOut)//超时
                    {
                        logger.LogDebug("\r\n 等待 4118 信号超时\r\n");
                        break;
                    }
                    if (this.Status == DeviceStatus.Exception)// 异常
                    {
                        logger.LogDebug("\r\n 程序异常跳出循环4118  \r\n");
                        break;
                    }
                    await Task.Delay(50);
                }

                if (isTimeOut || this.Status == DeviceStatus.Exception)//超时或者 异常
                {
                    return await Response(ErrorCodes.Sys.FAIL, "4118 超时或者异常", deviceServiceInvokeRequest.ReplyTopic);
                }

                logger.LogDebug($"\r\n 开始CompleteLoadMaterial \r\n");
                deviceOperationResponse = await CompleteLoadMaterial(deviceServiceInvokeRequest);
                errormsg = string.Format($"CompleteLoadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_LoadMaterialBehavior_CompleteLoadMaterialNoSuccess", "CompleteLoadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                //logger.LogDebug($"\r\n 料仓信息更改_上料 \r\n");
                //UpdateLoadingSiloInfo(deviceServiceInvokeRequest);

                logger.LogDebug("重置顶升下降信号");
                modbusIpMaster.WriteSingleRegister(slaveId, 4118, 0);
                logger.LogDebug("设置顶升下降完成");
                modbusIpMaster.WriteSingleRegister(slaveId, 4225, 1);

                return deviceOperationResponse;
            }
            catch (Exception ex)
            {
                errorInfo = new Tuple<string, string, string>("AGV_LoadMaterialBehavior_Exception", "Exception", "异常：" + ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ErrorCodes.Sys.EXCEPTION_MESSAGE, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }
        private async Task<DeviceServiceInvokeResponse> UnloadMaterialBehavior(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            try
            {
                logger.LogDebug($"\r\n 开始PrepareUnloadMaterial \r\n");
                var deviceOperationResponse = await PrepareUnloadMaterial(deviceServiceInvokeRequest);
                var errormsg = string.Format($"PrepareUnloadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_UnloadMaterialBehavior_PrepareUnloadMaterialNoSuccess", "PrepareUnloadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                logger.LogDebug($"\r\n 开始InvokeUnloadMaterial \r\n");
                deviceOperationResponse = await InvokeUnloadMaterial(deviceServiceInvokeRequest);
                errormsg = string.Format($"InvokeUnloadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_UnloadMaterialBehavior_InvokeUnloadMaterialNoSuccess", "InvokeUnloadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                logger.LogDebug($"\r\n 等待{item.SpindleId}轴下料动作完成信号\r\n");
                var startTime = DateTime.Now;
                var waitPlcSignalTime = configExtra["WaitPlcSignalTimeout"].ToInt();
                var isTimeOut = false;
                while (true)
                {
                    logger.LogDebug("等待4119");
                    var status = modbusIpMaster.ReadHoldingRegisters(slaveId, 4119, 1);
                    if (status[0] == 1)
                    {
                        logger.LogDebug($"\r\n 收到{item.SpindleId}轴下料动作完成信号\r\n");
                        break;
                    }
                    isTimeOut = IsTimeout(startTime, waitPlcSignalTime);
                    if (isTimeOut)//超时或者 异常
                    {
                        logger.LogDebug("\r\n 等待 4119 信号超时\r\n");
                        break;
                    }
                    if (this.Status == DeviceStatus.Exception)// 异常
                    {
                        logger.LogDebug("\r\n 程序异常跳出循环 4119 \r\n");
                        break;
                    }
                    await Task.Delay(50);
                }

                if (isTimeOut || this.Status == DeviceStatus.Exception)//超时或者 异常
                {
                    return await Response(ErrorCodes.Sys.FAIL, "4119超时或者异常", deviceServiceInvokeRequest.ReplyTopic);
                }

                logger.LogDebug($"\r\n 设置下料层数 \r\n");
                var plcSiloLayer = SetPlcSiloLayer(deviceServiceInvokeRequest, item);
                logger.LogDebug($"\r\n 设置下料层数{plcSiloLayer}完成 \r\n");

                logger.LogDebug($"\r\n 设置下料的板信息:{JsonSerializer.Serialize(item)} \r\n");
                SetUnloadingPanel(deviceServiceInvokeRequest, item);

                logger.LogDebug($"\r\n 开始CompleteUnloadMaterial \r\n");
                deviceOperationResponse = await CompleteUnloadMaterial(deviceServiceInvokeRequest);
                errormsg = string.Format($"CompleteUnloadMaterial：{deviceOperationResponse.Code}_{deviceOperationResponse.Message}");
                logger.LogDebug($"\r\n {errormsg} \r\n");
                if (deviceOperationResponse.Code != ErrorCodes.Sys.SUCCESS)
                {
                    errorInfo = new Tuple<string, string, string>("AGV_UnloadMaterialBehavior_CompleteUnloadMaterialNoSuccess", "CompleteUnloadMaterialNoSuccess", errormsg);
                    return deviceOperationResponse;
                }

                //logger.LogDebug($"\r\n 料仓信息更改_下料 \r\n");
                //UpdateUnloadingSiloInfo(deviceServiceInvokeRequest);

                return deviceOperationResponse;
            }
            catch (Exception ex)
            {
                errorInfo = new Tuple<string, string, string>("AGV_UnloadMaterialBehavior_Exception", "Exception", "异常：" + ex.Message);
                return await Response(ErrorCodes.Sys.EXCEPTION_CODE, ErrorCodes.Sys.EXCEPTION_MESSAGE, deviceServiceInvokeRequest.ReplyTopic);
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        private async Task<DeviceServiceInvokeResponse> MoveCheck(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            if (!this.Engine.DeviceConnector.IsConnected)
            {
                errorInfo = new Tuple<string, string, string>("AGV_Move_PlcUnConnected", "PlcUnConnected", "PLC未连接");
                return await Response(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE, deviceServiceInvokeRequest.ReplyTopic);
            }

            var carInfo = await GetCarInfo();
            if (string.IsNullOrEmpty(carInfo.nickname))
            {
                errorInfo = new Tuple<string, string, string>("AGV_Move_NoCarInfo", "NoCarInfo", "未查询到车辆信息");
                return await Response(ErrorCodes.Sys.FAIL, "获取不到AGV车辆信息:" + carInfo.nickname, deviceServiceInvokeRequest.ReplyTopic);
            }

            if (!deviceServiceInvokeRequest.Params.ContainsKey("MoveTargetPos"))
            {
                errorInfo = new Tuple<string, string, string>("AGV_Move_NoKey", "NoKey", "未找到参数：MoveTargetPos");
                return await Response(ErrorCodes.Sys.FAIL, "未找到参数：MoveTargetPos", deviceServiceInvokeRequest.ReplyTopic);
            }

            var agvTargetPos = deviceServiceInvokeRequest.Params["MoveTargetPos"].ToStr();
            logger.LogInformation("Move : CurrentStation" + carInfo.cur_station_no.ToStr() + ",   TargetStation" + agvTargetPos);

            if (modbusIpMaster == null)
            {
                errorInfo = new Tuple<string, string, string>("AGV_Move_modbusIpMasterNull", "modbusIpMasterNull", "modbusIpMasterNull");
                return await Response(ErrorCodes.Sys.FAIL, "modbusIpMasterNull", deviceServiceInvokeRequest.ReplyTopic);
            }
            var carCanMoveAndFinished = modbusIpMaster.ReadHoldingRegisters(slaveId, 4110, 1);

            if (carCanMoveAndFinished == null)
            {
                errorInfo = new Tuple<string, string, string>("AGV_Move_ReadPlcError", "ReadPlcError", "ReadPlcError:4110");
                return await Response(ErrorCodes.Sys.FAIL, "ReadPlcError:4110", deviceServiceInvokeRequest.ReplyTopic);
            }
            if (carCanMoveAndFinished[0] != 1)
            {
                errorInfo = new Tuple<string, string, string>("AGV_Move_carCanMoveAndFinishedError", "carCanMoveAndFinishedError", $"carCanMoveAndFinishedError:{carCanMoveAndFinished[0]}");
                return await Response(ErrorCodes.Sys.FAIL, $"小车不在原点4110:{carCanMoveAndFinished[0]}", deviceServiceInvokeRequest.ReplyTopic);
            }

            if (carInfo.cur_station_no.ToStr().Equals(agvTargetPos))
            {
                errorInfo = new Tuple<string, string, string>("AGV_Move_OnCurrentStation", "OnCurrentStation", "车辆已经在当前位置");
                return await Response(ErrorCodes.Sys.SUCCESS, "已经处于目标位置", deviceServiceInvokeRequest.ReplyTopic);
            }
            return await Response(ErrorCodes.Sys.SUCCESS, "", deviceServiceInvokeRequest.ReplyTopic);
        }

        /// <summary>
        /// //获取钻机在用的轴位，停用是用null表示
        /// 根据业务类型，获取操作列表
        /// </summary>
        /// <param name="deviceServiceInvokeRequest"></param>
        /// <param name="targetPoslist"></param>
        /// <returns>动作列表，PLC预备运动</returns>
        private List<TemporaryOperationEntity> GetActionList(DeviceServiceInvokeRequest deviceServiceInvokeRequest, string[] targetPoslist)
        {
            var actionList = new List<TemporaryOperationEntity>();

            var operation1 = new TemporaryOperationEntity();
            var operation2 = new TemporaryOperationEntity();

            var splideIndex = 0;
            foreach (var targetPos in targetPoslist)
            {
                splideIndex++;
                if (splideIndex <= 3)
                {
                    if (targetPos.ToLower() == "null" || targetPos == "0")
                    {
                        operation1.PlcPreActionList.Add(new PlcPreAction
                        {
                            SpindleId = splideIndex,
                            SpindleAction = 0
                        });
                        continue;
                    }
                    operation1.PlcPreActionList.Add(new PlcPreAction
                    {
                        SpindleId = splideIndex,
                        SpindleAction = deviceServiceInvokeRequest.CallerRequestInteractionBehavior
                    });
                    operation1.AgvPosition = targetPos;
                    var swapPanel = new SwapPanel();
                    swapPanel.SpindleId = splideIndex;
                    swapPanel.AgvPosition = targetPos;
                    swapPanel.InteractionBehavior = deviceServiceInvokeRequest.CallerRequestInteractionBehavior;
                    switch (deviceServiceInvokeRequest.CallerRequestInteractionBehavior)
                    {
                        case InteractionBehavior.LoadPanelOnly:
                        default:
                            break;
                        case InteractionBehavior.UnloadPanelOnly:
                            break;
                        case InteractionBehavior.UnloadPanelThenLoadPanel:
                            ////先取下料信息,首次运行时，可能不需要下料
                            var drilledPanel = deviceServiceInvokeRequest.PayloadPanels.FirstOrDefault(p => p != null
                                                                                         && p.DrillState == PanelDrillState.Drilled
                                                                                         && p.Position == splideIndex);
                            if (drilledPanel != null)
                            {
                                swapPanel.PanelList.Add(drilledPanel);
                            }
                            break;
                    }
                    operation1.OperationList.Add(swapPanel);
                }
                else if (splideIndex > 3 && splideIndex <= 6)
                {
                    if (targetPos.ToLower() == "null" || targetPos == "0")
                    {
                        operation2.PlcPreActionList.Add(new PlcPreAction
                        {
                            SpindleId = splideIndex,
                            SpindleAction = 0
                        });
                        continue;
                    }
                    operation2.PlcPreActionList.Add(new PlcPreAction
                    {
                        SpindleId = splideIndex,
                        SpindleAction = deviceServiceInvokeRequest.CallerRequestInteractionBehavior
                    });
                    operation2.AgvPosition = targetPos;
                    var swapPanel = new SwapPanel();
                    swapPanel.SpindleId = splideIndex;
                    swapPanel.AgvPosition = targetPos;
                    swapPanel.InteractionBehavior = deviceServiceInvokeRequest.CallerRequestInteractionBehavior;

                    switch (deviceServiceInvokeRequest.CallerRequestInteractionBehavior)
                    {
                        case InteractionBehavior.LoadPanelOnly:
                        default:
                            break;
                        case InteractionBehavior.UnloadPanelOnly:
                            break;
                        case InteractionBehavior.UnloadPanelThenLoadPanel:
                            ////先取下料信息,首次运行时，可能不需要下料
                            var drilledPanel = deviceServiceInvokeRequest.PayloadPanels.FirstOrDefault(p => p != null
                                                                                        && p.DrillState == PanelDrillState.Drilled
                                                                                        && p.Position == splideIndex);
                            if (drilledPanel != null)
                            {
                                swapPanel.PanelList.Add(drilledPanel);
                            }
                            break;
                    }
                    operation2.OperationList.Add(swapPanel);
                }
            }

            actionList.Add(operation1);
            actionList.Add(operation2);
            return actionList;
        }
        public async Task<DeviceServiceInvokeResponse> Arrived(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var startTime = DateTime.Now;
            while (true)
            {
                logger.LogDebug("等待车辆到达");
                var isArrived = WatchingProperties.Property("IsArrived").NewValue.ToBool();
                if (isArrived || IsTimeout(startTime, moveTimeout) || this.Status == DeviceStatus.Exception)
                {
                    WatchingProperties.Property("IsMoving").SetValue(false);
                    this.IsMoving = false;
                    break;
                }

                await Task.Delay(50);
            }

            if (IsTimeout(startTime, moveTimeout) || this.Status == DeviceStatus.Exception)
            {
                return await Response(ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_CODE, ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_MESSAGE, deviceServiceInvokeRequest.ReplyTopic);
            }
            return await Response(ErrorCodes.Sys.SUCCESS, "", deviceServiceInvokeRequest.ReplyTopic);
        }

        private void SetPlcPerAction(DeviceServiceInvokeRequest deviceServiceInvokeRequest, List<TemporaryOperationEntity> actionList, int i)
        {
            foreach (var item in actionList[i].PlcPreActionList)
            {
                var spindleId = item.SpindleId;
                var behavior = 0;
                if (item.SpindleAction == InteractionBehavior.LoadPanelOnly || item.SpindleAction == InteractionBehavior.LoadSiloOnly)
                {
                    behavior = 1;
                }
                else if (item.SpindleAction == InteractionBehavior.UnloadPanelOnly || item.SpindleAction == InteractionBehavior.UnloadSiloOnly)
                {
                    behavior = 2;
                }
                else if (item.SpindleAction == InteractionBehavior.UnloadPanelThenLoadPanel || item.SpindleAction == InteractionBehavior.UnloadSiloThenLoadSilo)
                {
                    behavior = 3;
                }

                if (item.SpindleId > 3)
                {
                    spindleId = spindleId - 3;
                }
                if (spindleId == 1)
                {
                    modbusIpMaster.WriteSingleRegister(slaveId, 4214, (ushort)behavior);
                    var panel = deviceServiceInvokeRequest.PayloadPanels.FirstOrDefault(x => x != null && x.Position == item.SpindleId);
                    if (panel != null)
                    {
                        modbusIpMaster.WriteMultipleRegisters(slaveId, 4219, panel.PanelWidth.FloatToReal());//夹爪打开的范围
                        modbusIpMaster.WriteMultipleRegisters(slaveId, 4233, panel.PinOffset.FloatToReal());//偏移量
                    }
                }
                else if (spindleId == 2)
                {
                    modbusIpMaster.WriteSingleRegister(slaveId, 4215, (ushort)behavior);
                    var panel = deviceServiceInvokeRequest.PayloadPanels.FirstOrDefault(x => x != null && x.Position == item.SpindleId);
                    if (panel != null)
                    {
                        modbusIpMaster.WriteMultipleRegisters(slaveId, 4221, panel.PanelWidth.FloatToReal());//夹爪打开的范围
                        modbusIpMaster.WriteMultipleRegisters(slaveId, 4235, panel.PinOffset.FloatToReal());//偏移量
                    }
                }
                else if (spindleId == 3)
                {
                    modbusIpMaster.WriteSingleRegister(slaveId, 4216, (ushort)behavior);
                    var panel = deviceServiceInvokeRequest.PayloadPanels.FirstOrDefault(x => x != null && x.Position == item.SpindleId);
                    if (panel != null)
                    {
                        modbusIpMaster.WriteMultipleRegisters(slaveId, 4223, panel.PanelWidth.FloatToReal());//夹爪打开的范围
                        modbusIpMaster.WriteMultipleRegisters(slaveId, 4237, panel.PinOffset.FloatToReal());//偏移量
                    }
                }
            }
        }

        private void SetPlcSingleSpindle(DeviceServiceInvokeRequest deviceServiceInvokeRequest, SwapPanel item)
        {
            //告知PLC几号轴要动作，什么动作,气夹打开范围
            var spindlePosition = item.SpindleId;
            if (spindlePosition > 3)
            {
                spindlePosition = spindlePosition - 3;
            }
            modbusIpMaster.WriteSingleRegister(slaveId, 4217, (ushort)spindlePosition);

            var behavior = 0;
            if (deviceServiceInvokeRequest.CallerRequestInteractionBehavior == InteractionBehavior.LoadPanelOnly
                || deviceServiceInvokeRequest.CallerRequestInteractionBehavior == InteractionBehavior.LoadSiloOnly)
            {
                behavior = 1;
            }
            else if (deviceServiceInvokeRequest.CallerRequestInteractionBehavior == InteractionBehavior.UnloadPanelOnly
                || deviceServiceInvokeRequest.CallerRequestInteractionBehavior == InteractionBehavior.UnloadSiloOnly)
            {
                behavior = 2;
            }
            else if (deviceServiceInvokeRequest.CallerRequestInteractionBehavior == InteractionBehavior.UnloadPanelThenLoadPanel
                || deviceServiceInvokeRequest.CallerRequestInteractionBehavior == InteractionBehavior.UnloadSiloThenLoadSilo)
            {
                behavior = 3;
            }
            modbusIpMaster.WriteSingleRegister(slaveId, 4218, behavior.ToUshort());//动作详情

        }

        private bool CanWritePlc()
        {
            try
            {
                var millionSecond = DateTime.Now.Second;
                var plcHeart = modbusIpMaster.ReadHoldingRegisters(slaveId, 4100, 1);

                if (millionSecond % 2 == 1)
                {
                    logger.LogInformation($"给PLC的心跳信号{millionSecond}：1,实际值：{plcHeart[0]}");

                    modbusIpMaster.WriteSingleRegister(slaveId, 4200, plcHeart[0]);
                }
                else
                {
                    logger.LogInformation($"给PLC的心跳信号{millionSecond}：0,实际值：{plcHeart[0]}");
                    modbusIpMaster.WriteSingleRegister(slaveId, 4200, plcHeart[0]);
                }
                //Engine.DeviceConnector.IsConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                errorInfo = new Tuple<string, string, string>("AGV_UnloadMaterialBehavior_Exception", "Exception", "异常：" + ex.Message);
                logger.LogWarning($"{DeviceDescriptor.DeviceId} cannot connect to plc.");
                //Engine.DeviceConnector.IsConnected = false;
                return false;
            }
            finally
            {
                AddWatchingErrorInfo(errorInfo);
            }
        }

        public Tuple<bool, string> CheckCanMove()
        {
            var carCanMoveAndFinished = modbusIpMaster.ReadHoldingRegisters(slaveId, 4110, 1);

            if (carCanMoveAndFinished == null)
            {
                errorInfo = new Tuple<string, string, string>("AGV_Move_ReadPlcError", "ReadPlcError", "ReadPlcError:4110");
                return new Tuple<bool, string>(false, "ReadPlcError:4110");
            }
            if (carCanMoveAndFinished[0] != 1)
            {
                errorInfo = new Tuple<string, string, string>("AGV_Move_carCanMoveAndFinishedError", "carCanMoveAndFinishedError", $"carCanMoveAndFinishedError:{carCanMoveAndFinished[0]}");
                return new Tuple<bool, string>(false, $"carCanMoveAndFinishedError:{carCanMoveAndFinished[0]}");
            }
            return new Tuple<bool, string>(true, "");
        }

        public Tuple<bool, string> ResetDeviceStatus()
        {
            this.WatchingProperties.Property("IsWorking").SetValue(false);
            Status = DeviceStatus.Online;
            this.IsWorking = false;
            return new Tuple<bool, string>(true, "属性 IsWorking 改为 false Status 改为 Online");
        }

        private void AddWatchingProperties()
        {
            WatchingProperties.AddProperty("ConnectionStatus", false)
            .AddProperty("IsWorking", false)
            .AddProperty("CanDispatch", false)
            .AddProperty("AgvStatus", "")
            .AddProperty("AgvTaskId", "")
            .AddProperty("AgvReturnTaskId", "0")//这个很重要
            .AddProperty("CarCurrentPos", "")
            .AddProperty("CarTargetPos", "")
            .AddProperty("MoveTargetPos", "")
            .AddProperty("Battery", 30)
            .AddProperty("IsMoving", false)
            .AddProperty("IsArrived", false)
            .AddProperty("Offset_X", 0)
            .AddProperty("Offset_Z", 0)
            .AddProperty("PlcIsReady", false)
            .AddProperty("IsAuto", false)
            .AddProperty("IsError", false)
            .AddProperty("IsHalt", false)
            .AddProperty("IsConnected", false)
            .AddProperty("PrepareLoadOk", false)
            .AddProperty("InvokeLoadOk", false)
            .AddProperty("CompleteLoadOk", false)
            .AddProperty("PrepareUnloadOk", false)
            .AddProperty("InvokeUnloadOk", false)
            .AddProperty("CompleteUnloadOk", false);
        }
    }
}
