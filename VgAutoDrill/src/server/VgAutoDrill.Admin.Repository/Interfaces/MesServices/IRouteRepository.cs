using VgAutoDrill.Admin.Model.Entites.Mes;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IRouteRepository : IBaseRepository<Route>
    {
        Task<bool> CheckKeyProcess(long Id);

        Task<bool> ExsitTask(long Id);
    }
}
