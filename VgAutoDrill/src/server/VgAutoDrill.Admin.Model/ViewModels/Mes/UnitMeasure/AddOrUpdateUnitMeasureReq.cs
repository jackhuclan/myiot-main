namespace VgAutoDrill.Admin.Model.ViewModels.Mes.UnitMeasure
{
    public class AddOrUpdateUnitMeasureReq : BaseAddOrUpdateDto
    {
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }
        public virtual string? PrimaryFlag { get; set; }

        /// <summary>
        /// 主单位ID
        /// </summary>
        public virtual int? PrimaryId { get; set; }

        /// <summary>
        /// 与主单位换算比例
        /// </summary>
        public virtual decimal? ChangeRate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
    }
}