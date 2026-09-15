using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockOverview;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 库存总览表
    /// </summary>
    public class MaterialStockOverviewService : BaseServiceWithoutTree<MaterialStockOverview, MaterialStockOverviewDto, AddOrUpdateMaterialStockOverviewReq>, IMaterialStockOverviewService
    {
        private readonly IMaterialStockOverviewHistoryDomainService _historyDomainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="materialStockOverviewHistoryDomainService"></param>
        /// <param name="mapper"></param>
        public MaterialStockOverviewService(IMaterialStockOverviewDomainService domainService,
            IMaterialStockOverviewHistoryDomainService materialStockOverviewHistoryDomainService,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _historyDomainService = materialStockOverviewHistoryDomainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<MaterialStockOverviewDto>>> GetList(GetMaterialStockOverviewListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<MaterialStockOverviewDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<MaterialStockOverview>();
            where = where.And(p => p.IsDeleted == 0);

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

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<MaterialStockOverview>, List<MaterialStockOverviewDto>>(result.ToList());
            return Success<PageDto<MaterialStockOverviewDto>>(pageDto);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateMaterialStockOverviewReq req)
        {
            var model = _mapper.Map<MaterialStockOverview>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);

            //同步添加到历史表中
            MaterialStockOverviewHistory hisModel = new MaterialStockOverviewHistory();
            hisModel.CreateTime = DateTime.Now;
            hisModel.CreatorId = UserId;
            hisModel.Status = (int)DataStatusEnum.Enable;
            hisModel.ProcessCode = req.ProcessCode;
            hisModel.ProcessName = req.ProcessName;
            hisModel.SumOnhand = req.SumOnhand;
            hisModel.SumPlan = req.SumPlan;
            hisModel.UsableCount = req.UsableCount;
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
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateMaterialStockOverviewReq req)
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
            var model = _mapper.Map<MaterialStockOverview>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);

            //同步添加到历史表中
            MaterialStockOverviewHistory hisModel = new MaterialStockOverviewHistory();
            hisModel.CreateTime = DateTime.Now;
            hisModel.CreatorId = UserId;
            hisModel.Status = (int)DataStatusEnum.Enable;
            hisModel.ProcessCode = req.ProcessCode;
            hisModel.ProcessName = req.ProcessName;
            hisModel.SumOnhand = req.SumOnhand;
            hisModel.SumPlan = req.SumPlan;
            hisModel.UsableCount = req.UsableCount;
            hisModel.ItemCode = req.ItemCode;
            hisModel.ItemName = req.ItemName;
            await _historyDomainService.Add(hisModel);

            return Success();
        }
    }
}

