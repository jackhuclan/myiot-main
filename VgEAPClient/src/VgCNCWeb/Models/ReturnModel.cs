// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgCNCServer.DAO.Models;

namespace VgCNCWeb.Models;

public class ReturnModel<T>
{
    public int PageNum { get; set; } = 0;
    public int PageSize { get; set; } = 0;
    public int Total { get; set; } = 0;

    public List<T> List { get; set; } = [];
}


public class ReturnSysdrill
{
    public int WorkingCount { get; set; } = 0;
    public int AlarmCount { get; set; } = 0;
    public int StopCount { get; set; } = 0;
    public int WaitingCount { get; set; } = 0;
    public int IdleCount { get; set; } = 0;
    public int OfflineCount { get; set; } = 0;

    public List<sysDrillInformation> drillList { get; set; } = [];
}
