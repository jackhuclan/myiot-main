// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;

/// <summary>
///mode = IDLE 不在自动模式     Not in automatic mode
///mode = WAIT 等待下一个程序   Wait for next program
///mode = WORK 正在处理         Processing in progress
///mode = STOP 程序停止         Program stopped
///mode = ALAM 机器出错         Machine error
///mode = SERV 服务(维修)       Service
/// </summary>
public enum CNCWorkMode
{
    IDLE = 0, WAIT = 1, WORK = 3, STOP, ALAM = 2, SERV = 4
}
