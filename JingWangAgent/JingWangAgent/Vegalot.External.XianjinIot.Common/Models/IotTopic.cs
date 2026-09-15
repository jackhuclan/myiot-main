// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Common.Models
{
    public static class IotTopic
    {
        public const string PUBLIC_KEY_TOPIC = "jlc/mes/report/public_key";
        public const string REGIST_TOPIC = "jlc/mes/report/regist";
        public const string DEVICE_INFO_TOPIC = "jlc/mes/report/device_info";
        public const string OTA_UPGRADE_ACK_TOPIC = "jlc/mes/report/OTA_upgrade_ack";

        #region AGV:代理=>MES
        public const string START_DRILLING = "jlc/mes/issued/drilling/start_drilling";
        public const string LOAD_SILO_TOPIC = "jlc/mes/report/agv/load_silo";
        public const string ADJUST_HEIGHT_TOPIC = "jlc/mes/report/agv/adjust_height";
        public const string PUSH_PANEL_TOPIC = "jlc/mes/report/agv/push_panel";
        public const string RECEIVE_PANEL_TOPIC = "jlc/mes/report/agv/receive_panel";
        public const string UNLOAD_SILO_TOPIC = "jlc/mes/report/agv/unload_silo";
        public const string LOCKING_UNIT_900_TOPIC = "jlc/mes/report/agv/900_locking_unit";
        public const string LOCKING_UNIT_950_TOPIC = "jlc/mes/report/agv/950_locking_unit";
        public const string HEIGHT_NUMBER_TOPIC = "jlc/mes/report/agv/height_number";
        public const string STOP_WORKING_TOPIC = "jlc/mes/report/agv/stop_working";

        #endregion AGV:代理=>MES

        #region AGV:MES=>代理
        public const string START_DRILLING_ACK = "jlc/mes/issued/drilling/start_drilling_ack";
        public const string ADJUST_HEIGHT_ACK_TOPIC = "jlc/mes/report/agv/adjust_height_ack";
        public const string ADJUST_HEIGHT_END_TOPIC = "jlc/mes/report/agv/adjust_height_report";
        public const string LOAD_SILO_ACK_TOPIC = "jlc/mes/report/agv/load_silo_ack";
        public const string LOAD_SILO_REPORT_TOPIC = "jlc/mes/report/agv/load_silo_report";
        public const string HEIGHT_NUMBER_ACK = "jlc/mes/report/agv/height_number_ack";
        public const string PUSH_PANEL_ACK_TOPIC = "jlc/mes/report/agv/push_panel_ack";
        public const string RECEIVE_PANEL_ACK_TOPIC = "jlc/mes/report/agv/receive_panel_ack";
        public const string UNLOAD_SILO_ACK_TOPIC = "jlc/mes/report/agv/unload_silo_ack";
        public const string UNLOAD_SILO_REPORT_TOPIC = "jlc/mes/report/agv/unload_silo_report";
        public const string RECEIVE_PANEL_REPORT_TOPIC = "jlc/mes/report/agv/receive_panel_report";
        public const string PUSH_PANEL_REPORT_TOPIC = "jlc/mes/report/agv/push_panel_report";
        public const string HEIGHT_NUMBER_ACK_TOPIC = "jlc/mes/report/agv/height_number_ack";
        public const string RECEIVE_UNIT_STATE_TOPIC = "jlc/mes/report/agv/receive_unit_state";
        public const string STOP_WORKING_ACK_TOPIC = "jlc/mes/report/agv/stop_working_ack";
        public const string RECEIVE_UNIT_ERROR_TOPIC = "jlc/mes/report/agv/receive_unit_error";
        public const string LOCKING_UNIT_ACK_900_TOPIC = "jlc/mes/report/agv/900_locking_unit_ack";
        public const string LOCKING_UNIT_ACK_950_TOPIC = "jlc/mes/report/agv/950_locking_unit_ack";
        public const string RECEIVE_UNIT_STATE = "jlc/mes/report/agv/receive_unit_state";
        public const string RECEIVE_UNIT_ERROR = "jlc/mes/report/agv/receive_unit_error";

        #endregion AGV:MES=>代理
    }
}
