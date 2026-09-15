using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class LocationPanelRepository : BaseRepository<ScheduleLocationPanel>, ILocationPanelRepository
    {
        public LocationPanelRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}
