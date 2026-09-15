using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class MesProcessRepository : BaseRepository<Process>, IMesProcessRepository
    {
        public MesProcessRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
