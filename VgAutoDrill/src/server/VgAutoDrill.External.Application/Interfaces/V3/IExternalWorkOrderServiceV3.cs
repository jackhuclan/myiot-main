using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;

namespace VgAutoDrill.External.Application.Interfaces.ExternalService
{
    public interface IExternalWorkOrderServiceV3
    {
        /// <summary>
        /// 查询工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ExternalWorkOrderDto>>> QueryData(ExternalWorkOrderQueryReq req);

        /// <summary>
        /// 批量查询工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ExternalWorkOrderDto>>> BatchQueryData(BatchWorkOrderQueryReq req);

        /// <summary>
        /// 添加工单数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdateExternalWorkOrderReq req);

        /// <summary>
        /// 更新工单数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateData(AddOrUpdateExternalWorkOrderReq req);

        /// <summary>
        /// 删除工单
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteData(string sourceCode);

        /// <summary>
        /// 获取工单状态及任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ExternalWorkTaskDto>>> GetWorkTask(ExternalWorkOrderQueryReq req);

        /// <summary>
        /// 批量获取工单状态及任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ExternalWorkTaskDto>>> BatchGetWorkTask(BatchWorkOrderQueryReq req);

        /// <summary>
        /// 解析工单
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> AnalyzeExternalWorkOrder();
    }
}
