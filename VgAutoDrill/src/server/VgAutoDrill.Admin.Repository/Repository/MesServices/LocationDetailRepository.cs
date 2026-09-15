using SqlSugar;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class LocationDetailRepository : BaseRepository<LocationDetail>, ILocationDetailRepository
    {
        public LocationDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<List<LocationDetail>> GetByCodeAsync(string code)
        {
            return await DBClient.Queryable<LocationDetail>()
                .Where(p => p.Code == code)
                .ToListAsync();
        }

        public async Task<List<(LocationDetail LocationDetail, string SiloCode)>> GetByCodeWithSiloCodeAsync(string code)
        {
            var locationDetails = await GetByCodeAsync(code);
            var result = new List<(LocationDetail LocationDetail, string SiloCode)>();

            foreach (var locationDetail in locationDetails)
            {
                var siloCode = await DBClient.Ado.GetStringAsync(
                    "SELECT COALESCE(silo_code, '') FROM t_rack WHERE code = @code",
                    new { code = locationDetail.Code }
                );
                result.Add((locationDetail, siloCode ?? string.Empty));
            }

            return result;
        }
    }
}