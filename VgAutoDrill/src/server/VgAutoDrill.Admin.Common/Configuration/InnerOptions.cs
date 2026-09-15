namespace VgAutoDrill.Admin.Common.Configuration
{
    public class InnerOptions
    {
        public const string Options = "InnerOptions";
        public int ConsumingPerMinutes { get; set; } = 10;
        /// <summary>
        /// 定时删除，监控日志数据
        /// </summary>
        public int DeleteUnusedDataPerMinutes { get; set; } = 600;
        /// <summary>
        /// 设备调度汇总任务扫描间隔（分钟），默认60分钟
        /// </summary>
        public int DeviceSummaryScanIntervalMinutes { get; set; } = 60;

        public string CancelScheduleUrl { get; set; } = "";
        public string SetScheduleUrgentUrl { get; set; } = "";
        /// <summary>
        /// 这里用户，不显示告警信息,逗号分给多个用户
        /// </summary>
        public string NoAlarmForUsers { get; set; } = "vega,";

        /// <summary>
        /// 仅提醒订阅消息的用户，逗号分隔多个用户
        /// </summary>
        public string SampleOrderForOnlyNotifyUsers { get; set; } = "admin,";

        /// <summary>
        /// 是否启用SampleOrder告警功能
        /// </summary>
        public bool EnableSampleOrderAlarm { get; set; } = false;

        /// <summary>
        /// SampleOrder告警检查的时间限制（小时）
        /// </summary>
        public int SampleOrderAlarmTimeLimitHours { get; set; } = 72;

        /// <summary>
        /// SampleOrder告警检查的缓冲时间（分钟）
        /// </summary>
        public int SampleOrderAlarmBufferMinutes { get; set; } = 10;

        /// <summary>
        /// SampleOrder告警检查的外部工单前缀，逗号分隔多个前缀
        /// </summary>
        public string SampleOrderAlarmWorkOrderPrefix { get; set; } = "S,";

        /// <summary>
        /// 是否启用SampleOrder告警自动处理功能
        /// </summary>
        public bool EnableSampleOrderAlarmAutoHandle { get; set; } = false;

        /// <summary>
        /// 是否启用板料长度验证要求
        /// </summary>
        public bool EnablePanelLengthRequirement { get; set; } = false;

        /// <summary>
        /// 位置料仓映射缓存过期时间（小时）
        /// </summary>
        public int LocationSiloCacheExpirationHours { get; set; } = 24;

        /// <summary>
        /// 数据导出最大条数限制
        /// </summary>
        public int ExportMaxCount { get; set; } = 100000;

        /// <summary>
        /// 是否允许料仓切换库位绑定（一个料仓只能绑定一个库位，但可以切换到其他库位）
        /// </summary>
        public bool AllowSiloMultiLocation { get; set; } = true;

        /// <summary>
        /// 是否允许料仓切换库位绑定（一个料仓只能绑定一个库位，但可以切换到其他库位）
        /// </summary>
        public bool AllowAgvMultiSilo { get; set; } = true;

        /// <summary>
        /// 是否允许板料切换料仓绑定（一个板料只能绑定一个料仓，但可以切换到其他料仓）
        /// </summary>
        public bool AllowPanelMultiSilo { get; set; } = true;

        public bool EableCentralAddTransferJobUrl { get; set; } = false;
        /// <summary>
        /// 中控接口，新增料仓任务接口
        /// </summary>
        public string CentralAddTransferJobUrl { get; set; } = string.Empty;
    }
}
