namespace VgAutoDrill.Fundation.Iot;

public static class Topics
{
    public static class Services
    {
        public const string CANCEL_SCHEDULE_SERVICE_ID = "CancelSchedule";
        public const string COMPLETE_SCHEDULE_SERVICE_ID = "CompleteSchedule";
        public const string SCHEDULE_TASK_SERVICE_ID = "ScheduleTask";
        public const string WORK_SERVICE_ID = "Work";
        public const string STANDBY_SERVICE_ID = "Standby";
        public const string SHUTDOWN_SERVICE_ID = "Shutdown";
        public const string MAINTAIN_SERVICE_ID = "Maintain";
        public const string PREPARE_LOAD_MATERIAL_SERVICE_ID = "PrepareLoadMaterial";
        public const string INVOKE_LOAD_MATERIAL_SERVICE_ID = "InvokeLoadMaterial";
        public const string COMPLETE_LOAD_MATERIAL_SERVICE_ID = "CompleteLoadMaterial";
        public const string PREPARE_UNLOAD_MATERIAL_SERVICE_ID = "PrepareUnloadMaterial";
        public const string INVOKE_UNLOAD_MATERIAL_SERVICE_ID = "InvokeUnloadMaterial";
        public const string COMPLETE_UNLOAD_MATERIAL_SERVICE_ID = "CompleteUnloadMaterial";
        public const string AGV_MOVE_SERVICE_ID = "Move";
        public const string PROPERTIES_READ_SERVICE_ID = "PropertiesRead";
        public const string PROPERTIES_WRITE_SERVICE_ID = "PropertiesWrite";
        public const string AGV_CHARGE_SERVICE_ID = "Charge";
        public const string REMOTE_COMMAND_SERVICE_ID = "RemoteCommand";
    }

    public static class Upstream
    {
        public const string OnlineTopic = "/central/mqtt/login";
        public const string EventTopic = "central/mqtt/event";
        public const string StatusTopic = "central/mqtt/status";
        public const string AlarmTopic = "central/mqtt/alarm";
        public const string PropertyTopic = "central/mqtt/property";
        public const string ServiceTopic = "central/mqtt/service";
        public const string PanelTopic = "central/mqtt/panel";
        public const string CutterTopic = "central/mqtt/cutter";

        public static string GetSubscribeTopic(string format)
        {
            return string.Format(format, "+", "+");
        }

        public static class V2
        {
            public const string OnlineTopicTemplate = "vega/mqtt/login/p/{0}/d/{1}";
            public const string EventTopicTemplate = "vega/mqtt/event/p/{0}/d/{1}";
            public const string StatusTopicTemplate = "vega/mqtt/status/p/{0}/d/{1}";
            public const string AlarmTopicTemplate = "vega/mqtt/alarm/p/{0}/d/{1}";
            public const string PropertyTopicTemplate = "vega/mqtt/property/p/{0}/d/{1}";
        }
    }

    public static class Downstream
    {
        public const string ServiceInvokeTopicTemplate = "/{0}/{1}/service/{2}/invoke";
        public static string GetSubscribeTopic(string format)
        {
            return string.Format(format, "+", "+", "+");
        }

        public static string ServiceInvokeTopic(string productId, string deviceId, string serviceName, string topicTemplate = ServiceInvokeTopicTemplate)
        {
            return string.Format(ServiceInvokeTopicTemplate, productId, deviceId, serviceName);
        }

        public static class V2
        {
            public const string ServiceInvokeTopicTemplate = "vega/mqtt/p/{0}/d/{1}/service/{2}/invoke";
        }
    }
}
