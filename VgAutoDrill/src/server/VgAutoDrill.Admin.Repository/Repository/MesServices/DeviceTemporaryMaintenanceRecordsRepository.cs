using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceTemporaryMaintenanceRecordsRepository : BaseRepository<DeviceTemporaryMaintenanceRecords>, IDeviceTemporaryMaintenanceRecordsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public DeviceTemporaryMaintenanceRecordsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageList<DeviceTemporaryMaintenanceRecords>> GetList(GetDeviceTemporaryMaintenanceRecordsListReq req)
        {
            var query = DBClient.Queryable<DeviceTemporaryMaintenanceRecords>();
            if (!string.IsNullOrWhiteSpace(req.DeviceCode))
            {
                query = query.Where(s => !string.IsNullOrEmpty(s.DeviceCode) && s.DeviceCode.ToLower() == req.DeviceCode.ToLower());
            }
            if (req.StartTime != null && req.EndTime != null)
            {
                query = query.Where(s => s.StartTime >= req.StartTime && s.EndTime <= req.EndTime && s.StartTime <= s.EndTime);
            }
            var totalCount = query.Count();
            var list = await query.OrderByDescending(s => s.Id).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            var data = new PageList<DeviceTemporaryMaintenanceRecords>(list, req.PageNum, req.PageSize, totalCount);
            return data;
        }
    }
}
