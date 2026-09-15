// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgDeviceGateway.Devices.Drill.DrillDevice
{
    public interface IDrillDevice
    {
        public void CompleteScheduleLocal(ScheduledTaskStatus ScheduledStatus);

        public void CanceledScheduleLocal(ScheduledTaskStatus ScheduledStatus);

        public void FailScheduleLocal();

        public Task InitializeLocal();

        public bool ConnectToCncAndOtherDevice();

        public Task<DeviceServiceInvokeResponse> WorkLocal();

        public Task<DeviceServiceInvokeResponse> StandbyLocal();

        public Task<DeviceServiceInvokeResponse> ShutdownLocal();

        public Task<DeviceServiceInvokeResponse> ScheduleTaskLocal();

        public string CompleteAllEndLocal();

        public List<Panel> InitPayloadPanels();

        void CodeReaderCheck(int checkResult);

        void CodeReaderSingleSplindleCheck(int splindle, int checkResult);

        public Task<DeviceServiceInvokeResponse> SetScannerLot(DeviceServiceInvokeRequest request);

        public Task<DeviceServiceInvokeResponse> LoadAtpFile(DeviceServiceInvokeRequest request);
        public Task<DeviceServiceInvokeResponse> CodeReaderOnOffToCnc(DeviceServiceInvokeRequest request);
    }
}
