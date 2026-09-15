using SqlSugar;
using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    [SugarTable("t_cutter_group")]
    public class CutterGroup : BaseEntity
    {
        /// <summary>
        /// 配刀组编号
        /// </summary>
        [SugarColumn(ColumnName = "group_no")]
        public virtual string? GroupNo { get; set; }

        /// <summary>
        /// 钻机编号
        /// </summary>
        [SugarColumn(ColumnName = "drill_no")]
        public virtual string? DrillNo { get; set; }

        /// <summary>
        /// 机型 2530，2537，2849
        /// </summary>
        [SugarColumn(ColumnName = "machine_size")]
        public virtual string? MachineSize { get; set; }

        /// <summary>
        /// 钻机名称
        /// </summary>
        [SugarColumn(ColumnName = "drill_name")]
        public virtual string? DrillName { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        [SugarColumn(ColumnName = "item_name")]
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 计划要板时间
        /// </summary>
        [SugarColumn(ColumnName = "planned_time")]
        public virtual DateTime? PlannedTime { get; set; }

        /// <summary>
        /// 配刀组计划对应的生产实际趟数(明细表所属数据的个数)
        /// </summary>
        [SugarColumn(ColumnName = "round_num")]
        public virtual int? RoundNum { get; set; }

        /// <summary>
        /// 配刀组计划对应的生产标准趟数
        /// </summary>
        [SugarColumn(ColumnName = "round_num_standard")]
        public virtual int? RoundNumStandard { get; set; }


        /// <summary>
        /// 轴数
        /// </summary>
        [SugarColumn(ColumnName = "axis_count")]
        public virtual int? AxisCount { get; set; }

        /// <summary>
        /// 尾轮轴数，默认:0
        /// </summary>
        [SugarColumn(ColumnName = "end_axis_count")]
        public virtual int? EndAxisCount { get; set; } = 0;

        /// <summary>
        /// 单轴刀盒数量
        /// </summary>
        [SugarColumn(ColumnName = "cutter_box_num")]
        public virtual int? CutterBoxNum { get; set; } = 0;

        /// <summary>
        /// 组计划生成时间
        /// </summary>
        [SugarColumn(ColumnName = "group_date")]
        public virtual DateTime? GroupDate { get; set; }

        /// <summary>
        /// 配刀状态 0-待配刀 1-配刀锁定 
        /// </summary>
        [SugarColumn(ColumnName = "cutter_group_status")]
        public virtual CutterGroupStatusEnum? CutterGroupStatus { get; set; }

        /// <summary>
        /// 锁定时间
        /// </summary>
        [SugarColumn(ColumnName = "locked_time")]
        public virtual DateTime? LockedTime { get; set; }


        /// <summary>
        /// apt文件路径
        /// </summary>
        [SugarColumn(ColumnName = "atp_file_path")]
        public virtual string? AtpFilePath { get; set; }


        /// <summary>
        /// 刀盒二维码集合，逗号分隔
        /// </summary>
        [SugarColumn(ColumnName = "box_nos")]
        public virtual string? BoxNos { get; set; }

        /// <summary>
        /// 刀盒码校验状态(-1:没有校验,0:校验失败,1:校验成功)
        /// </summary>
        [SugarColumn(ColumnName = "check_box")]
        public virtual CutterBoxStatusEnum? CheckBox { get; set; } = CutterBoxStatusEnum.Nothing;
    }
}
