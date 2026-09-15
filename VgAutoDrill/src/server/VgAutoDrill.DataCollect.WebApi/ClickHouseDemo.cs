using ClickHouse.Client.ADO;
using ClickHouse.Client.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VgAutoDrill.DataCollect.WebApi.Controllers;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.DataCollect.WebApi
{
    //[ApiController]
    //[Route("v1/datacollect/[controller]")]
    public class ClickHouseDemo
    {
        private readonly ClickhouseOptions clickhouseOptions;
        private readonly ClickHouseConnection connection;
        private readonly ILogger<ClickHouseDemo> _logger;

        public ClickHouseDemo(IOptions<ClickhouseOptions> options
            , ILoggerFactory loggerFactory)
        {
            this.clickhouseOptions = options.Value;
            var builder = new ClickHouseConnectionStringBuilder(clickhouseOptions.ConnectionString);
            builder.Compression = clickhouseOptions.Compression;
            builder.UseSession = clickhouseOptions.Session;
            builder.UseCustomDecimals = clickhouseOptions.CustomDecimals;
            _logger = loggerFactory.CreateLogger<ClickHouseDemo>();
            this.connection = new ClickHouseConnection(builder.ConnectionString);
        }

        [HttpPost("create/database", Name = "CreateDatabase")]
        public virtual async Task<JsonResult> CreateDatabase(string database)
        {
            try
            {
                var sql = $"CREATE DATABASE IF NOT EXISTS {database};";
                await this.connection.ExecuteStatementAsync(sql);

                return new JsonResult(new
                {
                    Code = 0,
                    Message = "succeed"
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    Code = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("create/table", Name = "CreateTable")]
        public virtual async Task<JsonResult> CreateTable(string database, string table)
        {
            try
            {
                string sql = string.Empty;
                switch (table)
                {
                    case "drill":
                        sql = $"CREATE TABLE IF NOT EXISTS {database}.{table} (id UInt32, name String) ENGINE MergeTree() ORDER BY id";
                        break;
                    case "agv":
                        sql = $"CREATE TABLE IF NOT EXISTS {database}.{table} (id UInt32, name String) ENGINE MergeTree() ORDER BY id";
                        break;
                    case "pin":
                        sql = $"CREATE TABLE IF NOT EXISTS {database}.{table} (id UInt32, name String) ENGINE MergeTree() ORDER BY id";
                        break;
                    default:
                        break;
                }

                await this.connection.ExecuteStatementAsync(sql);

                return new JsonResult(new
                {
                    Code = 0,
                    Message = "succeed"
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    Code = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("write", Name = "Write")]
        public virtual JsonResult Write(DevicePropertiesReportRequest request)
        {
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"insert into aa.drill(*) values({new Random().Next(100000)}, '{request.DeviceId}')";
                command.ExecuteScalar();

                return new JsonResult(new DeviceServiceInvokeResponse() { });

            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    Code = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("read", Name = "Read")]
        public virtual async Task<JsonResult> Read()
        {
            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "select * from aa.drill";
                var reader = await command.ExecuteReaderAsync();

                var list = new List<DeviceServiceInvokeResponse>();
                while (reader.Read())
                {
                    var id = reader.GetValue(0);
                    var name = reader.GetString(1);

                    var response = new DeviceServiceInvokeResponse();
                    response.Params.Add("id", id);
                    response.Params.Add("name", name);

                    list.Add(response);
                }

                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    Code = 500,
                    Message = ex.Message
                });
            }
        }
    }
}
