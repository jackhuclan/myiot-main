using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Application.Services;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.External.Application.Interfaces.ExternalService;

namespace VgAutoDrill.External.Application.Services.ExternalService
{
    public class ExternalWorkOrderServiceV3 : BaseService, IExternalWorkOrderServiceV3
    {
        private readonly IExternalWorkOrderService _externalWorkOrderService;

        /// <summary>
        /// 
        /// </summary>
        public ExternalWorkOrderServiceV3(IExternalWorkOrderService externalWorkOrderService)
        {
            _externalWorkOrderService = externalWorkOrderService;
        }

        /// <summary>
        /// 查询工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<ExternalWorkOrderDto>>> QueryData(ExternalWorkOrderQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<ExternalWorkOrderDto>>("信息格式错误!");
            }

            var result = await _externalWorkOrderService.QueryData(req);
            return result;
        }

        /// <summary>
        /// 批量查询工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<ExternalWorkOrderDto>>> BatchQueryData(BatchWorkOrderQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<ExternalWorkOrderDto>>("信息格式错误!");
            }

            var result = await _externalWorkOrderService.BatchQueryData(req);
            return result;
        }

        /// <summary>
        /// 添加工单数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateExternalWorkOrderReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _externalWorkOrderService.AddData(req);
            return result;
        }

        /// <summary>
        /// 修改工单数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateExternalWorkOrderReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var result = await _externalWorkOrderService.UpdateData(req);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteData(string sourceCode)
        {
            if (string.IsNullOrEmpty(sourceCode))
            {
                return Fail("未识别有效的sourceCode");
            }

            var result = await _externalWorkOrderService.DeleteData(sourceCode);
            return result;
        }

        /// <summary>
        /// 获取工单和任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<ExternalWorkTaskDto>>> GetWorkTask(ExternalWorkOrderQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<ExternalWorkTaskDto>>("信息格式错误!");
            }

            var result = await _externalWorkOrderService.GetWorkTask(req);
            return result;
        }

        /// <summary>
        /// 批量获取工单和任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<List<ExternalWorkTaskDto>>> BatchGetWorkTask(BatchWorkOrderQueryReq req)
        {
            if (req == null)
            {
                return Fail<List<ExternalWorkTaskDto>>("信息格式错误!");
            }

            var result = await _externalWorkOrderService.BatchGetWorkTask(req);
            return result;
        }

        /// <summary>
        /// 解析工单
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AnalyzeExternalWorkOrder()
        {
            var result = await _externalWorkOrderService.AnalyzeExternalWorkOrder();
            return result;
        }
    }
}
