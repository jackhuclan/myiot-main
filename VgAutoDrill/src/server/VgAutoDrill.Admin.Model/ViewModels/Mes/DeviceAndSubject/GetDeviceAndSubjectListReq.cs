namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndSubject
{
    public class GetDeviceAndSubjectListReq : Page
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int? DeviceId { get; set; }

        /// <summary>
        /// 点检保养项目id
        /// </summary>
        public int? SubjectId { get; set; }
    }
}
