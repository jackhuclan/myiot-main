using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 设备管理
    /// </summary>
    public class DeviceController : BaseController
    {
        private readonly IDeviceService _deviceService;
        private readonly IConfiguration _configuration;
        private readonly IAPIHelper _apiHelper;
        private readonly InnerOptions _innerOptions;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentService"></param>
        /// <param name="dataCollectService"></param>
        public DeviceController(IDeviceService equipmentService,
            IConfiguration configuration,
            IAPIHelper apiHelper,
            IOptions<InnerOptions> options)
        {
            _deviceService = equipmentService;
            _configuration = configuration;
            _apiHelper = apiHelper;
            _innerOptions = options.Value;
        }
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTreeSelect")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceTreeDto>>), 200)]
        [PermissionAuthorize("device:equipment:list")]
        public async Task<ActionResult> GetTreeSelectList()
        {
            var result = await _deviceService.GetEquipmentTreeList();
            return Ok(result);
        }
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<DeviceDto>>), 200)]
        [PermissionAuthorize("device:equipment:list")]
        public async Task<ActionResult> GetEquipmentList([FromBody] GetDeviceListReq req)
        {
            var result = await _deviceService.GetEquipmentList(req);
            return Ok(result);
        }

        /// <summary>
        ///获取设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<DeviceFullDataDto>), 200)]
        [PermissionAuthorize("device:equipment:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _deviceService.QueryFullDataByID(id);
            return Ok(result);
        }
        ///// <summary>
        /////获取设备状态日志
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("GetDeviceStatusList")]
        //[ProducesResponseType(typeof(ResponseDto<List<DeviceStatusDto>>), 200)]
        //[PermissionAuthorize("device:equipment:view")]
        //public async Task<ActionResult> GetDeviceStatusList(GetDeviceStatusList input)
        //{
        //    if (input == null)
        //    {
        //        throw new ArgumentNullException(nameof(input), "input should not be null");
        //    }
        //    if (string.IsNullOrEmpty(input.DeviceCode))
        //    {
        //        throw new ArgumentNullException(nameof(input), "input.DeviceCode should not be 0");
        //    }
        //    if (input.Limit == 0) input.Limit = 10;

        //    var result = await _dataCollectService.GetDeviceStatusList(input);
        //    return Ok(result);
        //}
        ///// <summary>
        /////获取设备服务日志
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("GetDeviceServiceList")]
        //[ProducesResponseType(typeof(ResponseDto<List<DeviceServiceDto>>), 200)]
        //[PermissionAuthorize("device:equipment:view")]
        //public async Task<ActionResult> GetDeviceServiceList(GetDeviceServiceList input)
        //{
        //    if (input == null)
        //    {
        //        throw new ArgumentNullException(nameof(input), "input should not be null");
        //    }
        //    if (string.IsNullOrEmpty(input.DeviceCode))
        //    {
        //        throw new ArgumentNullException(nameof(input), "input.DeviceCode should not be 0");
        //    }
        //    if (input.Limit == 0) input.Limit = 10;

        //    var result = await _dataCollectService.GetDeviceServiceList(input);
        //    return Ok(result);
        //}
        ///// <summary>
        /////获取设备属性日志
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("GetDevicePropertyList")]
        //[ProducesResponseType(typeof(ResponseDto<List<DevicePropertyDto>>), 200)]
        //[PermissionAuthorize("device:equipment:view")]
        //public async Task<ActionResult> GetDevicePropertyList(GetDevicePropertyList input)
        //{
        //    if (input == null)
        //    {
        //        throw new ArgumentNullException(nameof(input), "input should not be null");
        //    }
        //    if (string.IsNullOrEmpty(input.DeviceCode))
        //    {
        //        throw new ArgumentNullException(nameof(input), "input.DeviceCode should not be 0");
        //    }
        //    if (input.Limit == 0) input.Limit = 10;

        //    var result = await _dataCollectService.GetDevicePropertyList(input);
        //    return Ok(result);
        //}
        ///// <summary>
        /////获取设备事件日志
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("GetDeviceEventList")]
        //[ProducesResponseType(typeof(ResponseDto<List<DeviceEventDto>>), 200)]
        //[PermissionAuthorize("device:equipment:view")]
        //public async Task<ActionResult> GetDeviceEventList(GetDeviceEventList input)
        //{
        //    if (input == null)
        //    {
        //        throw new ArgumentNullException(nameof(input), "input should not be null");
        //    }
        //    if (string.IsNullOrEmpty(input.DeviceCode))
        //    {
        //        throw new ArgumentNullException(nameof(input), "input.DeviceCode should not be 0");
        //    }
        //    if (input.Limit == 0) input.Limit = 10;

        //    var result = await _dataCollectService.GetDeviceEventList(input);
        //    return Ok(result);
        //}

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equipment:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateDeviceReq req)
        {
            var result = await _deviceService.AddData(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改设备
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<DeviceInfoDto>), 200)]
        [PermissionAuthorize("device:equipment:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateDeviceReq req)
        {
            var result = await _deviceService.UpdateData(req);
            return Ok(result);
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equipment:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _deviceService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除设备集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equipment:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _deviceService.DeleteDataList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 获取中控系统在线设备列表
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        [Route("GetCentralOnlineDevice")]
        [ProducesResponseType(typeof(ResponseDto<List<CentralOnlineDeviceDto>>), 200)]
        [PermissionAuthorize("device:equipment:list")]
        public async Task<ActionResult> GetCentralOnlineDevice([FromBody] CentralOnlineDeviceReq req)
        {
            var result = await _deviceService.GetCentralOnlineDevice(req);
            return Ok(result);
        }

        /// <summary>
        ///获取中控系统在线设备详细信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOnlineDeviceInfo")]
        [ProducesResponseType(typeof(ResponseDto<CentralOnlineDeviceDto>), 200)]
        [PermissionAuthorize("device:equipment:view")]
        public async Task<ActionResult> GetOnlineDeviceInfo(string deviceId)
        {
            var result = await _deviceService.GetOnlineDeviceInfo(deviceId);
            return Ok(result);
        }

        /// <summary>
        /// 获取在线钻机待做任务
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        [Route("GetDrillDeviceTask")]
        [ProducesResponseType(typeof(ResponseDto<List<DrillDeviceTaskDto>>), 200)]
        [PermissionAuthorize("device:equipment:list")]
        public async Task<ActionResult> GetDrillDeviceTask([FromBody] GetDrillOrAgvDeviceInfoReq req)
        {
            var result = await _deviceService.GetDrillDeviceTask(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取在线AGV板料信息
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        [Route("GetAGVDeviceSiloInfo")]
        [ProducesResponseType(typeof(ResponseDto<List<AGVDeviceSiloInfo>>), 200)]
        [PermissionAuthorize("device:equipment:list")]
        public async Task<ActionResult> GetAGVDeviceSiloInfo([FromBody] GetDrillOrAgvDeviceInfoReq req)
        {
            var result = await _deviceService.GetAGVDeviceSiloInfo(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取首页设备数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetHomeDeviceData")]
        [ProducesResponseType(typeof(ResponseDto<DeviceStatusForHomeDto>), 200)]
        [PermissionAuthorize("device:equipment:view")]
        public async Task<ActionResult> GetHomeDeviceData()
        {
            var result = await _deviceService.GetHomeData();
            return Ok(result);
        }

        /// <summary>
        /// 根据设备获取关联的生产线
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRouteAndProcessInfo")]
        [ProducesResponseType(typeof(ResponseDto<RouteAndProcessInfoByDeviceDto>), 200)]
        [PermissionAuthorize("device:equipment:view")]
        public async Task<ActionResult> GetRouteAndProcessInfo(string agvDeviceCode, string anyDeviceCode)
        {
            var result = await _deviceService.GetRouteAndProcessInfo(agvDeviceCode, anyDeviceCode);
            return Ok(result);
        }

        /// <summary>
        /// 重置redis叫料 锁 的Key
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        [Route("CleanLockerData")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equipment:cleanLockerData")]
        public async Task<ActionResult> CleanLockerData([FromBody] CentralOnlineDeviceDto req)
        {
            //CleanLockerData
            string urlAddress = _configuration["AppConfig:CleanLockerData"];
            if (string.IsNullOrEmpty(urlAddress))
            {
                throw new ArgumentNullException("config", "未配置 CleanLockerData！");
            }

            if (string.IsNullOrEmpty(req.ProductId) || string.IsNullOrEmpty(req.RoutingKey) || string.IsNullOrEmpty(req.DeviceId))
            {
                return Ok("ProductId, RoutingKey, DeviceId should not be null");
            }

            string queryCondition = "?";
            if (!string.IsNullOrEmpty(req.ProductId))
            {
                queryCondition += string.Format("productId={0}&", req.ProductId);
            }
            if (!string.IsNullOrEmpty(req.DeviceId))
            {
                queryCondition += string.Format("deviceId={0}&", req.DeviceId);
            }
            if (!string.IsNullOrEmpty(req.RoutingKey))
            {
                queryCondition += string.Format("routingKey={0}&", req.RoutingKey);
            }
            if (queryCondition.Substring(queryCondition.Length - 1, 1) == "&")
            {
                queryCondition = queryCondition.Remove(queryCondition.Length - 1);
            }

            var result = _apiHelper.RequestData(urlAddress + queryCondition);
            if (result == null)
            {
                throw new ArgumentNullException("result", "未查询到结果！");
            }

            return Ok(result);
        }

        /// <summary>
        /// 重置AGV信号
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        [Route("ResetAgvCallLimit")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equipment:resetAgvCallLimit")]
        public async Task<ActionResult> ResetAgvCallLimit([FromBody] CentralOnlineDeviceDto req)
        {
            //ResetAgvCallLimit
            string urlAddress = _configuration["AppConfig:ResetAgvCallLimit"];
            if (string.IsNullOrEmpty(urlAddress))
            {
                throw new ArgumentNullException("config", "未配置 ResetAgvCallLimit！");
            }

            if (string.IsNullOrEmpty(req.ProductId) || string.IsNullOrEmpty(req.DeviceId))
            {
                return Ok("ProductId, DeviceId should not be null");
            }

            string queryCondition = "?";
            if (!string.IsNullOrEmpty(req.ProductId))
            {
                queryCondition += string.Format("productId={0}&", req.ProductId);
            }
            if (!string.IsNullOrEmpty(req.DeviceId))
            {
                queryCondition += string.Format("deviceId={0}&", req.DeviceId);
            }
            if (queryCondition.Substring(queryCondition.Length - 1, 1) == "&")
            {
                queryCondition = queryCondition.Remove(queryCondition.Length - 1);
            }

            var result = _apiHelper.RequestData(urlAddress + queryCondition);
            if (result == null)
            {
                throw new ArgumentNullException("result", "未查询到结果！");
            }

            return Ok(result);
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Enable")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equipment:edit")]
        public async Task<ActionResult> Enable(string deviceCode)
        {
            var result = await _deviceService.SetDeviceStatus(deviceCode, 1);
            return Ok(result);
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Disable")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equipment:edit")]
        public async Task<ActionResult> Disable(string deviceCode)
        {
            var result = await _deviceService.SetDeviceStatus(deviceCode, 0);
            if (result != null && result.Code == ResponseCode.Success)
            {
                ResponseDto<string> responseDto = new ResponseDto<string>();

                if (string.IsNullOrEmpty(_innerOptions.CancelScheduleUrl))
                {
                    responseDto.Code = ResponseCode.Fail;
                    responseDto.Message = "没有设置innerOptions.CancelScheduleUrl，取消设备的调度记录失败，请联系相关人员手动取消调度记录！";
                    return Ok(responseDto);
                }

                var request = new CancelScheduleTaskRequest
                {
                    Params = new Dictionary<string, object?>
                    {
                        { "DeviceCode", deviceCode },
                        { "CancelReason", "设备被禁用" }
                    }
                };

                _apiHelper.RequestData(_innerOptions.CancelScheduleUrl, "post", JsonSerializer.Serialize(request));
            }

            return Ok(result);
        }

        /// <summary>
        /// 导入设备
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadList")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equipment:upload")]
        public async Task<ActionResult> UploadListAsync([FromForm(Name = "file")] IFormFile objFile)
        {
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                //设备List
                List<DeviceToExcelDto> list = new List<DeviceToExcelDto>();

                //文件转成流
                using (var fileStream = objFile.OpenReadStream())
                {
                    //excel转实体
                    list = ExcelHelper.ParseExcelToList<DeviceToExcelDto>(fileStream, "Sheet1");
                }

                var result = await _deviceService.BulkInsert(list);
                return Ok(result);
            }
            catch
            {
                var result = new ResponseDto<string>();
                result.Code = ResponseCode.Fail;
                result.Message = "请参考模板准备导入数据！";
                return Ok(result);
            }
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoad")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("device:equipment:download")]
        public ActionResult DownLoad()
        {
            //添加示例数据
            List<DeviceToExcelDto> list = new List<DeviceToExcelDto> {
                new DeviceToExcelDto
                {
                    Code = "D5-2849-241",
                    Name = "D5-2849-241",
                    SpindleNum = 5,
                    DeviceTypeCode = "drill",
                },
                new DeviceToExcelDto
                {
                    Code = "MockAGV01",
                    Name = "MockAGV01",
                    SpindleNum = 1,
                    DeviceTypeCode = "agv",
                }};

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "设备模板.xlsx");
        }

        /// <summary>
        /// 下发设备指令
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AllotsDeviceCommand")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("device:equipment:edit")]
        public async Task<ActionResult> AllotsDeviceCommand(DeviceCommandRequest commandRequest)
        {
            var result = await _deviceService.AllotsDeviceCommand(commandRequest);
            return Ok(result);
        }
    }
}
