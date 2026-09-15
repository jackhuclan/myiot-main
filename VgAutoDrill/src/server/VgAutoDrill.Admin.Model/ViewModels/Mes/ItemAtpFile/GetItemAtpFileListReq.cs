namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFile
{
    public class GetItemAtpFileListReq : Page
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// ATP文件名
        /// </summary>
        public virtual string? AtpFileName { get; set; }

        /// <summary>
        /// ATP文件路径
        /// </summary>
        public virtual string? ATPFilePath { get; set; }

        /// <summary>
        /// ATP参数
        /// </summary>
        public virtual string? ATPParameters { get; set; }

        /// <summary>
        /// 钻带参数ID
        /// </summary>
        public virtual long? ItemDrillFileId { get; set; }

        /// <summary>
        /// 钻孔刀具参数主表ID
        /// </summary>
        public virtual long? CutterConfigMasterId { get; set; }

        /// <summary>
        /// 刀盘数量
        /// </summary>
        public virtual int? DiskCount { get; set; }

        /// <summary>
        /// 单盘刀列数
        /// </summary>
        public virtual int? ColumnsLimit { get; set; }

        /// <summary>
        /// 单盘刀行数
        /// </summary>
        public virtual int? RowsLimit { get; set; }

        /// <summary>
        /// 是否自动生成
        /// </summary>
        public virtual int? IsGenerated { get; set; }

        /// <summary>
        /// 自动生成时间
        /// </summary>
        public virtual DateTime? GenerateTime { get; set; }
    }
}
