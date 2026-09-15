using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmType;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AlarmSettingService : BaseServiceWithTree<AlarmSetting, AlarmSettingTreeDto, AlarmSettingDto, AddOrUpdateAlarmSettingReq>, IAlarmSettingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public AlarmSettingService(IAlarmSettingDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<List<AlarmSettingTreeDto>>> GetTreeList()
        {
            var list = await _domainService.QueryAsync(q => q.IsDeleted == 0 && q.Status == 1, q => q.Id, SqlSugar.OrderByType.Asc);
            var result = new ResponseDto<List<AlarmSettingTreeDto>>();
            if (list == null || !list.Any())
            {
                return result;
            }

            var allCodes = AddChildN(list, 0);

            result.Data = allCodes;

            return result;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<AlarmSettingDto>>> GetList(GetAlarmSettingListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<AlarmSettingDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<AlarmSetting>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }
            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }
            if (!string.IsNullOrEmpty(req.EventName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.EventName) && p.EventName.Contains(req.EventName));
            }
            if (!string.IsNullOrEmpty(req.NotifyWayIds))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.NotifyWayIds) && p.NotifyWayIds.Contains(req.NotifyWayIds));
            }
            if (!string.IsNullOrEmpty(req.NotifyWayNames))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.NotifyWayNames) && p.NotifyWayNames.Contains(req.NotifyWayNames));
            }
            if (req.EventId > 0)
            {
                where = where.And(p => p.EventId == req.EventId);
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }
            if (req.AlarmLevel > -1)
            {
                where = where.And(p => p.AlarmLevel == req.AlarmLevel);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<AlarmSetting>, List<AlarmSettingDto>>(result.ToList());
            return Success<PageDto<AlarmSettingDto>>(pageDto);
        }
    }
}
