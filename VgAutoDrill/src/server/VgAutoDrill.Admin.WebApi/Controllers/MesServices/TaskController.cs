using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Const;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Domain;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskController : BaseController
    {
        private readonly ITaskService _mainService;
        private readonly IWorkOrderService _workOrderService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TaskController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        /// <param name="workOrderService"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        public TaskController(ITaskService mainService, IWorkOrderService workOrderService, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _mainService = mainService;
            _workOrderService = workOrderService;
            _configuration = configuration;
            logger = loggerFactory.CreateLogger<TaskController>();
        }

        /// <summary>
        /// 获取生产任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTreeSelect")]
        [ProducesResponseType(typeof(ResponseDto<List<TaskTreeDto>>), 200)]
        [PermissionAuthorize("produce:task:list")]
        public async Task<ActionResult> GetTreeSelectList()
        {
            var result = await _mainService.GetTreeList();
            return Ok(result);
        }

        /// <summary>
        /// 获取生产任务列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<TaskDto>>), 200)]
        [PermissionAuthorize("produce:task:list")]
        public async Task<ActionResult> GetList([FromBody] GetTaskListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }

        /// <summary>
        /// 根据ItemTypeId获取生产任务列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEquipmentList")]
        [ProducesResponseType(typeof(ResponseDto<List<TaskDto>>), 200)]
        [PermissionAuthorize("produce:task:list")]
        public async Task<ActionResult> GetEquipmentList([FromBody] GetTaskListReq req)
        {
            var result = await _mainService.GetEquipmentList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取钻孔任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDrillList")]
        [ProducesResponseType(typeof(ResponseDto<DrillWorkOrderDto>), 200)]
        [PermissionAuthorize("produce:task:list")]
        public async Task<ActionResult> GetDrillList([FromBody] GetDrillTaskReq req)
        {
            var result = await _mainService.GetDrillTaskList(req);
            return Ok(result);
        }
        /// <summary>
        /// 获取工艺路线和关联工序列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRouteAndProcessList")]
        [ProducesResponseType(typeof(ResponseDto<RouteInfoAndProcessInfo>), 200)]
        [PermissionAuthorize("produce:task:list")]
        public async Task<ActionResult> GetRouteAndProcessList([FromBody] GetRouteAndProcessByItemReq req)
        {
            var result = await _mainService.GetRouteAndProcessList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取生产任务甘特图列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetGanttTaskList")]
        [ProducesResponseType(typeof(ResponseDto<List<GanttData>>), 200)]
        [PermissionAuthorize("produce:task:list")]
        public async Task<ActionResult> GetGanttTaskList([FromBody] GetWorkOrderListReq req)
        {
            var result = new List<GanttData>();
            var workOrderList = await _workOrderService.GetList(req);
            foreach (var workOrder in workOrderList.Data.List)
            {
                var tasks = await _mainService.GetList(new GetTaskListReq { PageSize = int.MaxValue, Status = 1, WorkOrderId = workOrder.Id });
                if (tasks == null || tasks.Data.List == null || tasks.Data.List.Count == 0)
                {
                    continue;
                }
                var workOrderGanttData = new GanttData
                {
                    Id = $"MO{workOrder.Id}",
                    Text = $"{workOrder.Code}-{workOrder.ItemName}-{workOrder.Quantity}-{workOrder.UnitOfMeasure}",
                    Product = workOrder.ItemName,
                    Quantity = workOrder.Quantity,
                    Parent = (workOrder.ParentId > 0) ? $"MO{workOrder.ParentId}" : "",
                    Progress = (workOrder.QuantityProduced > 0 && workOrder.Quantity > 0) ? Math.Round(workOrder.QuantityProduced.Value / workOrder.Quantity.Value, 2) : 0,
                    Process = "",
                    Duration = 0,
                    Type = UserConstants.GANTT_TASK_TYPE_WORKORDER,
                };
                result.Add(workOrderGanttData);

                foreach (var task in tasks.Data.List)
                {
                    var ganttData = new GanttData
                    {
                        Id = $"MO{task.Id}",
                        Text = $"{task.Code}-{task.ItemName}-{task.Quantity}-{task.UnitOfMeasure}",
                        Product = task.ItemName,
                        Quantity = task.Quantity,
                        Parent = workOrderGanttData.Id,
                        Progress = (task.QuantityProduced > 0 && task.Quantity > 0) ? Math.Round(task.QuantityProduced.Value / task.Quantity.Value, 2) : 0,
                        Process = task.ProcessName,
                        WorkStation = task.WorkStationName,
                        Type = UserConstants.GANTT_TASK_TYPE_TASK,
                        start_date = task.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        end_date = task.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        Duration = task.Duration,
                        Color = task.Color,
                        TaskStatus = task.TaskStatus,
                    };
                    result.Add(ganttData);
                }
            }

            return Ok(result);
        }

        /// <summary>
        ///获取生产任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<TaskDto>), 200)]
        [PermissionAuthorize("produce:task:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加生产任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateTaskReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改生产任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<TaskDto>), 200)]
        [PermissionAuthorize("produce:task:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateTaskReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 重置生产任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Reset")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:edit")]
        public async Task<ActionResult> Reset([FromBody] ResetTaskReq req)
        {
            logger.LogInformation($"重置生产任务 BeginTime " + DateTime.Now);
            var result = await _mainService.Reset(req);
            logger.LogInformation($"重置生产任务 EndTime " + DateTime.Now);
            return Ok(result);
        }
        /// <summary>
        /// 修改生产任务甘特图时间
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("PutGantt")]
        [ProducesResponseType(typeof(ResponseDto<TaskDto>), 200)]
        [PermissionAuthorize("produce:task:edit")]
        public async Task<ActionResult> PutGantt([FromBody] UpdatTaskGanttReq req)
        {
            var result = await _mainService.UpdateGantt(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除生产任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除生产任务集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _mainService.DeleteDataList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 提交生产任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Commit")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:commit")]
        public async Task<ActionResult> Commit([FromBody] CommitTaskReq req)
        {
            var result = await _mainService.Commit(req);
            return Ok(result);
        }

        /// <summary>
        /// 撤销提交生产任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("RevokeCommit")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:revokeCommit")]
        public async Task<ActionResult> RevokeCommit([FromBody] CommitTaskReq req)
        {
            var result = await _mainService.RevokeCommit(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改生产任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateByOutSide")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:UpdateByOutSide")]
        public async Task<ActionResult> UpdateByOutSide([FromBody] UpdateTaskByOutSideReq req)
        {
            var result = await _mainService.UpdateByOutSide(req);
            return Ok(result);
        }

        /// <summary>
        /// 批量修改生产任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateListByOutSide")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:UpdateListByOutSide")]
        public async Task<ActionResult> UpdateListByOutSide([FromBody] UpdateTaskListByOutSideReq req)
        {
            var result = await _mainService.UpdateListByOutSideBatch(req);
            return Ok(result);
        }

        /// <summary>
        /// 导出生产任务Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("produce:task:download")]
        public async Task<ActionResult> DownLoadListAsync()
        {
            List<TaskToExcelDto> list = new List<TaskToExcelDto>();

            var result = await _mainService.GetList(new GetTaskListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            if (result != null && result.Data.List != null)
            {
                foreach (var item in result.Data.List)
                {
                    TaskToExcelDto model = new TaskToExcelDto();
                    model.BatchCode = item.BatchCode;
                    model.ClientCode = item.ClientCode;
                    model.ClientId = item.ClientId;
                    model.ClientName = item.ClientName;
                    model.Code = item.Code;
                    model.Color = item.Color;
                    model.Duration = item.Duration;
                    model.EndTime = item.EndTime;
                    model.StartTime = item.StartTime;
                    model.ItemCode = item.ItemCode;
                    model.ItemId = item.ItemId;
                    model.ItemName = item.ItemName;
                    model.ItemTypeId = item.ItemTypeId;
                    model.KeyFlag = item.KeyFlag;
                    model.Name = item.Name;
                    model.ParentId = item.ParentId;
                    model.ProcessCode = item.ProcessCode;
                    model.ProcessId = item.ProcessId;
                    model.ProcessName = item.ProcessName;
                    model.Quantity = item.Quantity;
                    model.QuantityProduced = item.QuantityProduced;
                    model.QuantityQuanlify = item.QuantityQuanlify;
                    model.QuantityUnquanlify = item.QuantityUnquanlify;
                    model.NowWadCount = item.NowWadCount;
                    model.RequestDate = item.RequestDate;
                    model.Specification = item.Specification;
                    model.TaskStatus = item.TaskStatus.ToString();
                    model.UnitOfMeasure = item.UnitOfMeasure;
                    model.WorkOrderCode = item.WorkOrderCode;
                    model.WorkOrderId = item.WorkOrderId;
                    model.WorkOrderName = item.WorkOrderName;
                    model.WorkOrderCode = item.WorkOrderCode;
                    model.WorkStationCode = item.WorkStationCode;
                    model.WorkStationId = item.WorkStationId;
                    model.WorkStationName = item.WorkStationName;

                    list.Add(model);
                }
            }

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "生产任务.xlsx");
        }

        /// <summary>
        /// 获取可用机台列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFitWorkStationList")]
        [ProducesResponseType(typeof(ResponseDto<List<FitWorkStationDto>>), 200)]
        [PermissionAuthorize("produce:task:list")]
        public async Task<ActionResult> GetFitWorkStationList([FromBody] GetFitWorkStationListReq req)
        {
            var result = await _mainService.GetFitWorkStationList(req);
            return Ok(result);
        }

        /// <summary>
        /// 理顺生产任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("RationalizeTask")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:rationalizeTask")]
        public async Task<ActionResult> RationalizeTask([FromBody] RationalizeTaskReq req)
        {
            logger.LogInformation($"理顺生产任务 BeginTime " + DateTime.Now);
            var result = await _mainService.RationalizeTask(req);
            logger.LogInformation($"理顺生产任务 EndTime " + DateTime.Now);

            return Ok(result);
        }
        /// <summary>
        /// 批量重算生产任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("BatchRationalizeTask")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:rationalizeTask")]
        public async Task<ActionResult> BatchRationalizeTask([FromBody] RationalizeTaskReq req)
        {
            logger.LogInformation($"批量重算生产任务 BeginTime " + DateTime.Now);
            var result = await _mainService.BatchRationalizeTask(req);
            logger.LogInformation($"批量重算生产任务 EndTime " + DateTime.Now);

            return Ok(result);
        }

        /// <summary>
        /// 转发任务到其他工作站
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("TransferTask")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:task:transferTask")]
        public async Task<ActionResult> TransferTask([FromBody] TransferTaskReq req)
        {
            logger.LogInformation($"转发任务到其他工作站 BeginTime " + DateTime.Now);
            var result = await _mainService.TransferTask(req);
            logger.LogInformation($"转发任务到其他工作站 EndTime " + DateTime.Now);

            return Ok(result);
        }

        /// <summary>
        /// 判断多个任务是否属于同一个工艺路线、同一个工序
        /// </summary>
        /// <param name="taskList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VerifyTaskBelongOneRouteAndProcess")]
        [ProducesResponseType(typeof(ResponseDto<RouteAndProcessDtoByTask>), 200)]
        [PermissionAuthorize("produce:task:list")]
        public async Task<ActionResult> VerifyTaskBelongOneRouteAndProcess([FromBody] List<TaskDto> taskList)
        {
            var result = await _mainService.VerifyTaskBelongOneRouteAndProcess(taskList);
            return Ok(result);
        }

        /// <summary>
        /// 根据设备编码查询Commit状态的任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTaskByDevice")]
        [ProducesResponseType(typeof(ResponseDto<List<TaskDto>>), 200)]
        [PermissionAuthorize("produce:task:list")]
        public async Task<ActionResult> GetTaskByDevice([FromBody] GetDrillOrAgvDeviceInfoReq req)
        {
            var result = await _mainService.GetTaskByDevice(req);
            return Ok(result);
        }

        /// <summary>
        /// 修改生产任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MoveTask")]
        [ProducesResponseType(typeof(ResponseDto<TaskDto>), 200)]
        [PermissionAuthorize("produce:task:edit")]
        public async Task<ActionResult> MoveTask([FromBody] List<AddOrUpdateTaskReq> req)
        {
            var result = await _mainService.MoveTaskList(req);
            return Ok(result);
        }
    }
}
