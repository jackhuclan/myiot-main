using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Silo
{
    public class SiloExcelDto
    {   
        ///<summary>
        /// 料仓编码
        /// </summary>
        public virtual string? Code { get; set; }
        /// <summary>
        /// 运载尺寸
        /// </summary>
        public virtual string? Size { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
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
        /// 料仓状态
        /// </summary>
        public virtual SiloStatus SiloStatus { get; set; }
        /// <summary>
        /// 关联设备类别
        /// </summary>
        public virtual DeviceKind? RelatedDeviceKind { get; set; }
    }
}
