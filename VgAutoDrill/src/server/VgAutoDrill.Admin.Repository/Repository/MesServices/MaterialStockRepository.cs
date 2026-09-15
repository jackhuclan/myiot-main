using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStock;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class MaterialStockRepository : BaseRepository<MaterialStock>, IMaterialStockRepository
    {
        public MaterialStockRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<IPageList<MaterialStock>> GetList(GetMaterialStockListReq req, bool isTree = false)
        {
            var query = DBClient.Queryable<MaterialStock, ItemType>
                ((p, i) => new object[]
                    {
                        JoinType.Left, p.ItemTypeId == i.Id
                    });

            query = query.Where((p, i) => p.IsDeleted == 0 && i.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.SiloCode) && p.SiloCode.Contains(req.SiloCode));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.ItemName))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.ItemName) && p.ItemName.Contains(req.ItemName));
            }

            if (!string.IsNullOrEmpty(req.WarehouseCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.WarehouseCode) && p.WarehouseCode.Contains(req.WarehouseCode));
            }
            if (!string.IsNullOrEmpty(req.WarehouseName))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.WarehouseName) && p.WarehouseName.Contains(req.WarehouseName));
            }

            if (isTree)
            {
                //返回树形列表，每页的数量显示的是父类数量，子类不计算在每页数量中
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.InOrOut) && !p.InOrOut.ToLower().Equals("out"));
            }

            if (req.ItemTypeId > 0)
            {
                var sql = $"select id from t_item_type where find_in_set({req.ItemTypeId},ancestors)";
                var typeIdList = DBClient.SqlQueryable<ItemType>(sql).Select(d => d.Id).ToList();

                query = query.Where((p, i) => p.ItemTypeId == req.ItemTypeId
                            || (p.ItemTypeId.HasValue && typeIdList.Contains(p.ItemTypeId.Value)));
            }

            RefAsync<int> totalCount = 0;
            var data = await query.Select((p, i) => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((p, i) => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<MaterialStock>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }
    }
}
