using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMain;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceMaintainService : BaseServiceWithoutTree<DeviceMaintain, DeviceMaintainDto, AddOrUpdateDeviceMaintainReq>, IDeviceMaintainService
    {
        private readonly IDeviceMaintainDomainService _deviceMainDomainService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public DeviceMaintainService(IDeviceMaintainDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
            _deviceMainDomainService = domainService;
        }
        /// <inheritdoc/>

        public async Task<bool> BulkInsert(List<DeviceMaintain> list)
        {
            await _deviceMainDomainService.BulkInsert(list);
            return true;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DeviceMaintainDto>>> GetList(GetDeviceMaintainListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<DeviceMaintainDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<DeviceMaintain>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.DeviceName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.DeviceName) && p.DeviceName.Contains(req.DeviceName));
            }
            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.DeviceCode) && p.DeviceCode.Contains(req.DeviceCode));
            }
            if (!string.IsNullOrEmpty(req.MaintainPerson))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.MaintainPerson) && p.MaintainPerson.Contains(req.MaintainPerson));
            }
            if (!string.IsNullOrEmpty(req.MaintainStatus))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.MaintainStatus) && p.MaintainStatus.Equals(req.MaintainStatus));
            }
            if (req.DeviceId > 0)
            {
                where = where.And(p => p.DeviceId == req.DeviceId);
            }
            if (req.MaintainId > 0)
            {
                where = where.And(p => p.MaintainId == req.MaintainId);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<DeviceMaintain>, List<DeviceMaintainDto>>(result.ToList());
            return Success(pageDto);
        }

        /// <summary>
        /// 导出数据到Excel
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeviceMaintainToExcelDto>> GetToExcelList()
        {
            List<DeviceMaintainToExcelDto> list = await _deviceMainDomainService.GetToExcelList();
            return list;
        }
    }
}
