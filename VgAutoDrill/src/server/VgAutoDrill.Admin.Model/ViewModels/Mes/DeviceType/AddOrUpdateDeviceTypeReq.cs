namespace VgAutoDrill.Admin.Model.ViewModels.Mes.EquipmentType
{
    public class AddOrUpdateDeviceTypeReq : BaseAddOrUpdateWithTreeDto
    {
        /// <summary>
        /// 是否属于生产设备
        /// </summary>
        public virtual byte IsManufacture { get; set; } = 0;
    }
}
