using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class AgvRestRepository : BaseRepository<AgvRest>, IAgvRestRepository
    {
        public AgvRestRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
