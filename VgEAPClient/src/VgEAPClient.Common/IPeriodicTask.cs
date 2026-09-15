// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;

/// <summary>
/// 周期性的任务
/// </summary>
public interface IPeriodicTask : IDisposable
{
    Task StartAsync(CancellationToken cancellationToken);
}
