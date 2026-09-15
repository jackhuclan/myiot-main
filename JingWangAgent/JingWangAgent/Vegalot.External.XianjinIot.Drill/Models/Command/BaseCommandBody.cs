// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill.Models.Command
{
    internal class BaseCommandBody : BaseBody
    {
        /// <summary>
        /// 任务编码
        /// </summary>
        public string taskCode { get; set; } = string.Empty;
    }
}
