using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class CutterGroupDetailRepository : BaseRepository<Model.Entites.Mes.CutterGroupDetail>, ICutterGroupDetailRepository
    {
        public CutterGroupDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }




    }
}
