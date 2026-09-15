namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStock
{
    public class GetMaterialStockListReq : Page
    {
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 物料类型ID
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public virtual string? WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public virtual string? WarehouseName { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public virtual string? ProcessCode { get; set; }
    }
}
