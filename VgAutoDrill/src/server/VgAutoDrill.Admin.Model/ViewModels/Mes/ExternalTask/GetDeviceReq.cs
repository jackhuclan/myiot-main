using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask
{
    public class GetDeviceReq
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
