// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common;

public enum CNCExecutePacketKind
{
    PROGRAM,
    CLRNEXT,
    INFLGSET,
    INFLGCLR,
    COMMAND,
    CNCCOMMAND,
    CNCKEY,
    PCCOMMAND,
    PCKEY,
    CHANGEGROUP,
    CHANGECLIENT,
    CHANGEPAGE,
    STARTEXE,
    RUNTIMEVALUE,
    RUNTIMESTRING,
    RUNTIMECOMM,
    LOAD,
    SAVE,
    SETHANDLE
}
