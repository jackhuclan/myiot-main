using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;
using VgAutoDrill.External.Application.Interfaces.V3;
using VgAutoDrill.External.Model.V3;

namespace VgAutoDrill.External.WebApi.Controllers.v3;

[Route("api/v3/external/task")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly IExternalTaskServiceV3 _taskService;
    private readonly ILogger<TaskController> logger;

    public TaskController(IExternalTaskServiceV3 taskService, ILoggerFactory loggerFactory)
    {
        _taskService = taskService;
        logger = loggerFactory.CreateLogger<TaskController>();
    }

    /// <summary>
    /// 任务接口
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
        logger.LogInformation($"TaskController_ExternalInterface : " + req.Action + " BeginTime " + DateTime.Now);

        try
        {
            string action = req.Action.ToUpper();
            switch (action)
            {
                case "ADD":
                    if (!req.Data.ContainsKey("AddTaskParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的新增数据！";
                        return Ok(result);
                    }

                    break;
                case "UPDATE":
                    if (!req.Data.ContainsKey("UpdateTaskParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的修改数据！";
                        return Ok(result);
                    }

                    break;
                case "DELETE":
                    if (!req.Data.ContainsKey("DeleteTaskIDList"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的删除数据！";
                        return Ok(result);
                    }

                    break;
                case "MOVE_TASK_BY_TARGET_TASK":
                    if (!req.Data.ContainsKey("MoveByTargetTaskParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的MoveByTargetTaskParam！";
                        return Ok(result);
                    }
                    var reqData = req.Data["MoveByTargetTaskParam"];
                    if (reqData == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的MoveByTargetTaskParam！";
                        return Ok(result);
                    }
                    var moveByTargetTaskParam = JsonConvert.DeserializeObject<TaskMoveByTargetTaskReq>(reqData.ToString());
                    if (moveByTargetTaskParam == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的MoveByTargetTaskParam！";
                        return Ok(result);
                    }
                    var data = await _taskService.TaskMoveByTargetTask(moveByTargetTaskParam);
                    return Ok(data);

                case "MOVE_TASK_BY_DEVICE_AND_DATE":
                    if (!req.Data.ContainsKey("MoveTaskByDeviceAndDateParam"))
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的MoveTaskByDeviceAndDateParam！";
                        return Ok(result);
                    }
                    var taskData = req.Data["MoveTaskByDeviceAndDateParam"];
                    if (taskData == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的MoveTaskByDeviceAndDateParam！";
                        return Ok(result);
                    }
                    var moveTaskByDeviceAndDateParam = JsonConvert.DeserializeObject<TaskMoveByDeviceAndDateReq>(taskData.ToString());
                    if (moveTaskByDeviceAndDateParam == null)
                    {
                        result.Code = ResponseCode.Fail;
                        result.Message = "未识别有效的MoveTaskByDeviceAndDateParam！";
                        return Ok(result);
                    }
                    var DTData = await _taskService.TaskMoveByDeviceAndDate(moveTaskByDeviceAndDateParam);
                    return Ok(DTData);

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
