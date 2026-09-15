using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceServiceInvocationRepository : BaseRepository<DeviceServiceInvocation>, IDeviceServiceInvocationRepository
    {
        public DeviceServiceInvocationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
