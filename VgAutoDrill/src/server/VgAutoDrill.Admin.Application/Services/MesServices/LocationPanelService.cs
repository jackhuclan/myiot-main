using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.LocationPanel;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 
    /// </summary>
    public class LocationPanelService : BaseServiceWithoutTree<ScheduleLocationPanel, LocationPanelDto, AddOrUpdateLocationPanelReq>, ILocationPanelService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public LocationPanelService(ILocationPanelDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {

        }
        /// <summary>
        /// 根据locationCode获取区域panel信息
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>

        public async Task<List<ScheduleLocationPanel>> GetList(string locationCode)
        {
            var result = new List<ScheduleLocationPanel>();
            if (string.IsNullOrWhiteSpace(locationCode))
            {
                return result;
            }
            return result;


        }

    }
}
