using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar.Extensions;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class CentralOnlineDevice : ICentralOnlineDevice
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CentralOnlineDevice> _logger;
        private readonly IAPIHelper _apiHelper;
        private readonly IDeviceDomainService _deviceDomainService;

        public CentralOnlineDevice(IConfiguration configuration,
            ILoggerFactory loggerFactory,
            IAPIHelper apiHelper,
            IDeviceDomainService deviceDomainService
            )
        {
            _configuration = configuration;
            _apiHelper = apiHelper;
            _logger = loggerFactory.CreateLogger<CentralOnlineDevice>();
            _deviceDomainService = deviceDomainService;
        }

        public async Task<List<CentralOnlineDeviceDto>> GetOnlineDevicesAndRoute()
        {
            //获取中控在线列表（包括工艺路线），刷新数据状态
            var fullDeviceInfo = new List<CentralOnlineDeviceDto>();
            try
            {
                string urlAddress = _configuration["AppConfig:CentralOnlineDevice"];
                if (!string.IsNullOrEmpty(urlAddress))
                {
                    var deviceResult = _apiHelper.RequestData(urlAddress);
                    if (deviceResult != null)
                    {
                        var deviceData = JsonSerializer.Deserialize<List<CentralOnlineDeviceDto>>(deviceResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        foreach (var device in deviceData)
                        {
                            device.Properties["DeviceStandbyTime"] = device.DeviceStandbyTime;
                            device.LoginTime = device.Properties.ContainsKey("LogInTime") ? device.Properties["LogInTime"].ToString().ObjToDate() : null;
                            if (device.LoginTime == DateTime.MinValue)
                            {
                                device.LoginTime = null;
                            }
                        }
                        fullDeviceInfo = await _deviceDomainService.GetOnlineDeviceInfo(deviceData);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return fullDeviceInfo;
        }

        public async Task<List<CentralOnlineDeviceDto>> GetOnlineDevices()
        {
            var deviceData = new List<CentralOnlineDeviceDto>();
            try
            {
                string urlAddress = _configuration["AppConfig:CentralOnlineDevice"];
                if (!string.IsNullOrEmpty(urlAddress))
                {
                    var deviceResult = _apiHelper.RequestData(urlAddress);
                    if (deviceResult != null)
                    {
                        deviceData = JsonSerializer.Deserialize<List<CentralOnlineDeviceDto>>(deviceResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        foreach (var device in deviceData)
                        {
                            device.Properties["DeviceStandbyTime"] = device.DeviceStandbyTime == null ? string.Empty : device.DeviceStandbyTime;
                            device.LoginTime = device.Properties.ContainsKey("LogInTime") ? device.Properties["LogInTime"].ToString().ObjToDate() : null;
                            if (device.LoginTime == DateTime.MinValue)
                            {
                                device.LoginTime = null;
                            }
                            device.DrillState = device.Properties.ContainsKey("Drill_State") ? device.Properties["Drill_State"].ToStr() : string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return deviceData;
        }

        public async Task<string> AllotsDeviceCommand(string urlAddress, DeviceCommandCentralRequest commandRequest)
        {
            if (string.IsNullOrEmpty(urlAddress))
            {
                return "未识别有效的CentralAllotsDeviceCommand！";
            }
            if (commandRequest == null || string.IsNullOrEmpty(commandRequest.DeviceId) || string.IsNullOrEmpty(commandRequest.Command))
            {
                return "未识别有效的DeviceCommandCentralRequest！";
            }

            try
            {
                string requestJson = JsonSerializer.Serialize(commandRequest);
                var result = _apiHelper.RequestData(urlAddress, "post", requestJson);
                if (string.IsNullOrEmpty(result))
                {
                    return "AllotsDeviceCommand return null !";
                }

                _logger.LogInformation($"AllotsDeviceCommand Url{urlAddress} deviceId {commandRequest.DeviceId}, command {JsonSerializer.Serialize(commandRequest)}, result: {result}");

                var response = JsonSerializer.Deserialize<DeviceCommandResponse>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (response == null || (!string.IsNullOrEmpty(response.Code) && response.Code != ErrorCodes.Sys.SUCCESS))
                {
                    string msg = response != null ? response.Message : "";
                    return $"失败原因：{response.Code} {msg}";
                }
            }
            catch (Exception e)
            {
                return "异常：" + e.Message;
            }
            return string.Empty;
        }
    }
}
