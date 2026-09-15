using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class GetDrillOrAgvDeviceInfoReq : Page
    {
        public virtual string? DeviceCode { get; set; }
        public virtual string? ItemCode { get; set; }
        public virtual int IsRebrush { get; set; } = 0;
        public virtual bool IsVerifyAfterDrillPath { get; set; } = false;
        public virtual List<TaskStatusEnum>? TaskStatusList { get; set; }

        public virtual List<string>? RouteCodeList { get; set; }

        public virtual List<DeviceStatus>? DeviceStatuseLists { get; set; }

        /// <summary>
        /// 是否手动呼叫agv的请求
        /// </summary>
        public virtual bool IsManualCallAgvRequest { get; set; }
    }
}
