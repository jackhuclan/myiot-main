using SqlSugar;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class PartitionRepository : BaseRepository<Partition>, IPartitionRepository
    {
        public PartitionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<Partition> QueryWithSettingByID(object objId)
        {
            return await DBClient.Queryable<Partition>().Includes(s => s.Settings).InSingleAsync(objId);
        }

        public async Task<bool> AddOrUpdateWithSetting(Partition data)
        {
            return await DBClient.UpdateNav(data, new UpdateNavRootOptions()
            {
                IsInsertRoot = true//主表不存在就插入,存在就更新
            }).Include(s => s.Settings).ExecuteCommandAsync();
        }
    }
}
