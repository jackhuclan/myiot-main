using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominRequest;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominResponse;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingwangResponse;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IExternalWorkOrderService
    {
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdateExternalWorkOrderReq req);
        /// <summary>
        /// 批量添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> BatchAddData(List<AddOrUpdateExternalWorkOrderReq> req);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateData(AddOrUpdateExternalWorkOrderReq req);
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ExternalWorkOrderDto>>> QueryData(ExternalWorkOrderQueryReq req);
        /// <summary>
        /// 批量查询数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<List<ExternalWorkOrderDto>>> BatchQueryData(BatchWorkOrderQueryReq req);
        /// <summary>
        /// 解析工单
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> AnalyzeExternalWorkOrder();

        /// <summary>
        /// 导入江西景旺WIP数据，等待自动生成工单
        /// </summary>
        /// <returns></returns>
        Task ImportJiangXiKinWongWIP();

        Task SignMoveInLot();

        Task SignMoveOutLot();

        Task SignTrackInLot();

        Task SignTrackOutLot();

        Task GetAfterDrillFilePath();
        Task RebrushAfterDrillFilePath();
        Task RefreshWorkOrderToTask();
        Task SendMoveSiloCommand();
        Task KwAGVStockVerifyAndIn();
        Task RefreshLotStockNum();
        Task<ResponseDto<string>> KwAGVStockInByData(string externalWorkOrderCode);
        Task<ResponseDto<string>> KwHoldLotByData(string externalWorkOrderCode);
        Task<ResponseDto<string>> KwGenAgvSchedulingTask(string externalWorkOrderCode);
        Task<ResponseDto<List<StockInfoQueryDataDto>>> GetStockInfoList(MesStockInfoQueryReq req);

        Task<ResponseDto<List<ExternalWorkTaskDto>>> GetWorkTask(ExternalWorkOrderQueryReq req);
        Task<ResponseDto<List<ExternalWorkTaskDto>>> BatchGetWorkTask(BatchWorkOrderQueryReq req);
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteData(string sourceCode);

        Task<ResponseDto<PageDto<ExternalWorkOrderDto>>> GetExterWorkOrderList(GetExterWorkOrderListReq req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<ExternalWorkOrderDto>> QueryByID(long id);

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteDataList(List<long> idList);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteDataByID(long id);


        Task<GetDrillRecipesDto> GetDrillRecipes(GetDrillRecipesReq req);
    }
}
