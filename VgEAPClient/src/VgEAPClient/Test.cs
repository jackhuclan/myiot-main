// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http;

namespace VgEAPClient;

internal class Test
{
    public async Task Method1(HttpContext context)
    {
        await context.Response.WriteAsync("Hello World!");
    }
}
