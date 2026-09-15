namespace VgAutoDrill.Admin.Model.ViewModels.Req.Equipment
{
    public class GetEquipmentTypeListReq : Page
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;
    }
}
