using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    /// <summary>
    /// 工艺路线
    /// </summary>
    public interface IRouteService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<RouteDto>>> GetList(GetRouteListReq req);

        Task<ResponseDto<List<DropSelectDto>>> GetDropSelectDatas(GetRouteListReq req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<RouteDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdateRouteReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(AddOrUpdateRouteReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> VerifyUpdate(AddOrUpdateRouteReq req);

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


        Task<List<PartitionConfig>> GetRouteAndPartitionSetting();

        /// <summary>
        /// 审批工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> VettingRoute(VettingRouteReq req);

        /// <summary>
        /// 取消审批工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> CancelVettingRoute(VettingRouteReq req);

        /// <summary>
        /// 获取工艺路线绑定详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<RouteDto>> GetListDetail(GetRouteListReq req);
    }
}
