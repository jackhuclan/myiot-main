// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication;

public class EQPReportModel<THeader, TBody, TResult>
    where THeader : new()
    where TBody : EQPReportBody, new()
    where TResult : new()
{
    public EQPReportModel()
    {
        Header = new THeader();
        Body = new TBody();
        Result = new TResult();
    }

    public EQPReportModel(THeader header, TBody body, TResult result)
    {
        Header = header;
        Body = body;
        Result = result;
    }

    public THeader Header { get; set; }
    public TBody Body { get; set; }
    public TResult Result { get; set; }
}
