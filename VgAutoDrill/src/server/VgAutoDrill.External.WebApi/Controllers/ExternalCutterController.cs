
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;

namespace VgAutoDrill.External.WebApi.Controllers
{
    /// <summary>
    /// 配刀外部接口
    /// </summary>
    public class ExternalCutterController : BaseController
    {
        private readonly IExternalCutterService _externalCutterService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalTaskService"></param>
        public ExternalCutterController(IExternalCutterService externalCutterService)
        {
            _externalCutterService = externalCutterService;
        }

        /// <summary>
        ///检查组计划是否被修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Check")]
        public async Task<ActionResult> Check([FromBody] ExternalCutterGroupReq req)
        {
            var result = await _externalCutterService.Check(req);
            return Ok(result);
        }

        /// <summary>
        /// 锁定配刀组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Lock")]
        public async Task<ActionResult> Lock([FromBody] ExternalCutterGroupReq req)
        {
            var result = await _externalCutterService.Lock(req);
            return Ok(result);
        }

        /// <summary>
        /// 解锁配刀组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Unlock")]
        public async Task<ActionResult> Unlock([FromBody] ExternalCutterGroupReq req)
        {
            var result = await _externalCutterService.Unlock(req);
            return Ok(result);
        }

        /// <summary>
        /// 查询钻机配刀计划明细
        /// </summary>
        /// <param name="DeviceCodes"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPlanList")]
        public async Task<ActionResult> GetPlanList([FromBody] ExternalCutterDetailReq req)
        {
            var result = await _externalCutterService.GetPlanList(req.DeviceCodes);
            return Ok(result);
        }
    }
}
