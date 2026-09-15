using VgAutoDrill.Admin.Application.Services.MesServices.External;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWorkOrderService
    {
        /// <summary>
        /// 获取树形结构数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<WorkOrderTreeDto>>> GetTreeList();
        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> Exsist(string code);
        Task<WorkOrder> FindSingle(string code);
        Task<bool> ExsistAfterDrillFilePath(string code, string deviceCode, string beforeDrillFilePath);
        Task<ResponseDto<JWConversionPathResponse>> GetTransferDrillFilePath(string beforeDrillFilePath);
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<WorkOrderDto>>> GetList(GetWorkOrderListReq req);

        /// <summary>
        /// 根据ItemTypeId获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<WorkOrderDto>>> GetEquipmentList(GetWorkOrderListReq req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<WorkOrderDto>> QueryDataByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdateWorkOrderReq req);

        /// <summary>
        /// 批量添加信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> BulkInsert(List<WorkOrderToExcelDto> list);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateData(AddOrUpdateWorkOrderReq req);

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

        /// <summary>
        /// 提交信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Commit(CommitWorkOrderReq req);
        /// <summary>
        /// 撤销提交
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UnCommit(CommitWorkOrderReq req);

        /// <summary>
        /// 设置工艺路线
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateRoute(UpdateRouteReq req);

        /// <summary>
        /// 清空工单下的工作站
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> ClearWorkStation(string workOrderCode);

        /// <summary>
        /// 工单任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<DrillWorkOrderDto>> GetMOTaskList(GetMOTaskReq req);

        /// <summary>
        /// 撤回工单任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> RobackTask(AddOrUpdateTaskReq req);

        /// <summary>
        /// 提交工单任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> commitTask(AddOrUpdateTaskReq req);

        /// <summary>
        /// 工单颜色标记
        /// </summary>
        /// <param name="req"></param>
        Task<ResponseDto<string>> MoColorRemark(AddOrUpdateWorkOrderReq req);

        Task<ResponseDto<string>> SetMoveInTime(string workOrderCode, DateTime moveInTime);
        Task<ResponseDto<string>> SetMoveOutTime(string workOrderCode, DateTime moveOutTime);
        Task<ResponseDto<string>> SetTrackInTime(string workOrderCode, DateTime trackInTime);
        Task<ResponseDto<string>> SetTrackOutTime(string workOrderCode, DateTime trackOutTime);
        Task<bool> VerifyBeforePath(string beforeDrillFilePath);
        Task<ResponseDto<string>> VerifyWIPItemNum(AddOrUpdateWorkOrderReq req);
    }
}
