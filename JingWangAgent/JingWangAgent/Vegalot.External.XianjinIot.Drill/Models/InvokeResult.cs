// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill.Models;
internal class InvokeResult
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public static InvokeResult Ok()
    {
        return new InvokeResult()
        {
            Success = true
        };
    }
    public static InvokeResult Ok(string message)
    {
        return new InvokeResult()
        {
            Success = true,
            Message = message
        };
    }
    public static InvokeResult Fail(string message)
    {
        return new InvokeResult()
        {
            Message = message
        };
    }
    public static InvokeResult Fail()
    {
        return new InvokeResult();
    }
}
