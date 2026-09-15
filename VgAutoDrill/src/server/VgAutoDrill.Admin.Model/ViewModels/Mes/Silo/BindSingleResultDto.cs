using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Silo
{
    /// <summary>
    /// 告警等级
    /// </summary>
    public enum AlertLevel
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 警告
        /// </summary>
        Warning = 1,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// 板料绑定料仓结果DTO
    /// </summary>
    public class BindSingleResultDto
    {
        /// <summary>
        /// 库位编号
        /// </summary>
        public virtual string? LocationCode { get; set; }

        /// <summary>
        /// 当前库位上所有的板料信息
        /// </summary>
        public virtual List<LocationPanelDetailDto>? LocationDetails { get; set; }

        /// <summary>
        /// 告警等级
        /// </summary>
        public virtual AlertLevel AlertLevel { get; set; }
    }

    /// <summary>
    /// 库位板料明细DTO
    /// </summary>
    public class LocationPanelDetailDto
    {
        /// <summary>
        /// 板料编码
        /// </summary>
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 料仓编码
        /// </summary>
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 层号
        /// </summary>
        public virtual int? FloorNum { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 板料宽度
        /// </summary>
        public virtual decimal? PanelWidth { get; set; }

        /// <summary>
        /// 板料长度
        /// </summary>
        public virtual decimal? PanelLength { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        public virtual decimal? PinOffset { get; set; }

        /// <summary>
        /// 单叠数量
        /// </summary>
        public virtual int? Pcs { get; set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        public virtual int ProductStatus { get; set; }

        /// <summary>
        /// 产品状态描述
        /// </summary>
        public virtual string? ProductStatusDesc { get; set; }
    }
}
