
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class WorkOrderAndPanelRepository : BaseRepository<WorkOrderAndPanel>, IWorkOrderAndPanelRepository
    {
        public WorkOrderAndPanelRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
