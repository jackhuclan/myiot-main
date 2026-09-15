namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Silo
{
    public class SiloDetailQueryReq
    {
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string? SiloCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }
    }
}
