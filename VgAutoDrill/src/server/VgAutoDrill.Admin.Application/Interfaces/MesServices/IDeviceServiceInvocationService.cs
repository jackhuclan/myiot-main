using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceServiceInvocation;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IDeviceServiceInvocationService
    {
        /// <summary>
        /// 新增或更新(批量)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddDeviceServiceInvocationList(List<AddOrUpdateDeviceServiceInvocationReq> req);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteDeviceServiceInvocationList(List<int> req);

        /// <summary>
        /// 获取信息(List)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<List<DeviceServiceInvocationDto>> GetDeviceServiceInvocationList(GetDeviceServiceInvocationListReq req);

        /// <summary>
        /// 获取信息(Page)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DeviceServiceInvocationDto>>> GetDeviceServiceInvocations(GetDeviceServiceInvocationsReq req);
        Task RegularDeleteData();
    }
}
