// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VgEAPClient.Common.CNC.ATP.Models;

namespace VgEAPClient.Common.CNC.ATP;

internal class Cnc84AtpFileParser : IAtpFileParser
{
    private readonly ILogger<CncFileLoader> _logger;

    public Cnc84AtpFileParser(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CncFileLoader>();
    }

    private bool IsAtpFile(in List<string> lines)
    {
        try
        {
            int i = 0;
            while (lines.Count > 0)
            {
                if (lines[i].StartsWith("%%5") && lines[i + 1].StartsWith("$"))
                {
                    i = lines.Count - 1;
                    while (i > 0)
                    {
                        if (lines[i].StartsWith("$"))
                        {
                            return true;
                        }
                        i--;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return false;
    }

    public AtpDTO ParseFromFile(string FilePath)
    {
        AtpDTO atpDTO = new AtpDTO();
        try
        {
            if (!File.Exists(FilePath))//文件不存在就返回空值
            {
                _logger.LogError($"ParseFromFile - {FilePath} 文件不存在");
                return new AtpDTO();
            }
            atpDTO.InitAtpDTO();

            int drlStageFlag = -1;
            List<string> lines = new List<string>();
            //按行获取文件内容
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Equals("%%5001") || line.Equals("%%5002"))//程序头部标志
                    {
                        drlStageFlag = 0;
                    }
                    if (drlStageFlag >= 0)//程序头部标志之后的代码都进行解析
                    {
                        lines.Add(line);
                    }
                }
            }
            _logger.LogInformation($"ParseFromFile - {FilePath} 文件内容：\r\n{string.Join(Environment.NewLine, lines)}\r\n");

            if (!IsAtpFile(lines))
            {
                _logger.LogError($"ParseFromFile - ATP文件格式错误 - {FilePath}");
            }
            //对刀具内容进行解析
            for (int i = 0; i < lines.Count; i++)
            {
                //刀具信息行
                if (lines[i][0] == 'T')//刀具指令解析，添加刀具及基本信息
                {
                    int k = 2;
                    while (lines[i + k][0] == 'M')
                    {
                        k++;
                    }
                    int num = int.Parse(lines[i].Split('D')[0].Split('T')[1]);
                    if (num > atpDTO.ToolDTOCount)
                    {
                        atpDTO.toolDTOs.Add(new ToolDTO { toolId = num }); //AddRange(Enumerable.Range(atpDTO.ToolDTOCount, num).Select(i => new ToolDTO() { toolId = i }).ToList());
                        atpDTO.ToolDTOCount = num;
                    }
                    List<string> tempLine = lines.GetRange(i, k);
                    atpDTO.toolDTOs[num] = FileToDTO_8.StrToToolDTO(tempLine);
                    i += k - 1;
                }
                else if (lines[i][0] == 'L')//刀盘指令解析，刀盘位置
                {
                    string[] LValue = lines[i].Split(',');
                    int rowIndex = StringUtil.GetIntCode(LValue[0].Split('L')[1]);
                    if (rowIndex * 10 > atpDTO.MagazineDTOCount)
                    {
                        atpDTO.magazineDTOs.AddRange(Enumerable.Range(atpDTO.MagazineDTOCount + 1, 10).Select(i => new MagazineDTO() { magazineId = i }).ToList());
                        atpDTO.MagazineDTOCount = rowIndex * 10;
                    }
                    for (int j = 2; j < LValue.Length; j++)
                    {
                        int toolId = Math.Abs(StringUtil.GetIntCode(LValue[j]));
                        atpDTO.magazineDTOs[(rowIndex - 1) * 10 + j - 1].toolId = toolId;
                        if (!string.IsNullOrEmpty(LValue[j]))
                        {
                            atpDTO.magazineDTOs[(rowIndex - 1) * 10 + j - 1].toolDiameter = atpDTO.toolDTOs[toolId].toolDiameter;
                        }
                    }
                }
                else if (lines[i][0] == 'W')
                {
                    string[] WValue = lines[i].Split(',');
                    int rowIndex = StringUtil.GetIntCode(WValue[0].Split('W')[1]);
                    for (int j = 2; j < StringUtil.GetIntCode(WValue[1]) + 2; j++)
                    {
                        int magazineId = (rowIndex - 1) * 10 + j - 1;
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
                    string[] QValue = lines[i].Split(',');
                    int rowIndex = StringUtil.GetIntCode(QValue[0].Split('Q')[1]);
                    if (rowIndex > atpDTO.ToolTolCount)
                    {
                        atpDTO.toolToleranceTableDTOs.Add(new ToolToleranceTableDTO() { tableId = rowIndex });
                        atpDTO.ToolTolCount = rowIndex;
                    }
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
                    }
                }
                else if (lines[i][0] == 'P')
                {
                    List<char> delimiters = new List<char> { 'P', 'S', 'I', 'J', 'F', 'R' };
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
                                    if (rowIndex > atpDTO.PeckDrlDTOCount)
                                    {
                                        atpDTO.peckDrillingValuesDTOs.Add(new PeckDrillingValuesDTO() { toolNumber = rowIndex });
                                        atpDTO.PeckDrlDTOCount = rowIndex;
                                    }
                                    break;

                                case 'S':
                                    atpDTO.peckDrillingValuesDTOs[rowIndex].partialStrokeNumber = StringUtil.GetIntCode(segments[j++]);
                                    break;

                                case 'I':
                                    atpDTO.peckDrillingValuesDTOs[rowIndex].lowerPlane = segments[j++];
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
                    }
                }
                else if (lines[i][0] == 'N')
                {
                    string[] NValue = lines[i].Split(',');
                    int rowIndex = StringUtil.GetIntCode(NValue[0].Split('N')[1]);
                    if (rowIndex > atpDTO.ShortSlotNibCount)
                    {
                        atpDTO.shortSlotNibblingDTOs.Add(new ShortSlotNibblingDTO() { toolId = rowIndex });
                        atpDTO.ShortSlotNibCount = rowIndex;
                    }
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
                    atpDTO.shortSlotNibblingDTOs[rowIndex].TIN_ShiftOrthog = StringUtil.GetFloatCode(NValue[13]);
                }
                else if (lines[i][0] == 'A')
                {
                    string[] AValue = lines[i].Split(',');
                    int rowIndex = StringUtil.GetIntCode(AValue[0].Split('A')[1]);
                    if (rowIndex > atpDTO.LongSlotNibCount)
                    {
                        atpDTO.longSlotNibblingDTOs.Add(new LongSlotNibblingDTO() { toolId = rowIndex });
                        atpDTO.LongSlotNibCount = rowIndex;
                    }
                    atpDTO.longSlotNibblingDTOs[rowIndex].use = StringUtil.GetIntCode(AValue[1]);
                    atpDTO.longSlotNibblingDTOs[rowIndex].minD = StringUtil.GetFloatCode(AValue[2]);
                    atpDTO.longSlotNibblingDTOs[rowIndex].maxD = StringUtil.GetFloatCode(AValue[3]);
                    atpDTO.longSlotNibblingDTOs[rowIndex].type = StringUtil.GetIntCode(AValue[4]);
                    atpDTO.longSlotNibblingDTOs[rowIndex].TMET_Roughn = StringUtil.GetFloatCode(AValue[5]);
                    atpDTO.longSlotNibblingDTOs[rowIndex].InfeedFirst = StringUtil.GetFloatCode(AValue[6]);
                    atpDTO.longSlotNibblingDTOs[rowIndex].InfeedFast = StringUtil.GetFloatCode(AValue[7]);
                    atpDTO.longSlotNibblingDTOs[rowIndex].InfeedSlow = StringUtil.GetFloatCode(AValue[8]);
                    atpDTO.longSlotNibblingDTOs[rowIndex].TIN_Roughn = StringUtil.GetFloatCode(AValue[9]);
                }
                else if (lines[i][0] == 'B')
                {
                }
            }

            _logger.LogInformation($"ATP-Count - ToolDTO={atpDTO.ToolDTOCount}, Mag={atpDTO.MagazineDTOCount}, Q={atpDTO.ToolTolCount}, P={atpDTO.PeckDrlDTOCount}, N={atpDTO.ShortSlotNibCount}, A={atpDTO.LongSlotNibCount}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return atpDTO;
    }

    public MessageEntity ParseToFile(int magazienRange, string orginAtpPath, string orginInfo)
    {
        string resultPath = Path.GetDirectoryName(orginAtpPath) + "/test.ATP";
        AtpDTO atpDTO = new AtpDTO();
        var data = new Data();
        try
        {
            atpDTO = ParseFromFile(orginAtpPath);
            data = JsonConvert.DeserializeObject<RootObject>(orginInfo)?.Data;
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
            LifeDefine lifeDefine = data.lifeDefines.FirstOrDefault(e => e.PgmDiameter == arrange.PgmDiameter && e.MoCount == arrange.MoCount);
            //获取相应的刀具信息
            ToolDTO toolDTO = atpDTO.toolDTOs.FirstOrDefault(tool => (float)tool.toolDiameter == (float)arrange.PgmDiameter);
            toolDTO.magazines.Add(position);
            for (int i = 1; i <= magazienRange; i++)
            {
                if (atpDTO.magazineDTOs[i].toolId == toolDTO.toolId && (atpDTO.magazineDTOs[i].toolState == "E" || atpDTO.magazineDTOs[i].toolState == "GE"))
                {
                    atpDTO.magazineDTOs[position].toolId = atpDTO.magazineDTOs[i].toolId;
                    atpDTO.magazineDTOs[position].toolState = atpDTO.magazineDTOs[i].toolState;
                    atpDTO.magazineDTOs[position].toolLife = atpDTO.magazineDTOs[i].toolLife;
                    atpDTO.magazineDTOs[position].toolUseLife = atpDTO.magazineDTOs[i].toolUseLife;
                    atpDTO.magazineDTOs[position].toolDiameter = atpDTO.magazineDTOs[i].toolDiameter;
                    atpDTO.magazineDTOs[position].magazineId = position;
                    atpDTO.magazineDTOs[i].toolLife = arrange.Life;
                    atpDTO.magazineDTOs[i].toolState = "N";
                    atpDTO.magazineDTOs[i].toolUseLife = 0;
                    break;
                }
            }
        }
        try
        {
            using (StreamWriter writer = new StreamWriter(resultPath))
            {
                writer.WriteLine("%%5001");
                writer.WriteLine("$");
                for (int i = 1; i < atpDTO.toolDTOs.Count; i++)
                {
                    ToolDTO toolDTO = atpDTO.toolDTOs[i];
                    string info =
                          "T" + toolDTO.toolId
                        //+ "D" + toolDTO.toolDiameter.ToString("0.000")
                        + IsParamEmpty("D", toolDTO.toolDiameter)
                        + IsParamEmpty("E", toolDTO.toolType)
                        + IsParamEmpty("S", toolDTO.spindleSpeed)
                        + IsParamEmpty("F", toolDTO.infeedZAxis)
                        + IsParamEmpty("R", toolDTO.retractZAxis)
                        + IsParamEmpty("A", toolDTO.waitTime)
                        //+ IsParamEmpty("J", 0)
                        + IsParamEmpty("Z", toolDTO.workPlaneAdjustment)
                        + IsParamEmpty("N", toolDTO.toolLife)
                        + IsParamEmpty("B", toolDTO.drillToolLife)
                        //+ "C" + toolDTO.routerToolLife
                        + IsParamEmpty("P", toolDTO.toolLifeMonitoring)
                        + IsParamEmpty("U", toolDTO.compensationDiameter)
                        + IsParamEmpty("V", toolDTO.routingFeedRate)
                        + IsParamEmpty("W", toolDTO.routerWear)
                        + IsParamEmpty("X", toolDTO.circularRoutingFeedRate)
                        + FillFunParam("G", toolDTO.toolFunctions);
                    writer.WriteLine(info);
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
                    string lValue = "L" + (i + 1) + ",10";
                    for (var j = 0; j < 10; j++)
                    {
                        int element = atpDTO.magazineDTOs[i * 10 + j + 1].toolId;
                        if (element == 0)
                        {
                            lValue += "," + null;
                        }
                        else if (atpDTO.magazineDTOs[i * 10 + j + 1].toolState == "E" || atpDTO.magazineDTOs[i * 10 + j + 1].toolState == "GE")
                        {
                            lValue += "," + (-element);
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
                    string WValue = "W" + (i + 1) + ",10";
                    for (var j = 0; j < 10; j++)
                    {
                        int element = atpDTO.magazineDTOs[i * 10 + j + 1].toolId;
                        WValue += "," + atpDTO.magazineDTOs[i * 10 + j + 1].toolState;
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
        try
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vegaAtp");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string resultPath = null;
            Data? data = null;
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
                        LifeDefine lifeDefine = data.lifeDefines.FirstOrDefault(e => e.PgmDiameter == arrange.PgmDiameter && e.MoCount == arrange.MoCount);
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
                    _logger.LogError(ex, $"ParseToFile - 原始JSON解析错误 - {ex.Message}");
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
                    return GenerateAtpFile(resultPath, atpDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"ParseToFile - 生成AtpFile异常 {ex.Message}");
                    return new MessageEntity
                    {
                        IsSuccessful = false,
                        Information = "生成ATP文件过程中错误" + ex.Message,
                        Timestamp = DateTime.Now,
                        Data = data.WipId
                    };
                }
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return new MessageEntity
        {
            IsSuccessful = false,
            Information = "ParseToFile过程中错误",
            Timestamp = DateTime.Now,
            Data = ""
        };
    }

    public MessageEntity GenerateAtpFile(string strAtpFilePath, AtpDTO atpDTO)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(strAtpFilePath))
            {
                writer.WriteLine("%%5001");
                writer.WriteLine("$");
                for (int i = 1; i <= atpDTO.ToolDTOCount; i++)
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
                        //+ IsParamEmpty("J", 0)
                        + IsParamEmpty("Z", toolDTO.workPlaneAdjustment)
                        + IsParamEmpty("N", toolDTO.toolLife)
                        + IsParamEmpty("B", toolDTO.drillToolLife)
                        //+ "C" + toolDTO.routerToolLife
                        + IsParamEmpty("P", toolDTO.toolLifeMonitoring)
                        + IsParamEmpty("U", toolDTO.compensationDiameter)
                        + IsParamEmpty("V", toolDTO.routingFeedRate)
                        + IsParamEmpty("W", toolDTO.routerWear)
                        + IsParamEmpty("X", toolDTO.circularRoutingFeedRate)
                        + FillFunParam("G", toolDTO.toolFunctions);
                    writer.WriteLine(info);

                    string strTQ = "T" + toolDTO.toolId + "Q,,t1M" + (toolDTO.drillMethod[0] == 0 ? "" : toolDTO.drillMethod[0].ToString());
                    for (int j = 1; j < toolDTO.drillMethod.Count; j++)
                    {
                        strTQ += "," + (toolDTO.drillMethod[j] == 0 ? "" : toolDTO.drillMethod[j].ToString());
                    }
                    strTQ += "Y,,,,,,";
                    writer.WriteLine(strTQ);

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
                int nLWLines = atpDTO.MagazineDTOCount / 10;
                //L部分
                for (var i = 0; i < nLWLines; i++)
                {
                    string lValue = "L" + (i + 1) + ",10";
                    for (var j = 0; j < 10; j++)
                    {
                        int element = atpDTO.magazineDTOs[i * 10 + j + 1].toolId;
                        if (element == 0)
                        {
                            lValue += "," + null;
                        }
                        else if (atpDTO.magazineDTOs[i * 10 + j + 1].toolState == "E" || atpDTO.magazineDTOs[i * 10 + j + 1].toolState == "GE")
                        {
                            lValue += "," + (-element);
                        }
                        else
                        {
                            lValue += "," + element;
                        }
                    }
                    writer.WriteLine(lValue);
                }
                //W部分
                for (var i = 0; i < nLWLines; i++)
                {
                    string WValue = "W" + (i + 1) + ",10";
                    for (var j = 0; j < 10; j++)
                    {
                        int element = atpDTO.magazineDTOs[i * 10 + j + 1].toolId;
                        WValue += "," + atpDTO.magazineDTOs[i * 10 + j + 1].toolState;
                    }
                    writer.WriteLine(WValue);
                }

                // Q
                for (int i = 1; i <= atpDTO.ToolTolCount; i++)
                {
                    var toolTolTab = atpDTO.toolToleranceTableDTOs[i];
                    string QValue = "Q" + i
                        + IsParamEmpty(",", toolTolTab.diameter)
                        + IsParamEmpty(",", toolTolTab.nDiaTolerance)
                        + IsParamEmpty(",", toolTolTab.pDiaTolerance)
                        + IsParamEmpty(",", toolTolTab.nLenTolerance)
                        + IsParamEmpty(",", toolTolTab.pLenTolerance)
                        + IsParamEmpty(",", toolTolTab.warnOfDeviation)
                        + IsParamEmpty(",", toolTolTab.stopOfDeviation)
                        + IsParamEmpty(",K", toolTolTab.kZ1)
                        + IsParamEmpty(",", toolTolTab.kZ2)
                        + IsParamEmpty(",", toolTolTab.kZ3)
                        + IsParamEmpty(",", toolTolTab.kZ4)
                        + IsParamEmpty(",", toolTolTab.kZ5)
                        + IsParamEmpty(",", toolTolTab.kZ6);

                    writer.WriteLine(QValue);
                }

                // P
                for (int i = 1; i <= atpDTO.PeckDrlDTOCount; i++)
                {
                    var toolPeckDrill = atpDTO.peckDrillingValuesDTOs[i];
                    string PValue = "P" + i
                        + "S" + toolPeckDrill.partialStrokeNumber.ToString()
                        + "I" + toolPeckDrill.lowerPlane
                        + IsParamEmpty("J", toolPeckDrill.retractionPlane)
                        + IsParamEmpty("F", toolPeckDrill.infeedRate)
                        + IsParamEmpty("R", toolPeckDrill.retractRate);

                    writer.WriteLine(PValue);
                }

                // N
                for (int i = 1; i <= atpDTO.ShortSlotNibCount; i++)
                {
                    var toolShortNib = atpDTO.shortSlotNibblingDTOs[i];
                    string NValue = "N" + i
                        + "," + (toolShortNib.use == 0 ? "" : toolShortNib.use.ToString())
                        + IsParamEmpty(",", toolShortNib.minD)
                        + IsParamEmpty(",", toolShortNib.maxD)
                        + IsParamEmpty(",", toolShortNib.TMET_ShiftOrthog)
                        + IsParamEmpty(",", toolShortNib.type)
                        + IsParamEmpty(",", toolShortNib.TMET_Roughn)
                        + IsParamEmpty(",", toolShortNib.InfeedFirst)
                        + IsParamEmpty(",", toolShortNib.InfeedFast)
                        + IsParamEmpty(",", toolShortNib.InfeedSlow)
                        + IsParamEmpty(",", toolShortNib.TMET_ShiftLong)
                        + IsParamEmpty(",", toolShortNib.TIN_Roughn)
                        + IsParamEmpty(",", toolShortNib.TIN_ShiftLong)
                        + IsParamEmpty(",", toolShortNib.TIN_ShiftOrthog);

                    writer.WriteLine(NValue);
                }

                // A
                for (int i = 1; i <= atpDTO.LongSlotNibCount; i++)
                {
                    var toolLongSlot = atpDTO.longSlotNibblingDTOs[i];
                    string AValue = "A" + i
                        + "," + (toolLongSlot.use == 0 ? "" : toolLongSlot.use.ToString())
                        + IsParamEmpty(",", toolLongSlot.minD)
                        + IsParamEmpty(",", toolLongSlot.maxD)
                        + IsParamEmpty(",", toolLongSlot.type)
                        + IsParamEmpty(",", toolLongSlot.TMET_Roughn)
                        + IsParamEmpty(",", toolLongSlot.InfeedFirst)
                        + IsParamEmpty(",", toolLongSlot.InfeedFast)
                        + IsParamEmpty(",", toolLongSlot.InfeedSlow)
                        + IsParamEmpty(",", toolLongSlot.TIN_Roughn);

                    writer.WriteLine(AValue);
                }

                writer.WriteLine("$");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GenerateAtpFile - 生成ATP文件过程中错误" + ex.Message);
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = $"生成ATP文件过程中错误 - {ex.Message} - {strAtpFilePath}",
                Timestamp = DateTime.Now,
                Data = ""
            };
        }

        return new MessageEntity
        {
            IsSuccessful = true,
            Information = $"GenerateAtpFile - 生成ATP文件成功 - {strAtpFilePath}",
            Timestamp = DateTime.Now,
            Data = strAtpFilePath
        };
    }

    private static string FillFunParam(string name, List<bool> toolFunctions)
    {
        name += "\"" + CheckON(toolFunctions[0]) + "v"
             + CheckON(toolFunctions[1]) + "d"
             + CheckON(toolFunctions[2]) + "l"
             + CheckON(toolFunctions[3]) + "r"
             + CheckON(toolFunctions[4]) + "s"
             + CheckON(toolFunctions[5]) + "c"
             + CheckON(toolFunctions[6]) + "q"
             + CheckON(toolFunctions[7]) + "o"
             + CheckON(toolFunctions[8]) + "t"
             + CheckON(toolFunctions[9]) + "k"
             + CheckON(toolFunctions[10]) + "p\""
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
}
