using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices.ExternalXianJin;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalXianJin;

namespace VgAutoDrill.External.WebApi.Controllers
{
    /// <summary>
    /// 先进相关接口
    /// </summary>
    public class ExternalXianJinController : BaseController
    {
        private readonly IExternalXianJinService _externalXianJinService;

        public ExternalXianJinController(IExternalXianJinService externalXianJinService)
        {
            _externalXianJinService = externalXianJinService;
        }

        /// <summary>
        /// 查询设备未开始的任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("QueryTaskByDrill")]
        public async Task<ActionResult> QueryTaskByDrill(string deviceId)
        {
            var result = await _externalXianJinService.QueryTaskByDrill(deviceId);
            return Ok(result);
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SetBegin")]
        public async Task<ActionResult> SetBegin([FromBody] SetDrillCommandReq req)
        {
            var result = await _externalXianJinService.SetBegin(req);
            return Ok(result);
        }

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryDrillInfo")]
        public async Task<ActionResult> QueryDrillInfo(List<string> deviceIds)
        {
            var result = await _externalXianJinService.QueryDrillInfo(deviceIds);
            return Ok(result);
        }

        /// <summary>
        /// 获取工单状态及任务状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetWorkTaskBatch")]
        public async Task<ActionResult> GetWorkTaskBatch(List<ExternalWorkOrderQueryReq> reqs)
        {
            var result = await _externalXianJinService.GetWorkTaskBatch(reqs);
            return Ok(result);
        }

        /// <summary>
        /// 下熟料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UnderClinkerPanel")]
        public async Task<ActionResult> UnderClinkerPanel([FromBody] SetDrillCommandReq req)
        {
            var result = await _externalXianJinService.UnderClinkerPanel(req);
            return Ok(result);
        }

        /// <summary>
        /// 结束下熟料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SetComplete")]
        public async Task<ActionResult> SetComplete([FromBody] SetDrillCommandReq req)
        {
            var result = await _externalXianJinService.SetComplete(req);
            return Ok(result);
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ClearTaskByDevice")]
        public async Task<ActionResult> ClearTaskByDevice(string deviceId)
        {
            var result = await _externalXianJinService.ClearTaskByDevice(deviceId);
            return Ok(result);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteTaskBySourceCode")]
        public async Task<ActionResult> DeleteTaskBySourceCode(string sourceCode)
        {
            var result = await _externalXianJinService.DeleteTaskBySourceCode(sourceCode);
            return Ok(result);
        }
    }
}
