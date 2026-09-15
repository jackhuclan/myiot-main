using ClickHouse.Client.ADO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using VgAutoDrill.DataCollect.Application.Interfaces;
using VgAutoDrill.DataCollect.Application.Models;
using VgAutoDrill.DataCollect.Application.Models.DeviceEvent;
using VgAutoDrill.DataCollect.Application.Models.DeviceProperty;
using VgAutoDrill.DataCollect.Application.Models.DeviceService;
using VgAutoDrill.DataCollect.Application.Models.DeviceStatus;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.DataCollect.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class DataCollectService : BaseService, IDataCollectService
    {
        private readonly ClickHouseConnection connection;
        private readonly ILogger<DataCollectService> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        public DataCollectService(IOptions<ClickhouseOptions> options
            , ILoggerFactory loggerFactory)
        {
            var clickhouseOptions = options.Value;
            logger = loggerFactory.CreateLogger<DataCollectService>();

            var builder = new ClickHouseConnectionStringBuilder(clickhouseOptions.ConnectionString);
            builder.Compression = clickhouseOptions.Compression;
            builder.UseSession = clickhouseOptions.Session;
            builder.UseCustomDecimals = clickhouseOptions.CustomDecimals;
            connection = new ClickHouseConnection(builder.ConnectionString);
            logger.LogDebug($"this.connection :{connection}");
        }

        /// <summary>
        /// 校验ClickHouse是否连接成功
        /// </summary>
        /// <returns></returns>
        private bool CheckConnectState()
        {
            Stopwatch sw = new Stopwatch();
            bool connectSuccess = false;

            Thread t = new Thread(delegate ()
            {
                try
                {
                    sw.Start();
                    connection.Open();
                    connectSuccess = true;
                }
                catch
                {
                    connectSuccess = false;
                }
                finally
                {
                    sw.Stop();
                    connection.Close();
                }
            });

            t.IsBackground = true;
            t.Start();

            while (3000 > sw.ElapsedMilliseconds)
                if (t.Join(1))
                    break;

            return connectSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DevicePropertiesReportResponse DevicePropertiesReport(DevicePropertiesReportRequest request)
        {
            var response = new DevicePropertiesReportResponse()
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = string.Empty
            };

            try
            {
                var proproertyContent = JsonSerializer.Serialize(request);
                using var command = connection.CreateCommand();
                command.CommandText = $"insert into device_property_trace(device_id,product_id,properpty_json) values('{request.DeviceId}','{request.ProductId}','{proproertyContent}')";

                logger.LogInformation($"DevicePropertiesReport sql:{command.CommandText}");

                command.ExecuteScalar();

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError($"DevicePropertiesReport  {request.DeviceId} error:{ex.Message}");
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public void HandleDeviceEventReport(DeviceEventReportRequest request, DeviceEventReportResponse response)
        {
            var requestContent = JsonSerializer.Serialize(request);
            var responseContent = JsonSerializer.Serialize(response);

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"insert into device_event_trace(device_id,product_id,event_id,event_name,request_json,response_json)"
                    + $" values('{request.DeviceId}','{request.ProductId}','{request.EventId}','{request.EventName}','{requestContent}','{responseContent}')";
                logger.LogInformation($"HandleDeviceEventReport sql:{command.CommandText}");

                command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                logger.LogError($"HandleDeviceEventReport error:{ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public void HandleDeviceStatusReport(DeviceStatusReportRequest request)
        {
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "insert into device_status_trace(device_id,product_id,old_status,new_status)"
                    + $" values('{request.DeviceId}','{request.ProductId}','{request.OldStatus}','{request.NewStatus}')";
                logger.LogInformation($"HandleDeviceStatusReport sql:{command.CommandText}");

                command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                logger.LogError($"HandleDeviceStatusReport error:{ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public void HandleDeviceService(DeviceServiceInvokeRequest request, DeviceServiceInvokeResponse response)
        {
            var requestContent = JsonSerializer.Serialize(request);
            var responseContent = JsonSerializer.Serialize(response);

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "insert into device_service_trace(device_id,product_id,service_id,event_id,event_name,target_product_id,target_device_id,request_json,response_json) " +
                    $"values('{request.DeviceId}','{request.ProductId}','{request.ServiceId}','{request.EventId}','{request.EventName}','{request.TargetProductId}','{request.TargetDeviceId}','{requestContent}','{responseContent}')";
                logger.LogInformation($"DeviceServiceInvokeRequest sql:{command.CommandText}");

                command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"DeviceServiceInvokeRequest error:{ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CollectResponseDto<List<DeviceStatusDto>>> GetDeviceStatusList(GetDeviceStatusList input)
        {
            var result = new List<DeviceStatusDto>();
            if (!CheckConnectState())
            {
                return Success(result);
            }

            try
            {

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GetDeviceServiceList error:{ex.Message}");
            }

            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT  device_id,product_id,old_status,new_status,createtime " +
                $"FROM device_status_trace where device_id='{input.DeviceCode}'" +
                $"order by createtime DESC LIMIT {input.Limit}";
            var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                var statusDto = new DeviceStatusDto
                {
                    DeviceCode = reader.GetString(0),
                    DeviceTypeCode = reader.GetString(1),
                    OldStatus = reader.GetString(2),
                    NewStatus = reader.GetString(3),
                    CreateTime = reader.GetDateTime(4).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                };
                result.Add(statusDto);
            }

            return Success(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CollectResponseDto<List<DeviceServiceDto>>> GetDeviceServiceList(GetDeviceServiceList input)
        {
            var result = new List<DeviceServiceDto>();
            if (!CheckConnectState())
            {
                return Success(result);
            }

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT  device_id,product_id,createtime,service_id,event_id,event_name,target_device_id,target_product_id,request_json,response_json " +
                    $"FROM device_service_trace " +
                    $"where device_id='{input.DeviceCode}' order by createtime DESC LIMIT {input.Limit}";
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var statusDto = new DeviceServiceDto
                    {
                        DeviceCode = reader.GetString(0),
                        DeviceTypeCode = reader.GetString(1),
                        CreateTime = reader.GetDateTime(2).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                        ServiceCode = reader.GetString(3),
                        EventCode = reader.GetString(4),
                        EventName = reader.GetString(5),
                        TargetDeviceCode = reader.GetString(6),
                        TargetProductCode = reader.GetString(7),
                        RequestJson = reader.GetString(8),
                        ResponseJson = reader.GetString(9),
                    };
                    result.Add(statusDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GetDeviceServiceList error:{ex.Message}");
            }

            return Success(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CollectResponseDto<List<DevicePropertyDto>>> GetDevicePropertyList(GetDevicePropertyList input)
        {
            var result = new List<DevicePropertyDto>();
            if (!CheckConnectState())
            {
                return Success(result);
            }

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT  device_id,product_id,createtime,properpty_json " +
                    $"FROM device_property_trace " +
                    $"where device_id='{input.DeviceCode}' order by createtime DESC LIMIT {input.Limit}";
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var statusDto = new DevicePropertyDto
                    {
                        DeviceCode = reader.GetString(0),
                        DeviceTypeCode = reader.GetString(1),
                        CreateTime = reader.GetDateTime(2).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                        PropertyJson = reader.GetString(3),
                    };
                    result.Add(statusDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GetDevicePropertyList error:{ex.Message}");
            }

            return Success(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CollectResponseDto<List<DeviceEventDto>>> GetDeviceEventList(GetDeviceEventList input)
        {
            var result = new List<DeviceEventDto>();
            if (!CheckConnectState())
            {
                return Success(result);
            }

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT  device_id,product_id,createtime,event_id,event_name,request_json,response_json " +
                    $"FROM device_event_trace " +
                    $"where device_id='{input.DeviceCode}' order by createtime DESC LIMIT {input.Limit}";
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var statusDto = new DeviceEventDto
                    {
                        DeviceCode = reader.GetString(0),
                        DeviceTypeCode = reader.GetString(1),
                        CreateTime = reader.GetDateTime(2).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                        EventCode = reader.GetString(3),
                        EventName = reader.GetString(4),
                        RequestJson = reader.GetString(5),
                        ResponseJson = reader.GetString(6)
                    };
                    result.Add(statusDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GetDeviceEventList error:{ex.Message}");
            }

            return Success(result);
        }

        /// <summary>
        /// 获取设备调度日志
        /// </summary>
        /// <returns></returns>
        public async Task<List<List<string>>> GetDeviceServiceStatsList()
        {
            List<List<string>> result = new List<List<string>>();
            if (!CheckConnectState())
            {
                return result;
            }

            try
            {
                List<DeviceServiceDto> DeviceSData = new List<DeviceServiceDto>();
                //查最近一天前五十条的数据
                string time = string.Format("{0:yyyy-MM-dd HH.mm.ss}", DateTime.Now.AddDays(-1).Date);

                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT  device_id,createtime,target_device_id,request_json " +
                    $" FROM device_service_trace " +
                    $" WHERE product_id not in ('agv') AND target_product_id in ('agv')" +
                    $" AND event_id in ('REQUEST_AGV_LOAD_PANEL_ONLY','REQUEST_AGV_UNLOAD_PANEL_ONLY','REQUEST_AGV_LOAD_PANEL_THEN_UNLOAD_PANEL'," +
                    $"'REQUEST_AGV_UNLOAD_PANEL_THEN_LOAD_PANEL','REQUEST_AGV_LOAD_SILO_ONLY','REQUEST_AGV_UNLOAD_SILO_ONLY'," +
                    $"'REQUEST_AGV_LOAD_SILO_THEN_UNLOAD_SILO','REQUEST_AGV_UNLOAD_SILO_THEN_LOAD_SILO')" +
                    $" AND createtime > '{time}'" +
                    $" AND device_id IN (SELECT device_id from device_service_trace group by  device_id) " +
                    $" AND event_id IN (SELECT event_id from device_service_trace group by  event_id)" +
                    $" order by createtime DESC LIMIT 50";
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var statusDto = new DeviceServiceDto
                    {
                        DeviceCode = reader.GetString(0),
                        CreateTime = reader.GetDateTime(1).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                        TargetDeviceCode = reader.GetString(2),
                        RequestJson = reader.GetString(3),
                    };
                    DeviceSData.Add(statusDto);
                }

                if (DeviceSData.Count > 0)
                {
                    foreach (var item in DeviceSData)
                    {
                        if (string.IsNullOrEmpty(item.RequestJson))
                        {
                            continue;
                        }
                        string requestTime = string.Empty;
                        var objData = JsonObject.Parse(item.RequestJson);
                        if (objData != null)
                        {
                            requestTime = objData["Params"]["TaskCreateTime"].ToString();
                        }

                        if (string.IsNullOrEmpty(requestTime))
                        {
                            continue;
                        }
                        List<string> data = new List<string>()
                    {
                        item.DeviceCode,
                        requestTime,
                        item.TargetDeviceCode,
                        string.Format("{0:yyyy-MM-dd HH.mm.ss}", item.CreateTime)
                    };
                        result.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GetDeviceServiceStatsList error:{ex.Message}");
            }

            return result;
        }
    }
}