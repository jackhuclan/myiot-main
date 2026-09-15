// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;

namespace VgEAPClient.Common.CNC.ATP.Models
{
    internal class FileToDTO_9
    {
        public static ToolDTO StrToToolDTO(List<string> lines)
        {
            ToolDTO toolDTO = new ToolDTO();
            //第一行指令解析
            List<char> delimiters = new List<char> { 'T', 'D', 'E', 'S', 'F', 'R', 'A', 'Z', 'N', 'B', 'C', 'P', 'U', 'V', 'W', 'X', 'G', 'J', 'I' };
            var result = StringUtil.SplitString2(lines[0].Replace(",", ""), delimiters.ToArray());
            List<string> segments = result.segments;
            List<char> containsChars = result.containsChars;
            int j = 1;
            for (int i = 0; i < containsChars.Count; i++)
            {
                switch (containsChars[i])
                {
                    case 'T':
                        toolDTO.toolId = int.Parse(segments[i + 1]);
                        break;

                    case 'D':
                        toolDTO.toolDiameter = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'E':
                        toolDTO.toolType = StringUtil.GetIntCode(segments[i + 1]);
                        break;

                    case 'S':
                        toolDTO.spindleSpeed = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'F':
                        toolDTO.infeedZAxis = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'R':
                        toolDTO.retractZAxis = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'A':
                        toolDTO.waitTime = StringUtil.GetIntCode(segments[i + 1]);
                        break;

                    case 'Z':
                        toolDTO.workPlaneAdjustment = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'N':
                        toolDTO.toolLife = StringUtil.GetIntCode(segments[i + 1]);
                        break;

                    case 'B':
                        toolDTO.drillToolLife = StringUtil.GetIntCode(segments[i + 1]);
                        break;

                    case 'C':
                        toolDTO.routerToolLife = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'P':
                        toolDTO.toolLifeMonitoring = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'U':
                        toolDTO.compensationDiameter = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'V':
                        toolDTO.routingFeedRate = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'W':
                        toolDTO.routerWear = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'X':
                        toolDTO.circularRoutingFeedRate = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'J':
                        toolDTO.J = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'I':
                        toolDTO.I = StringUtil.GetFloatCode(segments[i + 1]);
                        break;

                    case 'G':
                        toolDTO.toolFunctions = Enumerable.Range(1, 16).Select(f => false).ToList();
                        List<char> delimiterFuns = new List<char> { 'v', 'd', 'l', 'r', 's', 'c', 'q', 'o', 't', 'k', 'p', 'f', '2', 'b', 'u' };
                        var resultFun = StringUtil.SplitString2(segments[i + 1].Replace("\"", ""), delimiterFuns.ToArray());
                        List<string> segmentFuns = resultFun.segments;
                        List<char> containsCharFuns = resultFun.containsChars;
                        int k = 0;
                        for (int s = 0; s < containsCharFuns.Count; s++)
                        {
                            switch (containsCharFuns[s])
                            {
                                case 'd':
                                    toolDTO.toolFunctions[0] = segmentFuns[s] == "+";
                                    break;

                                case 'l':
                                    toolDTO.toolFunctions[1] = segmentFuns[s] == "+";
                                    break;

                                case 'r':
                                    toolDTO.toolFunctions[2] = segmentFuns[s] == "+";
                                    break;

                                case 'c':
                                    toolDTO.toolFunctions[3] = segmentFuns[s] == "+";
                                    break;

                                case 'f':
                                    toolDTO.toolFunctions[4] = segmentFuns[s] == "+";
                                    break;

                                case 'k':
                                    toolDTO.toolFunctions[5] = segmentFuns[s] == "+";
                                    break;

                                case 'o':
                                    toolDTO.toolFunctions[6] = segmentFuns[s] == "+";
                                    break;

                                case 'p':
                                    toolDTO.toolFunctions[7] = segmentFuns[s] == "+";
                                    break;

                                case 'q':
                                    toolDTO.toolFunctions[8] = segmentFuns[s] == "+";
                                    break;

                                case 's':
                                    toolDTO.toolFunctions[9] = segmentFuns[s] == "+";
                                    break;

                                case 't':
                                    toolDTO.toolFunctions[10] = segmentFuns[s] == "+";
                                    break;

                                case 'v':
                                    toolDTO.toolFunctions[11] = segmentFuns[s] == "+";
                                    break;

                                case '2':
                                    toolDTO.toolFunctions[12] = segmentFuns[s] == "+";
                                    break;

                                case 'b':
                                    toolDTO.toolFunctions[13] = segmentFuns[s] == "+";
                                    break;

                                case 'u':
                                    toolDTO.toolFunctions[14] = segmentFuns[s] == "+";
                                    break;
                            }
                        }
                        break;
                }
            }

            //第2行
            string strTL = lines[1].Substring(lines[1].IndexOf("L") + 1);
            toolDTO.unknownL = ExtractLeadingNumber(strTL);

            //第4-5行指令解析
            toolDTO.drillMethod = Enumerable.Range(1, 6).Select(f => new float()).ToList();
            string secondValue = lines[3].Split('M')[1];
            string[] segmentDrills = secondValue.Split(',');
            toolDTO.drillMethod[0] = StringUtil.GetFloatCode(segmentDrills[0]);
            //刀盘位置获取
            toolDTO.magazines = new List<int>();
            for (int i = 4; i < lines.Count; i++)
            {
                string[] values = lines[i].Split('M');

                for (int k = 1; k < values.Length; k++)
                {
                    if (string.IsNullOrEmpty(values[k]))
                    {
                        continue;
                    }
                    toolDTO.magazines.Add(StringUtil.GetIntCode(values[k]));
                }
            }
            return toolDTO;
        }

        public static float ExtractLeadingNumber(string input)
        {
            // 匹配：可选的正负号 + 数字 + 可选的小数部分
            Match match = Regex.Match(input, @"^[-+]?(\d+\.?\d*|\.\d+)");
            float result = 0;
            if (match.Success && float.TryParse(match.Value, out result))
            {
                return result;
            }
            return 0;
        }
    }
}
