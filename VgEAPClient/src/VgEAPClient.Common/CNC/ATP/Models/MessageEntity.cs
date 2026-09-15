// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.ATP.Models;

public class MessageEntity
{
    public object Data { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsSuccessful { get; set; }
    public string Information { get; set; }
}
