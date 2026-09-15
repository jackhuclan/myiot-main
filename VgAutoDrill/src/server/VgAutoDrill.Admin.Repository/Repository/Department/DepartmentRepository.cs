using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Department;

namespace VgAutoDrill.Admin.Repository.Repository.Department
{
    public class DepartmentRepository : BaseRepository<SysDepartment>, IDepartmentRepository
    {
        public DepartmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
