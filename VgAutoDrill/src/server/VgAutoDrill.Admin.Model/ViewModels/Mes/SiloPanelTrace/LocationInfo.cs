namespace VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace
{
    /// <summary>
    /// 位置信息缓存模型
    /// </summary>
    public class LocationInfo
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 料仓代码
        /// </summary>
        public string? SiloCode { get; set; }
        
        /// <summary>
        /// 位置代码
        /// </summary>
        public string Location { get; set; } = string.Empty;
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 是否告警
        /// </summary>
        public bool IsWarning { get; set; }
        
        /// <summary>
        /// 警告描述
        /// </summary>
        public string? WarningDescription { get; set; }
        
        /// <summary>
        /// 参考记录ID
        /// </summary>
        public long? ReferenceRecordId { get; set; }
    }
}
