// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common
{
    public interface IEAPConnector
    {
        bool IsConnected { get; }

        Task<bool> Connect(CancellationToken cancellationToken);
        Task<bool> Disonnect(CancellationToken cancellationToken);
    }
}