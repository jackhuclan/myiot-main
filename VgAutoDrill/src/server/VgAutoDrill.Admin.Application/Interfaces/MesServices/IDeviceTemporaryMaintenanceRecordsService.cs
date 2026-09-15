using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IDeviceTemporaryMaintenanceRecordsService
    {

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddOrUpdateDeviceTemporaryMaintenanceRecordsReq req);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DeviceTemporaryMaintenanceRecordsDto>>> GetList(GetDeviceTemporaryMaintenanceRecordsListReq req);



        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<DeviceTemporaryMaintenanceRecordsDto>> QueryByID(long id);





        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(AddOrUpdateDeviceTemporaryMaintenanceRecordsReq req);


        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Delete(long id);




    }
}
