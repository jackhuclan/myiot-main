// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Common;

public class FormatPanelStatus
{
    /// <summary>
    /// 格式化板料状态
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public static string GetPanelOrSiloStatus(ProductStatus status)
    {
        string res = "";
        if (ProductStatusConstants.EmptySiloBox.Contains(status))
        {
            res = "空料仓";
        }
        else if (ProductStatusConstants.EmptyPayload.Contains(status))
        {
            res = "无料仓";
        }
        else if (ProductStatusConstants.Finished_PIN.Contains(status))
        {
            res = "生料";
        }
        else if (ProductStatusConstants.Finished_DRILL.Contains(status))
        {
            res = "熟料";
        }
        else
        {
            res = "异常";
        }
        return res;
    }
}
