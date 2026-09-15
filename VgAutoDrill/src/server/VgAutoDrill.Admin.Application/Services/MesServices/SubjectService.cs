using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Subject;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 点检保养项目
    /// </summary>
    public class SubjectService : BaseServiceWithTree<Subject, SubjectTreeDto, SubjectDto, AddOrUpdateSubjectReq>, ISubjectService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public SubjectService(ISubjectDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {

        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<SubjectDto>>> GetList(GetSubjectListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var pageDto = new PageDto<SubjectDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<Subject>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (!string.IsNullOrEmpty(req.SubjectType))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SubjectType) && p.SubjectType.Contains(req.SubjectType));
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, p => p.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<Subject>, List<SubjectDto>>(result.ToList());
            return Success<PageDto<SubjectDto>>(pageDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<List<SubjectTreeDto>>> GetTreeList()
        {
            var list = await _domainService.QueryAsync(q => q.IsDeleted == 0 && q.Status == 1, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<SubjectTreeDto>>();
            if (list == null || !list.Any())
            {
                return result;
            }

            var allCodes = AddChildN(list, 0);

            result.Data = allCodes;

            return result;
        }
    }
}

