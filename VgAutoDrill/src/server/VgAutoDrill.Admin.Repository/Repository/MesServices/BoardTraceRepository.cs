using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class BoardTraceRepository : BaseRepository<TracePanel>, IProBoardTraceRepository
    {
        public BoardTraceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IPageList<TracePanel>> GetPanelList(GetPanelListReq req)
        {
            var query = DBClient.Queryable<TracePanel, DevicePanelHistory>
                   ((p, ph) => new object[]
                   {
                      JoinType.Left , p.PanelCode == ph.PanelCode
                   });

            query = query.Where((p, ph) => p.IsDeleted == 0 && ph.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((p, ph) => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }

            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                query = query.Where((p, ph) => !string.IsNullOrEmpty(p.BatchCode) && p.BatchCode.Contains(req.BatchCode));
            }

            if (!string.IsNullOrEmpty(req.PanelCode))
            {
                query = query.Where((p, ph) => !string.IsNullOrEmpty(p.PanelCode) && p.PanelCode.Contains(req.PanelCode));
            }

            if (req.ProductStatusList != null && req.ProductStatusList.Count > 0)
            {
                query = query.Where((p, ph) => req.ProductStatusList.Contains(p.ProductStatus));
            }

            if (req.ProductStatus.HasValue)
            {
                query = query.Where((p, ph) => p.ProductStatus == req.ProductStatus);
            }

            if (req.Status > -1)
            {
                query = query.Where((p, ph) => p.Status == req.Status);
            }

            if (!string.IsNullOrEmpty(req.LocationCode))
            {
                query = query.Where((p, ph) => !string.IsNullOrEmpty(ph.LocationCode)
                && ph.LocationCode.ToLower().Contains(req.LocationCode.ToLower()));
            }

            query = query.OrderByDescending((p, ph) => p.CreateTime);

            RefAsync<int> totalCount = 0;
            var data = await query.Distinct().Select((p, ph) => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((p, ph) => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<TracePanel>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }
    }
}
