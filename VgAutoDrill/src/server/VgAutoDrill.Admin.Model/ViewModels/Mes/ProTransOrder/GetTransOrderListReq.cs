namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTransOrder
{
    public class GetTransOrderListReq : Page
    {
        public virtual string? Code { get; set; }

        public virtual string? ItemCode { get; set; }

        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;
    }
}
