namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndSubject
{
    public class AddOrUpdateDeviceAndSubjectReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int? DeviceId { get; set; }

        /// <summary>
        /// 点检保养项目id集合
        /// </summary>
        public List<int>? SubjectIds { get; set; }
    }
}
