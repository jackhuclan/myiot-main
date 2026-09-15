using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class RackRepository : BaseRepository<Rack>, IRackRepository
    {
        public RackRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
