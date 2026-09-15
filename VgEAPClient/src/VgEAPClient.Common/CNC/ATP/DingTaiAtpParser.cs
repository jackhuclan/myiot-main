// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.ATP.Models;

namespace VgEAPClient.Common.CNC.ATP;

internal class DingTaiAtpParser : IMesAtpParser
{
    private ILogger<DingTaiAtpParser> _logger;
    private readonly IAtpFileParser _atpFileParser;
    private Dictionary<int, ToolDTO> _oldTool_NewToolPairs = new Dictionary<int, ToolDTO>();
    private Dictionary<int, int> _oldToolId_New = new Dictionary<int, int>();
    private const int _calcToolBoxCount = 3;

    public DingTaiAtpParser(ILoggerFactory loggerFactory,
        ICNCOperatorProvider cNCOperatorProvider)
    {
        _logger = loggerFactory.CreateLogger<DingTaiAtpParser>();
        _atpFileParser = cNCOperatorProvider.GetAtpFileParser();
    }

    public MessageEntity ParseFromMesAtp(string strMesAtpPath, out AtpDTO atpDTO)
    {
        atpDTO = new AtpDTO();
        try
        {
            if (!File.Exists(strMesAtpPath))//文件不存在就返回空值
            {
                return new MessageEntity
                {
                    IsSuccessful = false,
                    Information = $"ParseFromMesAtp - MES ATP 文件不存在{strMesAtpPath}",
                    Timestamp = DateTime.Now,
                    Data = ""
                };
            }
            atpDTO.toolDTOs = Enumerable.Range(0, 1001).Select(i => new ToolDTO() { toolId = i }).ToList();
            atpDTO.magazineDTOs = Enumerable.Range(0, 2001).Select(i => new MagazineDTO() { magazineId = i }).ToList();

            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(strMesAtpPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Equals("%%") || line.Equals("$"))//程序头部标志
                    {
                        continue;
                    }
                    lines.Add(line);
                }
            }
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i][0] == 'T')//刀具指令解析，添加刀具及基本信息
                {
                    int num = int.Parse(lines[i].Split('D')[0].Split('T')[1]);
                    atpDTO.toolDTOs[num].toolId = num;
                    atpDTO.toolDTOs[num].toolDiameter = float.Parse(lines[i].Split('D')[1].Trim());
                }
                else if (lines[i][0] == 'L')
                {
                    string[] LValue = lines[i].Split(',');
                    int rowIndex = StringUtil.GetIntCode(LValue[0].Split('L')[1]);
                    for (int j = 2; j < LValue.Length; j++)
                    {
                        int toolId = Math.Abs(StringUtil.GetIntCode(LValue[j]));
                        atpDTO.magazineDTOs[(rowIndex - 1) * 10 + j - 1].toolId = toolId;
                        if (!string.IsNullOrEmpty(LValue[j]))
                        {
                            atpDTO.magazineDTOs[(rowIndex - 1) * 10 + j - 1].toolDiameter = atpDTO.toolDTOs[toolId].toolDiameter;
                            atpDTO.toolDTOs[toolId].magazines.Add((rowIndex - 1) * 10 + j - 1);
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
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = $"ParseFromMesAtp - MES ATP 解析异常 - {ex.Message}, {strMesAtpPath}",
                Timestamp = DateTime.Now,
                Data = ""
            };
        }
        return new MessageEntity
        {
            IsSuccessful = true,
            Information = $"ParseFromMesAtp - MES ATP 解析成功, {strMesAtpPath}",
            Timestamp = DateTime.Now,
            Data = ""
        };
    }

    /// <summary>
    /// 清空前三个其中指定刀盒信息
    /// </summary>
    /// <param name="atpDTO"></param>
    /// <param name="ClearToolBoxIndex">需要清空的刀盒下表eg:"12", "123"</param>
    /// <returns></returns>
    public MessageEntity ClearToolBoxInfo(AtpDTO atpDTO, string ClearToolBoxIndex)
    {
        try
        {
            int[] aryIndex = new int[3];
            for (int i = 0; i < ClearToolBoxIndex.Length; i++)
            {
                aryIndex[i] = int.Parse(ClearToolBoxIndex.Substring(i, 1));
            }

            foreach (int index in aryIndex)
            {
                if (index > 0)
                {
                    int nStart = 50 * (index - 1) + 1;
                    for (int j = nStart; j < nStart + 50; j++)
                    {
                        int nToolId = atpDTO.magazineDTOs[j].toolId;
                        if (nToolId > 0)
                        {
                            atpDTO.toolDTOs[nToolId].magazines.Remove(j);
                            atpDTO.magazineDTOs[j].ClearMagazData();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = "清空刀盒信息异常 - " + ex.Message,
                Timestamp = DateTime.Now,
                Data = ""
            };
        }

        return new MessageEntity
        {
            IsSuccessful = true,
            Information = $"清空刀盒信息成功 - {ClearToolBoxIndex}",
            Timestamp = DateTime.Now,
            Data = ""
        };
    }

    public MessageEntity GenerateAtpFile(string strMesAtpPath, string strDiaPath, AtpDTO orgAtpDTO, string strGenerateFileName, string strGeneratePath = "")
    {
        try
        {
            AtpDTO mesAtpDTO = null;
            var resultParseMes = ParseFromMesAtp(strMesAtpPath, out mesAtpDTO);
            _logger.LogInformation($"GenerateAtpFile - {resultParseMes.Information}");
            if (resultParseMes == null || !resultParseMes.IsSuccessful)
            {
                _logger.LogError($"GenerateAtpFile - {resultParseMes?.Information}");
                return resultParseMes;
            }

            string ClearToolBoxIndex = string.Empty;
            var resultCalc = CalcNeedClearBoxIndex(mesAtpDTO, out ClearToolBoxIndex);
            _logger.LogInformation($"GenerateAtpFile - {resultCalc.Information}");

            if (resultCalc != null && resultCalc.IsSuccessful)
            {
                var result = ClearToolBoxInfo(orgAtpDTO, ClearToolBoxIndex);
                _logger.LogInformation($"GenerateAtpFile - {result.Information}");
                if (result != null && result.IsSuccessful)
                {
                    List<ToolDTO> diaToolList = new List<ToolDTO>();
                    try
                    {
                        var resultDiaParse = DiaFileParser.ParseFromFile(strDiaPath, out diaToolList);
                        if (resultDiaParse.IsSuccessful && diaToolList != null)
                        {
                            _logger.LogInformation($"GenerateAtpFile - 整合刀具文件信息Org2NewAtpDTO - {strMesAtpPath}， {strDiaPath}， 生成路径{strGeneratePath}");
                            return Org2NewAtpDTO(diaToolList, orgAtpDTO, mesAtpDTO, strGenerateFileName, strGeneratePath);
                        }
                        else
                        {
                            _logger.LogError($"GenerateAtpFile - 直径表文件解析失败 - {strDiaPath}  - {resultDiaParse.Information}");
                            return resultDiaParse;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"GenerateAtpFile - {strDiaPath} 直径表文件解析异常 - {ex.Message}");
                    }
                }
                else
                {
                    _logger.LogError($"GenerateAtpFile - {result.Information}");
                    return result;
                }
            }
            else
            {
                _logger.LogError($"GenerateAtpFile - {resultCalc.Information}");
                return resultCalc;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"GenerateAtpFile - DingTai-生成刀具文件异常 - {ex.Message}");
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = "GenerateAtpFile-生成刀具文件异常 - " + ex.Message,
                Timestamp = DateTime.Now,
                Data = ""
            };
        }

        return new MessageEntity
        {
            IsSuccessful = true,
            Information = $"GenerateAtpFile-生成刀具文件成功 - {strMesAtpPath} - {strDiaPath}",
            Timestamp = DateTime.Now,
            Data = ""
        };
    }

    private MessageEntity CalcNeedClearBoxIndex(AtpDTO mesAtpDTO, out string strClearBoxIndex)
    {
        strClearBoxIndex = string.Empty;
        try
        {
            for (int i = 0; i < _calcToolBoxCount; i++)
            {
                for (int j = 1; j <= 50; j++)
                {
                    if (mesAtpDTO.magazineDTOs[i * 50 + j].toolId > 0)
                    {
                        strClearBoxIndex += (i + 1).ToString();
                        break;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(strClearBoxIndex))
            {
                return new MessageEntity
                {
                    IsSuccessful = true,
                    Information = $"CalcNeedClearBoxIndex-成功，需要清空的刀盒为{strClearBoxIndex}",
                    Timestamp = DateTime.Now,
                    Data = ""
                };
            }
            else
            {
                _logger.LogInformation($"CalcNeedClearBoxIndex - 计算需要清空的刀盒为空，请核查MES下发的刀具文件是否排刀");
                return new MessageEntity
                {
                    IsSuccessful = false,
                    Information = "CalcNeedClearBoxIndex - 计算需要清空的刀盒为空，请核查MES下发的刀具文件是否排刀",
                    Timestamp = DateTime.Now,
                    Data = ""
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"CalcNeedClearBoxIndex - 计算需要清空的刀盒异常 - {ex.Message}");
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = "CalcNeedClearBoxIndex-计算需要清空的刀盒异常" + ex.Message,
                Timestamp = DateTime.Now,
                Data = ""
            };
        }
    }

    public MessageEntity Org2NewAtpDTO(List<ToolDTO> diaToolList, AtpDTO orgAtpDTO, AtpDTO mesAtpDTO, string strGenerateFileName, string strGeneratePath)
    {
        try
        {
            foreach (var MagInfo in orgAtpDTO.magazineDTOs)
            {
                if (MagInfo.toolDiameter > 0 && MagInfo.toolId > 0 && !_oldTool_NewToolPairs.ContainsKey(MagInfo.toolId))
                {
                    if (IsToolDuplInMesAtp(MagInfo.toolDiameter, MagInfo.toolId, mesAtpDTO))
                    {
                        int nNewToolId = GetNewToolId(MagInfo.toolDiameter, MagInfo.toolId, orgAtpDTO, mesAtpDTO);
                        if (nNewToolId != 0)
                        {
                            var tmpToolDto = orgAtpDTO.toolDTOs.FirstOrDefault(t => t.toolDiameter == MagInfo.toolDiameter)?.Clone();
                            if (tmpToolDto == null)
                            {
                                tmpToolDto = orgAtpDTO.toolDTOs[MagInfo.toolId].Clone();
                                tmpToolDto.toolDiameter = MagInfo.toolDiameter;
                                UpdateToolParaFromDia(tmpToolDto, diaToolList);
                            }
                            tmpToolDto.toolId = nNewToolId;
                            _oldTool_NewToolPairs.Add(MagInfo.toolId, tmpToolDto);
                        }
                        else
                        {
                            _logger.LogError($"Org2NewAtpDTO - GetNewToolId失败 dia={MagInfo.toolDiameter}, toolId={MagInfo.toolId}");
                            return new MessageEntity
                            {
                                IsSuccessful = false,
                                Information = "DingTai-Org2NewAtpDTO-GetNewToolId失败",
                                Timestamp = DateTime.Now,
                                Data = ""
                            };
                        }
                    }
                }
            }

            AtpDTO newAtpDTO = new AtpDTO();
            newAtpDTO.InitAtpDTO(orgAtpDTO.ToolDTOCount, orgAtpDTO.MagazineDTOCount, orgAtpDTO.ToolTolCount, orgAtpDTO.PeckDrlDTOCount, orgAtpDTO.ShortSlotNibCount, orgAtpDTO.LongSlotNibCount);

            try
            {
                foreach (var toolDto in orgAtpDTO.toolDTOs)
                {
                    if (_oldTool_NewToolPairs.ContainsKey(toolDto.toolId))
                    {
                        var newToolDto = _oldTool_NewToolPairs[toolDto.toolId];
                        newAtpDTO.toolDTOs[newToolDto.toolId] = newToolDto;
                        _oldToolId_New.Add(toolDto.toolId, newToolDto.toolId);
                    }
                    else if (!_oldToolId_New.ContainsValue(toolDto.toolId))
                    {
                        var newToolDto = toolDto.Clone();
                        //UpdateToolParaFromDia(newToolDto, diaToolList);
                        newAtpDTO.toolDTOs[toolDto.toolId] = newToolDto;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"写入源文件的toolDTO失败 - {ex.Message}, {ex.StackTrace}");
                return new MessageEntity
                {
                    IsSuccessful = false,
                    Information = "写入源文件的toolDTO失败",
                    Timestamp = DateTime.Now,
                    Data = ""
                };
            }

            try
            {
                foreach (var MagInfo in orgAtpDTO.magazineDTOs)
                {
                    var newMagInfo = MagInfo.Clone();
                    int nOldMagToolId = newMagInfo.toolId;
                    if (nOldMagToolId > 0)
                    {
                        if (_oldTool_NewToolPairs.ContainsKey(nOldMagToolId))
                        {
                            newMagInfo.toolId = _oldTool_NewToolPairs[nOldMagToolId].toolId;
                        }
                    }
                    newAtpDTO.magazineDTOs[MagInfo.magazineId] = newMagInfo;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"写入源文件的magazineDTOs失败 - {ex.Message}, {ex.StackTrace}");
                return new MessageEntity
                {
                    IsSuccessful = false,
                    Information = "写入源文件的magazineDTOs失败",
                    Timestamp = DateTime.Now,
                    Data = ""
                };
            }

            try
            {
                foreach (var mesToolDto in mesAtpDTO.toolDTOs)
                {
                    if (mesToolDto.toolDiameter > 0)
                    {
                        var tmpToolDto = orgAtpDTO.toolDTOs.FirstOrDefault(t => t.toolDiameter == mesToolDto.toolDiameter)?.Clone();
                        if (tmpToolDto == null)
                        {
                            tmpToolDto = orgAtpDTO.toolDTOs[mesToolDto.toolId].Clone();
                            tmpToolDto.toolDiameter = mesToolDto.toolDiameter;
                            UpdateToolParaFromDia(tmpToolDto, diaToolList);
                        }
                        tmpToolDto.toolId = mesToolDto.toolId;

                        foreach (var toolMagInfo in mesToolDto.magazines)
                        {
                            tmpToolDto.magazines.Add(toolMagInfo);
                        }
                        newAtpDTO.toolDTOs[mesToolDto.toolId] = tmpToolDto;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"写入MESATP的toolDTO失败 - {ex.Message}, {ex.StackTrace}");
                return new MessageEntity
                {
                    IsSuccessful = false,
                    Information = "写入MESATP的toolDTO失败",
                    Timestamp = DateTime.Now,
                    Data = ""
                };
            }

            try
            {
                foreach (var mesMagInfo in mesAtpDTO.magazineDTOs)
                {
                    if (mesMagInfo.toolId > 0)
                    {
                        if (newAtpDTO.magazineDTOs[mesMagInfo.magazineId].toolId == 0)
                        {
                            newAtpDTO.magazineDTOs[mesMagInfo.magazineId] = mesMagInfo;
                            //newAtpDTO.toolDTOs[mesMagInfo.toolId].magazines.Add(mesMagInfo.magazineId);
                        }
                        else
                        {
                            _logger.LogError($"Org2NewAtpDTO - 写入MESATP的刀盘信息失败,刀座号被占用或者清空失败 - mesMagInfo.magazineId={mesMagInfo.magazineId} 已被orgMag占用toolId={newAtpDTO.magazineDTOs[mesMagInfo.magazineId].toolId}");
                            return new MessageEntity
                            {
                                IsSuccessful = false,
                                Information = $"写入MESATP的刀盘信息失败 - mesMagInfo.magazineId - {mesMagInfo.magazineId}已被占用",
                                Timestamp = DateTime.Now,
                                Data = ""
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"写入MESATP的magazineDTOs失败 - {ex.Message}, {ex.StackTrace}");
                return new MessageEntity
                {
                    IsSuccessful = false,
                    Information = "写入MESATP的magazineDTOs失败",
                    Timestamp = DateTime.Now,
                    Data = ""
                };
            }

            newAtpDTO.toolToleranceTableDTOs = new List<ToolToleranceTableDTO>(orgAtpDTO.toolToleranceTableDTOs);
            newAtpDTO.peckDrillingValuesDTOs = new List<PeckDrillingValuesDTO>(orgAtpDTO.peckDrillingValuesDTOs);
            newAtpDTO.shortSlotNibblingDTOs = new List<ShortSlotNibblingDTO>(orgAtpDTO.shortSlotNibblingDTOs);
            newAtpDTO.longSlotNibblingDTOs = new List<LongSlotNibblingDTO>(orgAtpDTO.longSlotNibblingDTOs);

            string folderPath = string.Empty;
            if (string.IsNullOrWhiteSpace(strGeneratePath))
            {
                folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vegaAtp");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
            else
            {
                folderPath = strGeneratePath;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
            //生成路径
            string resultPath = Path.Combine(folderPath, strGenerateFileName);
            // 写入文件
            return _atpFileParser.GenerateAtpFile(resultPath, newAtpDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{ex.Message}");
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = "Org2NewAtpDTO异常 - " + ex.Message,
                Timestamp = DateTime.Now,
                Data = ""
            };
        }
        finally
        {
            _oldTool_NewToolPairs.Clear();
            _oldToolId_New.Clear();
        }
    }

    private void UpdateToolParaFromDia(ToolDTO tmpToolDto, List<ToolDTO> diaToolList)
    {
        try
        {
            var toolDto = diaToolList.FirstOrDefault(t => t.toolDiameter == tmpToolDto.toolDiameter);
            if (toolDto != null)
            {
                tmpToolDto.spindleSpeed = toolDto.spindleSpeed;
                tmpToolDto.infeedZAxis = toolDto.infeedZAxis;
                tmpToolDto.retractZAxis = toolDto.retractZAxis;
                tmpToolDto.workPlaneAdjustment = toolDto.workPlaneAdjustment;
                tmpToolDto.toolLife = toolDto.toolLife;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public int GetNewToolId(float fDia, int nId, AtpDTO orgAtpDTO, AtpDTO mesAtpDTO)
    {
        try
        {
            var toolDto = mesAtpDTO.toolDTOs.FirstOrDefault(t => t.toolDiameter == fDia);
            if (toolDto != null)
            {
                return toolDto.toolId;
            }

            var lstToolD = orgAtpDTO.toolDTOs.FindAll(d => d.toolDiameter == fDia);
            if (lstToolD.Count > 1)
            {
                foreach (var ti in lstToolD)
                {
                    if (ti.toolId != nId && mesAtpDTO.toolDTOs.FirstOrDefault(t => t.toolId == ti.toolId)?.toolDiameter <= 0)
                    {
                        return ti.toolId;
                    }
                }
            }

            for (int i = 1; i <= mesAtpDTO.ToolDTOCount; i++)
            {
                if (mesAtpDTO.toolDTOs[i].toolDiameter <= 0)
                {
                    if (orgAtpDTO.toolDTOs[i].toolDiameter <= 0)
                    {
                        var value = _oldTool_NewToolPairs.FirstOrDefault(x => x.Value.toolId == i).Key;
                        if (value == 0)
                        {
                            return i;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return 0;
    }

    public bool IsToolDuplInMesAtp(float fDia, int nId, AtpDTO mesAtpDTO)
    {
        var findTool = mesAtpDTO.toolDTOs.FirstOrDefault(d => d.toolDiameter == fDia);
        if (findTool != null)
        {
            if (findTool.toolId != nId)
            {
                return true;
            }
        }

        var findToolId = mesAtpDTO.toolDTOs.FirstOrDefault(i => i.toolId == nId);
        if (findToolId != null && findToolId.toolDiameter > 0)
        {
            if (findToolId.toolDiameter != fDia)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 两个ATP文件是否有重复的刀信息（刀径一样刀号不一样）
    /// </summary>
    /// <param name="orgAtpDTO"></param>
    /// <param name="mesAtpDTO"></param>
    /// <returns></returns>
    public bool IsDuplToolInfo(AtpDTO orgAtpDTO, AtpDTO mesAtpDTO)
    {
        try
        {
            foreach (var toolInfo in mesAtpDTO.toolDTOs)
            {
                if (toolInfo.toolDiameter != 0)
                {
                    ToolDTO findToolDto = orgAtpDTO.toolDTOs.FirstOrDefault(d => d.toolDiameter == toolInfo.toolDiameter);
                    if (findToolDto != null && findToolDto.toolId == toolInfo.toolId)
                    {
                        return true;
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
}
