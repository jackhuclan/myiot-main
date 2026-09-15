using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IDeviceRecordsService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DeviceRecordsDto>>> GetList(GetDeviceRecordsListReq req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<DeviceRecordsDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddOrUpdateDeviceRecordsReq req);
        Task<ResponseDto<string>> InsertData(AddOrUpdateDeviceRecordsReq req);
        Task<ResponseDto<bool>> BulkInsert(List<AddOrUpdateDeviceRecordsReq> addList);
        Task<List<DeviceRecordsToExcelDto>> GetToExcelList(GetDeviceRecordsListReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(AddOrUpdateDeviceRecordsReq req);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Delete(long id);

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteList(object[] idList);

        Task<ResponseDto<PageDto<DeviceRecordsSummaryDto>>> GetSummaryList(GetDeviceRecordsSummaryListReq req);
        Task<List<DeviceRecordsSummaryToExcelDto>> GetSummaryToExcelList(GetDeviceRecordsSummaryListReq req);
        Task<List<DeviceRecordsSummaryToExcelDtoInner>> GetInnerSummaryToExcelList(GetDeviceRecordsSummaryListReq req);
        Task<ResponseDto<List<DrillRateFactorDto>>> GetRateReasonDetails(long recordSummaryId);
        Task RefreshSummaryDatas();
        Task RegularDeleteData();

        /// <summary>
        /// 获取设备保养配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<SysConfigDto>>> GetDeviceMaintenanceConfigs(GetSysConfigListReq req);


        // <summary>
        /// 保存设备保养配置
        /// </summary>
        /// <param name="reqs"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> SaveDeviceMaintenanceConfigs(List<AddOrUpdateSysConfigReq> reqs);
    }
}
