namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem
{
    public class ItemFullPropertiesTreeDto : ItemDto
    {
        /// <summary>
        /// 子对象
        /// </summary>
        public virtual List<ItemFullPropertiesTreeDto> Children { set; get; } = new List<ItemFullPropertiesTreeDto>();
    }
}
