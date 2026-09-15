using System.Collections.Generic;
using System.Threading.Tasks;
using VgAutoDrill.Admin.Domain.Interfaces;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using System;
using System.Linq;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    /// <summary>
    /// 料仓板料追溯
    /// </summary>
    public class SiloPanelTraceDomainService : BaseDomainService<SiloPanelTrace>, ISiloPanelTraceDomainService
    {
        private readonly ISiloPanelTraceRepository _repository;

        public SiloPanelTraceDomainService(ISiloPanelTraceRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
        }

        /// <summary>
        /// 获取料仓板料追溯列表
        /// </summary>
        /// <param name="request">查询请求参数</param>
        /// <returns>符合条件的追溯记录列表</returns>
        public async Task<PageDto<SiloPanelTraceDto>> GetList(GetSiloPanelTraceListReq request)
        {
            var pageDto = new PageDto<SiloPanelTraceDto>(request.PageNum, request.PageSize);
            var result = await _repository.GetList(request);
            pageDto.Total = result.TotalCount;

            pageDto.List = result.ToList().Select(entity => new SiloPanelTraceDto
            {
                Id = entity.Id,
                Code = entity.Code,
                SiloCode = entity.SiloCode,
                Location = entity.Location,
                SiloSummary = entity.SiloSummary,
                UndrilledItem = entity.UndrilledItem,
                DrilledItem = entity.DrilledItem,
                HasMultipleDrilled = entity.HasMultipleDrilled,
                Subject = entity.Subject,
                ScheduleId = entity.ScheduleId,
                TransportationTaskId = entity.TransportationTaskId,
                IsDeleted = entity.IsDeleted == 1,
                Status = entity.Status,
                CreatorId = entity.CreatorId,
                CreatorName = entity.CreatorName,
                CreateTime = entity.CreateTime,
                ModifyTime = entity.ModifyTime,
                ModifierId = entity.ModifierId,
                ModifierName = entity.ModifierName,
                IsWarning = entity.IsWarning,
                WarningDescription = entity.WarningDescription,
                ReferenceRecordId = entity.ReferenceRecordId
            }).ToList();
            return pageDto;
        }
    }
}
