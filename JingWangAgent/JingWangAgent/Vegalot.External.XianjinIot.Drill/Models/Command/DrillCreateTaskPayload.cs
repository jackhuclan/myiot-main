// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Vegalot.External.XianjinIot.Drill.Models.Command;

namespace Vegalot.External.XianjinIot.Drill.Models
{
    /// <summary>
    /// 用于MES下发钻孔任务给钻机
    /// </summary>
    internal class DrillCreateTaskPayload : BasePayload<DrillCreateTaskBody>
    {
    }
}
