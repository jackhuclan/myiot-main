using SqlSugar;
using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup
{
    public class AddOrUpdateCutterGroupReq : BaseAddOrUpdateDto
    {

        /// <summary>
        /// 计划要板时间
        /// </summary>
        public virtual DateTime? PlannedTime { get; set; }

        /// <summary>
        /// 配刀组计划对应的生产实际趟数(明细表所属数据的个数)
        /// </summary>
        public virtual int? RoundNum { get; set; }

        /// <summary>
        /// 配刀组计划对应的生产标准趟数
        /// </summary>
        public virtual int? RoundNumStandard { get; set; }


        /// <summary>
        /// 轴数
        /// </summary>
        public virtual int? AxisCount { get; set; }

        /// <summary>
        /// 尾轮轴数，默认:0
        /// </summary>
        public virtual int? EndAxisCount { get; set; } = 0;

        /// <summary>
        /// 单轴刀盒数量
        /// </summary>
        public virtual int? CutterBoxNum { get; set; } = 0;

        /// <summary>
        /// 组计划生成时间
        /// </summary>
        public virtual DateTime? GroupDate { get; set; }

        /// <summary>
        /// 配刀状态 0-待配刀 1-配刀锁定 
        /// </summary>
        public virtual CutterGroupStatusEnum? CutterGroupStatus { get; set; }

        /// <summary>
        /// 锁定时间
        /// </summary>
        public virtual DateTime? LockedTime { get; set; }


        /// <summary>
        /// apt文件路径
        /// </summary>
        public virtual string? AptFilePath { get; set; }


        /// <summary>
        /// 刀盒二维码集合，逗号分隔
        /// </summary>
        public virtual string? BoxNos { get; set; }
    }
}
