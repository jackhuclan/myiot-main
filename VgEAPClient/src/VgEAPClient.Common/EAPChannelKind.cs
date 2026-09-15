// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;

/// <summary>
/// EAP 通道类型
/// </summary>
public enum EAPChannelKind
{
    Http = 0,
    Socket = 1,
    DataBase = 2,
    MQTT = 3,
    Custom = 4
}
