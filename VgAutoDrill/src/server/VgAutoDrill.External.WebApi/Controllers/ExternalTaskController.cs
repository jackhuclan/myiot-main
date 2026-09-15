
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;

namespace VgAutoDrill.External.WebApi.Controllers
{
    /// <summary>
    /// 任务外部接口
    /// </summary>
    public class ExternalTaskController : BaseController
    {
        private readonly IExternalTaskService _externalTaskService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalTaskService"></param>
        public ExternalTaskController(IExternalTaskService externalTaskService)
        {
            _externalTaskService = externalTaskService;
        }

        /// <summary>
        /// 获取钻机信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceList")]
        public async Task<ActionResult> GetDeviceList([FromBody] GetDeviceReq req)
        {
            var result = await _externalTaskService.GetDeviceList(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取钻机任务列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTaskList")]
        public async Task<ActionResult> GetTaskList([FromBody] GetTaskReq req)
        {
            var result = await _externalTaskService.GetTaskList(req);
            Stopwatch stopwatch = Stopwatch.StartNew();
            var cumTime = stopwatch.ElapsedMilliseconds;
            return Ok(result);
        }

        /// <summary>
        /// 根据目标任务换机台
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TaskMoveByTargetTask")]
        public async Task<ActionResult> TaskMoveByTargetTask([FromBody] TaskMoveByTargetTaskReq req)
        {
            var result = await _externalTaskService.TaskMoveByTargetTask(req);
            return Ok(result);
        }

        /// <summary>
        /// 根据目标钻机和时间调整任务顺序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TaskMoveByDeviceAndDate")]
        public async Task<ActionResult> TaskMoveByDeviceAndDate([FromBody] TaskMoveByDeviceAndDateReq req)
        {
            var result = await _externalTaskService.TaskMoveByDeviceAndDate(req);
            return Ok(result);
        }
    }
}
