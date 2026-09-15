// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Common.CNC.ATP.Models;

namespace VgEAPClient.Common.CNC.ATP;

public interface IMesAtpParser
{
    MessageEntity ParseFromMesAtp(string strMesAtpPath, out AtpDTO atpDTO);

    MessageEntity ClearToolBoxInfo(AtpDTO atpDTO, string ClearToolBoxIndex);

    MessageEntity GenerateAtpFile(string strMesAtpPath, string strDiaPath, AtpDTO orgAtpDTO, string strGenerateFileName, string strGeneratePath = "");
}
