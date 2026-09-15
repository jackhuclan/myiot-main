using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMaintainDetail;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceMaintainDetailService : BaseServiceWithoutTree<DeviceMaintainDetail, DeviceMaintainDetailDto, AddOrUpdateDeviceMaintainDetailReq>, IDeviceMaintainDetailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public DeviceMaintainDetailService(IDeviceMaintainDetailDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DeviceMaintainDetailDto>>> GetList(GetDeviceMaintainDetailListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DeviceMaintainDetailDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DeviceMaintainDetail>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.BetterSteps))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.BetterSteps) && p.BetterSteps.Contains(req.BetterSteps));
            }
            if (!string.IsNullOrEmpty(req.MaintainResult))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.MaintainResult) && p.MaintainResult.Contains(req.MaintainResult));
            }
            if (!string.IsNullOrEmpty(req.SubjectCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SubjectCode) && p.SubjectCode.Contains(req.SubjectCode));
            }
            if (!string.IsNullOrEmpty(req.SubjectName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.SubjectName) && p.SubjectName.Contains(req.SubjectName));
            }
            if (req.DeviceId > 0)
            {
                where = where.And(p => p.DeviceId == req.DeviceId);
            }
            if (req.SubjectId > 0)
            {
                where = where.And(p => p.SubjectId == req.SubjectId);
            }
            if (req.MasterId > 0)
            {
                where = where.And(p => p.MasterId == req.MasterId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DeviceMaintainDetail>, List<DeviceMaintainDetailDto>>(result.ToList());
            return Success(pageDto);
        }
    }
}
