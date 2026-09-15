namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem.VegaRawMaterial
{
    public class GetVegaRawMaterialDto
    {
        /// <summary>
        /// 托盘号
        /// </summary>
        public string? SiloCode { get; set; }

        /// <summary>
        /// 批次号(物料号)
        /// </summary>
        public string? ItemCode { get; set; }

        /// <summary>
        /// 所在位置区域号
        /// </summary>
        public string? LocationCode { get; set; }

        /// <summary>
        /// 所在位置区域名字
        /// </summary>
        public string? LocationName { get; set; }

        /// <summary>
        /// 生料数量
        /// </summary>
        public int? RawMaterialCount { get; set; }
    }
}
