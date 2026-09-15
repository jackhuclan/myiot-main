using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IDrillPanelDetailService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DrillPanelDetailDto>>> GetList(GetDrillPanelDetailListReq req);

        Task<ResponseDto<List<DrillPanelDetailInfo>>> GetPanelList(string deviceCode);

        Task<ResponseDto<string>> BatchLoadPanelDetailData();

        Task<ResponseDto<string>> LoadPanelDetailData(LoadDrillPanelDetailReq req);

        Task<ResponseDto<string>> BatchAddOrUpdateData(BatchAddOrUpdateDrillPanelDetailReq req);

        Task<ResponseDto<string>> ClearData(ClearDrillPanelDetailReq req);

        Task<ResponseDto<string>> MovePanel(MovePanelDetailDto? startData, MovePanelDetailDto? targetData);

        Task<ResponseDto<string>> SynchronousPanelData(string deviceCode);

        Task<ResponseDto<string>> AllotsPanelData(AllotsPanelDataReq req);

        Task<ResponseDto<PageDto<DrillPanelFullData>>> GetDrillPanelFullData(GetDeviceListReq req);
        Task<List<DeviceDataToScreen>> GetDeviceDatasToScreen(GetDeviceListReq? request);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<DrillPanelDetailDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdateDrillPanelDetailReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateData(AddOrUpdateDrillPanelDetailReq req);

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
    }
}
