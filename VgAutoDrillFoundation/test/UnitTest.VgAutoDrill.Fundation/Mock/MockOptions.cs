// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using VgAutoDrill.Fundation.Iot;

namespace UnitTest.VgAutoDrill.Fundation.Mock
{
    internal class MockOptions<TOptions> : Mock<IOptions<TOptions>>
        where TOptions : class, new()
    {
        private Mock<IOptions<TOptions>> _mock;

        public MockOptions()
        {
            _mock = new Mock<IOptions<TOptions>>();
        }

        public Mock<IOptions<TOptions>> Mock(TOptions options)
        {
            _mock.SetupGet(x => x.Value).Returns(options);
            return _mock;
        }
    }
}
