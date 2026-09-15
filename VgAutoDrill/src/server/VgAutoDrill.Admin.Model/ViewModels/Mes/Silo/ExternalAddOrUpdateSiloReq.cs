using System.ComponentModel.DataAnnotations;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class ExternalAddOrUpdateSiloReq
    {
        // <summary>
        /// 编码
        /// </summary>
        [Required]
        public virtual string? Code { get; set; }
        //// <summary>
        /// 运载尺寸
        /// </summary>
        public virtual string? Size { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        [Required]
        [Range(1, 30, ErrorMessage = "层数超出范围")]
        public virtual int? FloorCount { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual string? Supplier { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public virtual string? Location { get; set; }
        /// <summary>
        /// 状态(1-启用，0-禁用)
        /// </summary>
        [Required]
        public virtual int? Status { get; set; }
    }

    public class ExternalUpdateSiloReq
    {
        // <summary>
        /// 编码
        /// </summary>
        [Required]
        public virtual string? Code { get; set; }
        /// <summary>
        /// 状态 0-禁用  1-启用
        /// </summary>
        [Required]
        public virtual int? Status { get; set; }
    }

    public class ExternalAddOrUpdateSiloWithPanelReq
    {
        // <summary>
        /// 料仓编码
        /// </summary>
        [Required]
        public virtual string? Code { get; set; }
        /// <summary>
        /// 板料明细列表
        /// </summary>
        [Required]
        public List<ExternalAddOrUpdateSiloDetailReq> SiloDetails { get; set; } = new List<ExternalAddOrUpdateSiloDetailReq>();
    }
}
