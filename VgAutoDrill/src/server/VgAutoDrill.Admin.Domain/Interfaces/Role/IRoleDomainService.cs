using VgAutoDrill.Admin.Model.Entites;

namespace VgAutoDrill.Admin.Domain.Interfaces.Role
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRoleDomainService : IBaseDomainService<SysRole>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<SysRole> GetRolesByUserID(int userId);
    }
}