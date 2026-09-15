// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VgAutoDrill.Fundation.Iot;

namespace UnitTest.VgAutoDrill.Fundation.Mock
{
    internal class MockDeviceProvider : Mock<IDeviceProvider>
    {
        private Mock<IDeviceProvider> _mock;

        public MockDeviceProvider()
        {
            _mock = new Mock<IDeviceProvider>();
        }

        public Mock<IDeviceProvider> Mock()
        {
            return _mock;
        }
    }
}
