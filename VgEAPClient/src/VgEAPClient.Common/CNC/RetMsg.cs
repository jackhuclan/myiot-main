// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC;

public class RetMsg
{
    public bool result { get; set; } = false;
    public string msg { get; set; } = "";
    public int other { get; set; } = 0;
}
