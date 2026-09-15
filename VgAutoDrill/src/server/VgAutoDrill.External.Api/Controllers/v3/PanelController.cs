using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.External.Model.V3;

namespace VgAutoDrill.External.WebApi.Controllers.v3
{
    [Route("api/v3/external/panel")]
    [ApiController]
    public class PanelController : ControllerBase
    {
        private readonly ILogger<PanelController> logger;

        public PanelController(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<PanelController>();
        }

        /// <summary>
        /// 板料接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ExternalInterface(ExternalBaseReq req)
        {
            var result = new ResponseDto<string>();
            if (req == null)
            {
                result.Code = ResponseCode.Fail;
                result.Message = "未识别有效的入参！";
                return Ok(result);
            }

            if (string.IsNullOrEmpty(req.Action))
            {
                result.Code = ResponseCode.Fail;
                result.Message = "未识别有效的Action！";
                return Ok(result);
            }

            if (string.IsNullOrEmpty(req.Version))
            {
                result.Code = ResponseCode.Fail;
                result.Message = "未识别有效的Version！";
                return Ok(result);
            }

            if (req.Data == null || req.Data.Count == 0)
            {
                result.Code = ResponseCode.Fail;
                result.Message = "未识别有效的Data！";
                return Ok(result);
            }
            logger.LogInformation($"PanelController_ExternalInterface : " + req.Action + " BeginTime " + DateTime.Now);

            return Ok(result);
        }
    }
}
