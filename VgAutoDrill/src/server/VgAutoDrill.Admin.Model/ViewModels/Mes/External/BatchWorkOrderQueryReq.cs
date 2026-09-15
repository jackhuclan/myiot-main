namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class BatchWorkOrderQueryReq
    {
        /// <summary>
        /// 多个外部工单号
        /// </summary>
        public virtual List<string>? BatchSourceCodes { get; set; }
        /// <summary>
        /// 多个内部工单号
        /// </summary>
        public virtual List<string>? BatchInnerCodes { get; set; }
    }
}
