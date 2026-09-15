using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 刀具管理
    ///</summary>
    [SugarTable("t_cutter")]
    public class Cutter : BaseEntity
    {
        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }
        /// <summary>
        /// 最高库存
        /// </summary>
        [SugarColumn(ColumnName = "max_stock")]
        public int? MaxStock { get; set; }

        /// <summary>
        /// 现有库存
        /// </summary>
        [SugarColumn(ColumnName = "current_stock")]
        public int? CurrentStock { get; set; }

        /// <summary>
        /// 最低库存
        /// </summary>
        [SugarColumn(ColumnName = "min_stock")]
        public int? MinStock { get; set; }

        /// <summary>
        /// 入库日期
        /// </summary>
        [SugarColumn(ColumnName = "inbound_date")]
        public DateTime? InboundDate { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [SugarColumn(ColumnName = "specification")]

        public virtual string? Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [SugarColumn(ColumnName = "unit_of_measure")]

        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 刀片寿命
        /// </summary>
        [SugarColumn(ColumnName = "life")]

        public virtual int? Life { get; set; }

        /// <summary>
        /// 预计加工件数
        /// </summary>
        [SugarColumn(ColumnName = "estimated_pieces")]

        public virtual int? EstimatedPieces { get; set; }

        /// <summary>
        /// 用途位置
        /// </summary>
        [SugarColumn(ColumnName = "purpose")]

        public virtual string? Purpose { get; set; }

        /// <summary>
        /// 刀具状态
        /// </summary>
        [SugarColumn(ColumnName = "cutter_status")]

        public virtual string? CutterStatus { get; set; }
    }
}
