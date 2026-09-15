using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanelHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DevicePanelHistoryRepository : BaseRepository<DevicePanelHistory>, IDevicePanelHistoryRepository
    {
        public DevicePanelHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IPageList<DevicePanelHistory>> GetList(GetDevicePanelHistoryListReq req)
        {
            var query = DBClient.Queryable<DevicePanelHistory>();

            query = query.Where((his) => his.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                query = query.Where((his) => !string.IsNullOrEmpty(his.BatchCode) && his.BatchCode.Contains(req.BatchCode));
            }
            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                query = query.Where((his) => !string.IsNullOrEmpty(his.DeviceCode) && his.DeviceCode.Contains(req.DeviceCode));
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                query = query.Where((his) => !string.IsNullOrEmpty(his.ItemCode) && his.ItemCode.Contains(req.ItemCode));
            }
            if (!string.IsNullOrEmpty(req.LotId))
            {
                query = query.Where((his) => !string.IsNullOrEmpty(his.LotId) && his.LotId.Contains(req.LotId));
            }
            if (!string.IsNullOrEmpty(req.PanelCode))
            {
                query = query.Where((his) => !string.IsNullOrEmpty(his.PanelCode) && his.PanelCode.Contains(req.PanelCode));
            }
            if (!string.IsNullOrEmpty(req.SiloCode))
            {
                query = query.Where((his) => !string.IsNullOrEmpty(his.SiloCode) && his.SiloCode.Contains(req.SiloCode));
            }
            if (req.ProductStatus > 0)
            {
                query = query.Where((his) => his.ProductStatus == req.ProductStatus);
            }

            query = query.OrderByDescending((his) => his.CreateTime);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((his) => his).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((his) => his).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var newData = data.DistinctBy(his => new { his.PanelCode, his.DeviceCode });

            totalCount = newData != null ? newData.Count() : totalCount;

            var list = new PageList<DevicePanelHistory>(newData == null ? data : newData, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 根据panelCode集合查询对应的历史数据
        /// </summary>
        /// <param name="panelCodeList"></param>
        /// <returns></returns>
        public async Task<List<DevicePanelHistory>> GetListByPanel(List<string?> panelCodeList, GetPanelListReq req)
        {
            if (panelCodeList == null || panelCodeList.Count == 0)
            {
                return new List<DevicePanelHistory>();
            }

            var query = DBClient.Queryable<DevicePanelHistory>();

            query = query.Where((his) => his.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.LocationCode))
            {
                query = query.Where((his) => !string.IsNullOrEmpty(his.LocationCode)
                && his.LocationCode.ToLower().Contains(req.LocationCode.ToLower()));
            }

            query = query.Where((his) => !string.IsNullOrEmpty(his.PanelCode) && panelCodeList.Contains(his.PanelCode));

            query = query.OrderByDescending((his) => his.CreateTime);

            var data = await query.Select((his) => his).ToListAsync();

            //var newData = data.DistinctBy(his => new { his.PanelCode, his.DeviceCode });

            return data.ToList();
        }
    }
}