// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.DrillDevice
{
    public class FrontPanelDrillDevice : DeviceShare<DefaultDrill>, IDrillDevice
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<FrontPanelDrillDevice> logger;

        public FrontPanelDrillDevice(ILoggerFactory loggerFactory, ILogger<FrontPanelDrillDevice> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.loggerFactory = loggerFactory;
            this.logger = logger;
            InteractingDevice.bufferAllUnloadAndLoadEnd = 1;
        }

        public bool ConnectToCncAndOtherDevice()
        {
            Cnc84Connect();
            var serialFlag = SerialConnect();
            var cnc84Flag = InteractingDevice.cnc84Command != null && InteractingDevice.cnc84Command.CNCCommandStatus();
            return cnc84Flag && serialFlag;
        }

        public void CompleteScheduleLocal(ScheduledTaskStatus ScheduledStatus)
        {
            logger.LogDebug($"CompleteScheduleLocal 方法被调用了 调度状态 {ScheduledStatus}：重置{InteractingDevice.allowAllAgv}");
            InteractingDevice.bufferAllUnloadAndLoadEnd = 1;
            InteractingDevice.allowAllAgv = true;
            InteractingDevice.TransactionId = string.Empty;
        }

        public void FailScheduleLocal()
        {
            logger.LogDebug($"FailScheduleLocal 方法被调用了：重置{InteractingDevice.allowAllAgv}");
            InteractingDevice.bufferAllUnloadAndLoadEnd = 0;
            InteractingDevice.TransactionId = string.Empty;
            InteractingDevice.allowAllAgv = true;
            Interlocked.Exchange(ref InteractingDevice.MaterialEnsureFlag, 0);
            Interlocked.Exchange(ref InteractingDevice.AgvOnWorkDrillStatus, 0);
            logger.LogDebug($"中控给钻机下发异常");
        }

        public Task InitializeLocal()
        {
            return Task.CompletedTask;
        }

        public async Task<DeviceServiceInvokeResponse> ShutdownLocal()
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Shutdown!");
            try
            {
                if (!InteractingDevice.cnc84Command.CNCCommandStatus())
                {
                    return await Response(ErrorCodes.Sys.FAIL, "CNC84断开链接");
                }

                InteractingDevice.cnc84Command.Shutdown();
                logger.LogDebug("发送关闭cnc84指令");
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} finish Shutdown!");

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await Response(ErrorCodes.Sys.FAIL, e.Message);
            }
        }

        public async Task<DeviceServiceInvokeResponse> StandbyLocal()
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Standby!");
            try
            {
                if (!InteractingDevice.cnc84Command.CNCCommandStatus())
                {
                    return await Response(ErrorCodes.Sys.FAIL, "CNC84断开链接");
                }

                InteractingDevice.cnc84Command.Stop();
                logger.LogDebug("发送暂停指令");
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} finish Standby!");

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await Response(ErrorCodes.Sys.FAIL, e.Message);
            }
        }

        public async Task<DeviceServiceInvokeResponse> ScheduleTaskLocal()
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to ScheduleTask!");
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} finish ScheduleTask!");
            return await Response(ErrorCodes.Sys.SUCCESS, string.Empty);
        }

        public async Task<DeviceServiceInvokeResponse> WorkLocal()
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} begin to Work!");
            try
            {
                if (!InteractingDevice.cnc84Command.CNCCommandStatus())
                {
                    return await Response(ErrorCodes.Sys.FAIL, "CNC84断开链接");
                }

                StartChangeF8();
                logger.LogDebug("切换到F8界面");
                InteractingDevice.cnc84Command.Start();
                logger.LogDebug("发送打板指令");
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId} finish Work!");

                return await Response(ErrorCodes.Sys.SUCCESS, string.Empty);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return await Response(ErrorCodes.Sys.FAIL, e.Message);
            }
        }

        public string CompleteAllEndLocal()
        {
            InteractingDevice.bufferAllUnloadAndLoadEnd = 1;
            return "";
        }

        private void StartChangeF8()
        {
            InteractingDevice.cnc84Command.SetChangePage("WORK_WORK");
        }

        private void Cnc84Connect()
        {
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}   begin to Cnc84Connect!");
            InteractingDevice.cnc84Command = InteractingDevice.cNC84CommandWrapper.CreateCNCCommand();
            InteractingDevice.cnc84Command.Init(DeviceDescriptor.Extra["CNC84Ip"].ToStr(), DeviceDescriptor.Extra["CNC84Port"].ToInt());
            logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}   finish Cnc84Connect!");
        }

        private bool SerialConnect()
        {
            try
            {
                return InteractingDevice.frontExtendDevice.OpenDevice(DeviceDescriptor.Extra["Com"].ToStr());
            }
            catch (Exception e)
            {
                logger.LogInformation($"{DateTime.Now.ToLongTimeString()}-{InteractingDevice.ProductId}-{InteractingDevice.DeviceName}-{InteractingDevice.DeviceId}  SerialConnect {e.Message}!");
            }
            return false;
        }

        public List<Panel> InitPayloadPanels()
        {
            return Panel.HasSilo.NoPanelForLayerFirst("", InteractingDevice.spindleNum, 1);
        }

        public void CanceledScheduleLocal(ScheduledTaskStatus ScheduledStatus)
        {
            logger.LogDebug($"CompleteScheduleLocal 方法被调用了 调度状态 {ScheduledStatus}：重置{InteractingDevice.allowAllAgv}");
            InteractingDevice.allowAllAgv = true;
            InteractingDevice.TransactionId = string.Empty;
        }

        public void CodeReaderCheck(int checkResult)
        {
        }

        public void CodeReaderSingleSplindleCheck(int splindle, int checkResult)
        {
        }

        public Task<DeviceServiceInvokeResponse> SetScannerLot(DeviceServiceInvokeRequest request) => throw new NotImplementedException();
        public Task<DeviceServiceInvokeResponse> LoadAtpFile(DeviceServiceInvokeRequest request) => throw new NotImplementedException();
        public Task<DeviceServiceInvokeResponse> CodeReaderOnOffToCnc(DeviceServiceInvokeRequest request) => throw new NotImplementedException();
    }
}
