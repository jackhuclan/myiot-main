namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords
{
    public class GetDeviceTemporaryMaintenanceRecordsListReq : Page
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        public virtual DateTime? StartTime { get; set; }

        public virtual DateTime? EndTime { get; set; }
    }
}
