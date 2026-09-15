using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 工艺路线表
    ///</summary>
    [SugarTable("t_route")]
    public class Route : BaseEntity
    {
        /// <summary>
        /// 工艺路线编号
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }

        /// <summary>
        /// 工艺路线说明
        /// </summary>
        [SugarColumn(ColumnName = "route_desc")]
        public virtual string? RouteDesc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 审批状态 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "vetting_status")]
        public virtual byte VettingStatus { get; set; } = 0;
    }
}
