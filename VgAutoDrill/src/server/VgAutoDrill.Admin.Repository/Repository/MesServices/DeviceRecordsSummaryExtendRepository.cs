using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceRecordsSummaryExtendRepository : BaseRepository<DeviceRecordsSummaryExtend>, IDeviceRecordsSummaryExtendRepository
    {
        public DeviceRecordsSummaryExtendRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<DeviceRecordsSummaryExtend>> GetListByDeviceCode(string deviceCode)
        {
            var query = DBClient.Queryable<DeviceRecordsSummaryExtend>();
            if (string.IsNullOrWhiteSpace(deviceCode))
            {
                return new List<DeviceRecordsSummaryExtend>();
            }
            query = query.Where(s => s.DeviceCode == deviceCode);
            var data = await query.OrderBy(s => s.Id).ToListAsync();
            return data;
        }

    }
}
