// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http;

namespace VgEAPClient.Common.Communication.Inbound;
public interface IVegaReceiver
{
    Task Invoke(HttpContext context);
}
