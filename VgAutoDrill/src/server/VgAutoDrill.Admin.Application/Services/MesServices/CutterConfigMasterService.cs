using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterConfigMaster;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class CutterConfigMasterService : BaseServiceWithoutTree<CutterConfigMaster, CutterConfigMasterDto, AddOrUpdateCutterConfigMasterReq>, ICutterConfigMasterService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public CutterConfigMasterService(ICutterConfigMasterDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<CutterConfigMasterDto>>> GetList(GetCutterConfigMasterListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<CutterConfigMasterDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<CutterConfigMaster>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.ConfigName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ConfigName) && p.ConfigName.Contains(req.ConfigName));
            }

            if (!string.IsNullOrEmpty(req.ConfigDesc))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ConfigDesc) && p.ConfigDesc.Contains(req.ConfigDesc));
            }

            if (!string.IsNullOrEmpty(req.DiaFileName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.DiaFileName) && p.DiaFileName.Contains(req.DiaFileName));
            }

            if (!string.IsNullOrEmpty(req.ProductCategoryName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProductCategoryName) && p.ProductCategoryName.Contains(req.ProductCategoryName));
            }

            if (!string.IsNullOrEmpty(req.ProductCategoryCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProductCategoryCode) && p.ProductCategoryCode.Contains(req.ProductCategoryCode));
            }

            if (req.ProductCategoryId > 0)
            {
                where = where.And(p => p.ProductCategoryId == req.ProductCategoryId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<CutterConfigMaster>, List<CutterConfigMasterDto>>(result.ToList());
            return Success(pageDto);
        }
    }
}
