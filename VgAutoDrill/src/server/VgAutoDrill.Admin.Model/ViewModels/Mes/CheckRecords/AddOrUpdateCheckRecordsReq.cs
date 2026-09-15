namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CheckRecords
{
    public class AddOrUpdateCheckRecordsReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 生产任务ID
        /// </summary>
        public virtual long? TaskId { get; set; }

        /// <summary>
        /// 生产任务名称
        /// </summary>
        public virtual string? TaskName { get; set; }

        /// <summary>
        /// 生产任务编号
        /// </summary>
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 生产工单ID
        /// </summary>
        public virtual long? WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单名称
        /// </summary>
        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 检验人员ID
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// 检验人员姓名
        /// </summary>
        public virtual string? UserName { get; set; }

        /// <summary>
        /// 是否检验合格(-1/0/1)
        /// </summary>
        public virtual string? IsCheckOk { get; set; }

        /// <summary>
        /// 检验日期
        /// </summary>
        public virtual DateTime? CheckTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
    }
}
