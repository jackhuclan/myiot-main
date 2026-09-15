using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 钻带参数文件明细
    /// </summary>
    [SugarTable("t_item_drill_file_detail")]
    public class ItemDrillFileDetail : BaseEntity
    {
        /// <summary>
        /// 主表文件ID
        /// </summary>
        [SugarColumn(ColumnName = "item_drill_file_id")]
        public virtual long? ItemDrillFileId { get; set; }

        /// <summary>
        /// 钻带参数文件明细编号
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        /// <summary>
        /// 直径
        /// </summary>
        [SugarColumn(ColumnName = "diameter")]
        public virtual decimal? Diameter { get; set; }
    }
}
