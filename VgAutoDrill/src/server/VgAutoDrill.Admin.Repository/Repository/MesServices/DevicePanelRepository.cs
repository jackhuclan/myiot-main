using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DevicePanelRepository : BaseRepository<DevicePanel>, IDevicePanelRepository
    {
        public DevicePanelRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}

