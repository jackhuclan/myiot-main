namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStock
{
    public class MaterialStockFullPropertiesTreeDto : MaterialStockDto
    {
        /// <summary>
        /// 子对象
        /// </summary>
        public virtual List<MaterialStockFullPropertiesTreeDto> Children { set; get; } = new List<MaterialStockFullPropertiesTreeDto>();
    }
}
