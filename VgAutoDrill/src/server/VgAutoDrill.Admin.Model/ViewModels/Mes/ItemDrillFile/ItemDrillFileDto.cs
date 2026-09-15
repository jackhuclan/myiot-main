namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFile
{
    public class ItemDrillFileDto : BaseDto
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
        /// 钻带参数文件名
        /// </summary>
        public virtual string? DrillFileName { get; set; }

        /// <summary>
        /// 钻带参数路径
        /// </summary>
        public virtual string? DrillFilePath { get; set; }
    }
}
