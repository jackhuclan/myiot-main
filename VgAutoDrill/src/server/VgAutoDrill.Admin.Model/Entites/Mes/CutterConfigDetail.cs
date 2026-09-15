using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 钻孔刀具参数明细表
    ///</summary>
    [SugarTable("t_cutter_config_detail")]
    public class CutterConfigDetail : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "master_id")]
        public virtual int? MasterId { get; set; }

        /// <summary>
        /// D直径MM
        /// </summary>
        [SugarColumn(ColumnName = "d")]
        public virtual decimal? D { get; set; }
        /// <summary>
        /// S转速KRPM
        /// </summary>
        [SugarColumn(ColumnName = "s")]
        public virtual decimal? S { get; set; }
        /// <summary>
        /// F(进刀速)M/MIN
        /// </summary>
        [SugarColumn(ColumnName = "f")]
        public virtual decimal? F { get; set; }
        /// <summary>
        /// R(退刀速)M/MIN
        /// </summary>
        [SugarColumn(ColumnName = "r")]
        public virtual decimal? R { get; set; }
        /// <summary>
        /// Z(深度补偿)MM
        /// </summary>
        [SugarColumn(ColumnName = "z")]
        public virtual decimal? Z { get; set; }
        /// <summary>
        /// 寿命
        /// </summary>
        [SugarColumn(ColumnName = "age")]
        public virtual int? Age { get; set; }
    }
}
