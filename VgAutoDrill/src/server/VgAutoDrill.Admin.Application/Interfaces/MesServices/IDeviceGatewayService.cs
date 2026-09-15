using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceGateway;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IDeviceGatewayService
    {
        /// <summary>
        ///设备网关添加
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddDeviceGateway(AddOrUpdateDeviceGatewayReq req);

        /// <summary>
        ///设备网关删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Delete(long id);

        /// <summary>
        ///设备网关查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DeviceGatewayDto>>> DeviceGatewayList(DeviceGatewayReq req);
        /// <summary>
        ///设备网关更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateDeviceGateway(AddOrUpdateDeviceGatewayReq req);

        /// <summary>
        ///设备网关更新/添加
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeviceGatewayAddOrUpdate(AddOrUpdateDeviceGatewayReq req);

        /// <summary>
        ///设备网关查询ById
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<DeviceGatewayDto>> QueryByID(long id);

        /// <summary>
        ///版本检查获取
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DeviceGatewayDto>>> GetVerson(DeviceGatewayReq req);

        /// <summary>
        /// 下载安装
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> InstallPackages(DeviceGatewayReq req);
    }
}
