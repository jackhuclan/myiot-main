// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Common.Models.Report
{
    public class PublicKeyPayLoad : BasePayload<BaseBody>
    {
        public new ReportBaseBodyEntity body { get; set; }
    }
}
