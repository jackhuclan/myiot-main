// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Socket.Models;

public enum EqpStatus
{
    Unknown = 0,
    Run = 1,//:Run 运行
    Pause = 2,//:Pause 暂停
    Idle = 3,//:Idle 待机
    Down = 4,//:Down 故障
    PM = 5,//:PM 保养
    Ready = 6,//:Ready准备
}
