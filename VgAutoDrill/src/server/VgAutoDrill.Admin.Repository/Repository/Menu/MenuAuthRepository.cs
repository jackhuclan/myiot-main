using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Menu;

namespace VgAutoDrill.Admin.Repository.Repository.Menu
{
    public class MenuAuthRepository : BaseRepository<SysMenuAuth>, IMenuAuthRepository
    {
        public MenuAuthRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }

}
