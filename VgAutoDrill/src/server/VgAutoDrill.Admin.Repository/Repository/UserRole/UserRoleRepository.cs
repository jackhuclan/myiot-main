using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.UserRole;

namespace VgAutoDrill.Admin.Repository.Repository.UserRole
{
    public class UserRoleRepository : BaseRepository<SysUserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
