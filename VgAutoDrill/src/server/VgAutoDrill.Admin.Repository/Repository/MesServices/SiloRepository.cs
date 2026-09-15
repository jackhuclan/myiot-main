using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class SiloRepository : BaseRepository<Silo>, ISiloRepository
    {
        public SiloRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
