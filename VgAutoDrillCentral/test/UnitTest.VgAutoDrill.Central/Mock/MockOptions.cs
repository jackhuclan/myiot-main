// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Options;
using Moq;

namespace UnitTest.VgAutoDrill.Central.Mock;

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
