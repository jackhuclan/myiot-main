using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment
{
    public class GetDeviceListReq : Page
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { set; get; }

        /// <summary>
        /// 编号
        /// </summary>
        public string? Code { set; get; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public int? DeviceTypeId { get; set; }
        /// <summary>
        /// 设备类型Code
        /// </summary>
        public string? DeviceTypeCode { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public int? Status { set; get; }

        /// <summary>
        /// 设备状态
        /// </summary>

        public virtual List<DeviceStatus>? DeviceStatusList { set; get; }

        public virtual List<DeviceKind>? DeviceKindList { set; get; }

        public virtual List<string>? RouteCodes { get; set; }

        public virtual List<string>? DeviceCodes { get; set; }


        /// <summary>
        /// 自动机器还是手动机器
        /// </summary>
        public virtual bool? IsAuto { get; set; }
    }
}
