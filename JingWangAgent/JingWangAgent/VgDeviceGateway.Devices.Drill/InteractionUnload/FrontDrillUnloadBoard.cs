// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Drill.InteractionUnload
{
    public class FrontDrillUnloadBoard : DeviceShare<DefaultDrill>, IDrillUnloadInteraction
    {
        private readonly ILogger<FrontDrillUnloadBoard> logger;

        public FrontDrillUnloadBoard(ILogger<FrontDrillUnloadBoard> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public async Task<DeviceServiceInvokeResponse> CompleteUnloadMaterialLocal(DeviceServiceInvokeRequest request, int position)
        {
            //添加 islastStep
            ////无板子下料结束
            Interlocked.Exchange(ref InteractingDevice.AgvOnWorkDrillStatus, 1);
            if (request.Params.ContainsKey("IsLastStep") && request.Params["IsLastStep"].ToBool())
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

                if (splindleInf == null || splindleInf.PanelState)
                {
                    return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"轴{position} 板还在 下料未完成");
                }
                logger.LogDebug($"\r\n给UnloadingPanel赋值\r\n");
                request!.Params["UnloadingPanel"] = new SwapPanel() { SpindleId = position, PanelList = new List<Panel>() { InteractingDevice.PayloadPanels[position - 1] } };

                await PayloadPanels.ChangeListSafely(InteractingDevice.DeviceId, Task.Run(() =>
                {
                    InteractingDevice.PayloadPanels[position - 1] = Panel.HasSilo.NoPanelForSingleSpindle("", position, 0, 1)[0];
                }));
                Interlocked.Exchange(ref InteractingDevice.AgvOnWorkDrillStatus, 1);
                return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
            }
        }

        public async Task<DeviceServiceInvokeResponse> InvokeUnloadMaterialLocal(int position)
        {
            Interlocked.Exchange(ref InteractingDevice.AgvOnWorkDrillStatus, 1);
            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> PrepareUnloadMaterialLocal(DeviceServiceInvokeRequest request, int position)
        {
            Interlocked.Exchange(ref InteractingDevice.AgvOnWorkDrillStatus, 1);

            var splindleInf = InteractingDevice.frontExtendDevice.ReadSingleSplineInf(position);

            if (splindleInf == null || !splindleInf.PanelState || !splindleInf.UpDownState)
            {
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"轴{position} 板不存在 {!splindleInf.PanelState}  或者 顶升没升起来 {!splindleInf.UpDownState}  不能执行下料");
            }
            if (PayloadPanels[position - 1].ProductStatus != ProductStatus.Finished_DRILL)
            {
                return await InteractingDevice.Response(ErrorCodes.Sys.FAIL, $"轴{position} 板材信息不是熟料 不能执行下料");
            }

            return await InteractingDevice.Response(ErrorCodes.Sys.SUCCESS, JsonSerializer.Serialize(InteractingDevice.PayloadPanels), request.Params);
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
                return await Response(ErrorCodes.Sys.FAIL, "钻机正在工作 不能执行下料");
            }
            await Task.Delay(100);
            var clampOpenFlag = (InteractingDevice.cnc84Command.GetOutput(DeviceDescriptor.Extra["ClampOpenFlag"].ToInt()) == "1:1");
            if (!clampOpenFlag)
            {
                return await Response(ErrorCodes.Sys.FAIL, "气夹没开 不能执行下料");
            }

            if (!InteractingDevice.frontExtendDevice.IsConnected)
            {
                logger.LogDebug($"\r\n外置设备断开\r\n");
                return await Response(ErrorCodes.Sys.FAIL, "外置设备断开");
            }
            if (!InteractingDevice.frontExtendDevice.DoorIsOpen())
            {
                logger.LogDebug($"\r\n准备下料过程中验证门未打开状态\r\n");
                return await Response(ErrorCodes.Sys.FAIL, "钻机门没开");
            }

            ushort position = 1;
            if (deviceServiceInvokeRequest.Params == null || !deviceServiceInvokeRequest.Params.ContainsKey("Position"))
            {
                eventId = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT;
                eventName = Events.Drill.DRILL_PARAMETER_NOT_CONTAIN_POSITION_EVENT_NAME;
                eventMessage = $"{methodName} 传入的参数不包含位置信息";
                return await Response(ErrorCodes.Sys.FAIL, $"{methodName} 传入的参数不包含位置信息");
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
