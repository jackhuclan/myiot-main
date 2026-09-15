using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Menu;

namespace VgAutoDrill.Admin.Repository.Repository.Menu
{
    public class MenuRepository : BaseRepository<SysMenu>, IMenuRepository
    {
        public MenuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }

}
