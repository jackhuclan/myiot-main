using VgAutoDrill.Admin.Model.Entites.Mes;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDeviceRecordsSummaryExtendDomainService : IBaseDomainService<DeviceRecordsSummaryExtend>
    {
        Task<List<DeviceRecordsSummaryExtend>> GetListByDeviceCode(string deviceCode);


    }
}
