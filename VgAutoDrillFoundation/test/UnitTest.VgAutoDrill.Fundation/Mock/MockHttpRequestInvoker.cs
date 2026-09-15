// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.OpenAPI;

namespace UnitTest.VgAutoDrill.Fundation.Mock
{
    public class MockHttpRequestInvoker : Mock<IHttpRequestInvoker>
    {
        private Mock<IHttpRequestInvoker> _mock;

        public MockHttpRequestInvoker()
        {
            _mock = new Mock<IHttpRequestInvoker>();
        }

        public Mock<IHttpRequestInvoker> Mock(string code = ErrorCodes.Sys.SUCCESS)
        {
            MockPostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(new DeviceEventReportResponse
            {
                Code = code
            });
            MockPostAsJsonAsync<DevicePropertiesReportRequest, DevicePropertiesReportResponse>(new DevicePropertiesReportResponse
            {
                Code = code
            });
            MockPostAsJsonAsync<DeviceServiceInvokeRequest, DeviceServiceInvokeResponse>(new DeviceServiceInvokeResponse
            {
                Code = code
            });
            MockPostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(new DeviceStatusReportResponse
            {
                Code = code
            });
            MockPostAsJsonAsync<DeviceAlarmReportRequest, DeviceAlarmReportResponse>(new DeviceAlarmReportResponse
            {
                Code = code
            });

            return _mock;
        }

        public Mock<IHttpRequestInvoker> MockPostAsJsonAsync<TValue, TResult>(TResult result)
        {
            _mock.Setup(x => x.PostAsJsonAsync<TValue, TResult>(It.IsAny<string>(),
                It.IsAny<TValue>(), It.IsAny<string>())).Returns(Task.FromResult(result));
            return _mock;
        }
    }
}
