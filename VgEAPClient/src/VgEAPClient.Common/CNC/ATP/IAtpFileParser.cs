// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Common.CNC.ATP.Models;

namespace VgEAPClient.Common.CNC.ATP;

public interface IAtpFileParser
{
    AtpDTO ParseFromFile(string FilePath);

    MessageEntity ParseToFile(int magazienRange, string orginAtpPath, string orginInfo);

    MessageEntity ParseToFile(string orginInfo, string orginAtpPath, string orginDiaPath);

    MessageEntity GenerateAtpFile(string strAtpFilePath, AtpDTO atpDTO);
}
