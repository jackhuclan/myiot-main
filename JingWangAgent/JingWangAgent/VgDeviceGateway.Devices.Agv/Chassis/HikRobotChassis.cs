using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Targets;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;
using VgDeviceGateway.Devices.Common.Agv;
using VgDeviceGateway.Devices.Common.Agv.Hik;
using static VgDeviceGateway.Devices.Common.Agv.Hik.HikCarStatus;

namespace VgDeviceGateway.Devices.Agv.Chassis
{
    public class HikRobotChassis : IAgvChassis
    {
        public string getReqCode()
        {
            string tempReqCode = Guid.NewGuid().ToString();
            return tempReqCode;
        }

        public async Task<DefaultCallBackEntity> ArrivedInfoLocal(DefaultAgv InteractingDevice)
        {
            var arrivedinfo = await ArrivedInfo(InteractingDevice);
            if (arrivedinfo == null)
            {
                return new DefaultCallBackEntity();
            }
            var defaultentity = new DefaultCallBackEntity()
            {
                AGV_ID = arrivedinfo.robotCode,
                IsSuccess = arrivedinfo.method.ToLower() == "end" ? true : false,
                position = arrivedinfo.cooX + arrivedinfo.mapCode + arrivedinfo.cooY,
                X_Error = "0",
                Z_Error = "0",
                //Exception = arrivedinfo.Exception,
                Status = arrivedinfo.method,
                TaskId = arrivedinfo.taskCode
            };
            return defaultentity;
        }

        /// <summary>
        /// 小车信息查询
        /// </summary>
        /// <param name="InteractingDevice"></param>
        /// <returns></returns>
        private async Task<HikArrivedRequestEntity?> ArrivedInfo(DefaultAgv InteractingDevice)
        {
            try
            {
                var deviceOnline = InteractingDevice.DeviceDescriptor.Extra["DeviceOnline"].ToStr();
                DeviceListOptions? options = InteractingDevice.configuration.GetSection(DeviceListOptions.Options).Get<DeviceListOptions>();

                uint? port = InteractingDevice.configuration.GetValue<uint?>("port");
                if (!port.HasValue) port = 8004;
                deviceOnline = deviceOnline.Replace("8004", port.ToString());

                var toMoveTaskId = InteractingDevice.publicMoveId == "" ? "0" : InteractingDevice.publicMoveId;
                InteractingDevice.logger.LogInformation("查询车辆信息online，ArrivedInfo:" + toMoveTaskId);
                var hikarrivedInfo = await InteractingDevice.HttpRequestInvoker.GetFromJsonAsync<HikArrivedRequestEntity>(deviceOnline, new Dictionary<string, object> { { "taskId", toMoveTaskId } });
                if (hikarrivedInfo == null)
                {
                    InteractingDevice.logger.LogDebug($"查询车辆信息online:{toMoveTaskId}，null");
                }
                return hikarrivedInfo;
            }
            catch (Exception ex)
            {
                InteractingDevice.logger.LogDebug($"查询车辆信息online:异常啦:{ex.Message}");
                InteractingDevice.logger.LogError(ex, ex.Message);
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_ArrivedInfo_Exception", "Exception", ex.Message);
                return null;
            }
            finally
            {
                InteractingDevice.AddWatchingErrorInfo(InteractingDevice.errorInfo);
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
                var arrivedinfo = await ArrivedInfo(InteractingDevice);
                if (arrivedinfo == null)
                {
                    InteractingDevice.logger.LogDebug("Arrived ArrivedInfo arrivedinfo==null");
                    await Task.Delay(500);
                    continue;
                }

                var returnTaskId = moveid != "" ? moveid : InteractingDevice.WatchingProperties.Property("AgvReturnTaskId").NewValue.ToStr();

                var agvTaskId = arrivedinfo.taskCode;
                var IsArrived = deviceServiceInvokeRequest.Params["method"].ToString();
                var isSuccess = arrivedinfo.method.ToLower() == IsArrived ? true : false;
                InteractingDevice.logger.LogDebug($"判断是否到达  returnTaskId:{returnTaskId}，AgvTaskId：{agvTaskId}，IsSuccess：{isSuccess}");
                if (returnTaskId == agvTaskId && isSuccess)
                {
                    arrivedresult = true;
                    InteractingDevice.logger.LogDebug($"车辆到达 returnTaskId:{returnTaskId}，AgvTaskId：{agvTaskId}，X偏差值：{arrivedinfo.cooX.ToFloat() * 1000}，Y偏差值：{arrivedinfo.cooY.ToFloat() * 1000}");
                    InteractingDevice.WatchingProperties.Property("Offset_X").SetValue(0);
                    InteractingDevice.WatchingProperties.Property("Offset_Y").SetValue(0);//todo 待AGV补充
                    InteractingDevice.WatchingProperties.Property("Offset_Z").SetValue(0);
                    InteractingDevice.WatchingProperties.Property("AgvTaskId").SetValue(agvTaskId);
                    InteractingDevice.WatchingProperties.Property("IsArrived").SetValue(true);
                    InteractingDevice.WatchingProperties.Property("IsMoving").SetValue(false);
                    InteractingDevice.isMoving = false;
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
                var errmessage = $"等待车辆到达失败！_给PLC 5002点位,写值 999 报警无则反馈，超时{timeout},异常{isFinishedWork},拍二维码{InteractingDevice.AgvScanResult},Status=DeviceStatus.Exception,";
                InteractingDevice.logger.LogDebug(errmessage);
                return await InteractingDevice.Response(ErrorCodes.Sys.HTTPREQUEST_TIMEOUT_CODE, $"等待车辆到达超时{timeout},异常{isFinishedWork},拍二维码{InteractingDevice.AgvScanResult}");
            }
            if (arrivedresult)
            {
                InteractingDevice.AgvMoveArrivedTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
            }
            else
            {
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "未到达ArrivedLocal=false");
            }
        }

        public DateTime date = DateTime.Now;

        public async Task<bool> CanDispatchLocal(DefaultAgv InteractingDevice)
        {
            //var carInfo = await GetHikCarInfo(InteractingDevice);
            var carInfo = InteractingDevice.CarInfoExtra["CarInfoExtra"] as CarModel;
            if (carInfo == null)
            {
                return false;
            }
            var canDispatch = carInfo.CanDispatch.ToBool();
            if (canDispatch)
            {
                date = DateTime.Now;
                return true;
            }
            if (!canDispatch && (DateTime.Now - date).TotalSeconds < 60)
            {
                var msg = $"AGV CanDispatch属性FALSE,1分钟内持续保持TRUE";
                InteractingDevice.ReportingProcess(msg);
                InteractingDevice.logger.LogDebug(msg);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 查询小车状态
        /// </summary>
        /// <param name="InteractingDevice"></param>
        /// <returns></returns>
        private async Task<CarModel> GetHikCarInfo(DefaultAgv InteractingDevice)
        {
            var carAllInfostr = InteractingDevice.DeviceDescriptor.Extra["CarAllInfo"].ToStr();
            var mapCode = InteractingDevice.DeviceDescriptor.Extra["AGVMapId"].ToStr();
            var request = new HikCarState()
            {
                reqCode = $"Query{DateTime.Now.ToString("yyyyMMddHHmmss")}",//getReqCode(),
                reqTime = DateTime.Now.ToString("yyyy-MM-ddHH:mm:ss"),
                clientCode = "",
                tokenCode = "",
                mapCode = mapCode
            };
            //查询AGV状态
            var CarsInfo = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<HikCarState, HikCarStateRequest>(carAllInfostr, request);

            var carModel = new CarModel();
            if (CarsInfo != null)
            {
                //carModel = CarsInfo.Result?.FirstOrDefault(x => x.Name == InteractingDevice.DeviceName);
                if (CarsInfo.data.ToList().Select(t => t.robotCode == InteractingDevice.DeviceName).Count() > 0)
                {
                    var data = CarsInfo.data.Where(t => t.robotCode == InteractingDevice.DeviceName).OrderBy(t => t.robotCode).ToList().FirstOrDefault();
                    carModel.CarType = "";
                    carModel.Battery = data.battery.ToFloat();
                    carModel.IP = data.robotIp;
                    carModel.Name = data.robotCode;
                    string temppoint = data.posX.Length == 6 ? data.posX : "0" + data.posX;
                    carModel.CurrentStation = temppoint + data.mapCode + data.posY; //当前点位为 X轴 mapcode Y轴 拼接而成
                    //string status = GetDeviceStstus(data.status);
                    //carModel.Status = status;
                    carModel.Status = data.status;
                    carModel.IseeConfigBattery = data.battery;
                    carModel.CanDispatch = data.exclType == "0" ? true : false;
                    carModel.IsLowBattery = data.battery.ToDouble() > InteractingDevice.DeviceDescriptor.Extra["LowBattery"].ToDouble() ? false : true;
                }
            }
            return carModel;
        }

        private string GetDeviceStstus(string status)
        {
            string res = "";
            switch (status)
            {
                case "1":
                    res = "TaskCompleted"; break;
                case "2":
                    res = "Working"; break;
                case "4":
                    res = "IdleTask"; break;
                case "5":
                    res = "RobotStopped"; break;
                case "6":
                    res = "Working"; break;
                case "7":
                    res = "Charging"; break;
                default:
                    res = "小车异常";
                    break;
            }
            return res;
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
                    InteractingDevice.logger.LogDebug($"CheckIsArrivedLocal  arrivedinfo == null");
                    return false;
                }

                //var isSuccess = arrivedinfo.IsSuccess;
                var isSuccess = arrivedinfo.method.ToLower() == "end" ? true : false;
                if (returnTaskId == agvTaskId && isSuccess)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                InteractingDevice.logger.LogDebug($"CheckIsArrivedLocal");
            }

            return false;
        }

        //获取AGV信息
        public async Task<DeviceServiceInvokeResponse> GetAgvInfoLocal(DefaultAgv InteractingDevice)
        {
            var response = new DeviceServiceInvokeResponse();

            var carModel = await GetHikCarInfo(InteractingDevice);
            response.Code = ErrorCodes.Sys.SUCCESS;
            response.Message = "海康车辆信息";
            response.Params["CarInfoResponse"] = carModel;
            InteractingDevice.CarInfoExtra["CarInfoExtra"] = carModel;
            return response;
        }

        //是否低电量
        public async Task<bool> IsLowBatteryLocal(DefaultAgv InteractingDevice)
        {
            var carInfo = InteractingDevice.CarInfoExtra["CarInfoExtra"] as CarModel;
            if (carInfo == null)
            {
                return false;
            }
            var configLowBattery = InteractingDevice.DeviceDescriptor.Extra["LowBattery"].ToFloat();
            var battery = carInfo.Battery.ToFloat();
            if (battery < configLowBattery)
            {
                InteractingDevice.logger.LogDebug($"CheckIsArrivedLocal  arrivedinfo == null");
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

            var carInfo = await GetHikCarInfo(InteractingDevice);
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

            if (InteractingDevice.modbusIpMaster == null)
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_modbusIpMasterNull", "modbusIpMasterNull", "modbusIpMasterNull");
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "modbusIpMasterNull");
            }

            var checkMovePoint = deviceServiceInvokeRequest.Params["CheckMovePoint"].ToInt();
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

            if (carInfo.CurrentStation.ToStr().Equals(agvTargetPos))
            {
                InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_OnCurrentStation", "OnCurrentStation", "车辆已经在当前位置");
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "已经处于目标位置");
            }
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, "");
        }

        public async Task<DeviceServiceInvokeResponse> MoveLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var response = new DeviceServiceInvokeResponse();
            try
            {
                var count = deviceServiceInvokeRequest?.Params["count"]!.ToString();

                var movecheck = await MoveCheckLocal(InteractingDevice, deviceServiceInvokeRequest!);
                if (movecheck.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_MoveCheckError", "MoveCheckError", movecheck.Message);
                    return await InteractingDevice.Response(movecheck.Code, movecheck.Message);
                }

                var agvTargetPos = deviceServiceInvokeRequest?.Params["MoveTargetPos"]!.ToString();
                InteractingDevice.logger.LogDebug("Move : TargetStation : " + agvTargetPos);

                var whichAgv = InteractingDevice.DeviceDescriptor.DeviceName;

                bool unlock = deviceServiceInvokeRequest!.Params["Unlock"].ToBool();
                if (unlock)
                {
                    //return await UnlockChassisForTask(InteractingDevice, deviceServiceInvokeRequest);
                    return await ReleaseAgvForInterface(InteractingDevice, deviceServiceInvokeRequest);
                }

                if (count == "0")
                {
                    response = await ExecuteMoveTask(InteractingDevice, deviceServiceInvokeRequest, false);
                }
                else
                {
                    response = await ExecuteMoveTask(InteractingDevice, deviceServiceInvokeRequest, true);
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = ex.Message;
                response.Params["MoveResponse"] = "";
                return response;
            }
        }

        /// <summary>
        /// 解锁小车
        /// </summary>
        /// <param name="InteractingDevice"></param>
        /// <param name="deviceServiceInvokeRequest"></param>
        /// <returns></returns>
        private async Task<DeviceServiceInvokeResponse> UnlockChassisForTask(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var response = new DeviceServiceInvokeResponse();

            var agvTargetPos = deviceServiceInvokeRequest?.Params["MoveTargetPos"]!.ToString();

            var reqCode = deviceServiceInvokeRequest?.Params["reqCode"]!.ToString();

            var taskCode = deviceServiceInvokeRequest?.Params["taskCode"]!.ToString();

            var whichAgv = InteractingDevice.DeviceDescriptor.DeviceName;

            string taskTypes = InteractingDevice.DeviceDescriptor.Extra["unLockAgv"].ToStr();

            object[] temp = new object[]
            {
                new HikLocation{ positionCode = agvTargetPos, type = "00"},
                new HikLocation{ positionCode = agvTargetPos, type = "00"}
            };

            var request = new HikTaskRequest()
            {
                reqCode = reqCode,
                reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                clientCode = "",
                tokenCode = "",
                //taskTyp = "DPCD",
                taskTyp = taskTypes,
                ctnrTyp = "",
                ctnrCode = "",
                ctnrNum = "",
                taskMode = "",
                wbCode = "",
                positionCodePath = temp,
                podCode = "",
                podDir = "",
                podTyp = "",
                materialLot = "",
                materialType = "",
                priority = "",
                taskCode = taskCode,
                agvCode = whichAgv,
                groupId = "",
                agvTyp = "",
                positionSelStrategy = "",
                data = ""
            };

            var moveRequestPath = InteractingDevice.DeviceDescriptor.Extra["AGVMoveStart"].ToStr();
            InteractingDevice.logger.LogDebug($"Agv执行点到点任务或解锁任务接口参数 ：{JsonSerializer.Serialize(request)}");

            var moveresult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<HikTaskRequest, HikTaskResponse>(moveRequestPath, request);
            InteractingDevice.logger.LogDebug($"Agv部署点到点任务或解锁任务返回消息：{JsonSerializer.Serialize(moveresult)}");

            if (moveresult?.code != "0")
            {
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = "Agv点到点部署任务或解锁任务失败";
                response.Params["MoveResponse"] = moveresult;

                InteractingDevice.logger.LogDebug($"调用接口{moveRequestPath}失败, 返回消息: {moveresult.Message}");
                return response;
            }

            InteractingDevice.logger.LogDebug($"Agv点到点部署任务或解锁任务成功, taskId：{moveresult.data}");
            InteractingDevice.publicMoveId = moveresult.data;
            response.Code = ErrorCodes.Sys.SUCCESS;
            response.Message = "";
            response.Params["MoveResponse"] = moveresult;

            return response;
        }

        /// <summary>
        /// 小车执行上下料任务
        /// </summary>
        /// <param name="InteractingDevice"></param>
        /// <param name="deviceServiceInvokeRequest"></param>
        /// <param name="isContinue"></param>
        /// <returns></returns>
        private async Task<DeviceServiceInvokeResponse> ExecuteMoveTask(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest, bool isContinue)
        {
            var response = new DeviceServiceInvokeResponse();

            var reqCode = deviceServiceInvokeRequest?.Params["reqCode"]!.ToString();

            var agvTargetPos = deviceServiceInvokeRequest?.Params["MoveTargetPos"]!.ToString();

            var taskCode = deviceServiceInvokeRequest?.Params["taskCode"]!.ToString();

            string[] taskTypes = InteractingDevice.DeviceDescriptor.Extra["taskType"].ToStr().Split(',');
            var whichAgv = InteractingDevice.DeviceDescriptor.DeviceName;

            if (!isContinue)
            {
                var AllTargetPos = deviceServiceInvokeRequest.Params["AllTargetPos"] as List<string>;
                object[] temp = new object[AllTargetPos.Count() + 1];
                temp[0] = new HikLocation { positionCode = AllTargetPos[0], type = "00" };
                for (int i = 0; i < AllTargetPos.Count(); i++)
                {
                    temp[i + 1] = new HikLocation { positionCode = AllTargetPos[i].ToString(), type = "00" };
                }

                string taskType = string.Empty;
                int type = 0;

                if (InteractingDevice.materialType == MaterialKind.PanelSilo)
                {
                    string[] taskTypeTemp = InteractingDevice.DeviceDescriptor.Extra["HikTransferTaskType"].ToStr().Split(',');
                    var siloCode = InteractingDevice.PayloadPanels.SiloCode;

                    if (string.IsNullOrEmpty(siloCode) || siloCode == InteractingDevice.DeviceId)
                    {
                        //D08
                        taskType = taskTypeTemp[0];
                        InteractingDevice.logger.LogDebug($"Agv继续执行 取料仓的任务的taskType {taskType}：{siloCode}");
                    }
                    else
                    {
                        //D081
                        taskType = taskTypeTemp[1];
                        InteractingDevice.logger.LogDebug($"Agv继续执行 放料仓的任务的taskType {taskType}：{siloCode}");
                    }
                    InteractingDevice.ReportingProcess($"Agv继续执行 取料仓的任务的taskType {taskType}：{siloCode}");
                    //type = 0;
                }
                else
                {
                    //taskType = GetTaskType(AllTargetPos.Count());
                    type = AllTargetPos.Count();
                    taskType = GetTaskType(type, taskTypes);
                    if (taskType.Contains("taskType") || string.IsNullOrEmpty(taskType))
                    {
                        response.Code = ErrorCodes.Sys.FAIL;
                        response.Message = $"不存在任务模版:{taskType}";
                        return response;
                    }
                }

                var request = new HikTaskRequest()
                {
                    reqCode = reqCode,
                    reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    clientCode = "",
                    tokenCode = "",
                    taskTyp = taskType,
                    ctnrTyp = "",
                    ctnrCode = "",
                    ctnrNum = "",
                    taskMode = "",
                    wbCode = "",
                    positionCodePath = temp,
                    podCode = "",
                    podDir = "",
                    podTyp = "",
                    materialLot = "",
                    materialType = "",
                    priority = "",
                    taskCode = taskCode,
                    agvCode = whichAgv,
                    groupId = "",
                    agvTyp = "",
                    positionSelStrategy = "",
                    data = ""
                };

                //获取AGV下任务接口 genAgvSchedulingTask
                var moveRequestPath = InteractingDevice.DeviceDescriptor.Extra["AGVMoveStart"].ToStr();
                InteractingDevice.logger.LogDebug($"Agv执行任务接口参数 ：{JsonSerializer.Serialize(request)}");

                //下发任务
                var moveresult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<HikTaskRequest, HikTaskResponse>(moveRequestPath, request);
                InteractingDevice.logger.LogDebug($"Agv部署任务返回消息：{JsonSerializer.Serialize(moveresult)}");

                if (moveresult?.code != "0")
                {
                    response.Code = ErrorCodes.Sys.FAIL;
                    response.Message = $"Agv部署任务失败_{moveresult.Message}";
                    response.Params["MoveResponse"] = moveresult;

                    InteractingDevice.logger.LogDebug($"调用接口{moveRequestPath}失败, 返回信息: {moveresult.Message}");
                    return response;
                }
                InteractingDevice.AgvTaskId = request.reqCode;
                InteractingDevice.AgvMoveStartTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                InteractingDevice.logger.LogDebug($"Agv任务taskkCode：{moveresult.data}");
                InteractingDevice.publicMoveId = moveresult.data;
                response.Code = ErrorCodes.Sys.SUCCESS;
                response.Message = "";
                response.Params["MoveResponse"] = moveresult;
            }
            else
            {
                HikLocation hik = new HikLocation();
                hik.positionCode = agvTargetPos;
                hik.type = "00";

                var drillSpindleId = deviceServiceInvokeRequest?.Params["SiloPosition"]!.ToString();

                HikContinueTask request = new HikContinueTask()
                {
                    reqCode = reqCode,
                    reqTime = "",
                    clientCode = "",
                    tokenCode = "",
                    wbCode = "",
                    podCode = "",
                    agvCode = whichAgv,
                    taskCode = taskCode,
                    taskSeq = "",
                    nextPositionCode = hik
                };

                //给AGV下继续任务 ContinueTask
                var moveRequestPath = InteractingDevice.DeviceDescriptor.Extra["HikContinueTask"].ToStr();
                InteractingDevice.logger.LogDebug($"Agv继续执行任务接口参数 ：{JsonSerializer.Serialize(request)}");

                var moveresult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<HikContinueTask, HikCancelOrContinueReq>(moveRequestPath, request);
                InteractingDevice.logger.LogDebug($"Agv继续执行任务接口 ：{JsonSerializer.Serialize(moveresult)}");

                if (moveresult?.Code != "0")
                {
                    response.Code = ErrorCodes.Sys.FAIL;
                    response.Message = $"Agv继续执行任务失败:{moveresult.message}";
                    response.Params["MoveResponse"] = moveresult;
                    InteractingDevice.logger.LogDebug($"调用接口 {moveRequestPath}失败, 返回消息: {moveresult.message}");
                    return response;
                }

                InteractingDevice.logger.LogDebug($"Agv继续执行任务成功, taskID：{taskCode}");
                InteractingDevice.AgvTaskId = request.reqCode;
                InteractingDevice.AgvMoveStartTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                InteractingDevice.publicMoveId = taskCode;
                response.Code = ErrorCodes.Sys.SUCCESS;
                response.Message = "";
                response.Params["MoveResponse"] = moveresult;
            }

            return response;
        }

        public string GetTaskType(int count, string[] taskTypes)
        {
            string type = string.Empty;
            if (count > (taskTypes.Length + 1))
            {
                type = "配置文件的配置项 taskType 配置的模板数量或者内容错误";
                return type;
            }

            switch (count)
            {
                case 0:
                    type = taskTypes[0]; break;
                case 1:
                    type = taskTypes[1]; break;
                case 2:
                    type = taskTypes[2]; break;
                case 3:
                    type = taskTypes[3]; break;
                case 4:
                    type = taskTypes[4]; break;
                case 5:
                    type = taskTypes[5]; break;
                case 6:
                    type = taskTypes[6]; break;
                default:
                    type = string.Empty;
                    break;
            }
            return type;
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="InteractingDevice"></param>
        /// <param name="deviceServiceInvokeRequest"></param>
        /// <returns></returns>
        public async Task<DeviceServiceInvokeResponse> CancelLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var taskCode = deviceServiceInvokeRequest.Params["taskCode"].ToString();
            var response = new DeviceServiceInvokeResponse();
            var agvid = InteractingDevice.DeviceDescriptor.DeviceId;
            var request = new HikCancelTask()
            {
                reqCode = $"cancel{DateTime.Now.ToString("yyyyMMddHHmmss")}",  //getReqCode(),
                reqTime = "",
                clientCode = "",
                tokenCode = "",
                forceCancel = "",
                matterArea = "",
                agvCode = agvid,
                taskCode = ""
            };
            var moveRequestPath = InteractingDevice.DeviceDescriptor.Extra["HikCancelTask"].ToStr();
            InteractingDevice.logger.LogDebug($"Agv任务取消请求{taskCode} ：{JsonSerializer.Serialize(request)}");
            var moveresult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<HikCancelTask, HikCancelOrContinueReq>(moveRequestPath, request);
            InteractingDevice.logger.LogDebug($"Agv任务取消结果{taskCode} ：moveresult：{JsonSerializer.Serialize(moveresult)}");
            if (moveresult?.Code != "0")
            {
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = $"Agv取消任务失败：{moveresult?.message}";
                response.Params["MoveResponse"] = moveresult;
                return response;
            }
            response.Code = ErrorCodes.Sys.SUCCESS;
            return response;
        }

        public async Task<DeviceServiceInvokeResponse> MoveOperationLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest, int operationType)
        {
            var response = new DeviceServiceInvokeResponse();
            try
            {
                var count = deviceServiceInvokeRequest?.Params["count"]!.ToString();
                var taskCode = deviceServiceInvokeRequest?.Params["taskCode"]!.ToString();
                var movecheck = await MoveCheckLocal(InteractingDevice, deviceServiceInvokeRequest!);
                if (movecheck.Code != ErrorCodes.Sys.SUCCESS)
                {
                    InteractingDevice.errorInfo = new Tuple<string, string, string>("AGV_Move_MoveCheckError", "MoveCheckError", movecheck.Message);
                    return await InteractingDevice.Response(movecheck.Code, movecheck.Message);
                }
                var agvTargetPos = deviceServiceInvokeRequest?.Params["MoveTargetPos"]!.ToString();
                var reqCode = deviceServiceInvokeRequest?.Params["reqCode"]!.ToString();

                InteractingDevice.logger.LogDebug("Move : TargetStation : " + agvTargetPos);
                var whichAgv = InteractingDevice.DeviceDescriptor.DeviceName;

                object[] temp = new object[]
                {
                    new HikLocation{ positionCode = agvTargetPos, type = "00"},
                    new HikLocation{ positionCode = agvTargetPos, type = "00"}
                };

                var request = new HikTaskRequest()
                {
                    reqCode = reqCode,
                    reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    clientCode = "",
                    tokenCode = "",
                    taskTyp = "D08",
                    ctnrTyp = "",
                    ctnrCode = "",
                    ctnrNum = "",
                    taskMode = "",
                    wbCode = "",
                    positionCodePath = temp,
                    podCode = "",
                    podDir = "",
                    podTyp = "",
                    materialLot = "",
                    materialType = "",
                    priority = "",
                    taskCode = taskCode,
                    agvCode = whichAgv,
                    groupId = "",
                    agvTyp = "",
                    positionSelStrategy = "",
                    data = ""
                };

                var moveRequestPath = InteractingDevice.DeviceDescriptor.Extra["AGVMoveStart"].ToStr();
                InteractingDevice.logger.LogDebug($"Agv执行点到点任务参数 ：{JsonSerializer.Serialize(request)}");

                var moveresult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<HikTaskRequest, HikTaskResponse>(moveRequestPath, request);
                InteractingDevice.logger.LogDebug($"Agv部署点到点任务返回消息：{JsonSerializer.Serialize(moveresult)}");

                if (moveresult?.code != "0")
                {
                    response.Code = ErrorCodes.Sys.FAIL;
                    response.Message = $"Agv点到点部署任务失败:{moveresult?.Message}";
                    response.Params["MoveResponse"] = moveresult;

                    InteractingDevice.logger.LogDebug($"调用接口 {moveRequestPath}失败：{moveresult?.Message}");
                    return response;
                }

                InteractingDevice.logger.LogDebug($"Agv点到点部署任务, taskId：{moveresult.data}");
                InteractingDevice.publicMoveId = moveresult?.data;
                response.Code = ErrorCodes.Sys.SUCCESS;
                response.Message = "";
                response.Params["MoveResponse"] = moveresult;

                return response;
            }
            catch (Exception ex)
            {
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = ex.Message;
                response.Params["MoveResponse"] = "";
                return response;
            }
        }

        /// <summary>
        /// 接口释放AGV
        /// </summary>
        /// <param name="InteractingDevice"></param>
        /// <param name="deviceServiceInvokeRequest"></param>
        /// <returns></returns>
        private async Task<DeviceServiceInvokeResponse> ReleaseAgvForInterface(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var response = new DeviceServiceInvokeResponse();
            if (!InteractingDevice.DeviceDescriptor.Extra.ContainsKey("UnlockHttp"))
            {
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = "UnlockAgvForInterface 请检查配置 UnlockHttp";
                InteractingDevice.logger.LogDebug("UnlockAgvForInterface 请检查配置 UnlockHttp");
                await InteractingDevice.ReportingProcess("UnlockAgvForInterface 请检查配置 UnlockHttp");
                response.Params["MoveResponse"] = response;
                return response;
            }
            var moveRequestPath = InteractingDevice.DeviceDescriptor.Extra["UnlockHttp"].ToStr();
            var deviceId = InteractingDevice.DeviceDescriptor.DeviceId;

            var carModel = await GetHikCarInfo(InteractingDevice);


            if (carModel.Status == "2" || carModel.Status == "7")
            {
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = $"UnlockAgvForInterface 非空闲状态不能释放：{carModel.Status} ";
                response.Params["MoveResponse"] = response;
                return response;
            }
            else
            {
                var request = new HikUnlockRequestEntity()
                {
                    reqCode = "UnlockI_" + deviceId + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    robotCode = deviceId
                };

                var moveresult = await InteractingDevice.HttpRequestInvoker.PostAsJsonAsync<HikUnlockRequestEntity, HikUnlockResponseEntity>(moveRequestPath, request);
                InteractingDevice.logger.LogDebug($"Agv释放接口返回消息：{JsonSerializer.Serialize(moveresult)}");
                if (moveresult?.code == "0")
                {
                    response.Code = ErrorCodes.Sys.SUCCESS;
                    response.Message = "";
                    response.Params["MoveResponse"] = moveresult;
                    return response;
                }
                else
                {
                    response.Code = ErrorCodes.Sys.FAIL;
                    response.Message = $"UnlockAgvForInterface 解锁失败{moveresult?.message}";
                    response.Params["MoveResponse"] = response;
                    return response;
                }
            }

            //response.Code = ErrorCodes.Sys.FAIL;
            //response.Message = $"UnlockAgvForInterface 非空闲状态不能释放：{carModel.Status} ";
            //response.Params["MoveResponse"] = response;
            //return response;
        }

        /// <summary>
        /// 释放AGV
        /// </summary>
        /// <param name="InteractingDevice"></param>
        /// <param name="deviceServiceInvokeRequest"></param>
        /// <returns></returns>
        public async Task<DeviceServiceInvokeResponse> ReleaseLocal(DefaultAgv InteractingDevice, DeviceServiceInvokeRequest deviceServiceInvokeRequest)
        {
            var response = await ReleaseAgvForInterface(InteractingDevice, deviceServiceInvokeRequest);

            return response;
        }
    }
}
