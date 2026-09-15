using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ExternalTaskRepository : BaseRepository<Model.Entites.Mes.WorkTask>, IExternalTaskRepository
    {
        public ExternalTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
