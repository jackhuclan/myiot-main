// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Socket;

internal class TransactionIdMaker : ITransactionIdMaker
{
    public string NextId()
    {
        return DateTime.Now.ToString("yyyyMMddHHmmssfff");
    }
}
