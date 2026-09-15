using System.Linq.Expressions;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// 获取树形结构数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<TaskTreeDto>>> GetTreeList();

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<TaskDto>>> GetList(GetTaskListReq req);


        Task<TaskDto> GetTaskByCode(string code);

        /// <summary>
        /// 根据ItemTypeId获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<TaskDto>>> GetEquipmentList(GetTaskListReq req);

        /// <summary>
        /// 获取工艺路线和关联工序列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<RouteInfoAndProcessInfo>> GetRouteAndProcessList(GetRouteAndProcessByItemReq req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<TaskDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddOrUpdateTaskReq req);

        /// <summary>
        /// 用于钻孔任务，再次生成时
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> BulkAddDrillTask(AddOrUpdateDrillWorkOrderReq req);
        /// <summary>
        /// 钻孔任务分配机台
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Allocate(AddOrUpdateDrillWorkOrderReq req);
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(AddOrUpdateTaskReq req);

        /// <summary>
        /// 中控调用修改信息
        /// Set TaskStatus:BEGIN/FINISH
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateByOutSide(UpdateTaskByOutSideReq req);

        /// <summary>
        /// 中控调用修改信息（批量）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateListByOutSideBatch(UpdateTaskListByOutSideReq req);

        /// <summary>
        /// 修改生产任务甘特图时间
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateGantt(UpdatTaskGanttReq req);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Reset(ResetTaskReq req);
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
        Task<ResponseDto<string>> Commit(CommitTaskReq req);

        /// <summary>
        /// 撤销提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> RevokeCommit(CommitTaskReq req);

        /// <summary>
        /// 获取钻孔任务
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<DrillWorkOrderDto>> GetDrillTaskList(GetDrillTaskReq req);

        /// <summary>
        /// 获取所有工作站
        /// </summary>
        /// <returns></returns>
        Task<List<WorkStation>> GetWorkStationList(GetDrillTaskReq req);

        /// <summary>
        /// 根据空闲时间，查询适合的机台集合
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<FitWorkStationDto>>> GetFitWorkStationList(GetFitWorkStationListReq req);

        /// <summary>
        /// 理顺当天草稿状态任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> RationalizeTask(RationalizeTaskReq req);
        Task<ResponseDto<string>> BatchRationalizeTask(RationalizeTaskReq req);

        /// <summary>
        /// 获取钻机的生产任务Model
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="realNeedCount">钻机本次呼叫的数量，0表示未限制；如果所给数量少于初始分配的数量时，将多出来的数量转移到其他任务或者新增任务中</param>
        /// <param name="existRawNum">钻机本次呼叫时，已有的生料数量</param>
        /// <param name="spindleUseNum">钻机本次呼叫时，当前使用的轴数</param>
        /// <returns></returns>
        Task<ResponseDto<TaskDto>> GetNextTask(string deviceId, int realNeedCount, int existRawNum, int spindleUseNum, IReadOnlyList<string> undrilledItemCodesFromDrill);

        /// <summary>
        /// 转发任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> TransferTask(TransferTaskReq req);

        /// <summary>
        /// 判断多个任务是否属于同一个工艺路线、同一个工序
        /// </summary>
        /// <param name="taskList"></param>
        /// <returns></returns>
        Task<ResponseDto<RouteAndProcessDtoByTask>> VerifyTaskBelongOneRouteAndProcess(List<TaskDto> taskList);

        /// <summary>
        /// 根据设备编码查询Commit状态的任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<TaskDto>>> GetTaskByDevice(GetDrillOrAgvDeviceInfoReq req);

        /// <summary>
        /// 根据批量工单生成任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> BulkAddDrillTaskByMOList(List<AddOrUpdateDrillWorkOrderReq> req);

        /// <summary>
        /// 拖动Task
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> MoveTaskList(List<AddOrUpdateTaskReq> req);
        Task CanceledSchedule(Expression<Func<Schedule, bool>> where);


    }
}
