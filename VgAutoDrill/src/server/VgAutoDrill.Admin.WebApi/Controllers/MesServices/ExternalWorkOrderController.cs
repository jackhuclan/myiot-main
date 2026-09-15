using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingwangResponse;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    public class ExternalWorkOrderController : BaseController
    {
        private readonly IExternalWorkOrderService _externalWorkOrderService;

        public ExternalWorkOrderController(IExternalWorkOrderService externalWorkOrderService)
        {
            _externalWorkOrderService = externalWorkOrderService;
        }
        /// <summary>
        /// 获取外部生产工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetExterWorkOrderList")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<ExternalWorkOrderDto>>), 200)]
        [PermissionAuthorize("produce:workorder:list")]
        public async Task<ActionResult> GetExterWorkOrderList([FromBody] GetExterWorkOrderListReq req)
        {
            var result = await _externalWorkOrderService.GetExterWorkOrderList(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除外部生产工单
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteExternalWorkOrder")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:remove")]
        public async Task<ActionResult> DeleteExternalWorkOrder(string sourceCode)
        {
            var result = await _externalWorkOrderService.DeleteData(sourceCode);
            return Ok(result);
        }

        /// <summary>
        ///获取单个外部生产工单数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<ExternalWorkOrderDto>), 200)]
        [PermissionAuthorize("produce:workorder:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _externalWorkOrderService.QueryByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 更新外部工单数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:edit")]
        public async Task<ActionResult> Update([FromBody] AddOrUpdateExternalWorkOrderReq req)
        {
            var result = await _externalWorkOrderService.UpdateData(req);
            return Ok(result);
        }

        /// <summary>
        /// 批量删除外部工单列表
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _externalWorkOrderService.DeleteDataList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 删除单个外部生产工单数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _externalWorkOrderService.DeleteDataByID(id);
            return Ok(result);
        }

        /// <summary>
        /// 查询区域中的库存信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetStockInfoQuery")]
        [ProducesResponseType(typeof(ResponseDto<List<StockInfoQueryDataDto>>), 200)]
        [PermissionAuthorize("produce:workorder:list")]
        public async Task<ActionResult> GetStockInfoQuery([FromBody] MesStockInfoQueryReq req)
        {
            var result = await _externalWorkOrderService.GetStockInfoList(req);
            return Ok(result);
        }

        /// <summary>
        /// 发起mes转仓
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("KwAGVStockIn")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:edit")]
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
        [PermissionAuthorize("produce:workorder:edit")]
        public async Task<ActionResult> KwHoldLot(string externalWorkOrderCode)
        {
            var result = await _externalWorkOrderService.KwHoldLotByData(externalWorkOrderCode);
            return Ok(result);
        }
    }
}
