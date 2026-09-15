// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Opc.Ua;

namespace Vegalot.External.XianjinIot.Agv
{
    internal class XianJinIotAgvOptions
    {
        public int SignCode { get; set; } = 0;
        public string Signature { get; set; } = string.Empty;
        public string TaskCode { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public int LoadSiloTimeOut { get; set; } = 1000 * 60 * 10;
        public int LoadSiloAckTimeOut { get; set; } = 1000 * 60 * 10;
        public int AdjustHeightTimeOut { get; set; } = 1000 * 60 * 10;
        public int AdjustHeightAckTimeOut { get; set; } = 1000 * 60 * 10;
        public int PushPanelTimeOut { get; set; } = 1000 * 60 * 10;
        public int PushPanelAckTimeOut { get; set; } = 1000 * 60 * 10;
        public int ReceivePanelTimeOut { get; set; } = 1000 * 60 * 10;
        public int ReceivePanelAckTimeOut { get; set; } = 1000 * 60 * 10;
        public int UnLoadSiloTimeOut { get; set; } = 1000 * 60 * 10;
        public int UnLoadSiloAckTimeOut { get; set; } = 1000 * 60 * 10;
        public int UpHeightNumberTimeOut { get; set; } = 1000 * 60 * 10;
        public int UpHeightNumberAckTimeOut { get; set; } = 1000 * 60 * 10;
        public int LockingUnitAck900 { get; set; } = 1000 * 60 * 10;
        public int LockingUnitAck950 { get; set; } = 1000 * 60 * 10;
        public int StopWorkingtAck { get; set; } = 1000 * 60 * 10;


        //等待plc返回ack时间
        public int AckListeningTime { get; set;}= 1000 * 60;

        //验签开关
        public bool VerfiySignatureSwitch { get; set; } = false;
    }
}
