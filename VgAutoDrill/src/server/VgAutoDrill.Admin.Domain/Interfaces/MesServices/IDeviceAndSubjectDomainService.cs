using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndSubject;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDeviceAndSubjectDomainService : IBaseDomainService<DeviceAndSubject>
    {
        Task<PageDto<DeviceAndSubjectDto>> GetList(GetDeviceAndSubjectListReq req);
    }
}

