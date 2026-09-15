// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill.Models
{
    internal static class IotTopic
    {
        public const string RECEIVE_PANEL_REPORT_TOPIC = "jlc/mes/report/drilling/receive_panel_report";
        public const string LOAD_PANEL_REPORT_TOPIC = "jlc/mes/report/drilling/load_panel_report";
        public const string LOAD_DRILLING_FILE_REPORT_TOPIC = "jlc/mes/report/drilling/load_drilling_file_report";
        public const string START_DRILLING_REPORT_TOPIC = "jlc/mes/report/drilling/start_drilling_report";
        public const string DRILLING_TASK_REPORT_TOPIC = "jlc/mes/report/drilling/drilling_task_report";
        public const string PUSH_PANEL_REPORT_TOPIC = "jlc/mes/report/drilling/push_panel_report";
        public const string UP_PANEL_REPORT_TOPIC = "jlc/mes/report/drilling/up_panel_report";

        public const string RECEIVE_PANEL_ACK_TOPIC = "jlc/mes/report/drilling/receive_panel_ack";
        public const string CREATE_TASK_ACK_TOPIC = "jlc/mes/report/drilling/create_task_ack";
        public const string LOAD_DRILLING_FILE_ACK_TOPIC = "jlc/mes/report/drilling/load_drilling_file_ack";
        public const string START_DRILLING_ACK_TOPIC = "jlc/mes/report/drilling/start_drilling_ack";
        public const string PUSH_PANEL_ACK_TOPIC = "jlc/mes/report/drilling/push_panel_ack";


        public const string PUBLIC_KEY_TOPIC = "jlc/mes/report/public_key";
        public const string REGIST_TOPIC = "jlc/mes/report/regist";
        public const string DEVICE_INFO_TOPIC = "jlc/mes/report/device_info";
        public const string OTA_UPGRADE_ACK_TOPIC = "jlc/mes/report/OTA_upgrade_ack";
    }
}
