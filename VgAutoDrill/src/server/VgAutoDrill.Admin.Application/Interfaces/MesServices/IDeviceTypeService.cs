using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EquipmentType;
using VgAutoDrill.Admin.Model.ViewModels.Req.Equipment;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    /// <summary>
    /// 设备类别
    /// </summary>
    public interface IDeviceTypeService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DeviceTypeDto>>> GetEquipmentTypeList(GetEquipmentTypeListReq req);

        /// <summary>
        /// 获取树形结构数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<DeviceTypeTreeDto>>> GetEquipmentTypeTreeList();
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<DeviceTypeInfoDto>> QueryByID(long id);
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddOrUpdateDeviceTypeReq req);
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(AddOrUpdateDeviceTypeReq req);
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
