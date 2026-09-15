// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Common.CNC.ATP;

namespace VgEAPClient.Common.CNC;

public interface ICNCOperatorProvider
{
    ICNCConnector GetCNCConnector();

    IAtpFileParser GetAtpFileParser();
}
