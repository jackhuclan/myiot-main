using SqlSugar;
using System.Linq.Expressions;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface ITransportationTaskService
    {
        Task<int> ClearTimeoutTransportationTask();

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<TransferJobDto>>> GetList(GetTransferJobListReq req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<TransferJobDto>> QueryByID(long id);
        Task<List<TransferJobDto>> QueryAsync(Expression<Func<TransferJob, bool>> predicate, Expression<Func<TransferJob, object>> orderByExpression, OrderByType orderByType);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdateTransferJobReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateData(AddOrUpdateTransferJobReq req);

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

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<string> UpdateTransportation(AddOrUpdateTransferJobReq req);

        Task<bool> Exsist(string code);

        Task<TransferJobDto> FindSingle(string code);

        /// <summary>
        /// 根据料仓任务ID获取详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<TransferJobLogsDto>> QueryLogsByID(long id);

        Task<ResponseDto<string>> CancelSingle(long id, bool isForce = false);
        Task<ResponseDto<string>> BulkCancel(List<long> ids, bool isForce = false);
        

        /// <summary>
        /// 根据任务代号，更新料仓任务信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<string> UpdateTransportationByHikResponseKey(AddOrUpdateTransferJobReq req);

        Task<TransferJobDto> FindSingleByHikResponseKey(string hikResponseKey);
        Task RegularDeleteHisData();
        Task<bool> TransferTransportationHistoryTaskData(TransferJobHistoryDataReq req);

        /// <summary>
        /// 获取料仓历史数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<TransferJobDto>>> GetHistoryList(GetTransferJobListReq req);

        Task<ResponseDto<TransferJobDto>> GetHistory(long id);


        Task<List<TransportationTaskToExcelDto>> GetToExcelList(GetTransferJobListReq req);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<List<TransportationTaskToExcelDto>> GetToHistoryExcelList(GetTransferJobListReq req);
        Task<ResponseDto<int>> AddWithReturnId(AddOrUpdateTransferJobReq req);
    }
}