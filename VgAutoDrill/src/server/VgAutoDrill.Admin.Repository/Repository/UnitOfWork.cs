using SqlSugar;
using SqlSugar.IOC;
using VgAutoDrill.Admin.Common.Log;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        public UnitOfWork()
        {
            _sqlSugarClient = DbScoped.SugarScope;
        }

        public SqlSugarScope GetDbClient()
        {
            return _sqlSugarClient as SqlSugarScope;
        }

        public void BeginTran()
        {
            GetDbClient().BeginTran();
        }

        public void CommitTran()
        {
            try
            {
                GetDbClient().CommitTran();
            }
            catch (Exception ex)
            {
                GetDbClient().RollbackTran();
                LoggerHelper.Error(ex, "事务提交异常");
                throw;
            }
        }

        public void RollbackTran()
        {
            GetDbClient().RollbackTran();
        }


    }
}
