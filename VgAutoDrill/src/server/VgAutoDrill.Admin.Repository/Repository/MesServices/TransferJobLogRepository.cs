using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class TransferJobLogRepository : BaseRepository<TransferJobLog>, ITransferJobLogRepository
    {
        public TransferJobLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IPageList<TransferJobLog>> GetList(GetTransferJobLogListReq req)
        {
            var query = DBClient.Queryable<TransferJobLog>();

            query = query.Where(p => p.IsDeleted == 0);

            if (req.MasterId > 0)
            {
                query = query.Where(p => p.MasterId == req.MasterId);
            }
            if (!string.IsNullOrEmpty(req.Message))
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.Message) && p.Message.Contains(req.Message));
            }

            query.OrderByDescending(p => p.Id);

            RefAsync<int> totalCount = 0;
            var data = await query.Select(p => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select(p => p).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<TransferJobLog>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

    }
}