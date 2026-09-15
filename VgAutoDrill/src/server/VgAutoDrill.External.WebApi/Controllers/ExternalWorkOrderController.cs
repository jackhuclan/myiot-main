using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominRequest;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominResponse;

namespace VgAutoDrill.External.WebApi.Controllers
{
    /// <summary>
    /// 工单相关对外接口
    /// </summary>
    public class ExternalWorkOrderController : BaseController
    {
        private readonly IExternalWorkOrderService _externalWorkOrderService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderService"></param>
        public ExternalWorkOrderController(IExternalWorkOrderService orderService)
        {
            _externalWorkOrderService = orderService;
        }

        /// <summary>
        /// 接收外部工单信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Add([FromBody] AddOrUpdateExternalWorkOrderReq req)
        {
            var result = await _externalWorkOrderService.AddData(req);
            return Ok(result);
        }

        /// <summary>
        /// 批量接收外部工单信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BatchAdd")]
        public async Task<ActionResult> BatchAdd([FromBody] List<AddOrUpdateExternalWorkOrderReq> req)
        {
            var result = await _externalWorkOrderService.BatchAddData(req);
            return Ok(result);
        }

        /// <summary>
        /// 更新工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> Update([FromBody] AddOrUpdateExternalWorkOrderReq req)
        {
            var result = await _externalWorkOrderService.UpdateData(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除工单，
        /// 仅能删除尚未投产，并且没有任务在调度的工单
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <returns></returns>        
        [HttpDelete("{sourceCode}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        public async Task<ActionResult> Delete(string sourceCode)
        {
            var result = await _externalWorkOrderService.DeleteData(sourceCode);
            return Ok(result);
        }
        /// <summary>
        /// 查询工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        public async Task<ActionResult> Query(ExternalWorkOrderQueryReq req)
        {
            var result = await _externalWorkOrderService.QueryData(req);
            return Ok(result);
        }
        /// <summary>
        /// 批量查询工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BatchQuery")]
        public async Task<ActionResult> BatchQuery(BatchWorkOrderQueryReq req)
        {
            var result = await _externalWorkOrderService.BatchQueryData(req);
            return Ok(result);
        }
        /// <summary>
        /// Test
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Job")]
        public async Task<ActionResult> AnalyzeExternalWorkOrder()
        {
            var result = await _externalWorkOrderService.AnalyzeExternalWorkOrder();
            return Ok(result);
        }

        /// <summary>
        /// 获取工单状态及任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetWorkTask")]
        public async Task<ActionResult> GetWorkTask(ExternalWorkOrderQueryReq req)
        {
            var result = await _externalWorkOrderService.GetWorkTask(req);
            return Ok(result);
        }
        /// <summary>
        /// 批量查询，获取工单状态及任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BatchGetWorkTask")]
        public async Task<ActionResult> BatchGetWorkTask(BatchWorkOrderQueryReq req)
        {
            var result = await _externalWorkOrderService.BatchGetWorkTask(req);
            return Ok(result);
        }

        /// <summary>
        /// 发起mes转仓
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("KwAGVStockIn")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        public async Task<ActionResult> KwAGVStockIn(string externalWorkOrderCode)
        {
            var result = await _externalWorkOrderService.KwAGVStockInByData(externalWorkOrderCode);
            return Ok(result);
        }

        /// <summary>
        /// 发起mes暂停Lot
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("KwHoldLot")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        public async Task<ActionResult> KwHoldLot(string externalWorkOrderCode)
        {
            var result = await _externalWorkOrderService.KwHoldLotByData(externalWorkOrderCode);
            return Ok(result);
        }

        /// <summary>
        /// 下发转仓任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("KwGenAgvSchedulingTask")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        public async Task<ActionResult> KwGenAgvSchedulingTask(string externalWorkOrderCode)
        {
            var result = await _externalWorkOrderService.KwGenAgvSchedulingTask(externalWorkOrderCode);
            return Ok(result);
        }


        /// <summary>
        /// 获取钻带文件接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDrillRecipes")]
        [ProducesResponseType(typeof(ResponseDto<GetDrillRecipesDto>), 200)]
        [AllowAnonymous]
        public async Task<ActionResult> GetDrillRecipes(GetDrillRecipesReq req)
        {
            var result = await _externalWorkOrderService.GetDrillRecipes(req);
            return Ok(result);
        }
    }

}
