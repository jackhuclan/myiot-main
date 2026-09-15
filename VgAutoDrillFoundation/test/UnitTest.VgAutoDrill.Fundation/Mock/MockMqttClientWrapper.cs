// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Mqtt.Client;

namespace UnitTest.VgAutoDrill.Fundation.Mock
{
    internal class MockMqttClientWrapper : Mock<IMqttClientWrapper>
    {
        private Mock<IMqttClientWrapper> _mock;

        public MockMqttClientWrapper()
        {
            _mock = new Mock<IMqttClientWrapper>();
        }

        public Mock<IMqttClientWrapper> Mock()
        {
            return _mock;
        }
    }
}
