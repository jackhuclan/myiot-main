using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
