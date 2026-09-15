// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Quartz.Util;
using VgEAPClient.Common.CNC.ATP.Models;

namespace VgEAPClient.Common.CNC.ATP;

public class DiaFileParser
{
    public static bool IsDiaLine(string strLine)
    {
        if (strLine.IsNullOrWhiteSpace() || strLine.StartsWith("%%"))
        {
            return true;
        }
        return strLine.StartsWith("C", StringComparison.OrdinalIgnoreCase);
    }

    public static MessageEntity ParseFromFile(string orginDiaPath, out List<ToolDTO> tools)
    {
        tools = new List<ToolDTO>();
        if (!File.Exists(orginDiaPath))
        {
            return new MessageEntity
            {
                IsSuccessful = false,
                Information = $"直径表文件不存在：{orginDiaPath}",
                Timestamp = DateTime.Now,
                Data = ""
            };
        }

        List<string> lines = new List<string>();
        //按行获取文件内容
        using (StreamReader reader = new StreamReader(orginDiaPath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
                if (!IsDiaLine(line))
                {
                    return new MessageEntity
                    {
                        IsSuccessful = false,
                        Information = $"直径表文件格式错误 - {line} - {orginDiaPath}",
                        Timestamp = DateTime.Now,
                        Data = ""
                    };
                }
                ToolDTO tool = new ToolDTO();
                char[] delimiters = new char[] { 'C', 'S', 'U', 'F', 'B', 'A', 'Z', 'H' };
                Dictionary<char, string> result = ParseString(line, delimiters);
                foreach (var pair in result)
                {
                    switch (pair.Key)
                    {
                        case 'C':
                            tool.toolDiameter = StringUtil.GetFloatCode(pair.Value);
                            break;

                        case 'S':
                            tool.spindleSpeed = StringUtil.GetFloatCode(pair.Value);
                            break;

                        case 'U':
                            //tool.spindleSpeed = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'F':
                            tool.infeedZAxis = StringUtil.GetFloatCode(pair.Value);
                            break;

                        case 'B':
                            tool.retractZAxis = StringUtil.GetFloatCode(pair.Value);
                            break;

                        case 'A':
                            //tool. = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'Z':
                            tool.workPlaneAdjustment = StringUtil.GetFloatCode(pair.Value);
                            break;

                        case 'H':
                            tool.toolLife = StringUtil.GetIntCode(pair.Value);
                            break;
                    }
                }
                /*var result = StringUtil.SplitString(line, delimiters.ToArray());
                List<string> segments = result.segments;
                List<char> missingChars = result.missingChars;
                int j = 1;
                for (int i = 0; i < delimiters.Count; i++)
                {
                    if (!missingChars.Contains(delimiters[i]))
                    {
                        switch (delimiters[i])
                        {
                            case 'C':
                                tool.toolDiameter = StringUtil.GetFloatCode(segments[j++]);
                                break;

                            case 'S':
                                tool.spindleSpeed = StringUtil.GetFloatCode(segments[j++]);
                                break;

                            case 'U':
                                //tool.spindleSpeed = StringUtil.GetFloatCode(segments[j++]);
                                break;

                            case 'F':
                                tool.infeedZAxis = StringUtil.GetFloatCode(segments[j++]);
                                break;

                            case 'B':
                                tool.retractZAxis = StringUtil.GetFloatCode(segments[j++]);
                                break;

                            case 'A':
                                j++;
                                //tool. = StringUtil.GetFloatCode(segments[j++]);
                                break;

                            case 'Z':
                                tool.workPlaneAdjustment = StringUtil.GetFloatCode(segments[j++]);
                                break;

                            case 'H':
                                tool.toolLife = StringUtil.GetIntCode(segments[j++]);
                                break;
                        }
                    }
                }*/
                tools.Add(tool);
            }
        }

        return new MessageEntity
        {
            IsSuccessful = true,
            Information = $"直径表文件解析成功：{orginDiaPath}",
            Timestamp = DateTime.Now,
            Data = ""
        };
    }

    private static Dictionary<char, string> ParseString(string input, char[] targetChars)
    {
        Dictionary<char, string> result = new Dictionary<char, string>();

        // 优化后的正则表达式模式：支持以小数点开头的数字
        string pattern = $"[{string.Join("", targetChars)}](?<number>-?\\.?\\d+(?:\\.\\d+)?)";

        MatchCollection matches = Regex.Matches(input, pattern);

        foreach (Match match in matches)
        {
            if (match.Success)
            {
                char key = match.Value[0]; // 获取字符
                string number = match.Groups["number"].Value; // 获取数字部分

                if (!result.ContainsKey(key))
                {
                    result.Add(key, number);
                }
                else
                {
                    // 如果同一个字符已经存在，可以选择更新或忽略
                    // result[key] = number; // 取消注释这行来更新为最新值
                }
            }
        }

        return result;
    }
}
