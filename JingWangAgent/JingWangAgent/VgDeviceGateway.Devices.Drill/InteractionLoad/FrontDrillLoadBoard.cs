using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Drill.InteractionLoad
{
    public class FrontDrillLoadBoard : DeviceShare<DefaultDrill>, IDrillLoadInteraction
    {
        private readonly ILogger<FrontDrillLoadBoard> logger;

        public FrontDrillLoadBoard(ILogger<FrontDrillLoadBoard> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public async Task<DeviceServiceInvokeResponse> CompleteLoadMaterialLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest, int position)
        {
            Interlocked.Exchange(ref InteractingDevice.AgvOnWorkDrillStatus, 1);
            InteractingDevice.IsLastStep = 0;
            if (deviceServiceInvokeRequest.Params.ContainsKey("IsLastStep") && deviceServiceInvokeRequest.Params["IsLastStep"].ToBool())
            {
                logger.LogDebug($"\r\n整个上下料最后一步 \r\n");
                Interlocked.Exchange(ref InteractingDevice.AgvOnWorkDrillStatus, 0);
                InteractingDevice.IsLastStep = 1;

                logger.LogDebug($"CompleteLoadMaterial 机器【{DeviceDescriptor.DeviceName}】  整个上下料动作结束");
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
            }
            else
            {
                var splindleInf = InteractingDevice.frontExtendDevice.ReadSingleSplineInf(position);

                if (splindleInf == null || !splindleInf.PanelState)
                {
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"轴{position} 板不存在 上料未完成");
                }

                if (deviceServiceInvokeRequest.Params != null && !deviceServiceInvokeRequest.Params.ContainsKey("LoadingPanel"))
                {
                    logger.LogDebug($"\r\n整个上下料最后一步 未包含 LoadingPanel 参数 \r\n");
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, "未包含LoadingPanel信息");
                }

                logger.LogDebug($"上生料获取的LoadingPanel的信息：{JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingPanel"])}");
                var operationEntity = JsonSerializer.Deserialize<SwapPanel>(JsonSerializer.Serialize(deviceServiceInvokeRequest.Params["LoadingPanel"])
                  , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                logger.LogDebug($"CompleteLoadMaterial 上料完成未修改 InteractingDevice.PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                logger.LogDebug($"上生料获取的板材信息：{JsonSerializer.Serialize(operationEntity)}");

                if (operationEntity != null && operationEntity.PanelList != null && operationEntity.PanelList.Count == 1)
                {
                    var panel = operationEntity.PanelList[0];
                    if (panel != null)
                    {
                        panel.Layer = 0;
                        //panel.DrillState = PanelDrillState.Undrilled;
                        panel.ProductStatus = ProductStatus.WaitingForDrill;
                    }
                    else
                    {
                        logger.LogDebug($"\r\n panel 为空  不更新PanelList {position - 1} \r\n");
                    }

                    //
                    await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                    {
                        InteractingDevice.PayloadPanels[position - 1] = panel;
                    }));

                    logger.LogDebug($"\r\n更新PanelList[ {position - 1}] 为{panel} \r\n");
                    logger.LogDebug($"CompleteLoadMaterial 上料完成修改后 InteractingDevice.PayloadPanels {JsonSerializer.Serialize(InteractingDevice.PayloadPanels)}");
                    return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
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
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"LoadingPanel PanelList参数 不正确, 不能更新PanelList[ {position - 1}]列表");
                }
            }
        }

        public async Task<DeviceServiceInvokeResponse> InvokeLoadMaterialLocal(int position)
        {
            Interlocked.Exchange(ref InteractingDevice.AgvOnWorkDrillStatus, 1);
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> PrepareLoadMaterialLocal(DeviceServiceInvokeRequest request, int position)
        {
            Interlocked.Exchange(ref InteractingDevice.AgvOnWorkDrillStatus, 1);

            var splindleInf = InteractingDevice.frontExtendDevice.ReadSingleSplineInf(position);

            if (splindleInf == null || splindleInf.PanelState || !splindleInf.UpDownState)
            {
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"轴{position} 有板{splindleInf.PanelState} 或者 顶升没升起来{!splindleInf.UpDownState} 不能执行上料");
            }

            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> CanExecuteDeviceServiceInvokeLocal(DeviceServiceInvokeRequest deviceServiceInvokeRequest,
                                                                                 DefaultDrill InteractingDevice,
                                                                                 string eventId = "",
                                                                                 string eventName = "",
                                                                                 string eventMessage = "",
                                                                                 string methodName = "")
        {
            if (InteractingDevice.MaterialEnsureFlag != 1)
            {
                return await Response(ErrorCodes.Sys.FAIL, "请先确认板材信息后再上下料");
            }
            if (!InteractingDevice.cnc84Command.CNCCommandStatus())
            {
                logger.LogDebug($"\r\nCNC连接断开\r\n");
                return await Response(ErrorCodes.Sys.FAIL, "CNC连接断开");
            }
            var onWork = (InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["WorkFlag"].ToInt()) == "1:1");
            if (onWork)
            {
                return await Response(ErrorCodes.Sys.FAIL, "钻机正在工作 不能执行上料");
            }
            await Task.Delay(100);
            var clampOpenFlag = (InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["ClampOpenFlag"].ToInt()) == "1:1");
            if (!clampOpenFlag)
            {
                return await Response(ErrorCodes.Sys.FAIL, "气夹没开 不能执行上料");
            }

            if (!InteractingDevice.frontExtendDevice.IsConnected)
            {
                logger.LogDebug($"\r\n外置设备断开\r\n");
                return await Response(ErrorCodes.Sys.FAIL, "外置设备断开");
            }
            if (!InteractingDevice.frontExtendDevice.DoorIsOpen())
            {
                logger.LogDebug($"\r\n准备上料过程中验证门未打开状态\r\n");
                return await Response(ErrorCodes.Sys.FAIL, "钻机门没开");
            }

            ushort position = 1;
            if (deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("Position"))
            {
                eventId = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT_NAME;
                eventMessage = $"{methodName} 传入的参数不包含位置信息";
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"{methodName} 传入的参数不包含位置信息");
            }

            position = deviceServiceInvokeRequest.Params["Position"].ToUshort();
            if ((!(position > 0 && position <= InteractingDevice.spindleNum)))
            {
                eventId = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_POSITION_FALT_EVENT_NAME;
                eventMessage = $"{methodName} 下发Position参数值{position}不在1和{InteractingDevice.spindleNum}之间，请下发正确的上料轴信息";
                logger.LogError(eventMessage);
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, eventMessage);
            }
            InteractingDevice.IsLastStep = 0;
            return await this.InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }
    }
}
