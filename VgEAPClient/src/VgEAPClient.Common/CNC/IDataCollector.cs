// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC;

public interface IDataCollector<T> : IDisposable
{
    T Data { get; }
    Task StartAsync(CancellationToken cancellationToken);
}
