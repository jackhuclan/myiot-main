namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItemType
{
    public class AddOrUpdateItemTypeReq : BaseAddOrUpdateWithTreeDto, IDtoWithTree
    {
        /// <summary>
        /// 物料1产品2
        /// Item Or Product
        /// </summary>
        public virtual int? ItemOrProduct { get; set; }
    }
}