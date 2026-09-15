namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItemType
{
    public class ItemTypeFullPropertiesTreeDto : ItemTypeDto
    {
        /// <summary>
        /// 子对象
        /// </summary>
        public virtual List<ItemTypeFullPropertiesTreeDto> Children { set; get; } = new List<ItemTypeFullPropertiesTreeDto>();
    }
}
