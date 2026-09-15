using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DrillTaskRepository : BaseRepository<DrillWorkOrder>, IDrillTaskRepository
    {
        public DrillTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
