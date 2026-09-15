using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanel
{
    public class AddOrUpdateDevicePanelReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 板料二维码
        /// </summary>
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 料仓二维码
        /// </summary>
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// Lot二维码
        /// </summary>
        public virtual string? LotId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 第几层
        /// </summary>
        public virtual int? Layer { get; set; }

        /// <summary>
        /// 位置ID
        /// </summary>
        public virtual int? Position { get; set; }

        /// <summary>
        /// 成品状态
        /// </summary>
        public virtual ProductStatus? ProductStatus { get; set; }
        /// <summary>
        /// Panel的钻孔状态
        /// </summary>
        public virtual int? DrillState { get; set; }
        /// <summary>
        /// 板宽
        /// </summary>
        public virtual decimal? PanelWidth { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        public virtual decimal? PinOffset { get; set; }

        public virtual string? LocationCode { get; set; }
    }
}
