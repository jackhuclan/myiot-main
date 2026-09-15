using System.ComponentModel.DataAnnotations;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Common
{
    public class GetNextPanelRequest
    {
        /// <summary>
        /// 从第几层开始（起始是0）
        /// </summary>
        [Required]
        public int BeginLayer { get; set; }

        /// <summary>
        ///
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
        /// 板料销钉距中心偏移量
        /// </summary>
        public float PinOffset { get; set; }

        /// <summary>
        /// 料仓二维码
        /// </summary>
        public string SiloCode { get; set; } = string.Empty;

        /// <summary>
        /// 料号
        /// </summary>
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// Lot二维码
        /// </summary>
        public string LotId { get; set; } = string.Empty;

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchCode { get; set; } = string.Empty;

        /// <summary>
        /// 放在哪个位置
        /// 从1开始
        /// 六轴钻机，分别对应1~6
        /// </summary>
        public int Position { get; set; } = 1;

        /// <summary>
        /// 成品状态
        /// </summary>
        public ProductStatus ProductStatus { get; set; } = ProductStatus.EmptyPayload;
    }
}
