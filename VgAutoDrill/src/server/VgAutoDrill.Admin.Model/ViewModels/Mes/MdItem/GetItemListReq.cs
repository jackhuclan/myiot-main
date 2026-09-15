using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem
{
    public class GetItemListReq : Page
    {
        public virtual string? Name { get; set; }

        public virtual string? Code { get; set; }

        public virtual int? ItemOrProduct { get; set; }

        public virtual int ItemTypeId { get; set; }
        /// <summary>
        /// 库房ID
        /// </summary>
        public virtual int? WarehouseId { get; set; }
        /// <summary>
        /// 产品大类ID
        /// </summary>
        public virtual int? ProductCategoryId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>

        public virtual int Status { set; get; } = -1;

        /// <summary>
        /// 数据排序方式
        /// </summary>
        public QueryOrderByEnum? QueryOrderBy { get; set; }
    }
}
