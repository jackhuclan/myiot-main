using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 钻孔工单
    /// </summary>
    public class DrillWorkOrderController : BaseController
    {
        private readonly IDrillWorkOrderService _mainService;
        private readonly ITaskService _taskService;
        private readonly ILogger<DrillWorkOrderController> logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        /// <param name="taskService"></param>
        public DrillWorkOrderController(IDrillWorkOrderService mainService,
            ILoggerFactory loggerFactory,
            ITaskService taskService)
        {
            _mainService = mainService;
            _taskService = taskService;
            logger = loggerFactory.CreateLogger<DrillWorkOrderController>();
        }

        /// <summary>
        /// 获取钻孔工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DrillWorkOrderDto>>), 200)]
        [PermissionAuthorize("produce:drillTask:list")]
        public async Task<ActionResult> GetList([FromBody] GetDrillWorkOrderListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取钻孔工单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DrillWorkOrderDto>), 200)]
        [PermissionAuthorize("produce:drillTask:view")]
        public async Task<ActionResult> Get(long id)
        {
            //var result = await _mainService.QueryByID(id);
            var result = await _mainService.QueryListByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加钻孔工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:drillTask:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDrillWorkOrderReq req)
        {
            logger.LogInformation($"添加钻孔工单 BeginTime " + DateTime.Now);
            var result = await _mainService.AddData(req);
            logger.LogInformation($"添加钻孔工单 EndTime " + DateTime.Now);

            if (result.Code != ResponseCode.Fail)
            {
                logger.LogInformation($"批量添加钻孔任务 BeginTime " + DateTime.Now);
                result = await _taskService.BulkAddDrillTask(req);
                logger.LogInformation($"批量添加钻孔任务 EndTime " + DateTime.Now);
            }

            return Ok(result);
        }

        /// <summary>
        /// 钻孔任务,按可用库存排产时，再次生成任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BulkAddDrillTask")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:drillTask:add")]
        public async Task<ActionResult> BulkAddDrillTask([FromBody] AddOrUpdateDrillWorkOrderReq req)
        {
            logger.LogInformation($"批量添加钻孔任务 BeginTime " + DateTime.Now);
            var result = await _taskService.BulkAddDrillTask(req);
            logger.LogInformation($"批量添加钻孔任务 EndTime " + DateTime.Now);

            return Ok(result);
        }

        /// <summary>
        /// 批量生成钻孔任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BulkAddDrillTaskByMO")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:drillTask:add")]
        public async Task<ActionResult> BulkAddDrillTaskByMOList([FromBody] List<AddOrUpdateDrillWorkOrderReq> req)
        {
            logger.LogInformation($"批量添加钻孔任务 BeginTime " + DateTime.Now);
            var result = await _taskService.BulkAddDrillTaskByMOList(req);
            logger.LogInformation($"批量添加钻孔任务 EndTime " + DateTime.Now);

            return Ok(result);
        }

        /// <summary>
        /// 修改钻孔工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DrillWorkOrderDto>), 200)]
        [PermissionAuthorize("produce:drillTask:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDrillWorkOrderReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 钻孔工单分配机台
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Allocate")]
        [ProducesResponseType(typeof(ResponseDto<DrillWorkOrderDto>), 200)]
        [PermissionAuthorize("produce:drillTask:edit")]
        public async Task<ActionResult> Allocate([FromBody] AddOrUpdateDrillWorkOrderReq req)
        {
            var result = await _taskService.Allocate(req);
            if (result.Code != ResponseCode.Fail)
            {
                result = await _mainService.Update(req);
            }
            return Ok(result);
        }
        /// <summary>
        /// 删除钻孔工单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:drillTask:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// 删除钻孔工单集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:drillTask:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 提交钻孔工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Commit")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:drillTask:commit")]
        public async Task<ActionResult> Commit([FromBody] CommitDrillWorkOrderReq req)
        {
            var result = await _mainService.Commit(req);
            return Ok(result);
        }

        /// <summary>
        /// 刷新库存数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("RefreshStockData")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:drillTask:list")]
        public async Task<ActionResult> RefreshStockData()
        {
            logger.LogInformation($"刷新库存数据 BeginTime " + DateTime.Now);
            var result = await _mainService.RefreshStockData();
            logger.LogInformation($"刷新库存数据 EndTime " + DateTime.Now);

            return Ok(result);
        }

        /// <summary>
        ///获取库存流转信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMaterialData")]
        [ProducesResponseType(typeof(ResponseDto<GetMaterialDataDto>), 200)]
        [PermissionAuthorize("produce:drillTask:list")]
        public async Task<ActionResult> GetMaterialData(GetMaterialDataReq req)
        {
            logger.LogInformation($"获取库存流转信息 BeginTime " + DateTime.Now);
            var result = await _mainService.GetMaterialData(req);
            logger.LogInformation($"获取库存流转信息 EndTime " + DateTime.Now);

            return Ok(result);
        }
    }
}

