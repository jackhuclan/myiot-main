using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    [SugarTable("t_cutter_group_detail")]
    public class CutterGroupDetail : BaseEntity
    {

        /// <summary>
        /// 配刀组计划No
        /// </summary>
        [SugarColumn(ColumnName = "group_no")]
        public virtual string? CutterGroupNo { get; set; }
        /// <summary>
        /// lot编号
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 每趟的实际钻板数
        /// </summary>
        [SugarColumn(ColumnName = "panel_num")]
        public virtual int? PanelNum { get; set; }

        /// <summary>
        /// 任务code
        /// </summary>
        [SugarColumn(ColumnName = "task_code")]
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 是否首次（首刀，用作钻机换刀标识）
        /// </summary>
        [SugarColumn(ColumnName = "is_first_cutter")]
        public virtual bool? IsFirstCutter { get; set; }
    }
}
