using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.Chassis
{
    public class StdRobotChassis : IAgvChassis
    {
        private static readonly Dictionary<string, string> _agvInfo;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<StdRobotChassis> _logger;
        private JsonSerializerOptions _jsonSerializerOptions;

        public StdRobotChassis(IMemoryCache memoryCache, ILogger<StdRobotChassis> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task<DefaultCallBackEntity> ArrivedInfoLocal(DefaultAgv InteractingDevice)
        {
            try
            {
                var arrivedinfo = GetArrivedInfo(InteractingDevice);
                if (arrivedinfo == null)
                {
                    return new DefaultCallBackEntity();
                }
                var defaultentity = new DefaultCallBackEntity()
                {
                    AGV_ID = arrivedinfo.VehicleName,
                    IsSuccess = true,
                    position = arrivedinfo.PositionCode,
                    //X_Error = arrivedinfo.X_Error,
                    //Z_Error = arrivedinfo.Z_Error,
                    //Exception = arrivedinfo.Exception,
                    //Status = arrivedinfo.Status,
                    TaskId = arrivedinfo.OrderId.ToStr(),
                };
                return defaultentity;
            }
            catch (Exception ex)
            {
                InteractingDevice.logger.LogError(ex, ex.Message);
                return new DefaultCallBackEntity();
            }
        }

        public async Task<DeviceServiceInvokeResponse> ArrivedLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest, string moveid = "")
        {
            var startTime = DateTime.Now;
            var isFinishedWork = false;
            var timeout = false;
            var arrivedresult = false;
            InteractingDevice.logger.LogDebug($"等待车辆到达_{moveid}");
            while (true)
            {
                timeout = InteractingDevice.IsTimeout(startTime, InteractingDevice.moveTimeout);
                isFinishedWork = InteractingDevice.CanProceedNextStep();
                if (timeout || isFinishedWork)
                {
                    InteractingDevice.Status = DeviceStatus.Exception;

                    InteractingDevice.logger.LogDebug($"判断车辆到达方法:timeout：{timeout}，isFinishedWork：{isFinishedWork}");
                    break;
                }

                var arrivedinfo = GetArrivedInfo(InteractingDevice);
                if (arrivedinfo == null)
                {
                    //  InteractingDevice.logger.LogDebug("STDArrivedLocal_GetArrivedInfo_arrivedinfo==null");
                    await Task.Delay(500);
                    continue;
                }

                var returnTaskId = moveid != "" ? moveid : InteractingDevice.WatchingProperties.Property("AgvReturnTaskId").NewValue.ToStr();
                var agvTaskId = arrivedinfo.OrderId.ToStr();
                var isSuccess = true;
                InteractingDevice.logger.LogDebug($"判断是否到达  returnTaskId:{returnTaskId}，AgvTaskId：{agvTaskId}，IsSuccess：{isSuccess}");
                if (returnTaskId == agvTaskId && isSuccess)
                {
                    arrivedresult = true;
                    InteractingDevice.logger.LogDebug($"车辆到达 returnTaskId:{returnTaskId}，AgvTaskId：{agvTaskId}");
                    //InteractingDevice.logger.LogDebug($"车辆到达 returnTaskId:{returnTaskId}，AgvTaskId：{agvTaskId}，X偏差值：{arrivedinfo.X_Error.ToFloat() * 1000}，Y偏差值：{arrivedinfo.Z_Error.ToFloat() * 1000}");
                    //InteractingDevice.WatchingProperties.Property("Offset_X").SetValue((arrivedinfo.X_Error.ToFloat() * 1000).ToString("0.00"));
                    InteractingDevice.WatchingProperties.Property("Offset_Y").SetValue(0);//todo 待AGV补充
                    //InteractingDevice.WatchingProperties.Property("Offset_Z").SetValue((arrivedinfo.Z_Error.ToFloat() * 1000).ToString("0.00"));
                    InteractingDevice.WatchingProperties.Property("AgvTaskId").SetValue(agvTaskId);
                    InteractingDevice.WatchingProperties.Property("IsArrived").SetValue(true);
                    InteractingDevice.WatchingProperties.Property("IsMoving").SetValue(false);
                    InteractingDevice.isMoving = false;
                    //InteractingDevice.deviationValue = arrivedinfo.X_Error.ToFloat() * 1000;
                    break;
                }
                if (!InteractingDevice.AgvScanResult)
                {
                    InteractingDevice.Status = DeviceStatus.Exception;
                    InteractingDevice.logger.LogDebug($"判断是否到达 AgvScanResult:{InteractingDevice.AgvScanResult}");
                    break;
                }

                await Task.Delay(500);
            }

            if (timeout || isFinishedWork || !InteractingDevice.AgvScanResult)
            {
                InteractingDevice.Status = DeviceStatus.Exception;
                InteractingDevice.modbusIpMaster.WriteSingleRegister(InteractingDevice.slaveId, 5002, 999);
                var errmessage = $"等待车辆到达失败！_给PLC 5002,写值 999 报警,无则反馈，超时{timeout},异常{isFinishedWork},拍二维码{InteractingDevice.AgvScanResult},Status=DeviceStatus.Exception,";
                InteractingDevice.logger.LogDebug(errmessage);
                return await InteractingDevice.Response(ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_CODE, errmessage);
            }
            if (arrivedresult)
            {
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            else
            {
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "未到达ArrivedLocal=false");
            }
        }

        public async Task<DeviceServiceInvokeResponse> CancelLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            STDCancelTaskRequestEntity request = new STDCancelTaskRequestEntity()
            {
                reqCode = Guid.NewGuid().ToString(),
                reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                clientCode = "weijia"
            };

            request.taskId = deviceServiceInvokeRequest.Params["taskCode"].ToInt();
            var url = InteractingDevice.DeviceDescriptor.Extra["CancelOrder"].ToStr();
            InteractingDevice.logger.LogInformation($"CancelTask: Request: {JsonSerializer.Serialize(request, _jsonSerializerOptions)}");
            var cancelResult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<STDCancelTaskRequestEntity, STDCancelTaskResponseEntity>(url, request, _jsonSerializerOptions);
            InteractingDevice.logger.LogDebug($"CancelTask: Result:{JsonSerializer.Serialize(cancelResult)}");

            if (cancelResult == null || cancelResult?.code != 0)
            {
                await InteractingDevice.Response(ErrorCodes.Sys.FAIL, cancelResult?.message);
            }

            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
        }

        private DateTime date = DateTime.Now;

        public async Task<bool> CanDispatchLocal(DefaultAgv InteractingDevice)
        {
            try
            {
                var carInfo = InteractingDevice.CarInfoExtra["CarInfoExtra"] as CarModel;
                if (carInfo == null)
                {
                    return false;
                }
                var canDispatch = carInfo.CanDispatch.ToBool();
                if (canDispatch)
                {
                    return true;
                }

                if (canDispatch)
                {
                    date = DateTime.Now;
                    return true;
                }
                if (!canDispatch && (DateTime.Now - date).TotalSeconds < 60)
                {
                    var msg = $"AGV CanDispatch属性FALSE,1分钟内持续保持TRUE";
                    await InteractingDevice.ReportingProcess(msg);
                    InteractingDevice.logger.LogDebug(msg);
                    return true;
                }
            }
            catch (Exception ex)
            {
                await InteractingDevice.ReportingProcess(ex.Message);
                InteractingDevice.logger.LogDebug(ex, ex.Message);
            }

            return false;
        }

        public Task<DeviceServiceInvokeResponse> ChargeLocal(DefaultAgv InteractingDevice)
        {
            return InteractingDevice.ResponseFail("ChargeLocal 待完善");
        }

        public async Task<bool> CheckIsArrivedLocal(DefaultAgv InteractingDevice)
        {
            try
            {
                var returnTaskId = InteractingDevice.WatchingProperties.Property("AgvReturnTaskId").NewValue.ToStr();
                var agvTaskId = InteractingDevice.WatchingProperties.Property("AgvTaskId").NewValue.ToStr();
                var arrivedinfo = GetArrivedInfo(InteractingDevice);
                if (arrivedinfo == null)
                {
                    //  InteractingDevice.logger.LogDebug($"STD_CheckIsArrivedLocal  arrivedinfo == null");
                    return false;
                }
                var isSuccess = true;
                if (returnTaskId == agvTaskId && isSuccess)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                InteractingDevice.logger.LogDebug($"STD_CheckIsArrivedLocal");
            }

            return false;
        }

        public async Task<DeviceServiceInvokeResponse> GetAgvInfoLocal(DefaultAgv InteractingDevice)
        {
            if (_memoryCache.TryGetValue("StdCarCache", out DeviceServiceInvokeResponse cacheResponse))
            {
                return cacheResponse!;
            }

            var response = new DeviceServiceInvokeResponse();

            var carModel = await GetStdCarInfo(InteractingDevice);
            response.Code = ErrorCodes.Sys.SUCCESS;
            response.Message = "斯坦德车辆信息";
            response.Params["CarInfoResponse"] = carModel;
            InteractingDevice.CarInfoExtra["CarInfoExtra"] = carModel;

           _memoryCache.Set("StdCarCache", response, new TimeSpan(0, 0, 10));
            return response;
        }

        public async Task<bool> IsLowBatteryLocal(DefaultAgv InteractingDevice)
        {
            //var carInfo = await GetAjwCarInfo(InteractingDevice);
            var carInfo = InteractingDevice.CarInfoExtra["CarInfoExtra"] as CarModel;
            if (carInfo == null)
            {
                return false;
            }
            var configLowBattery = InteractingDevice.DeviceDescriptor.Extra["LowBattery"].ToFloat();
            var battery = carInfo.Battery;
            if (battery < configLowBattery)
            {
                return true;
            }
            return false;
        }

        public async Task<DeviceServiceInvokeResponse> MoveCheckLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            if (!InteractingDevice.Engine.DeviceConnector.IsConnected)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_PlcUnConnected", "PlcUnConnected", "PLC未连接");
                return await InteractingDevice.Response(ErrorCodes.Sys.PLC_UNCONNECT_CODE, ErrorCodes.Sys.PLC_UNCONNECT_MESSAGE);
            }

            var carInfo = await GetStdCarInfo(InteractingDevice);
            if (carInfo == null || string.IsNullOrEmpty(carInfo.Name))
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_NoCarInfo", "NoCarInfo", "未查询到车辆信息");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "获取不到AGV车辆信息:" + InteractingDevice.DeviceId);
            }

            if (!deviceServiceInvokeRequest.Params.ContainsKey("MoveTargetPos"))
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_NoKey", "NoKey", "未找到参数：MoveTargetPos");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "未找到参数：MoveTargetPos");
            }

            var agvTargetPos = deviceServiceInvokeRequest.Params["MoveTargetPos"].ToStr();
            InteractingDevice.logger.LogInformation("Move : CurrentStation " + carInfo.CurrentStation.ToStr() + ",   TargetStation" + agvTargetPos);

            if (!string.IsNullOrEmpty(InteractingDevice.DeviceDescriptor.Extra["ModbusTcpUri"].ToStr()))
            {
                if (InteractingDevice.modbusIpMaster == null)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_modbusIpMasterNull", "modbusIpMasterNull", "modbusIpMasterNull");
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "modbusIpMasterNull");
                }

                if (InteractingDevice.IsCheckMovePoint)
                {
                    var checkMovePoint = deviceServiceInvokeRequest.Params.ContainsKey("CheckMovePoint") ? deviceServiceInvokeRequest.Params["CheckMovePoint"].ToInt() :
                        InteractingDevice.DeviceDescriptor.Extra["CheckMovePoint"].ToInt();
                    var carCanMoveAndFinished = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, (ushort)checkMovePoint, 1);

                    InteractingDevice.logger.LogInformation($"checkMovePoint的参数值:{carCanMoveAndFinished[0]}");
                    InteractingDevice.ReportingProcess($"checkMovePoint的参数值:{carCanMoveAndFinished[0]}");

                    if (carCanMoveAndFinished == null)
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_ReadPlcError", "ReadPlcError", "ReadPlcError");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "ReadPlcError");
                    }

                    if (carCanMoveAndFinished[0] != 1 && InteractingDevice.Status != DeviceStatus.Working)
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_carCanMoveAndFinishedError", "carCanMoveAndFinishedError", $"carCanMoveAndFinishedError:{carCanMoveAndFinished[0]}");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"小车结构不在原点 {checkMovePoint}:{carCanMoveAndFinished[0]}");
                    }
                }
            }
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
        }

        public async Task<DeviceServiceInvokeResponse> MoveLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var response = new DeviceServiceInvokeResponse();
            try
            {
                var movecheck = await MoveCheckLocal(InteractingDevice, deviceServiceInvokeRequest);

                if (movecheck.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_MoveCheckError", "MoveCheckError", movecheck.Message);
                    return await InteractingDevice.Response(movecheck.Code, movecheck.Message);
                }
                var agvTargetPos = deviceServiceInvokeRequest.Params["MoveTargetPos"].ToStr();
                var carCurrentPos = InteractingDevice.WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
                InteractingDevice.logger.LogDebug("Move : CurrentStation" + carCurrentPos + ",   TargetStation" + agvTargetPos);
                var whichAgv = InteractingDevice.DeviceDescriptor.DeviceName;

                var request = new StdMoveRequest()
                {
                    TargetType = 1,
                    TargetPosCode = agvTargetPos,
                    VehicleId = whichAgv,
                    GroupId = InteractingDevice.StdGroupNo
                };

                var moveRequestPath = InteractingDevice.DeviceDescriptor.Extra["AGVMoveStart"].ToStr();
                InteractingDevice.logger.LogInformation($"接口发送移动命令，AGV编号:{whichAgv},目标点位{agvTargetPos},req: {JsonSerializer.Serialize(request, _jsonSerializerOptions)}");
                var moveresult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<StdMoveRequest, StdMoveResponse>(moveRequestPath, request, _jsonSerializerOptions);
                InteractingDevice.logger.LogDebug($"Agv移动结果：{JsonSerializer.Serialize(moveresult, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                })}");
                if (moveresult?.Code != 0)
                {
                    response.Code = ErrorCodes.Sys.FAIL;
                    response.Message = "移动失败";
                    response.Params["MoveResponse"] = moveresult;
                    return response;
                }
                InteractingDevice.logger.LogDebug($"Agv移动结果,移动ID：{moveresult.Data.OrderId}");
                InteractingDevice.publicMoveId = moveresult.Data.OrderId.ToStr();
                response.Code = ErrorCodes.Sys.SUCCESS;
                response.Message = "";
                response.Params["MoveResponse"] = moveresult;
                return response;
            }
            catch (Exception ee)
            {
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = ee.Message;
                response.Params["MoveResponse"] = "";
                return response;
            }
        }

        public async Task<DeviceServiceInvokeResponse> MoveOperationLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest, int operationType)
        {
            var response = new DeviceServiceInvokeResponse();
            try
            {
                var movecheck = await MoveCheckLocal(InteractingDevice, deviceServiceInvokeRequest);
                if (movecheck.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_MoveCheckError", "MoveCheckError", movecheck.Message);
                    return await InteractingDevice.Response(movecheck.Code, movecheck.Message);
                }
                var agvTargetPos = deviceServiceInvokeRequest.Params["MoveTargetPos"].ToStr();
                var carCurrentPos = InteractingDevice.WatchingProperties.Property("CarCurrentPos").NewValue.ToStr();
                InteractingDevice.logger.LogDebug("Move : CurrentStation" + carCurrentPos + ",   TargetStation" + agvTargetPos);
                var whichAgv = InteractingDevice.DeviceDescriptor.DeviceName;

                var operation = "0";
                if (operationType == (int)AgvOperationType.ToTop)
                {
                    operation = "100";
                }
                else if (operationType == (int)AgvOperationType.ToBottom)
                {
                    operation = "50";
                }
                var listParams = new List<TaskParams>() {
                     new TaskParams(){
                         Height=operation
                     }
                };

                var request = new StdMoveRequest()
                {
                    TargetType = 1,
                    TargetPosCode = agvTargetPos,
                    VehicleId = whichAgv,
                    GroupId = InteractingDevice.StdGroupNo
                };

                var moveRequestPath = InteractingDevice.DeviceDescriptor.Extra["AGVMoveStart"].ToStr();
                var moveresult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<StdMoveRequest, StdMoveResponse>(moveRequestPath, request, _jsonSerializerOptions);
                InteractingDevice.logger.LogDebug($"Agv移动结果：{JsonSerializer.Serialize(moveresult)}");
                if (moveresult?.Code != 0)
                {
                    response.Code = ErrorCodes.Sys.FAIL;
                    response.Message = "移动失败";
                    response.Params["MoveResponse"] = moveresult;
                    return response;
                }
                InteractingDevice.logger.LogDebug($"Agv移动结果,移动ID：{moveresult.Data.OrderId}");
                InteractingDevice.publicMoveId = moveresult.Data.OrderId.ToStr();
                response.Code = ErrorCodes.Sys.SUCCESS;
                response.Message = "";
                response.Params["MoveResponse"] = moveresult;
                return response;
            }
            catch (Exception ee)
            {
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = ee.Message;
                response.Params["MoveResponse"] = "";
                return response;
            }
        }

        private StdArrivedRequestEntity? GetArrivedInfo(DefaultAgv InteractingDevice)
        {
            try
            {
                var toMoveTaskId = InteractingDevice.publicMoveId == "" ? "0" : InteractingDevice.publicMoveId;
                //InteractingDevice.logger.LogInformation("查询车辆信息online，ArrivedInfo:" + toMoveTaskId);

                if (toMoveTaskId == "0")
                {
                    return null;
                }

                var arrivedInfo = GetStdAgv(toMoveTaskId);
                if (arrivedInfo == null)
                {
                    //InteractingDevice.logger.LogDebug($"查询车辆信息online:{toMoveTaskId}，null");
                }
                return arrivedInfo;
            }
            catch (Exception ex)
            {
                InteractingDevice.logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_ArrivedInfo_Exception", "Exception", ex.Message);
                return null;
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
            }
        }

        public StdArrivedRequestEntity? GetStdAgv(string orderid)
        {
            StdArrivedRequestEntity? value;
            if (_memoryCache.TryGetValue(orderid, out value))
            {
                //  _logger.LogDebug($"GetStdAgv orderid: {orderid},StdArrivedRequestEntity: {JsonSerializer.Serialize(value)}");
            }

            return value;
        }

        /// <summary>
        /// todo, 斯坦德需要提供接口， 查询车辆实时信息
        /// </summary>
        /// <param name="InteractingDevice"></param>
        /// <returns></returns>
        private async Task<CarModel?> GetStdCarInfo(DefaultAgv InteractingDevice)
        {
            var carAllInfostr = InteractingDevice.DeviceDescriptor.Extra["CarAllInfo"].ToStr();
            string currentStation = string.Empty;
            var stdCarResponse = await InteractingDevice.HttpRequestInvoker.GetFromJsonAsync<StdCarResponse>(string.Format(carAllInfostr, InteractingDevice.DeviceId));
            var carModel = new CarModel();
            if (stdCarResponse != null && stdCarResponse.code == 0 && stdCarResponse.data != null)
            {
                var stdModel = stdCarResponse.data;

                carModel.CarType = stdModel.vehicleType;
                carModel.Battery = stdModel.power;
                carModel.IP = stdModel.ip;
                carModel.Name = stdModel.vehicleName;
                carModel.CurrentStation = stdModel.nowStation;
                carModel.Status = stdModel.agvStates;
                carModel.IseeConfigBattery = stdModel.power.ToString();
                carModel.CanDispatch = stdModel.dispatch;
                carModel.IsLowBattery = stdModel.power.ToDouble() > InteractingDevice.DeviceDescriptor.Extra["LowBattery"].ToDouble() ? false : true;
            }
            return carModel;
        }

        public async Task<DeviceServiceInvokeResponse> ReleaseLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var response = DeviceServiceInvokeResponse.SUCCESS;
            try
            {
                var request = new StdCompleteGroupRequest()
                {
                    GroupId = InteractingDevice.StdGroupNo
                };

                var requestPath = InteractingDevice.DeviceDescriptor.Extra["CompleteGroup"].ToStr();
                _logger.LogDebug($"斯坦德取消组标识接口请求: URL:{requestPath},参数:{request.ToJson()}");
                var result = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<StdCompleteGroupRequest, StdMoveResponse>(requestPath, request, _jsonSerializerOptions);
                _logger.LogDebug($"斯坦德取消组标识接口返回: {result.ToJson()}");

                if (result == null || result.Code != 0)
                {
                    response.Message = "斯坦德接口异常";
                    response.Code = ErrorCodes.Sys.FAIL;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"斯坦德取消组标识接口异常");
                response.Message = ex.Message;
                response.Code = ErrorCodes.Sys.FAIL;
            }

            return response;
        }
    }
}
