namespace VegaIot.External.XianjinIot.Models
{
    /// <summary>
    /// 用于MES下发调取钻孔资料命令给钻机
    /// </summary>
    internal class DrillLoadDrillingFilePayload
    {
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderEntity header { get; set; } = new();
        /// <summary>
        /// 内容
        /// </summary>
        public DrillLoadDrillingFileBody body { get; set; } = new();
    }
    internal class DrillLoadDrillingFileBody
    {
        /// <summary>
        /// 任务编码
        /// </summary>
        public string askCode { get; set; } = string.Empty;

        /// <summary>
        /// 料号编码，如果filePath为空则用该字段使用原来的调资料方式调取
        /// </summary>
        public string incodeNumber { get; set; } = string.Empty;
        /// <summary>
        /// 资料路径
        /// </summary>
        public string filePath { get; set; } = string.Empty;
    }

    }
