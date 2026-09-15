using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// AGV休息点
    ///</summary>
    [SugarTable("t_agv_rest")]
    public class AgvRest : BaseEntity
    {
        /// <summary>
        /// 休息点编码
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        /// <summary>
        /// 休息点名称
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }

        /// <summary>
        /// 物理点位
        /// </summary>
        [SugarColumn(ColumnName = "point")]
        public virtual string? Point { get; set; }

        /// <summary>
        /// 该分区该点预定分配的AGV
        /// </summary>
        [SugarColumn(ColumnName = "pre_book_agv")]
        public virtual string? PreBookAgv { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        [SugarColumn(ColumnName = "pre_book_time")]
        public virtual DateTime? PreBookTime { get; set; }

        /// <summary>
        /// 当前点正占用的AGV
        /// </summary>
        [SugarColumn(ColumnName = "current_agv")]
        public virtual string? CurrentAgv { get; set; }

    }
}
