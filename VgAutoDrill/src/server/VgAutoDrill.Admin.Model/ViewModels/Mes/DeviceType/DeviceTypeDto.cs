using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.EquipmentType
{
    public class DeviceTypeDto
    {
        /// <summary>
        ///  主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public virtual string? Code { get; set; }
        /// <summary>
        /// 父对象Id(0表示是根对象) 
        ///</summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 所有层级父节点
        /// </summary>
        public virtual string? Ancestors { get; set; }
        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        public DataStatusEnum Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 是否属于生产设备
        /// </summary>
        public virtual byte IsManufacture { get; set; }
    }
}
