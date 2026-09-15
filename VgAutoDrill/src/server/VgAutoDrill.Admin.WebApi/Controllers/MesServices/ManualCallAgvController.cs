using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.res;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 手动呼叫agv
    /// </summary>
    public class ManualCallAgvController : BaseController
    {
        private readonly IManualCallAgvLogService _manualCallAgvLogService;
        private readonly IManualCallAgvTaskService _manualCallAgvTaskService;

        public ManualCallAgvController(IManualCallAgvLogService manualCallAgvLogService,
            IManualCallAgvTaskService manualCallAgvTaskService)
        {
            _manualCallAgvTaskService = manualCallAgvTaskService;
            _manualCallAgvLogService = manualCallAgvLogService;
        }

        /// <summary>
        /// 手动呼叫AGV上下料操作记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Log/List")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<ManualCallAgvLogDto>>), 200)]
        [PermissionAuthorize("")]
        public async Task<ActionResult> List(ListReq req)
        {
            var data = await _manualCallAgvLogService.List(req);
            return Ok(data);
        }



        /// <summary>
        /// agv操作
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AgvOperate")]
        [ProducesResponseType(typeof(ResponseDto<bool>), 200)]
        [PermissionAuthorize("")]
        public async Task<ActionResult> AgvOperate(AgvOperateReq req)
        {
            var result = await _manualCallAgvLogService.AgvOperate(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取agv运行状态(页面轮询)
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAgvRunStatus")]
        [ProducesResponseType(typeof(ResponseDto<GetAgvRunStatusResponse>), 200)]
        [PermissionAuthorize("")]
        public async Task<ActionResult> GetAgvRunStatus(string locationCode)
        {
            var result = await _manualCallAgvLogService.GetAgvRunStatus(locationCode);
            return Ok(result);
        }
        /// <summary>
        /// 解绑时获取呼叫agv任务信息
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAgvTaskByLocationCode")]
        [ProducesResponseType(typeof(ResponseDto<ManualCallAgvTaskDto>), 200)]
        [PermissionAuthorize("")]
        public async Task<ActionResult> GetAgvTaskByLocationCode(string locationCode)
        {
            var result = await _manualCallAgvTaskService.GetAgvTaskByLocationCode(locationCode);
            return Ok(result);
        }



        /// <summary>
        /// 根据钻机code获取任务列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TaskList")]
        [ProducesResponseType(typeof(ResponseDto<PageDto<TaskDto>>), 200)]
        [PermissionAuthorize("")]
        [AllowAnonymous]
        public async Task<ActionResult> TaskList(GetDrillOrAgvDeviceInfoReq req)
        {
            var result = await _manualCallAgvLogService.TaskList(req);
            return Ok(result);
        }

        /// <summary>
        /// 加载钻带文件
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoadingDrillFile")]
        [ProducesResponseType(typeof(ResponseDto<bool>), 200)]
        [PermissionAuthorize("")]
        public async Task<ActionResult> LoadingDrillFile(LoadingDrillfileReq req)
        {
            var result = await _manualCallAgvLogService.LoadingDrillFile(req);
            return Ok(result);
        }

        /// <summary>
        /// 加载ATP文件
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoadingATPFile")]
        [ProducesResponseType(typeof(ResponseDto<bool>), 200)]
        [PermissionAuthorize("")]
        public async Task<ActionResult> LoadingATPFile(LoadingAtpFileReq req)
        {
            var result = await _manualCallAgvLogService.LoadingATPFile(req);
            return Ok(result);
        }
        
        /// <summary>
        /// 更新任务状态(开始和结束)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateTaskStatus")]
        [ProducesResponseType(typeof(ResponseDto<bool>), 200)]
        [PermissionAuthorize("")]
        public async Task<ActionResult> UpdateTaskStatus(UpdateTaskByOutSideReq req)
        {
            var result = await _manualCallAgvLogService.UpdateTaskStatus(req);
            return Ok(result);
        }

        /// <summary>
        /// 根据钻机code查询手动呼叫agv任务信息
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ManualCallAgvTask/QueryByDeviceCode")]
        [ProducesResponseType(typeof(ResponseDto<List<ManualCallAgvTaskDto>>), 200)]
        [PermissionAuthorize("")]
        public async Task<ActionResult> QueryManualCallAgvTaskByDeviceCode(string deviceCode)
        {
            var result = await _manualCallAgvTaskService.QueryByDeviceCode(deviceCode);
            return Ok(result);
        }

        /// <summary>
        /// 更新手动呼叫agv任务信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ManualCallAgvTask/Update")]
        [ProducesResponseType(typeof(ResponseDto<List<ManualCallAgvTaskDto>>), 200)]
        [PermissionAuthorize("")]
        public async Task<ActionResult> UpdateManualCallAgvTask(AddOrUpdateManualCallAgvTaskReq req)
        {
            var result = await _manualCallAgvTaskService.UpdateManualCallAgvTask(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取系统刀盒码
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAptBoxBarcode")]
        [PermissionAuthorize("")]
        public async Task<ActionResult> GetAptBoxBarcode(string groupNo,string deviceCode)
        {
            var result = await _manualCallAgvTaskService.GetAptBoxBarcode(groupNo, deviceCode);
            return Ok(result);
        }

        /// <summary>
        /// 比对刀盒码 并更新状态
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConfirmAptBoxBarcode")]
        [PermissionAuthorize("")]
        public async Task<ActionResult> ConfirmAptBoxBarcode(AptBoxReq aptBoxReq)
        {
            var result = await _manualCallAgvTaskService.ConfirmAptBoxBarcode(aptBoxReq);
            return Ok(result);
        }
    }
}
