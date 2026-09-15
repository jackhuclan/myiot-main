using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class EventDefineRepository : BaseRepository<EventDefine>, IEventDefineRepository
    {
        public EventDefineRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
