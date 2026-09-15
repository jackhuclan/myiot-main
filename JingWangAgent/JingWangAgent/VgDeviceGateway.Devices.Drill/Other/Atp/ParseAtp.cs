using Newtonsoft.Json;
using VgDeviceGateway.Devices.Drill.Other.Atp.Entity;
using VgDeviceGateway.Devices.Drill.Other.Dto;

namespace VgDeviceGateway.Devices.Drill.Other.Atp
{
    public class ParseAtp
    {
        public static AtpDTO ParseFromFile(string FilePath)
        {
            if (!File.Exists(FilePath))//文件不存在就返回空值
            {
                return new AtpDTO();
            }
            int drlStageFlag = -1;
            List<string> lines = new List<string>();
            AtpDTO atpDTO = new AtpDTO();
            atpDTO.toolDTOs = Enumerable.Range(0, 1001).Select(i => new ToolDTO() { toolId = i }).ToList();
            atpDTO.magazineDTOs = Enumerable.Range(0, 2001).Select(i => new MagazineDTO() { magazineId = i }).ToList();
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
                    List<string> tempLine = lines.GetRange(i, k);
                    atpDTO.toolDTOs[num] = FileToDTO.StrToToolDTO(tempLine);
                    i += k - 1;
                }
                else if (lines[i][0] == 'L')//刀盘指令解析，刀盘位置
                {
                    string[] LValue = lines[i].Split(',');
                    int rowIndex = StringUtil.GetIntCode(LValue[0].Split('L')[1]);
                    for (int j = 2; j < 12; j++)
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
                        if (String.IsNullOrEmpty(WValue[j]))
                        {
                            WValue[j] = "Null";
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
                    }
                }
                else if (lines[i][0] == 'N')
                {
                    string[] NValue = lines[i].Split(',');
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
                    atpDTO.shortSlotNibblingDTOs[rowIndex].TIN_ShiftOrthog = StringUtil.GetFloatCode(NValue[13]);
                }
                else if (lines[i][0] == 'A')
                {
                    string[] AValue = lines[i].Split(',');
                    int rowIndex = StringUtil.GetIntCode(AValue[0].Split('A')[1]);
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
            return atpDTO;
        }

        public static MessageEntity ParseToFile(string newMagazineInfo, int[] newSides, string orginAtpPath, string orginDiaPath, int[] oldSites, int toolBoxCount = 4, int toolBoxSize = 50)
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vegaAtp");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string resultPath = null;
            List<ToolDTO> toolDTOs = new List<ToolDTO>();
            List<ToolDTO> diaToolList = new List<ToolDTO>();
            List<MagazineDTO> magazineDTOs = new List<MagazineDTO>();

            try
            {
                AtpDTO atpDTO = ParseFromFile(orginAtpPath);
                diaToolList = ParseDia.ParseFromFile(orginDiaPath);
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

            RootObject rootObject = null;
            Data data = null;
            try
            {
                //rootObject = JsonSerializer.Deserialize<RootObject>(newMagazineInfo);
                rootObject = JsonConvert.DeserializeObject<RootObject>(newMagazineInfo);
                data = rootObject.Data;
                //生成路径
                resultPath = Path.Combine(folderPath, rootObject.Info + ".ATP");
                //清空旧刀盘
                if (oldSites.Length > 0)
                {
                    ClearSideMagazine(magazineDTOs, oldSites, toolBoxCount, toolBoxSize);
                }
                if (newSides.Length > 0)
                {
                    //清空新刀盘
                    ClearSideMagazine(magazineDTOs, newSides, toolBoxCount, toolBoxSize);
                    //修改新刀盘刀具位置
                    foreach (var arrange in data.Arranges)
                    {
                        //JSON刀具总寿命信息
                        LifeDefine lifeDefine = data.LifeDefines.FirstOrDefault(e => e.PgmDiameter == arrange.PgmDiameter && e.MoCount == arrange.MoCount);
                        //获取相应的刀具信息
                        ToolDTO toolDTO = toolDTOs.FirstOrDefault(tool => (float)tool.toolDiameter == (float)arrange.PgmDiameter);
                        if (toolDTO == null)
                        {
                            ToolDTO tempTool = toolDTOs.FirstOrDefault(tool => (float)tool.toolDiameter == (float)0);//获取第一个空的刀具
                            toolDTO = diaToolList.FirstOrDefault(tool => (float)tool.toolDiameter == (float)arrange.PgmDiameter);//从DIA获取当前直径的刀具
                            if (toolDTO == null)
                            {
                                return new MessageEntity
                                {
                                    IsSuccessful = false,
                                    Information = "原始JSON解析错误" + " 没有直径" + arrange.PgmDiameter + "的刀具",
                                    Timestamp = DateTime.Now,
                                    Data = rootObject.Info
                                };
                            }
                            toolDTO.toolId = tempTool.toolId;//将DIA刀具信息的刀具id更新成第一个空的刀具id
                            toolDTOs[toolDTO.toolId] = toolDTO;//整体替换第一个空的刀具
                        }
                        toolDTOs[toolDTO.toolId].toolLife = lifeDefine.Life;
                        toolDTOs[toolDTO.toolId].drillToolLife = 0;
                        //刀盘位置更新
                        int position = (arrange.TrayIndex - 1) * toolBoxCount * toolBoxSize + arrange.Site;
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
            }
            catch (Exception ex)
            {
                return new MessageEntity
                {
                    IsSuccessful = false,
                    Information = "原始JSON解析错误" + ex.Message,
                    Timestamp = DateTime.Now,
                    Data = rootObject.Info
                };
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(resultPath))
                {
                    writer.WriteLine("%%5001");
                    writer.WriteLine("$");
                    for (int i = 1; i < toolDTOs.Count; i++)
                    {
                        ToolDTO toolDTO = toolDTOs[i];
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
                            + IsParamEmpty("B", 0)
                            //+ "C" + toolDTO.routerToolLife
                            + IsParamEmpty("P", toolDTO.toolLifeMonitoring)
                            + IsParamEmpty("U", toolDTO.compensationDiameter)
                            + IsParamEmpty("V", toolDTO.routingFeedRate)
                            + IsParamEmpty("W", toolDTO.routerWear)
                            + IsParamEmpty("X", toolDTO.circularRoutingFeedRate)
                            + FillFunParam("G", toolDTO.toolFunctions);
                        writer.WriteLine(info);
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
                    for (var i = 0; i < 200; i++)
                    {
                        string lValue = "L" + (i + 1) + ",10";
                        for (var j = 0; j < 10; j++)
                        {
                            int element = magazineDTOs[i * 10 + j + 1].toolId;
                            if (element == 0)
                            {
                                lValue += "," + null;
                            }
                            else if (magazineDTOs[i * 10 + j + 1].toolState == "E" || magazineDTOs[i * 10 + j + 1].toolState == "GE")
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
                            int element = magazineDTOs[i * 10 + j + 1].toolId;
                            WValue += "," + magazineDTOs[i * 10 + j + 1].toolState;
                        }
                        writer.WriteLine(WValue);
                    }
                    /*int index = 1;
                    //刀具信息
                    foreach (DictionaryEntry pair in dict)
                    {
                        LifeDefine key = (LifeDefine)pair.Key;
                        List<Arrange> value = (List<Arrange>)pair.Value;
                        ToolDTO toolDTO = toolDTOs.FirstOrDefault(tool => (float)tool.toolDiameter == (float)key.PgmDiameter);
                        if (toolDTO == null)
                        {
                            toolDTO = diaToolList.FirstOrDefault(tool => (float)tool.toolDiameter == (float)key.PgmDiameter);
                        }
                        if (toolDTO == null)
                        {
                            return new MessageEntity
                            {
                                IsSuccessful = false,
                                Information = "生成ATP文件过程中错误，" + "没有找到直径"+ key.PgmDiameter+"的刀具信息",
                                Timestamp = DateTime.Now,
                                Data = data.WipId
                            };
                        }
                        List<int> magazines = toolDTO.magazines;
                        string info = "T" + index + "D" + key.PgmDiameter.ToString(".###")
                            + IsParamEmpty("E", toolDTO.toolType)
                            + IsParamEmpty("S", toolDTO.spindleSpeed)
                            + IsParamEmpty("F", toolDTO.infeedZAxis)
                            + IsParamEmpty("R", toolDTO.retractZAxis)
                            + IsParamEmpty("A", toolDTO.waitTime)
                            + IsParamEmpty("J", 0)
                            + IsParamEmpty("Z", toolDTO.workPlaneAdjustment)
                            + IsParamEmpty("N", key.Life)
                            + IsParamEmpty("B", key.Life - (int)lifeDict[key])
                            + IsParamEmpty("C", toolDTO.routerToolLife)
                            + IsParamEmpty("P", toolDTO.toolLifeMonitoring)
                            + IsParamEmpty("U", toolDTO.compensationDiameter)
                            + IsParamEmpty("V", toolDTO.routingFeedRate)
                            + IsParamEmpty("W", toolDTO.routerWear)
                            + IsParamEmpty("X", toolDTO.circularRoutingFeedRate)
                            + FillFunParam("G", toolDTO.toolFunctions);

                        //string info  = "T"+index+"D"+ pair.Key.ToString(".##")+ "ESFRAJZ15.N16,BCU,V,W,XI17.G\"+d+l+r-c-f-k+o-p+q-s-t-v-2-b-u\"";
                        writer.WriteLine(info);
                        string magazine = string.Empty;
                        foreach (var val in value)
                        {
                            magazine += "M" + val.Site;
                        }
                        foreach (var val in magazines)
                        {
                            magazine += "M" + val;
                            int x = 0;
                            int y = 0;
                            if (val % 10 == 0)
                            {
                                x = val / 10 - 1;
                                y = 9;
                            }
                            else
                            {
                                x = val / 10;
                                y = val % 10 - 1;
                            }
                            *//*int i = val / 10;
                            int j = val % 10 -1;*//*
                            MagazineDTO magazineDTO = magazineDTOs[val];
                            WArray[x, y] = magazineDTO.toolState;
                            LArray[x, y] = magazineDTO.toolState.Equals("E") ? -index : index;
                        }
                        writer.WriteLine(magazine);
                        index++;
                    }
                    //L部分
                    int rows = LArray.GetLength(0);
                    int columns = LArray.GetLength(1);

                    for (int i = 0; i < rows; i++)
                    {
                        string lValue = "L"+(i+1)+",10";
                        for (int j = 0; j < columns; j++)
                        {
                            int element = LArray[i, j];
                            string num = (element != 0) ? element.ToString() : null;
                            lValue += ","+ num;
                        }
                        writer.WriteLine(lValue);
                    }
                    rows = WArray.GetLength(0);
                    columns = WArray.GetLength(1);

                    for (int i = 0; i < rows; i++)
                    {
                        string wValue = "W" + (i + 1) + ",10";
                        for (int j = 0; j < columns; j++)
                        {
                            string element = WArray[i, j];
                            wValue += "," + element;
                        }
                        writer.WriteLine(wValue);
                    }
                    //W部分
                    //刀盘信息
                    writer.WriteLine("$");
                }*/
                }
            }
            catch (Exception ex)
            {
                return new MessageEntity
                {
                    IsSuccessful = false,
                    Information = "生成ATP文件过程中错误" + ex.Message,
                    Timestamp = DateTime.Now,
                    Data = rootObject.Info
                };
            }

            return new MessageEntity
            {
                IsSuccessful = true,
                Information = "生成ATP文件成功",
                Timestamp = DateTime.Now,
                Data = rootObject.Info,
                AtpFile = resultPath
            };
        }

        public static int[] ParseToolLifetimeByTray(List<MagazineDTO> magazineDTOs, int region = 3, int toolBoxCount = 4, int toolBoxSize = 50)
        {
            List<int> regionIds = new List<int>();
            for (int i = 1; i <= region; i++)
            {
                var data = magazineDTOs.Where(magazineDTO => magazineDTO.toolState != "Null").Where(magazineDTO => magazineDTO.magazineId >= toolBoxSize * toolBoxCount * (i - 1) + 1 && magazineDTO.magazineId <= toolBoxSize * toolBoxCount * i);
                if (data.Any() && data.All(s => s.toolState == "E"))
                {
                    regionIds.Add(i);
                }
            }
            return regionIds.ToArray();
        }

        private static void ClearSideMagazine(List<MagazineDTO> magazineDTOs, int[] clearMagazines, int toolBoxCount = 4, int toolBoxSize = 50)
        {
            foreach (var clearMagazine in clearMagazines)
            {
                for (int i = toolBoxSize * toolBoxCount * (clearMagazine - 1) + 1; i <= toolBoxSize * toolBoxCount * clearMagazine; i++)
                {
                    magazineDTOs[i].toolId = 0;
                    magazineDTOs[i].toolUseLife = 0;
                    magazineDTOs[i].toolLife = 0;
                    magazineDTOs[i].toolDiameter = 0;
                    magazineDTOs[i].magazineId = i;
                    magazineDTOs[i].toolState = null;
                }
            }
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
                return name + value.ToString("0.000");
                string result = name;
                switch (name)
                {
                    case "D":
                        result = name + value.ToString().Replace("0.", ".");
                        break;

                    case "S":
                        if (value < 1)
                        {
                            result = name + value.ToString().Replace("0.", ".");
                        }
                        else
                        {
                            int i;
                            if (int.TryParse(value.ToString(), out i))
                                result = name + i.ToString() + ".";
                            else
                                result = name + value.ToString();
                        }
                        break;

                    case "F":
                        if (value < 1)
                        {
                            result = name + value.ToString().Replace("0.", ".");
                        }
                        else
                        {
                            int i;
                            if (int.TryParse(value.ToString(), out i))
                                result = name + i.ToString() + ".";
                            else
                                result = name + value.ToString();
                        }
                        break;

                    case "R":
                        if (value < 1)
                        {
                            result = name + value.ToString().Replace("0.", ".");
                        }
                        else
                        {
                            int i;
                            if (int.TryParse(value.ToString(), out i))
                                result = name + i.ToString() + ".";
                            else
                                result = name + value.ToString();
                        }
                        break;

                    case "Z":
                        if (value < 0)
                        {
                            result = name + value.ToString().Replace("0.", ".");
                        }
                        else
                        {
                            int i;
                            if (int.TryParse(value.ToString(), out i))
                                result = name + i.ToString() + ".";
                            else
                                result = name + value.ToString();
                        }
                        break;

                    case "N":
                        result = name + value.ToString();
                        break;

                    case "B":
                        result = name + value.ToString();
                        break;

                    default:
                        result = name + value.ToString("0.000");
                        break;
                }
                return result;
            }
        }
    }
}
