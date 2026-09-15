using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// ATP文件明细
    /// </summary>
    [SugarTable("t_item_atp_file_detail")]
    public class ItemAtpFileDetail : BaseEntity
    {
        /// <summary>
        /// ATP文件ID
        /// </summary>
        [SugarColumn(ColumnName = "item_atp_file_id")]
        public virtual long? ItemAtpFileId { get; set; }

        /// <summary>
        /// ATP文件明细编号
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        /// <summary>
        /// 直径
        /// </summary>
        [SugarColumn(ColumnName = "diameter")]
        public virtual decimal? Diameter { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "order_num")]
        public virtual int? OrderNum { get; set; }
    }
}
