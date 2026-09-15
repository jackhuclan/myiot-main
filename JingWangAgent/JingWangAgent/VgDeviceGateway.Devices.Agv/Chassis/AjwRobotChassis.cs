using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Common.Agv;

namespace VgDeviceGateway.Devices.Agv.Chassis
{
    public class AjwRobotChassis : IAgvChassis
    {
        public async Task<AjwCallBackEntity?> ArrivedInfo(DefaultAgv InteractingDevice)
        {
            try
            {
                var deviceOnline = InteractingDevice.DeviceDescriptor.Extra["DeviceOnline"].ToStr();
                uint? port = InteractingDevice.configuration.GetValue<uint?>("port");
                if (!port.HasValue) port = 8004;
                deviceOnline = deviceOnline.Replace("8004", port.ToString());

                var toMoveTaskId = InteractingDevice.publicMoveId == "" ? "0" : InteractingDevice.publicMoveId;
                InteractingDevice.logger.LogInformation("查询车辆信息online，ArrivedInfo:" + toMoveTaskId);
                var arrivedInfo = await InteractingDevice.HttpRequestInvoker.GetFromJsonAsync<AjwCallBackEntity>(deviceOnline, new Dictionary<string, object> { { "taskId", toMoveTaskId } });
                if (arrivedInfo == null)
                {
                    InteractingDevice.logger.LogDebug($"查询车辆信息online:{toMoveTaskId}，null");
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

        public async Task<DefaultCallBackEntity> ArrivedInfoLocal(DefaultAgv InteractingDevice)
        {
            try
            {
                var arrivedinfo = await ArrivedInfo(InteractingDevice);
                if (arrivedinfo == null)
                {
                    return new DefaultCallBackEntity();
                }
                var defaultentity = new DefaultCallBackEntity()
                {
                    AGV_ID = arrivedinfo.AGV_ID,
                    IsSuccess = arrivedinfo.IsSuccess,
                    position = arrivedinfo.position,
                    X_Error = arrivedinfo.X_Error,
                    Z_Error = arrivedinfo.Z_Error,
                    Exception = arrivedinfo.Exception,
                    Status = arrivedinfo.Status,
                    TaskId = arrivedinfo.TaskId
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
                var arrivedinfo = await ArrivedInfo(InteractingDevice);
                if (arrivedinfo == null)
                {
                    InteractingDevice.logger.LogDebug("Arrived ArrivedInfo arrivedinfo==null");
                    break;
                }

                var returnTaskId = moveid != "" ? moveid : InteractingDevice.WatchingProperties.Property("AgvReturnTaskId").NewValue.ToStr();
                var agvTaskId = arrivedinfo.TaskId;
                var isSuccess = arrivedinfo.IsSuccess;
                InteractingDevice.logger.LogDebug($"判断是否到达  returnTaskId:{returnTaskId}，AgvTaskId：{agvTaskId}，IsSuccess：{isSuccess}");
                if (returnTaskId == agvTaskId && isSuccess)
                {
                    arrivedresult = true;
                    InteractingDevice.logger.LogDebug($"车辆到达 returnTaskId:{returnTaskId}，AgvTaskId：{agvTaskId}，X偏差值：{arrivedinfo.X_Error.ToFloat() * 1000}，Y偏差值：{arrivedinfo.Z_Error.ToFloat() * 1000}");
                    InteractingDevice.WatchingProperties.Property("Offset_X").SetValue((arrivedinfo.X_Error.ToFloat() * 1000).ToString("0.00"));
                    InteractingDevice.WatchingProperties.Property("Offset_Y").SetValue(0);//todo 待AGV补充
                    InteractingDevice.WatchingProperties.Property("Offset_Z").SetValue((arrivedinfo.Z_Error.ToFloat() * 1000).ToString("0.00"));
                    InteractingDevice.WatchingProperties.Property("AgvTaskId").SetValue(agvTaskId);
                    InteractingDevice.WatchingProperties.Property("IsArrived").SetValue(true);
                    InteractingDevice.WatchingProperties.Property("IsMoving").SetValue(false);
                    InteractingDevice.isMoving = false;
                    InteractingDevice.deviationValue = arrivedinfo.X_Error.ToFloat() * 1000;
                    break;
                }
                if (!InteractingDevice.AgvScanResult)
                {
                    InteractingDevice.Status = DeviceStatus.Exception;
                    InteractingDevice.logger.LogDebug($"判断是否到达 AgvScanResult:{InteractingDevice.AgvScanResult}");
                    break;
                }
                timeout = InteractingDevice.IsTimeout(startTime, InteractingDevice.moveTimeout);
                isFinishedWork = InteractingDevice.CanProceedNextStep();
                if (timeout || isFinishedWork)
                {
                    InteractingDevice.Status = DeviceStatus.Exception;

                    InteractingDevice.logger.LogDebug($"判断车辆到达方法:timeout：{timeout}，isFinishedWork：{isFinishedWork}");
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
                return await InteractingDevice.Response( ErrorCodes.Sys.SUCCESS, "");
            }
            else {
                return await InteractingDevice.Response( ErrorCodes.Sys.FAIL, "未到达ArrivedLocal=false");
            }
        }

        public async Task<DeviceServiceInvokeResponse> CancelLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var response = new DeviceServiceInvokeResponse();
            response.Code = ErrorCodes.Sys.FAIL;
            response.Message = $"无此逻辑请确认";

            return response;
        }

        public async Task<bool> CanDispatchLocal(DefaultAgv InteractingDevice)
        {
            try
            {
                //var carInfo = await GetAjwCarInfo(InteractingDevice);
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
                return false;
            }
            catch (Exception)
            {
                return false;
            }
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
                var arrivedinfo = await ArrivedInfo(InteractingDevice);
                if (arrivedinfo == null)
                {
                    InteractingDevice.logger.LogDebug($"AJW_CheckIsArrivedLocal  arrivedinfo == null");
                    return false;
                }
                var isSuccess = arrivedinfo.IsSuccess;
                if (returnTaskId == agvTaskId && isSuccess)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                InteractingDevice.logger.LogDebug($"AJW_CheckIsArrivedLocal");
            }

            return false;
        }

        public async Task<DeviceServiceInvokeResponse> GetAgvInfoLocal(DefaultAgv InteractingDevice)
        {
            var response = new DeviceServiceInvokeResponse();

            var carModel = await GetAjwCarInfo(InteractingDevice);
            response.Code = ErrorCodes.Sys.SUCCESS;
            response.Message = "艾吉威车辆信息";
            response.Params["CarInfoResponse"] = carModel;
            InteractingDevice.CarInfoExtra["CarInfoExtra"] = carModel;
            return response;
        }

        public async Task<CarModel> GetAjwCarInfo(DefaultAgv InteractingDevice)
        {
            var carAllInfostr = InteractingDevice.DeviceDescriptor.Extra["CarAllInfo"].ToStr();
            var CarsInfo = await InteractingDevice.HttpRequestInvoker.GetFromJsonAsync<CarState>(carAllInfostr);
            var carModel = new CarModel();
            if (CarsInfo != null)
            {
                carModel = CarsInfo.Result?.FirstOrDefault(x => x.Name == InteractingDevice.DeviceName);
            }
            return carModel == null ? new CarModel() : carModel;
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
            var battery = carInfo.Battery.ToFloat();
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

            var carInfo = await GetAjwCarInfo(InteractingDevice);
            if (string.IsNullOrEmpty(carInfo.Name))
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_NoCarInfo", "NoCarInfo", "未查询到车辆信息");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "获取不到AGV车辆信息:" + carInfo.Name);
            }

            if (!deviceServiceInvokeRequest.Params.ContainsKey("MoveTargetPos"))
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_NoKey", "NoKey", "未找到参数：MoveTargetPos");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "未找到参数：MoveTargetPos");
            }

            var agvTargetPos = deviceServiceInvokeRequest.Params["MoveTargetPos"].ToStr();
            InteractingDevice.logger.LogInformation("Move : CurrentStation" + carInfo.CurrentStation.ToStr() + ",   TargetStation" + agvTargetPos);

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
                    //var checkMovePoint = deviceServiceInvokeRequest.Params["CheckMovePoint"].ToInt();
                    var carCanMoveAndFinished = InteractingDevice.modbusIpMaster.ReadHoldingRegisters(InteractingDevice.slaveId, (ushort)checkMovePoint, 1);

                    if (carCanMoveAndFinished == null)
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_ReadPlcError", "ReadPlcError", "ReadPlcError");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "ReadPlcError");
                    }
                    if (carCanMoveAndFinished[0] != 1)
                    {
                        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_carCanMoveAndFinishedError", "carCanMoveAndFinishedError", $"carCanMoveAndFinishedError:{carCanMoveAndFinished[0]}");
                        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"小车结构不在原点 {checkMovePoint}:{carCanMoveAndFinished[0]}");
                    }
                }
            }
            //var item = JsonSerializer.Deserialize<SwapPanel>(deviceServiceInvokeRequest.Params["OperationListItem"].ToStr());
            //if (item != null)
            //{
            //    if (InteractingDevice.DeviceDescriptor.DeviceKind.ToStr() == "ShelfSiloAgv" && item.InteractionSequence == InteractionSequence.LoadOnly && carInfo.CurrentStation.ToStr().Equals(agvTargetPos))
            //    {
            //        InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_OnCurrentStation", "OnCurrentStation", "该设备无法原地提升：请移到其他点位开始任务");
            //        return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "该设备无法原地提升：请移到其他点位开始任务");
            //    }
            //}

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

                var request = new TaskRequest()
                {
                    FromStation = agvTargetPos,
                    ToStation = agvTargetPos,
                    Type = "WJDefault",
                    Priority = 0,
                    Rest_Station = agvTargetPos,
                    ExpectCar = whichAgv,
                    ThirdPartyOrder = ""
                };

                var moveRequestPath = InteractingDevice.DeviceDescriptor.Extra["AGVMoveStart"].ToStr();
                var moveresult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<TaskRequest, TaskResponse>(moveRequestPath, request);
                InteractingDevice.logger.LogDebug($"Agv移动结果：{JsonSerializer.Serialize(moveresult)}");
                if (moveresult?.ErrorCode != 0)
                {
                    response.Code = ErrorCodes.Sys.FAIL;
                    response.Message = "移动失败";
                    response.Params["MoveResponse"] = moveresult;
                    return response;
                }
                InteractingDevice.logger.LogDebug($"Agv移动结果,移动ID：{moveresult.Result.TaskId}");
                InteractingDevice.publicMoveId = moveresult.Result.TaskId;
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

                var request = new TaskRequest()
                {
                    FromStation = agvTargetPos,
                    ToStation = agvTargetPos,
                    Type = "WJDefault",
                    Priority = 0,
                    Rest_Station = agvTargetPos,
                    ExpectCar = whichAgv,
                    ThirdPartyOrder = "",
                    ActParam = JsonSerializer.Serialize(listParams)
                };

                var moveRequestPath = InteractingDevice.DeviceDescriptor.Extra["AGVMoveStart"].ToStr();
                var moveresult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<TaskRequest, TaskResponse>(moveRequestPath, request);
                InteractingDevice.logger.LogDebug($"Agv移动结果：{JsonSerializer.Serialize(moveresult)}");
                if (moveresult?.ErrorCode != 0)
                {
                    response.Code = ErrorCodes.Sys.FAIL;
                    response.Message = "移动失败";
                    response.Params["MoveResponse"] = moveresult;
                    return response;
                }
                InteractingDevice.logger.LogDebug($"Agv移动结果,移动ID：{moveresult.Result.TaskId}");
                InteractingDevice.publicMoveId = moveresult.Result.TaskId;
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

        public async Task<DeviceServiceInvokeResponse> ReleaseLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var response = new DeviceServiceInvokeResponse();
            response.Code = ErrorCodes.Sys.FAIL;
            response.Message = $"无此逻辑请确认";

            return response;
        }
    }
}
