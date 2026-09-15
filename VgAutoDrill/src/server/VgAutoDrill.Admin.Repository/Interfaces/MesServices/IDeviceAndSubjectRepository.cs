using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndSubject;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IDeviceAndSubjectRepository : IBaseRepository<DeviceAndSubject>
    {
        Task<PageList<DeviceAndSubjectDto>> GetList(GetDeviceAndSubjectListReq req);
    }
}
