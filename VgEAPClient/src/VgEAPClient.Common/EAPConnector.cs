// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;

public class EAPConnector : IEAPConnector
{
    private volatile bool _isConnected = false;

    public bool IsConnected
    {
        get => _isConnected;
        private set => _isConnected = value;
    }

    public Task<bool> Connect(CancellationToken cancellationToken)
    {
        _isConnected = true;
        return Task.FromResult(true);
    }

    public Task<bool> Disonnect(CancellationToken cancellationToken)
    {
        _isConnected = false;
        return Task.FromResult(true);
    }
}
