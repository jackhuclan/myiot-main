using System.ComponentModel.DataAnnotations;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Common
{
    public class GetDrillNextPanelRequest
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [Required]
        public string DeviceId { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        [Required]
        public string ItemCode { get; set; }

        /// <summary>
        /// 0钻机, 1生料仓,2熟料仓
        /// </summary>
        [Required]
        public int Layer { get; set; }

        /// <summary>
        ///  起始轴
        /// </summary>
        [Required]
        public int BeginPosition { get; set; }

        /// <summary>
        /// 连续轴数
        /// </summary>
        [Required]
        public int Count { get; set; } = 1;

        /// <summary>
        /// 板料宽度
        /// </summary>
        public float PanelWidth { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        public float PanelLength { get; set; }
        /// <summary>
        /// 叠板层数
        /// </summary>
        public int Pcs { get; set; }
        /// <summary>
        /// 板料销钉距中心偏移量
        /// </summary>
        public float PinOffset { get; set; }
        /// <summary>
        /// 料仓二维码
        /// </summary>
        public string SiloCode { get; set; } = string.Empty;

        /// <summary>
        /// Lot二维码
        /// </summary>
        public string LotId { get; set; } = string.Empty;
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchCode { get; set; } = string.Empty;

        /// <summary>
        /// 成品状态
        /// </summary>
        public ProductStatus ProductStatus { get; set; } = ProductStatus.Noop;
    }
}
