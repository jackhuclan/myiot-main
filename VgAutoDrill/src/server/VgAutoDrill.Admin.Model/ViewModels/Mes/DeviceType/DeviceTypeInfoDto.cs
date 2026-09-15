namespace VgAutoDrill.Admin.Model.ViewModels.Mes.EquipmentType
{
    public class DeviceTypeInfoDto
    {   /// <summary>
        ///  
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 部门名称 
        ///</summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 父对象Id(0表示是根对象) 
        ///</summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        public string Status { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间 
        ///</summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 是否属于生产设备
        /// </summary>
        public virtual byte IsManufacture { get; set; }
    }
}
