using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceEvent;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceEventService : BaseServiceWithoutTree<EventDefine, EventDefineDto, AddOrUpdateEventDefineReq>, IEventDefineService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public DeviceEventService(IEventDefineDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {

        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<EventDefineDto>>> GetDeviceEventList(GetEventDefineListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<EventDefineDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<EventDefine>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.EventId))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.EventId) && p.EventId.Contains(req.EventId));
            }
            if (!string.IsNullOrEmpty(req.EventName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.EventName) && p.EventName.Contains(req.EventName));
            }
            if (req.EventLevel > -1)
            {
                where = where.And(p => p.EventLevel == req.EventLevel);
            }
            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.EventId, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<EventDefine>, List<EventDefineDto>>(result.ToList());
            return Success<PageDto<EventDefineDto>>(pageDto);
        }

    }
}
