using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.External.Application.Interfaces.ExternalService;
using VgAutoDrill.External.Model.V3;

namespace VgAutoDrill.External.WebApi.Controllers.v3;

[Route("api/v3/external/workorder")]
[ApiController]
public class WorkOrderController : ControllerBase
{
    private readonly IExternalWorkOrderServiceV3 _workOrderService;
    private readonly ILogger<WorkOrderController> logger;

    public WorkOrderController(IExternalWorkOrderServiceV3 workOrderService, ILoggerFactory loggerFactory)
    {
        _workOrderService = workOrderService;
        logger = loggerFactory.CreateLogger<WorkOrderController>();
    }

    /// <summary>
    /// 工单接口
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

        if (req.Data == null)
        {
            result.Code = ResponseCode.Fail;
            result.Message = "未识别有效的Data！";
            return Ok(result);
        }
        logger.LogInformation($"WorkOrderController_ExternalInterface : " + req.Action + " BeginTime " + DateTime.Now);

        try
        {
            string action = req.Action.ToUpper();
            switch (action)
            {
                case "ADD_WORKORDER":
                    if (!req.Data.ContainsKey("AddWorkOrderParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的新增数据！";
                        return Ok(result);
                    }
                    var addParam = req.Data["AddWorkOrderParam"];
                    if (addParam == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的新增数据！";
                        return Ok(result);
                    }
                    var addData = JsonConvert.DeserializeObject<AddOrUpdateExternalWorkOrderReq>(addParam.ToString());
                    if (addData == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的AddWorkOrderParam！";
                        return Ok(result);
                    }

                    var addResult = await _workOrderService.AddData(addData);
                    return Ok(addResult);

                case "UPDATE_WORKORDER":
                    if (!req.Data.ContainsKey("UpdateWorkOrderParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的修改数据！";
                        return Ok(result);
                    }
                    var updateParam = req.Data["UpdateWorkOrderParam"];
                    if (updateParam == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的修改数据！";
                        return Ok(result);
                    }
                    var updateData = JsonConvert.DeserializeObject<AddOrUpdateExternalWorkOrderReq>(updateParam.ToString());
                    if (updateData == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的UpdateWorkOrderParam！";
                        return Ok(result);
                    }

                    var updateResult = await _workOrderService.UpdateData(updateData);
                    return Ok(updateResult);

                case "DELETE_WORKORDER":
                    if (!req.Data.ContainsKey("DeleteWorkOrderParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的删除数据！";
                        return Ok(result);
                    }
                    var deleteParam = req.Data["DeleteWorkOrderParam"];
                    if (deleteParam == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的删除数据！";
                        return Ok(result);
                    }
                    var deleteData = JsonConvert.DeserializeObject<string>(deleteParam.ToString());
                    if (deleteData == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的DeleteWorkOrderParam！";
                        return Ok(result);
                    }

                    var deleteResult = await _workOrderService.DeleteData(deleteData);
                    return Ok(deleteResult);

                case "QUERY_WORKORDER":
                    if (!req.Data.ContainsKey("QueryWorkOrderParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的QueryWorkOrderParam！";
                        return Ok(result);
                    }
                    var queryParam = req.Data["QueryWorkOrderParam"];
                    if (queryParam == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的QueryWorkOrderParam！";
                        return Ok(result);
                    }
                    var queryData = JsonConvert.DeserializeObject<ExternalWorkOrderQueryReq>(queryParam.ToString());
                    if (queryData == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的QueryWorkOrderParam！";
                        return Ok(result);
                    }

                    var queryResult = await _workOrderService.QueryData(queryData);
                    return Ok(queryResult);

                case "BATCH_QUERY_WORKORDER":
                    if (!req.Data.ContainsKey("BatchQueryWorkOrderParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的BatchQueryWorkOrderParam！";
                        return Ok(result);
                    }
                    var batchQueryParam = req.Data["BatchQueryWorkOrderParam"];
                    if (batchQueryParam == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的BatchQueryWorkOrderParam！";
                        return Ok(result);
                    }
                    var batchQueryData = JsonConvert.DeserializeObject<BatchWorkOrderQueryReq>(batchQueryParam.ToString());
                    if (batchQueryData == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的BatchQueryWorkOrderParam！";
                        return Ok(result);
                    }

                    var batchQueryResult = await _workOrderService.BatchQueryData(batchQueryData);
                    return Ok(batchQueryResult);

                case "GET_WORKORDER_TASK":
                    if (!req.Data.ContainsKey("GetWorkOrderTaskParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的GetWorkOrderTaskParam！";
                        return Ok(result);
                    }
                    var getWorkTaskParam = req.Data["GetWorkOrderTaskParam"];
                    if (getWorkTaskParam == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的GetWorkOrderTaskParam！";
                        return Ok(result);
                    }
                    var getWorkTaskData = JsonConvert.DeserializeObject<ExternalWorkOrderQueryReq>(getWorkTaskParam.ToString());
                    if (getWorkTaskData == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的GetWorkOrderTaskParam！";
                        return Ok(result);
                    }

                    var getWorkTaskResult = await _workOrderService.GetWorkTask(getWorkTaskData);
                    return Ok(getWorkTaskResult);

                case "BATCH_GET_WORKORDER_TASK":
                    if (!req.Data.ContainsKey("BatchGetWorkOrderTaskParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的BatchGetWorkOrderTaskParam！";
                        return Ok(result);
                    }
                    var batchGetWorkTaskParam = req.Data["BatchGetWorkOrderTaskParam"];
                    if (batchGetWorkTaskParam == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的BatchGetWorkOrderTaskParam！";
                        return Ok(result);
                    }
                    var batchGetWorkTaskData = JsonConvert.DeserializeObject<BatchWorkOrderQueryReq>(batchGetWorkTaskParam.ToString());
                    if (batchGetWorkTaskData == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的BatchGetWorkOrderTaskParam！";
                        return Ok(result);
                    }

                    var batchGetWorkTaskResult = await _workOrderService.BatchGetWorkTask(batchGetWorkTaskData);
                    return Ok(batchGetWorkTaskResult);

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

    /// <summary>
    /// Test
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("Job")]
    public async Task<ActionResult> AnalyzeExternalWorkOrder()
    {
        var result = await _workOrderService.AnalyzeExternalWorkOrder();
        return Ok(result);
    }
}
