using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProduceTask;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 生产任务表
    /// </summary>
    public class ProduceTaskService : BaseServiceWithoutTree<ProduceTask, ProduceTaskDto, AddOrUpdateProduceTaskReq>, IProduceTaskService
    {
        private readonly IProduceTaskHistoryDomainService _historyDomainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="ProduceTaskHistoryDomainService"></param>
        /// <param name="mapper"></param>
        public ProduceTaskService(IProduceTaskDomainService domainService,
            IProduceTaskHistoryDomainService ProduceTaskHistoryDomainService,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _historyDomainService = ProduceTaskHistoryDomainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<ProduceTaskDto>>> GetList(GetProduceTaskListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<ProduceTaskDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<ProduceTask>();
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
            pageDto.List = _mapper.Map<List<ProduceTask>, List<ProduceTaskDto>>(result.ToList());
            return Success<PageDto<ProduceTaskDto>>(pageDto);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateProduceTaskReq req)
        {
            var model = _mapper.Map<ProduceTask>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);

            //同步添加到历史表中
            ProduceTaskHistory hisModel = new ProduceTaskHistory();
            hisModel.CreateTime = DateTime.Now;
            hisModel.CreatorId = UserId;
            hisModel.Status = (int)DataStatusEnum.Enable;
            hisModel.ProcessCode = req.ProcessCode;
            hisModel.ProcessName = req.ProcessName;
            hisModel.TaskCode = req.TaskCode;
            hisModel.TaskStatus = req.TaskStatus;
            hisModel.NowWadCount = req.NowWadCount;
            hisModel.WorkOrderCode = req.WorkOrderCode;
            hisModel.WorkOrderName = req.WorkOrderName;
            hisModel.ItemCode = req.ItemCode;
            hisModel.ItemName = req.ItemName;
            await _historyDomainService.Add(hisModel);

            return Success();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateProduceTaskReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }
            var model = _mapper.Map<ProduceTask>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);

            //同步添加到历史表中
            ProduceTaskHistory hisModel = new ProduceTaskHistory();
            hisModel.CreateTime = DateTime.Now;
            hisModel.CreatorId = UserId;
            hisModel.Status = (int)DataStatusEnum.Enable;
            hisModel.ProcessCode = req.ProcessCode;
            hisModel.ProcessName = req.ProcessName;
            hisModel.TaskCode = req.TaskCode;
            hisModel.TaskStatus = req.TaskStatus;
            hisModel.NowWadCount = req.NowWadCount;
            hisModel.WorkOrderCode = req.WorkOrderCode;
            hisModel.WorkOrderName = req.WorkOrderName;
            hisModel.ItemCode = req.ItemCode;
            hisModel.ItemName = req.ItemName;
            await _historyDomainService.Add(hisModel);

            return Success();
        }
    }
}

