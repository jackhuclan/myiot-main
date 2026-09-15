using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.External.Model.V3.ExternalDevice
{
    public class ExternalDeviceReq : ExternalBaseReq
    {
        /// <summary>
        /// 新增设备
        /// </summary>
        public virtual List<AddDeviceParam>? AddDeviceParam { get; set; }

        /// <summary>
        /// 修改设备
        /// </summary>
        public virtual List<UpdateDeviceParam>? UpdateDeviceParam { get; set; }

        /// <summary>
        /// 删除设备
        /// </summary>
        public virtual List<int>? DeleteDeviceIDList { get; set; }

        /// <summary>
        /// 查询设备
        /// </summary>
        public virtual SelectDrillDeviceParam? SelectDrillDeviceParam { get; set; }

    }

    /// <summary>
    /// 新增
    /// </summary>
    public class AddDeviceParam : AddOrUpdateDeviceReq
    {

    }

    /// <summary>
    /// 更新
    /// </summary>
    public class UpdateDeviceParam : AddOrUpdateDeviceReq
    {

    }

    /// <summary>
    /// 查询钻机信息
    /// </summary>
    public class SelectDrillDeviceParam
    {
        /// <summary>
        /// 钻机编码
        /// </summary>
        public virtual string? Code { get; set; }
        /// <summary>
        /// 钻机名称
        /// </summary>
        public virtual string? Name { get; set; }
        /// <summary>
        /// 钻机状态
        /// </summary>
        public virtual DeviceStatus? DeviceStatus { get; set; }//注释各个状态描述

        /// <summary>
        /// 钻机工艺路线
        /// </summary>
        public virtual string? Route { get; set; }
    }
}
