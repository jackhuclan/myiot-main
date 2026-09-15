using VgAutoDrill.Admin.Model.Entites.Mes;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IDeviceRecordsSummaryExtendRepository : IBaseRepository<DeviceRecordsSummaryExtend>
    {

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<List<DeviceRecordsSummaryExtend>> GetListByDeviceCode(string deviceCode);

    }
}
