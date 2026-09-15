namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation
{
    public class GetWorkstationListReq : Page
    {
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }
        /// <summary>
        /// 所在车间ID
        /// </summary>
        public virtual long? WorkshopId { get; set; }
        /// <summary>
        /// 所在车间编码
        /// </summary>
        public virtual string? WorkshopCode { get; set; }
        /// <summary>
        /// 所在车间名称
        /// </summary>
        public virtual string? WorkshopName { get; set; }
        /// <summary>
        /// 默认工序编码
        /// </summary>
        public virtual string? ProcessCode { get; set; }
        /// <summary>
        /// 默认工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }
    }
}
