namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceParameter
{
    public class GetDeviceParameterListReq : Page
    {
        public int DeviceTypeId { get; set; }

        public int DeviceId { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { set; get; }

        /// <summary    
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;
    }
}
