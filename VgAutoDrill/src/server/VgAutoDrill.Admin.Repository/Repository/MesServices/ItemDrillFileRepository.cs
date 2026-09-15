using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFile;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    internal class ItemDrillFileRepository : BaseRepository<ItemDrillFile>, IItemDrillFileRepository
    {
        public ItemDrillFileRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<IPageList<ItemDrillFile>> GetList(GetItemDrillFileListReq req)
        {
            var query = DBClient.Queryable<ItemDrillFile, ItemType>
                ((p, i) => new object[]
                    {
                        JoinType.Left, p.ItemTypeId == i.Id
                    });

            query = query.Where((p, i) => p.IsDeleted == 0 && i.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.DrillFileName))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.DrillFileName) && p.DrillFileName.Contains(req.DrillFileName));
            }
            if (!string.IsNullOrEmpty(req.ItemName))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.ItemName) && p.ItemName.Contains(req.ItemName));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((p, i) => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }
            if (req.ItemId > 0)
            {
                query = query.Where((p, i) => p.ItemId == req.ItemId);
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

            var list = new PageList<ItemDrillFile>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

    }
}
