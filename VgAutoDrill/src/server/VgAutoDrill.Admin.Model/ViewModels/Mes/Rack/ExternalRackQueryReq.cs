using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class ExternalRackQueryReq
    {
        /// <summary>
        /// 所属仓库编号
        /// </summary>
        public virtual string? WareHouseCode { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary
        public virtual string? Code { get; set; }
        /// <summary>
        /// 状态 0-禁用 1-启用
        /// </summary>
        public virtual int? Status { get; set; }
        /// <summary>
        /// 设备类别
        /// </summary>
        public virtual DeviceKind? DeviceKind { get; set; }
    }
}
