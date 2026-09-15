using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Role;

namespace VgAutoDrill.Admin.Repository.Repository.Role
{
    public class RoleRepository : BaseRepository<SysRole>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
