
using VgAutoDrill.Admin.Domain.Interfaces.Role;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Role;

namespace VgAutoDrill.Admin.Domain.Services.Role
{
    public class RoleDomainService : BaseDomainService<SysRole>, IRoleDomainService
    {
        private readonly IRoleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RoleDomainService(IUnitOfWork unitOfWork,
            IRoleRepository repository)
        {
            this._repository = repository;
            base._baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        ///�����û�id��ȡ��ɫ�б�
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<SysRole> GetRolesByUserID(int userId)
        {
            var db = _unitOfWork.GetDbClient();
            var model = await db.Queryable<SysRole>()
            .InnerJoin<SysUserRole>((s, sm) => s.Id == sm.RoleId)
            .Where((s, sm) => s.IsDeleted == 0 && sm.UserId == userId)
            .Select(s => s)
            .FirstAsync();
            return model;
        }
    }
}
