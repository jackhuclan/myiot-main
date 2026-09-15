// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;
using VgEAPClient.Common.CNC.ATP.Models;

namespace VgEAPClient.Common.CNC.ATP;

public class Cnc95AtpFileParser : IAtpFileParser
{
    public AtpDTO ParseFromFile(string FilePath)
    {
        if (!File.Exists(FilePath))//文件不存在就返回空值
        {
            return new AtpDTO();
        }
        int drlStageFlag = -1;
        List<string> lines = new List<string>();
        AtpDTO atpDTO = new AtpDTO();
        atpDTO.toolDTOs = Enumerable.Range(0, 1001).Select(i => new ToolDTO() { toolId = i }).ToList();
        atpDTO.magazineDTOs = new List<MagazineDTO>() { new MagazineDTO() };
        atpDTO.toolToleranceTableDTOs = Enumerable.Range(1, 52).Select(i => new ToolToleranceTableDTO() { tableId = i }).ToList();
        atpDTO.peckDrillingValuesDTOs = Enumerable.Range(1, 501).Select(i => new PeckDrillingValuesDTO() { toolNumber = i }).ToList();
        atpDTO.shortSlotNibblingDTOs = Enumerable.Range(1, 36).Select(i => new ShortSlotNibblingDTO() { toolId = i }).ToList();
        atpDTO.longSlotNibblingDTOs = Enumerable.Range(1, 36).Select(i => new LongSlotNibblingDTO() { toolId = i }).ToList();
        //按行获取文件内容
        using (StreamReader reader = new StreamReader(FilePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Equals("%%9001") || line.Equals("%%9002"))//程序头部标志
                {
                    drlStageFlag = 0;
                }
                if (drlStageFlag >= 0)//程序头部标志之后的代码都进行解析
                {
                    lines.Add(line);
                }
            }
        }

        //对刀具内容进行解析
        for (int i = 0; i < lines.Count; i++)
        {
            //刀具信息行
            if (lines[i][0] == 'T')//刀具指令解析，添加刀具及基本信息
            {
                int k = 4;
                while (lines[i + k][0] == 'M')
                {
                    k++;
                }
                int num = int.Parse(lines[i].Split('D')[0].Split('T')[1]);
                if (num > 999)
                {
                    continue;
                }
                List<string> tempLine = lines.GetRange(i, k);
                atpDTO.toolDTOs[num] = FileToDTO_9.StrToToolDTO(tempLine);
                i += k - 1;
            }
            else if (lines[i][0] == 'L')//刀盘指令解析，刀盘位置
            {
                string[] LValue = lines[i].Split(',');
                int rowIndex = StringUtil.GetIntCode(LValue[0].Split('L')[1]);
                int originNum = atpDTO.magazineDTOs.Count - 1;
                int rowNum = StringUtil.GetIntCode(LValue[1]);
                List<MagazineDTO> magazineDTOs = new List<MagazineDTO>(rowNum);
                for (int j = 0; j < rowNum; j++)
                {
                    magazineDTOs.Add(new MagazineDTO());
                }

                atpDTO.magazineDTOs.AddRange(magazineDTOs);
                for (int j = 2; j < LValue.Length; j++)
                {
                    int toolId = Math.Abs(StringUtil.GetIntCode(LValue[j]));
                    atpDTO.magazineDTOs[originNum + j - 1].magazineId = originNum + j - 1;
                    atpDTO.magazineDTOs[originNum + j - 1].toolId = toolId;
                    if (!string.IsNullOrEmpty(LValue[j]))
                    {
                        atpDTO.magazineDTOs[originNum + j - 1].toolDiameter = atpDTO.toolDTOs[toolId].toolDiameter;
                    }
                }
            }
            else if (lines[i][0] == 'W')
            {
                string[] WValue = lines[i].Split(',');
                int rowIndex = StringUtil.GetIntCode(WValue[0].Split('W')[1]);
                int rowNum = StringUtil.GetIntCode(WValue[1]);
                for (int j = 2; j < rowNum + 2; j++)
                {
                    int magazineId = (rowIndex - 1) * rowNum + j - 1;
                    if (string.IsNullOrEmpty(WValue[j]))
                    {
                        WValue[j] = "";
                    }
                    atpDTO.magazineDTOs[magazineId].toolState = WValue[j];
                    atpDTO.magazineDTOs[magazineId].toolLife = atpDTO.toolDTOs[atpDTO.magazineDTOs[magazineId].toolId].toolLife;
                    if (WValue[j] == "E" || WValue[j] == "GE")
                    {
                        atpDTO.magazineDTOs[magazineId].toolUseLife = atpDTO.toolDTOs[atpDTO.magazineDTOs[magazineId].toolId].toolLife;
                    }
                    else if (WValue[j] == "U" || WValue[j] == "GU")
                    {
                        atpDTO.magazineDTOs[magazineId].toolUseLife = atpDTO.toolDTOs[atpDTO.magazineDTOs[magazineId].toolId].drillToolLife;
                    }
                    else if (WValue[j] == "N")
                    {
                        atpDTO.magazineDTOs[magazineId].toolUseLife = 0;
                    }
                }
            }
            else if (lines[i][0] == 'Q')
            {
                /*string[] QValue = lines[i].Split(',');
                int rowIndex = StringUtil.GetIntCode(QValue[0].Split('Q')[1]);
                atpDTO.toolToleranceTableDTOs[rowIndex].diameter = StringUtil.GetFloatCode(QValue[1]);
                atpDTO.toolToleranceTableDTOs[rowIndex].nDiaTolerance = StringUtil.GetFloatCode(QValue[2]);
                atpDTO.toolToleranceTableDTOs[rowIndex].pDiaTolerance = StringUtil.GetFloatCode(QValue[3]);
                atpDTO.toolToleranceTableDTOs[rowIndex].nLenTolerance = StringUtil.GetFloatCode(QValue[4]);
                atpDTO.toolToleranceTableDTOs[rowIndex].pLenTolerance = StringUtil.GetFloatCode(QValue[5]);
                atpDTO.toolToleranceTableDTOs[rowIndex].warnOfDeviation = StringUtil.GetFloatCode(QValue[6]);
                atpDTO.toolToleranceTableDTOs[rowIndex].stopOfDeviation = StringUtil.GetFloatCode(QValue[7]);
                atpDTO.toolToleranceTableDTOs[rowIndex].kZ1 = StringUtil.GetFloatCode(QValue[8].Split('K')[1]);
                atpDTO.toolToleranceTableDTOs[rowIndex].kZ2 = StringUtil.GetFloatCode(QValue[9]);
                if (QValue.Length > 10)
                {
                    atpDTO.toolToleranceTableDTOs[rowIndex].kZ3 = StringUtil.GetFloatCode(QValue[10]);
                    atpDTO.toolToleranceTableDTOs[rowIndex].kZ4 = StringUtil.GetFloatCode(QValue[11]);
                    atpDTO.toolToleranceTableDTOs[rowIndex].kZ5 = StringUtil.GetFloatCode(QValue[12]);
                    atpDTO.toolToleranceTableDTOs[rowIndex].kZ6 = StringUtil.GetFloatCode(QValue[13]);
                }*/
            }
            else if (lines[i][0] == 'P')
            {
                /*List<char> delimiters = new List<char> { 'P', 'S', 'I', 'J', 'F', 'R' };
                var result = StringUtil.SplitString(lines[i].Replace(",", ""), delimiters.ToArray());
                List<string> segments = result.segments;
                List<char> missingChars = result.missingChars;
                int rowIndex = 0;
                int j = 1;
                for (int k = 0; k < delimiters.Count; k++)
                {
                    if (!missingChars.Contains(delimiters[k]))
                    {
                        switch (delimiters[k])
                        {
                            case 'P':
                                rowIndex = StringUtil.GetIntCode(segments[j++]);
                                break;

                            case 'S':
                                atpDTO.peckDrillingValuesDTOs[rowIndex].partialStrokeNumber = StringUtil.GetIntCode(segments[j++]);
                                break;

                            case 'I':
                                atpDTO.peckDrillingValuesDTOs[rowIndex].lowerPlane = StringUtil.GetFloatCode(segments[j++]);
                                break;

                            case 'J':
                                atpDTO.peckDrillingValuesDTOs[rowIndex].retractionPlane = StringUtil.GetFloatCode(segments[j++]);
                                break;

                            case 'F':
                                atpDTO.peckDrillingValuesDTOs[rowIndex].infeedRate = StringUtil.GetFloatCode(segments[j++]);
                                break;

                            case 'R':
                                atpDTO.peckDrillingValuesDTOs[rowIndex].retractRate = StringUtil.GetFloatCode(segments[j++]);
                                break;
                        }
                    }
                }*/
            }
            else if (lines[i][0] == 'N')
            {
                /* string[] NValue = lines[i].Split(',');
                 int rowIndex = StringUtil.GetIntCode(NValue[0].Split('N')[1]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].use = StringUtil.GetIntCode(NValue[1]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].minD = StringUtil.GetFloatCode(NValue[2]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].maxD = StringUtil.GetFloatCode(NValue[3]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].TMET_ShiftOrthog = StringUtil.GetFloatCode(NValue[4]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].type = StringUtil.GetIntCode(NValue[5]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].TMET_Roughn = StringUtil.GetFloatCode(NValue[6]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].InfeedFirst = StringUtil.GetFloatCode(NValue[7]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].InfeedFast = StringUtil.GetFloatCode(NValue[8]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].InfeedSlow = StringUtil.GetFloatCode(NValue[9]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].TMET_ShiftLong = StringUtil.GetFloatCode(NValue[10]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].TIN_Roughn = StringUtil.GetFloatCode(NValue[11]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].TIN_ShiftLong = StringUtil.GetFloatCode(NValue[12]);
                 atpDTO.shortSlotNibblingDTOs[rowIndex].TIN_ShiftOrthog = StringUtil.GetFloatCode(NValue[13]);*/
            }
            else if (lines[i][0] == 'A')
            {
                /* string[] AValue = lines[i].Split(',');
                 int rowIndex = StringUtil.GetIntCode(AValue[0].Split('A')[1]);
                 atpDTO.longSlotNibblingDTOs[rowIndex].use = StringUtil.GetIntCode(AValue[1]);
                 atpDTO.longSlotNibblingDTOs[rowIndex].minD = StringUtil.GetFloatCode(AValue[2]);
                 atpDTO.longSlotNibblingDTOs[rowIndex].maxD = StringUtil.GetFloatCode(AValue[3]);
                 atpDTO.longSlotNibblingDTOs[rowIndex].type = StringUtil.GetIntCode(AValue[4]);
                 atpDTO.longSlotNibblingDTOs[rowIndex].TMET_Roughn = StringUtil.GetFloatCode(AValue[5]);
                 atpDTO.longSlotNibblingDTOs[rowIndex].InfeedFirst = StringUtil.GetFloatCode(AValue[6]);
                 atpDTO.longSlotNibblingDTOs[rowIndex].InfeedFast = StringUtil.GetFloatCode(AValue[7]);
                 atpDTO.longSlotNibblingDTOs[rowIndex].InfeedSlow = StringUtil.GetFloatCode(AValue[8]);
                 atpDTO.longSlotNibblingDTOs[rowIndex].TIN_Roughn = StringUtil.GetFloatCode(AValue[9]);*/
            }
            else if (lines[i][0] == 'B')
            {
            }
        }
        return atpDTO;
    }

    public MessageEntity ParseToFile(int magazienRange, string orginAtpPath, string orginInfo)
    {
        string resultPath = Path.GetDirectoryName(orginAtpPath) + "/temp.ATP";
        AtpDTO atpDTO = new AtpDTO();
        var data = new Data();
        try
        {
            atpDTO = ParseFromFile(orginAtpPath);
            data = JsonConvert.DeserializeObject<RootObject>(orginInfo).Data;
        }
        catch (Exception ex)
        {
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = "原始ATP文件或JSON解析错误" + ex.Message,
                Timestamp = DateTime.Now,
                Data = "orginAtpPath"
            };
        }
        foreach (var arrange in data.Arranges)
        {
            if (arrange.MoCount != -1)
            {
                continue;
            }
            int position = (arrange.BoxIndex - 1) * 50 + arrange.Site;
            //JSON刀具总寿命信息
            LifeDefine lifeDefine = data.lifeDefines.FirstOrDefault(e => e.PgmDiameter == arrange.PgmDiameter);
            //获取相应的刀具信息
            ToolDTO toolDTO = atpDTO.toolDTOs.FirstOrDefault(tool => (float)tool.toolDiameter == (float)arrange.PgmDiameter);

            toolDTO.magazines.Add(position);
            for (int i = 1; i <= magazienRange; i++)
            {
                if (atpDTO.magazineDTOs[i].toolId == toolDTO.toolId && (atpDTO.magazineDTOs[i].toolState == "E" || atpDTO.magazineDTOs[i].toolState == "GE"))
                {
                    atpDTO.magazineDTOs[i].toolState = atpDTO.magazineDTOs[position].toolState;
                    atpDTO.magazineDTOs[i].toolUseLife = atpDTO.magazineDTOs[position].toolUseLife;
                    atpDTO.magazineDTOs[position].toolState = "E";
                    atpDTO.magazineDTOs[position].toolUseLife = 0;
                    break;
                }
            }
        }
        try
        {
            using (StreamWriter writer = new StreamWriter(resultPath))
            {
                writer.WriteLine("%%9001");
                writer.WriteLine("$");
                for (int i = 1; i < atpDTO.toolDTOs.Count; i++)
                {
                    ToolDTO toolDTO = atpDTO.toolDTOs[i];
                    string info =
                          "T" + toolDTO.toolId
                        + IsParamEmpty("D", toolDTO.toolDiameter)
                        + IsParamEmpty("E", toolDTO.toolType)
                        + IsParamEmpty("S", toolDTO.spindleSpeed)
                        + IsParamEmpty("F", toolDTO.infeedZAxis)
                        + IsParamEmpty("R", toolDTO.retractZAxis)
                        + IsParamEmpty("A", toolDTO.waitTime)
                        + IsParamEmpty("J", toolDTO.J)
                        + IsParamEmpty("Z", toolDTO.workPlaneAdjustment)
                        + IsParamEmpty("N", toolDTO.toolLife)
                        + IsParamEmpty("B", toolDTO.drillToolLife)
                        //+ IsParamEmpty("C", toolDTO.routerToolLife)
                        + IsParamEmpty("U", toolDTO.compensationDiameter)
                        + IsParamEmpty("V", toolDTO.routingFeedRate)
                        + IsParamEmpty("W", toolDTO.routerWear)
                        + IsParamEmpty("X", toolDTO.circularRoutingFeedRate)
                        + IsParamEmpty("I", toolDTO.I)
                        + IsParamEmpty("P", toolDTO.toolLifeMonitoring)
                        + FillFunParam("G", toolDTO.toolFunctions);
                    writer.WriteLine(info);
                    writer.WriteLine("T" + toolDTO.toolId + "Q,");
                    writer.WriteLine("T" + toolDTO.toolId + "Nm" + toolDTO.toolLife.ToString("0.000") + "Np" + toolDTO.toolLife.ToString("0.000"));
                    writer.WriteLine("T" + toolDTO.toolId + "M" + toolDTO.drillMethod[0] + ",,,,,");
                    string magazine = string.Empty;
                    List<MagazineDTO> magazineList = atpDTO.magazineDTOs.Where(e => e.toolId == toolDTO.toolId).ToList();
                    foreach (var val in magazineList)
                    {
                        magazine += "M" + val.magazineId;
                    }
                    if (string.IsNullOrEmpty(magazine))
                    {
                        writer.WriteLine("M");
                    }
                    else
                    {
                        writer.WriteLine(magazine);
                    }
                }
                //L部分
                for (var i = 0; i < 200; i++)
                {
                    if ((i + 1) * 50 > atpDTO.magazineDTOs.Count)
                    {
                        break;
                    }
                    string lValue = "L" + (i + 1) + ",50";
                    for (var j = 0; j < 50; j++)
                    {
                        int element = atpDTO.magazineDTOs[i * 50 + j + 1].toolId;
                        if (element == 0)
                        {
                            lValue += "," + null;
                        }
                        else if (atpDTO.magazineDTOs[i * 50 + j + 1].toolState == "E" || atpDTO.magazineDTOs[i * 50 + j + 1].toolState == "GE")
                        {
                            lValue += "," + -element;
                        }
                        else
                        {
                            lValue += "," + element;
                        }
                    }
                    writer.WriteLine(lValue);
                }
                //W部分
                for (var i = 0; i < 200; i++)
                {
                    if ((i + 1) * 50 > atpDTO.magazineDTOs.Count)
                    {
                        break;
                    }
                    string WValue = "W" + (i + 1) + ",50";
                    for (var j = 0; j < 50; j++)
                    {
                        int element = atpDTO.magazineDTOs[i * 50 + j + 1].toolId;
                        WValue += "," + atpDTO.magazineDTOs[i * 50 + j + 1].toolState;
                    }
                    writer.WriteLine(WValue);
                }
            }
        }
        catch (Exception ex)
        {
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = "生成ATP文件过程中错误" + ex.Message,
                Timestamp = DateTime.Now,
                Data = data.WipId
            };
        }

        return new MessageEntity
        {
            IsSuccessful = true,
            Information = "生成ATP文件成功",
            Timestamp = DateTime.Now,
            Data = data.WipId
        };
    }

    public MessageEntity ParseToFile(string orginInfo, string orginAtpPath, string orginDiaPath)
    {
        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vegaAtp");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string resultPath = null;
        Data data = null;
        List<ToolDTO> toolDTOs = new List<ToolDTO>();
        List<ToolDTO> diaToolList = new List<ToolDTO>();
        List<MagazineDTO> magazineDTOs = new List<MagazineDTO>();
        /*OrderedDictionary dict = new OrderedDictionary();
        OrderedDictionary lifeDict = new OrderedDictionary();
        int[,] LArray = new int[200, 10];
        string[,] WArray = new string[200, 10];*/

        try
        {
            AtpDTO atpDTO = ParseFromFile(orginAtpPath);
            var resultDiaParse = DiaFileParser.ParseFromFile(orginDiaPath, out diaToolList);
            toolDTOs = atpDTO.toolDTOs;
            magazineDTOs = atpDTO.magazineDTOs;
        }
        catch (Exception ex)
        {
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = "原始ATP文件解析错误" + ex.Message,
                Timestamp = DateTime.Now,
                Data = "orginAtpPath"
            };
        }

        try
        {
            // 将 JSON 字符串转换为对象并获取data值
            data = JsonConvert.DeserializeObject<RootObject>(orginInfo).Data;
            //生成路径
            resultPath = Path.Combine(folderPath, data.WipId + ".ATP");
            //清空刀盘
            int clearMagazine = 4;
            for (int i = 50 * (clearMagazine - 1) + 1; i <= 50 * clearMagazine; i++)
            {
                magazineDTOs[i].toolId = 0;
                magazineDTOs[i].toolUseLife = 0;
                magazineDTOs[i].toolLife = 0;
                magazineDTOs[i].toolDiameter = 0;
                magazineDTOs[i].magazineId = i;
                magazineDTOs[i].toolState = null;
            }
            //修改刀具位置
            foreach (var arrange in data.Arranges)
            {
                //JSON刀具总寿命信息
                LifeDefine lifeDefine = data.lifeDefines.FirstOrDefault(e => e.PgmDiameter == arrange.PgmDiameter);
                //获取相应的刀具信息
                ToolDTO toolDTO = toolDTOs.FirstOrDefault(tool => (float)tool.toolDiameter == (float)arrange.PgmDiameter);
                if (toolDTO == null)
                {
                    ToolDTO tempTool = toolDTOs.FirstOrDefault(tool => (float)tool.toolDiameter == 0);//获取第一个空的刀具
                    toolDTO = diaToolList.FirstOrDefault(tool => (float)tool.toolDiameter == (float)arrange.PgmDiameter);//从DIA获取当前直径的刀具
                    if (toolDTO == null)
                    {
                        return new MessageEntity
                        {
                            IsSuccessful = false,
                            Information = "原始JSON解析错误" + " 没有直径" + arrange.PgmDiameter + "的刀具",
                            Timestamp = DateTime.Now,
                            Data = data.WipId
                        };
                    }
                    toolDTO.toolId = tempTool.toolId;//将DIA刀具信息的刀具id更新成第一个空的刀具id
                    toolDTOs[toolDTO.toolId] = toolDTO;//整体替换第一个空的刀具
                }
                toolDTOs[toolDTO.toolId].toolLife = lifeDefine.Life;
                //刀盘位置更新
                int position = (arrange.BoxIndex - 1) * 50 + arrange.Site;
                magazineDTOs[position].toolId = toolDTO.toolId;

                if (arrange.Life == 0)
                {
                    magazineDTOs[position].toolState = "E";
                }
                else if (arrange.Life != lifeDefine.Life)
                {
                    toolDTOs[toolDTO.toolId].drillToolLife = lifeDefine.Life - arrange.Life;
                    magazineDTOs[position].toolState = "U";
                }
                else
                {
                    magazineDTOs[position].toolState = "N";
                }
            }
        }
        catch (Exception ex)
        {
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = "原始JSON解析错误" + ex.Message,
                Timestamp = DateTime.Now,
                Data = data.WipId
            };
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(resultPath))
            {
                writer.WriteLine("%%9001");
                writer.WriteLine("$");
                for (int i = 1; i < toolDTOs.Count; i++)
                {
                    ToolDTO toolDTO = toolDTOs[i];
                    string info =
                          "T" + toolDTO.toolId
                        + IsParamEmpty("D", toolDTO.toolDiameter)
                        + IsParamEmpty("E", toolDTO.toolType)
                        + IsParamEmpty("S", toolDTO.spindleSpeed) + ","
                        + IsParamEmpty("F", toolDTO.infeedZAxis)
                        + IsParamEmpty("R", toolDTO.retractZAxis)
                        + IsParamEmpty("A", toolDTO.waitTime)
                        + IsParamEmpty("J", toolDTO.J)
                        + IsParamEmpty("Z", toolDTO.workPlaneAdjustment)
                        + IsParamEmpty("N", toolDTO.toolLife) + ","
                        + IsParamEmpty("B", toolDTO.drillToolLife)
                        + IsParamEmpty("C", toolDTO.routerToolLife)
                        + IsParamEmpty("U", toolDTO.compensationDiameter) + ","
                        + IsParamEmpty("V", toolDTO.routingFeedRate) + ","
                        + IsParamEmpty("W", toolDTO.routerWear) + ","
                        + IsParamEmpty("X", toolDTO.circularRoutingFeedRate)
                        + IsParamEmpty("I", toolDTO.I)
                        + IsParamEmpty("P", toolDTO.toolLifeMonitoring)
                        + FillFunParam("G", toolDTO.toolFunctions);
                    writer.WriteLine(info);
                    writer.WriteLine("T" + toolDTO.toolId + "Q," + (toolDTO.unknownL == 0 ? "" : IsParamEmpty("L", toolDTO.unknownL)));
                    writer.WriteLine("T" + toolDTO.toolId + "Nm" + (toolDTO.toolLife == 0 ? "" : toolDTO.toolLife.ToString()) + "Np" + (toolDTO.toolLife == 0 ? "" : toolDTO.toolLife.ToString()));
                    writer.WriteLine("T" + toolDTO.toolId + "M" + toolDTO.drillMethod[0] + ",,,,,");
                    string magazine = string.Empty;
                    List<MagazineDTO> magazineList = magazineDTOs.Where(e => e.toolId == toolDTO.toolId).ToList();
                    foreach (var val in magazineList)
                    {
                        magazine += "M" + val.magazineId;
                    }
                    if (string.IsNullOrEmpty(magazine))
                    {
                        writer.WriteLine("M");
                    }
                    else
                    {
                        writer.WriteLine(magazine);
                    }
                }
                //L部分
                for (var i = 0; i < (magazineDTOs.Count / 50); i++)
                {
                    if ((i + 1) * 50 > magazineDTOs.Count)
                    {
                        break;
                    }
                    string lValue = "L" + (i + 1) + ",50";

                    for (var j = 0; j < 50; j++)
                    {
                        int element = magazineDTOs[i * 50 + j + 1].toolId;
                        if (element == 0)
                        {
                            lValue += "," + null;
                        }
                        else if (magazineDTOs[i * 50 + j + 1].toolState == "E" || magazineDTOs[i * 50 + j + 1].toolState == "GE")
                        {
                            lValue += "," + -element;
                        }
                        else
                        {
                            lValue += "," + element;
                        }
                    }
                    writer.WriteLine(lValue);
                }
                //W部分
                for (var i = 0; i < (magazineDTOs.Count / 50); i++)
                {
                    if ((i + 1) * 50 > magazineDTOs.Count)
                    {
                        break;
                    }
                    string WValue = "W" + (i + 1) + ",50";
                    for (var j = 0; j < 50; j++)
                    {
                        int element = magazineDTOs[i * 50 + j + 1].toolId;
                        WValue += "," + magazineDTOs[i * 50 + j + 1].toolState;
                    }
                    writer.WriteLine(WValue);
                }
            }
        }
        catch (Exception ex)
        {
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = "生成ATP文件过程中错误" + ex.Message,
                Timestamp = DateTime.Now,
                Data = data.WipId
            };
        }

        return new MessageEntity
        {
            IsSuccessful = true,
            Information = "生成ATP文件成功",
            Timestamp = DateTime.Now,
            Data = data.WipId
        };
    }

    private static string FillFunParam(string name, List<bool> toolFunctions)
    {
        name += "\""
             + CheckON(toolFunctions[0]) + "d"
             + CheckON(toolFunctions[1]) + "l"
             + CheckON(toolFunctions[2]) + "r"
             + CheckON(toolFunctions[3]) + "c"
             + CheckON(toolFunctions[4]) + "f"
             + CheckON(toolFunctions[5]) + "k"
             + CheckON(toolFunctions[6]) + "o"
             + CheckON(toolFunctions[7]) + "p"
             + CheckON(toolFunctions[8]) + "q"
             + CheckON(toolFunctions[9]) + "s"
             + CheckON(toolFunctions[10]) + "t"
             + CheckON(toolFunctions[11]) + "v"
             + CheckON(toolFunctions[12]) + "2"
             + CheckON(toolFunctions[13]) + "b"
             + CheckON(toolFunctions[14]) + "u"
             + "\""
             ;
        return name;
    }

    private static string CheckON(bool on)
    {
        return on ? "+" : "-";
    }

    private static string IsParamEmpty(string name, float value)
    {
        if (value == 0)
        {
            return name;
        }
        else
        {
            string strValue = value.ToString("F5");
            strValue = strValue.TrimEnd('0');
            strValue = strValue.TrimStart('0');
            return name + strValue;
        }
    }

    public MessageEntity GenerateAtpFile(string strAtpFilePath, AtpDTO atpDTO) => throw new NotImplementedException();
}
