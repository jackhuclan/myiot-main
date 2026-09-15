namespace VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAlterLog
{
    public class WorkOrderAlterLogDto : BaseDto
    {
        /// <summary>
        /// 工单编码
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public virtual DateTime? ActionTime { get; set; }

        /// <summary>
        /// 具体操作
        /// </summary>
        public virtual string? ActionDetail { get; set; }

        /// <summary>
        /// 转换前文件路径
        /// </summary>
        public virtual string? BeforeDrillFilePath { get; set; }

        /// <summary>
        /// 转换后文件路径
        /// </summary>
        public virtual string? AfterDrillFilePath { get; set; }
    }
}
