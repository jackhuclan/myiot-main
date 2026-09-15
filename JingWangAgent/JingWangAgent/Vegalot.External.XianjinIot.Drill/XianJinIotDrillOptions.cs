// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill
{
    internal class XianJinIotDrillOptions
    {
        public int SignCode { get; set; } = 0;
        public string Signature { get; set; } = string.Empty;
        public string TaskCode { get; set; } = string.Empty;

        public string DeviceId { get; set; } = string.Empty;

        public int LoadPrepareTimeOut { get; set; } = 5000;

        public int UnloadPrepareTimeOut { get; set; } = 5000;

        public int LoadCompletePrepareTimeOut { get; set; } = 10000;

        public int UnloadCompletePrepareTimeOut { get; set; } = 10000;

        public string FtpHost { get; set; } = "127.0.0.1";
        public string FtpUsername { get; set; } = "dril";
        public string FtpPassword { get; set; } = "JlcDril123";
        public int FtpPort { get; set; } = 21;
        public string FtpPath { get; set; } = "Dril/PCB1-ZK/";
        public string LocalDirectory { get; set; } = "D:\\data";
        public bool ValidateDrlImgCountOnOff { get; set; } = false;
        public bool ReportBarcodeAllOnOff { get; set; } = true;
        public int HasStartedOnCnc { get; set; } = 59;
        public int StartedSpan { get; set; } = 100;
    }
}
