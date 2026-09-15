namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItemType
{
    public class GetItemTypeListReq : Page
    {
        public virtual string? Name { get; set; }

        public virtual string? Code { get; set; }

        public virtual int? ItemOrProduct { get; set; }

        /// <summary>
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;
    }
}
