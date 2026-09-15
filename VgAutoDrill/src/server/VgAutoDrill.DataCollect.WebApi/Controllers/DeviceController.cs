using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VgAutoDrill.DataCollect.Application.Interfaces;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.DataCollect.WebApi.Controllers
{
    [ApiController]
    [Route("v1/datacollect/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> _logger;
        private readonly IDataCollectService _dataCollectService;

        public DeviceController(IDataCollectService dataCollectService
            , ILoggerFactory loggerFactory)
        {
            _dataCollectService = dataCollectService;
            _logger = loggerFactory.CreateLogger<DeviceController>();
        }

        /// <summary>
        /// 保存属性数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("properties/report", Name = "DevicePropertiesReport")]
        public virtual DevicePropertiesReportResponse DevicePropertiesReport(DevicePropertiesReportRequest request)
        {
            ThrowIfNull(request);

            var requestContent = JsonSerializer.Serialize(request);
            _logger.LogInformation($"DevicePropertiesReport  requestContent:{requestContent}--{DateTime.Now}");

            return _dataCollectService.DevicePropertiesReport(request);
        }

        private static void ThrowIfNull(DevicePropertiesReportRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.ProductId == null)
            {
                throw new ArgumentNullException(nameof(request.ProductId));
            }

            if (request.DeviceId == null)
            {
                throw new ArgumentNullException(nameof(request.DeviceId));
            }

            if (request.Params == null || request.Params.Count == 0)
            {
                throw new ArgumentNullException(nameof(request.Params));
            }
        }
    }
}