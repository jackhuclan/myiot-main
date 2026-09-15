using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProduceTaskHistory;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 生产任务历史表
    /// </summary>
    public class ProduceTaskHistoryService : BaseServiceWithoutTree<ProduceTaskHistory, ProduceTaskHistoryDto, AddOrUpdateProduceTaskHistoryReq>, IProduceTaskHistoryService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public ProduceTaskHistoryService(IProduceTaskHistoryDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {

        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ProduceTaskHistoryDto>>> GetList(GetProduceTaskHistoryListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ProduceTaskHistoryDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<ProduceTaskHistory>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.TaskCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.TaskCode) && p.TaskCode.Contains(req.TaskCode));
            }

            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.Contains(req.WorkOrderCode));
            }

            if (!string.IsNullOrEmpty(req.WorkOrderName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkOrderName) && p.WorkOrderName.Contains(req.WorkOrderName));
            }

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }

            if (!string.IsNullOrEmpty(req.ItemName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemName) && p.ItemName.Contains(req.ItemName));
            }

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.Contains(req.ProcessCode));
            }

            if (!string.IsNullOrEmpty(req.ProcessName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProcessName) && p.ProcessName.Contains(req.ProcessName));
            }

            if (!string.IsNullOrEmpty(req.TaskStatus))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.TaskStatus) && p.TaskStatus.ToUpper().Equals(req.TaskStatus.ToUpper()));
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<ProduceTaskHistory>, List<ProduceTaskHistoryDto>>(result.ToList());
            return Success<PageDto<ProduceTaskHistoryDto>>(pageDto);
        }

    }
}
