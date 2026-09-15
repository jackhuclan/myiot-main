using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.External.Application.Interfaces.V3;
using VgAutoDrill.External.Model.V3;

namespace VgAutoDrill.External.WebApi.Controllers.v3;

[Route("api/v3/external/cutter")]
[ApiController]
public class CutterController : ControllerBase
{
    private readonly IExternalCutterServiceV3 _externalCutterService;
    private readonly ILogger<CutterController> logger;

    public CutterController(IExternalCutterServiceV3 externalCutterService, ILoggerFactory loggerFactory)
    {
        _externalCutterService = externalCutterService;
        logger = loggerFactory.CreateLogger<CutterController>();
    }

    /// <summary>
    /// 刀具接口
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
        logger.LogInformation($"CutterController_ExternalInterface : " + req.Action + " BeginTime " + DateTime.Now);

        try
        {
            string action = req.Action.ToUpper();
            switch (action)
            {
                case "ADD":
                    if (!req.Data.ContainsKey("AddCutterParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的新增数据！";
                        return Ok(result);
                    }

                    break;
                case "UPDATE":
                    if (!req.Data.ContainsKey("UpdateCutterParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的修改数据！";
                        return Ok(result);
                    }

                    break;
                case "DELETE":
                    if (!req.Data.ContainsKey("DeleteCutterIDList"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的删除数据！";
                        return Ok(result);
                    }

                    break;
                default:
                    result.Code = ResponseCode.Fail;
                    result.Message = "未识别有效的Action！";
                    return Ok(result);
            }
        }
        catch (Exception ex)
        {
            result.Code = ResponseCode.Fail;
            result.Message = ex.Message;
        }
        return Ok(result);
    }

}
