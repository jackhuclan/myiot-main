// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Socket;

public class ReplyModelTypeAttribute : Attribute
{
    public ReplyModelTypeAttribute(Type replyType)
    {
        ReplyType = replyType;
    }

    public Type ReplyType { get; }
}
