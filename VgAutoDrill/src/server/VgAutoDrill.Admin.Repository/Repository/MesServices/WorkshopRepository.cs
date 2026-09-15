using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class WorkshopRepository : BaseRepository<Workshop>, IWorkshopRepository
    {
        public WorkshopRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
