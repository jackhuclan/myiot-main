using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFile;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ItemAtpFileRepository : BaseRepository<ItemAtpFile>, IItemAtpFileRepository
    {
        public ItemAtpFileRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IPageList<ItemAtpFile>> GetList(GetItemAtpFileListReq req)
        {
            var query = DBClient.Queryable<ItemAtpFile, ItemType>
                ((f, i) => new object[]
                    {
                        JoinType.Left, f.ItemTypeId == i.Id
                    });

            query = query.Where((f, i) => f.IsDeleted == 0 && i.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.AtpFileName))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.AtpFileName) && f.AtpFileName.Contains(req.AtpFileName));
            }
            if (!string.IsNullOrEmpty(req.ATPParameters))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.ATPParameters) && f.ATPParameters.Contains(req.ATPParameters));
            }
            if (!string.IsNullOrEmpty(req.ItemName))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.ItemName) && f.ItemName.Contains(req.ItemName));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((f, i) => !string.IsNullOrEmpty(f.ItemCode) && f.ItemCode.Contains(req.ItemCode));
            }
            if (req.IsGenerated != null)
            {
                query = query.Where((f, i) => f.IsGenerated == req.IsGenerated);
            }
            if (req.ItemId > 0)
            {
                query = query.Where((f, i) => f.ItemId == req.ItemId);
            }
            if (req.ItemDrillFileId > 0)
            {
                query = query.Where((f, i) => f.ItemDrillFileId == req.ItemDrillFileId);
            }
            if (req.CutterConfigMasterId > 0)
            {
                query = query.Where((f, i) => f.CutterConfigMasterId == req.CutterConfigMasterId);
            }

            if (req.ItemTypeId > 0)
            {
                var sql = $"select id from t_item_type where find_in_set({req.ItemTypeId},ancestors)";
                var typeIdList = DBClient.SqlQueryable<ItemType>(sql).Select(d => d.Id).ToList();

                query = query.Where((f, i) => f.ItemTypeId == req.ItemTypeId
                            || (f.ItemTypeId.HasValue && typeIdList.Contains(f.ItemTypeId.Value)));
            }

            RefAsync<int> totalCount = 0;
            var data = await query.Select((f, i) => f).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((f, i) => f).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<ItemAtpFile>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

    }
}