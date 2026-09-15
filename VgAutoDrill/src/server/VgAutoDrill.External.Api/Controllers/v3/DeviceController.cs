using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;
using VgAutoDrill.External.Application.Interfaces.ExternalService;
using VgAutoDrill.External.Model.V3;

namespace VgAutoDrill.External.WebApi.Controllers.v3
{
    [Route("api/v3/external/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IExternalDeviceServiceV3 _deviceExternalService;
        private readonly ILogger<DeviceController> logger;

        public DeviceController(IExternalDeviceServiceV3 deviceService, ILoggerFactory loggerFactory)
        {
            _deviceExternalService = deviceService;
            logger = loggerFactory.CreateLogger<DeviceController>();
        }

        /// <summary>
        /// 设备接口
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

            logger.LogInformation($"DeviceController_ExternalInterface : " + req.Action + " BeginTime " + DateTime.Now);

            try
            {
                string action = req.Action.ToUpper();
                switch (action)
                {
                    case "DELETE":
                        if (!req.Data.ContainsKey("DeleteDeviceIDList"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的删除数据！";
                            return Ok(result);
                        }
                        var delValue = req.Data["DeleteDeviceIDList"];
                        if (delValue == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的删除数据！";
                            return Ok(result);
                        }
                        var delData = JsonConvert.DeserializeObject<List<long>>(delValue.ToString());
                        if (delData == null || delData.Count == 0)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的删除数据！";
                            return Ok(result);
                        }
                        result = await _deviceExternalService.DeleteData(delData);
                        return Ok(result);

                    case "SELECT_DRILL_DEVICE":
                        if (!req.Data.ContainsKey("SelectDrillDeviceParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的SelectDeviceParam！";
                            return Ok(result);
                        }
                        var reqData = req.Data["SelectDrillDeviceParam"];
                        if (reqData == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的SelectDeviceParam！";
                            return Ok(result);
                        }
                        var selectDrillDeviceParam = JsonConvert.DeserializeObject<GetDeviceReq>(reqData.ToString());
                        if (selectDrillDeviceParam == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的SelectDrillDeviceParam！";
                            return Ok(result);
                        }
                        var data = await _deviceExternalService.GetDrillDeviceList(selectDrillDeviceParam);
                        return Ok(data);

                    case "SELECT_DRILL_TASK":
                        if (!req.Data.ContainsKey("SelectDrillTaskParam"))
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的SelectDrillTaskParam！";
                            return Ok(result);
                        }
                        var taskData = req.Data["SelectDrillTaskParam"];
                        if (taskData == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的SelectDrillTaskParam！";
                            return Ok(result);
                        }
                        var selectDrillTaskParam = JsonConvert.DeserializeObject<GetTaskReq>(taskData.ToString());
                        if (selectDrillTaskParam == null)
                        {
                            result.Code = ResponseCode.Fail;
                            result.Message = "未识别有效的SelectDrillTaskParam！";
                            return Ok(result);
                        }
                        var DTData = await _deviceExternalService.GetTaskList(selectDrillTaskParam);
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
}
