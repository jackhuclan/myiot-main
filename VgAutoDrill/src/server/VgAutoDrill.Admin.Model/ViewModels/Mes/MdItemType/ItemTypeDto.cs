namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItemType
{
    public class ItemTypeDto : BaseDtoWithTreeDto
    {
        /// <summary>
        /// 物料1产品2
        /// Item Or Product
        /// </summary>
        public virtual int? ItemOrProduct { get; set; }

        /// <summary>
        /// 是否系统自带
        /// </summary>
        public virtual string? IsSystem { get; set; }

    }
}
