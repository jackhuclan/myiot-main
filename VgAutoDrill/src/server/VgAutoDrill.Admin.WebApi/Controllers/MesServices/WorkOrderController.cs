using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderController : BaseController
    {
        private readonly IWorkOrderService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public WorkOrderController(IWorkOrderService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取生产工单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTreeSelect")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkOrderTreeDto>>), 200)]
        [PermissionAuthorize("produce:workorder:list")]
        public async Task<ActionResult> GetTreeSelectList()
        {
            var result = await _mainService.GetTreeList();
            return Ok(result);
        }

        /// <summary>
        /// 获取生产工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkOrderDto>>), 200)]
        [PermissionAuthorize("produce:workorder:list")]
        public async Task<ActionResult> GetList([FromBody] GetWorkOrderListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取生产工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMOTaskList")]
        [ProducesResponseType(typeof(ResponseDto<List<DrillWorkOrderDto>>), 200)]
        [PermissionAuthorize("produce:workorder:list")]
        public async Task<ActionResult> GetMOTaskList([FromBody] GetMOTaskReq req)
        {
            var result = await _mainService.GetMOTaskList(req);
            return Ok(result);
        }


        /// <summary>
        /// 根据ItemTypeId获取生产工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEquipmentList")]
        [ProducesResponseType(typeof(ResponseDto<List<WorkOrderDto>>), 200)]
        [PermissionAuthorize("produce:workorder:list")]
        public async Task<ActionResult> GetEquipmentList([FromBody] GetWorkOrderListReq req)
        {
            var result = await _mainService.GetEquipmentList(req);
            return Ok(result);
        }

        /// <summary>
        ///获取生产工单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<WorkOrderDto>), 200)]
        [PermissionAuthorize("produce:workorder:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryDataByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加生产工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateWorkOrderReq req)
        {
            var result = await _mainService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改生产工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<WorkOrderDto>), 200)]
        [PermissionAuthorize("produce:workorder:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateWorkOrderReq req)
        {
            var result = await _mainService.UpdateData(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改生产工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("RobackTask")]
        [ProducesResponseType(typeof(ResponseDto<WorkOrderDto>), 200)]
        [PermissionAuthorize("produce:workorder:edit")]
        public async Task<ActionResult> RobackTask([FromBody] AddOrUpdateTaskReq req)
        {
            var result = await _mainService.RobackTask(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改生产工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("commitTask")]
        [ProducesResponseType(typeof(ResponseDto<WorkOrderDto>), 200)]
        [PermissionAuthorize("produce:workorder:edit")]
        public async Task<ActionResult> commitTask([FromBody] AddOrUpdateTaskReq req)
        {
            var result = await _mainService.commitTask(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateRoute")]
        [ProducesResponseType(typeof(ResponseDto<WorkOrderDto>), 200)]
        [PermissionAuthorize("produce:workorder:edit")]
        public async Task<ActionResult> UpdateRoute([FromBody] UpdateRouteReq req)
        {
            var result = await _mainService.UpdateRoute(req);
            return Ok(result);
        }

        /// <summary>
        /// 清空工单绑定的工作站
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ClearWorkStation")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:edit")]
        public async Task<ActionResult> ClearWorkStation(string workOrderCode)
        {
            var result = await _mainService.ClearWorkStation(workOrderCode);
            return Ok(result);
        }

        /// <summary>
        /// 删除生产工单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除生产工单集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _mainService.DeleteDataList(idList);
            return Ok(result);
        }
        /// <summary>
        /// 提交生产工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Commit")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:commit")]
        public async Task<ActionResult> Commit([FromBody] CommitWorkOrderReq req)
        {
            var result = await _mainService.Commit(req);
            return Ok(result);
        }

        /// <summary>
        /// 导入生产工单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workOrder:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                //生产工单List
                List<WorkOrderToExcelDto> list = new List<WorkOrderToExcelDto>();

                //文件转成流
                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    list = ExcelHelper.ParseExcelToList<WorkOrderToExcelDto>(fileStream, "Sheet1");
                }

                var result = await _mainService.BulkInsert(list);
                return Ok(result);
            }
            catch
            {
                var result = new ResponseDto<string>();
                result.Code = ResponseCode.Fail;
                result.Message = "请参考模板准备导入数据！";
                return Ok(result);
            }
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoad")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("produce:workOrder:download")]
        public ActionResult DownLoad()
        {
            List<WorkOrderToExcelDto> list = new List<WorkOrderToExcelDto>();
            //添加示例数据
            WorkOrderToExcelDto model = new WorkOrderToExcelDto();
            model.Code = "MO202304280004";
            model.Name = "PCB单层";
            model.ParentId = 0;
            model.BatchCode = "XX 0022";
            model.ClientCode = "15200000001";
            model.ClientId = 2;
            model.ClientName = "张老板";
            model.ItemCode = "IF2023040500002";
            model.ItemId = 36;
            model.ItemName = "PCB单层板";
            model.ItemTypeId = 22;
            model.OrderSource = "客户订单";
            model.Quantity = 200;
            model.QuantityProduced = 0;
            model.QuantityScheduled = 0;
            model.QuantityChanged = 210;
            model.RequestDate = DateTime.Now.Date;
            model.SourceCode = "XX 00002";
            model.Specification = "DDD XXXX DD";
            model.UnitOfMeasure = "Panel";
            model.PanelCount = 0;
            model.DrillCount = 0;
            model.WadCount = 0;
            model.RouteId = 2;
            model.RouteName = "工艺路线B0002";
            model.RouteCode = "B0002";
            model.DispenseMachines = 2;
            list.Add(model);

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "生产工单模板.xlsx");
        }

        /// <summary>
        ///生产工单颜色标记
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("MoColorRemark")]
        [ProducesResponseType(typeof(ResponseDto<WorkOrderDto>), 200)]
        [PermissionAuthorize("produce:workorder:edit")]
        public async Task<ActionResult> MoColorRemark([FromBody] AddOrUpdateWorkOrderReq req)
        {
            var result = await _mainService.MoColorRemark(req);
            return Ok(result);
        }

        /// <summary>
        ///设置MoveInTime
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("SetMoveInTime")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:setMoveInTime")]
        public async Task<ActionResult> SetMoveInTime(string workOrderCode)
        {
            var result = await _mainService.SetMoveInTime(workOrderCode, DateTime.Now);
            return Ok(result);
        }
        /// <summary>
        ///设置MoveOutTime
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("SetMoveOutTime")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:setMoveOutTime")]
        public async Task<ActionResult> SetMoveOutTime(string workOrderCode)
        {
            var result = await _mainService.SetMoveOutTime(workOrderCode, DateTime.Now);
            return Ok(result);
        }
        /// <summary>
        ///设置TrackInTime
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("SetTrackInTime")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:setTrackInTime")]
        public async Task<ActionResult> SetTrackInTime(string workOrderCode)
        {
            var result = await _mainService.SetTrackInTime(workOrderCode, DateTime.Now);
            return Ok(result);
        }
        /// <summary>
        ///设置TrackOutTime
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("SetTrackOutTime")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:setTrackOutTime")]
        public async Task<ActionResult> SetTrackOutTime(string workOrderCode)
        {
            var result = await _mainService.SetTrackOutTime(workOrderCode, DateTime.Now);
            return Ok(result);
        }

        /// <summary>
        ///校验料号数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("VerifyWIPItemNum")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:workorder:edit")]
        public async Task<ActionResult> VerifyWIPItemNum([FromBody] AddOrUpdateWorkOrderReq req)
        {
            var result = await _mainService.VerifyWIPItemNum(req);
            return Ok(result);
        }
    }
}
