namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class ExternalWorkOrderQueryReq
    {
        /// <summary>
        /// 外部工单编号
        public virtual string? SourceCode { get; set; }
        /// <summary>
        /// 内部工单号
        /// </summary>
        public virtual string? InnerCode { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public virtual string? IncodeNumber { get; set; }
    }
}
