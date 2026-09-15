namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess
{
    public class RouteAndProcessDto : BaseDto
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public virtual long? ProcessId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public virtual long? OrderNum { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { set; get; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public virtual string? ProcessCode { set; get; }

        /// <summary>
        /// TASK的颜色
        /// </summary>
        public virtual string? Color { get; set; }

        /// <summary>
        /// 是否关键工序(0/1)
        /// </summary>
        public virtual string? KeyFlag { get; set; }

        /// <summary>
        /// 本工序耗时(Min)
        /// </summary>
        public virtual int? RequiredTime { get; set; }

        /// <summary>
        /// 是否需要手动检查(0/1)
        /// </summary>
        public virtual string? IsManualCheck { get; set; }

        /// <summary>
        /// 自检数量
        /// </summary>
        public virtual int? SelfCheckNum { get; set; }
    }
}
