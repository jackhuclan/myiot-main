using VgAutoDrill.Admin.Model.Entites.Mes;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IRouteDomainService : IBaseDomainService<Route>
    {
        Task<bool> CheckKeyProcess(long Id);

        /// <summary>
        /// 校验是否存在已提交的任务与该工艺路线关联
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<bool> ExsitTask(long Id);
    }
}
