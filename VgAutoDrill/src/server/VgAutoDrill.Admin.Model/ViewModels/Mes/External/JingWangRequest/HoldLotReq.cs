namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingWangRequest
{
    public class HoldLotReq
    {
        /// <summary>
        /// 产品批次代码(sourceCode)
        /// </summary>
        public virtual string? ContainerName { get; set; }

        /// <summary>
        /// HOLD原因
        /// </summary>
        public virtual string? HoldReason { get; set; }
    }
}
