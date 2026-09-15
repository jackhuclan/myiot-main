using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    /// <summary>
    /// 工序与工作站关系
    /// </summary>
    public interface IRouteProcessAndWorkStationService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<RouteProcessAndWorkStationDto>>> GetList(GetRouteProcessAndWorkStationListReq req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<RouteProcessAndWorkStationDto>> MultiQueryByID(long id);

        Task<ResponseDto<string>> ExsitWorkStation(AddRouteProcessAndWorkStationReq req);
        Task<ResponseDto<string>> ExsitWorkStationByRoute(AddRPAndWByWorkStationReq req);

        /// <summary>
        /// 根据工作站获取关联的工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<RouteInfo>>> GetRoutesByWorkStation(GetRouteProcessAndWorkStationListReq req);
        Task<List<WorkstationDto>> GetDrillRouteCodes();

        /// <summary>
        /// 根据工艺路线、工序获取工作站（按空闲时间排序）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<FitWorkStationDto>>> GetFitWorkStationListByRoute(GetFitWorkStationListByRouteReq req);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddRouteProcessAndWorkStationReq req);

        /// <summary>
        /// 添加
        /// 工作站关联多个工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddDataByWorkStation(AddRPAndWByWorkStationReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateData(UpdateRouteProcessAndWorkStationReq req);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteData(long id);

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteDataList(List<long> idList);
    }
}
