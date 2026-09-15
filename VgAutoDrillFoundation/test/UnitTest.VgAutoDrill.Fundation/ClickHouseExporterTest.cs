// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UnitTest.VgAutoDrill.Fundation.Mock;
using VgAutoDrill.Fundation.Alarm;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Channel.Http;
using VgAutoDrill.Fundation.Channel.Http;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.State;
using VgAutoDrill.OpenAPI;
using static VgAutoDrill.Fundation.Iot.Events;

namespace UnitTest.VgAutoDrill.Fundation
{
    public class ClickHouseExporterTest
    {
        [Fact]
        public void ClickHouseExporterCouldBeConstructed()
        {
            IMessageChannel exporter = BuildExporter();
            Assert.NotNull(exporter);
        }

        [Fact]
        public async void DeviceAlarmReportShouldWork()
        {
            IMessageChannel exporter = BuildExporter();

            for (int i = 0; i < 200; i++)
            {
                var response = await exporter.DeviceAlarmReport(new DeviceAlarmReportRequest
                {
                    ProductId = "drill",
                    DeviceId = "D5-2849-242",
                    EventId = Events.Drill.BUFFER_AXIS_LOAD_RAW_MATERIAL_END_EVENT,
                    TraceId = "54dc237a-f83b-4f9f-aeb8-699db6fbee37",
                    AlarmLevel = (AlarmLevel)new Random().Next(4),
                    Handled = new Random().Next(3),
                    Params = new Dictionary<string, object> { },
                    AlarmTime = DateTime.Now,
                    AlarmCode = "Test",
                    AlarmName = "Test",
                });
                Thread.Sleep(100);

                Assert.True(response.Code == ErrorCodes.Sys.SUCCESS);
            }

            Assert.NotNull(exporter);
        }

        private static IMessageChannel BuildExporter()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMessageChannel, HttpMessageChannel>();
            services.AddSingleton<IHttpRequestInvoker>(new MockHttpRequestInvoker().Mock(ErrorCodes.Sys.SUCCESS).Object);
            services.AddSingleton<IOptions<ClickHouseOptions>>(new MockOptions<ClickHouseOptions>().Mock(new ClickHouseOptions
            {
                ConnectionString = "Compress=False;BufferSize=32768;SocketTimeout=10000;CheckCompressedHash=False;Compressor=lz4;Host=192.168.104.253;Port=8123;Database=vg_autodrill_db_ck;User=default;Password=",
                Compression = true,
                Session = false,
                CustomDecimals = true,
                AlarmReportEnabled = true,
                Enabled = true,
            }).Object);

            services.AddSingleton<IOptions<CentralWebOptions>>(new MockOptions<CentralWebOptions>().Mock(new CentralWebOptions()).Object);
            services.AddSingleton<ILoggerFactory>(new LoggerFactory());

            var serviceProvider = services.BuildServiceProvider();
            var exporter1 = serviceProvider.GetRequiredService<IOptions<ClickHouseOptions>>();
            var exporter = serviceProvider.GetRequiredService<IMessageChannel>();
            return exporter;
        }
    }
}
