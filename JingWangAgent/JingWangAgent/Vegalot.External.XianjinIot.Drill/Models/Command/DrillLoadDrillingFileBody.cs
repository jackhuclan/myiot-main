// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Drill.Models.Command
{
    internal class DrillLoadDrillingFileBody : BaseCommandBody
    {
        /// <summary>
        /// 料号编码，如果filePath为空则用该字段使用原来的调资料方式调取
        /// </summary>
        public string incodeNumber { get; set; } = string.Empty;

        /// <summary>
        /// 资料路径
        /// </summary>
        public string filePath { get; set; } = string.Empty;
    }
}
