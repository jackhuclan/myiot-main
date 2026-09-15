using VgDeviceGateway.Devices.Drill.Other.Dto;

namespace VgDeviceGateway.Devices.Drill.Other.Atp
{
    public class FileToDTO
    {
        public static ToolDTO StrToToolDTO(List<string> lines)
        {
            ToolDTO toolDTO = new ToolDTO();
            //第一行指令解析
            List<char> delimiters = new List<char> { 'T', 'D', 'E', 'S', 'F', 'R', 'A', 'Z', 'N', 'B', 'C', 'P', 'U', 'V', 'W', 'X', 'G' };
            var result = StringUtil.SplitString(lines[0].Replace(",", ""), delimiters.ToArray());
            List<string> segments = result.segments;
            List<char> missingChars = result.missingChars;
            int j = 1;
            for (int i = 0; i < delimiters.Count; i++)
            {
                if (!missingChars.Contains(delimiters[i]))
                {
                    switch (delimiters[i])
                    {
                        case 'T':
                            toolDTO.toolId = int.Parse(segments[j++]);
                            break;

                        case 'D':
                            toolDTO.toolDiameter = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'E':
                            toolDTO.toolType = StringUtil.GetIntCode(segments[j++]);
                            break;

                        case 'S':
                            toolDTO.spindleSpeed = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'F':
                            toolDTO.infeedZAxis = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'R':
                            toolDTO.retractZAxis = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'A':
                            toolDTO.waitTime = StringUtil.GetIntCode(segments[j++]);
                            break;

                        case 'Z':
                            toolDTO.workPlaneAdjustment = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'N':
                            toolDTO.toolLife = StringUtil.GetIntCode(segments[j++]);
                            break;

                        case 'B':
                            toolDTO.drillToolLife = StringUtil.GetIntCode(segments[j++]);
                            break;

                        case 'C':
                            toolDTO.routerToolLife = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'P':
                            toolDTO.toolLifeMonitoring = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'U':
                            toolDTO.compensationDiameter = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'V':
                            toolDTO.routingFeedRate = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'W':
                            toolDTO.routerWear = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'X':
                            toolDTO.circularRoutingFeedRate = StringUtil.GetFloatCode(segments[j++]);
                            break;

                        case 'G':
                            toolDTO.toolFunctions = Enumerable.Range(1, 12).Select(f => false).ToList();
                            List<char> delimiterFuns = new List<char> { 'v', 'd', 'l', 'r', 's', 'c', 'q', 'o', 't', 'k', 'p' };
                            var resultFun = StringUtil.SplitString(segments[j++].Replace("\"", ""), delimiterFuns.ToArray());
                            List<string> segmentFuns = resultFun.segments;
                            List<char> missingCharFuns = resultFun.missingChars;
                            int k = 0;
                            for (int s = 0; s < delimiterFuns.Count; s++)
                            {
                                if (!missingCharFuns.Contains(delimiterFuns[s]))
                                {
                                    switch (delimiterFuns[s])
                                    {
                                        case 'v':
                                            toolDTO.toolFunctions[0] = segmentFuns[k++] == "+";
                                            break;

                                        case 'd':
                                            toolDTO.toolFunctions[1] = segmentFuns[k++] == "+";
                                            break;

                                        case 'l':
                                            toolDTO.toolFunctions[2] = segmentFuns[k++] == "+";
                                            break;

                                        case 'r':
                                            toolDTO.toolFunctions[3] = segmentFuns[k++] == "+";
                                            break;

                                        case 's':
                                            toolDTO.toolFunctions[4] = segmentFuns[k++] == "+";
                                            break;

                                        case 'c':
                                            toolDTO.toolFunctions[5] = segmentFuns[k++] == "+";
                                            break;

                                        case 'q':
                                            toolDTO.toolFunctions[6] = segmentFuns[k++] == "+";
                                            break;

                                        case 'o':
                                            toolDTO.toolFunctions[7] = segmentFuns[k++] == "+";
                                            break;

                                        case 't':
                                            toolDTO.toolFunctions[8] = segmentFuns[k++] == "+";
                                            break;

                                        case 'k':
                                            toolDTO.toolFunctions[9] = segmentFuns[k++] == "+";
                                            break;

                                        case 'p':
                                            toolDTO.toolFunctions[11] = segmentFuns[k++] == "+";
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            //第二行指令解析
            toolDTO.drillMethod = Enumerable.Range(1, 6).Select(f => new float()).ToList();
            string secondValue = lines[1].Split('Q')[1];
            string[] segmentDrills = secondValue.Split(',');
            toolDTO.drillMethod[0] = StringUtil.GetFloatCode(segmentDrills[0]);
            toolDTO.drillMethod[1] = StringUtil.GetIntCode(segmentDrills[2].Substring(3));
            toolDTO.drillMethod[2] = StringUtil.GetIntCode(segmentDrills[3]);
            toolDTO.drillMethod[3] = StringUtil.GetFloatCode(segmentDrills[4]);
            toolDTO.drillMethod[4] = StringUtil.GetIntCode("0");
            toolDTO.drillMethod[5] = string.IsNullOrWhiteSpace(segmentDrills[6].ToString()) ? 0 : StringUtil.GetFloatCode(segmentDrills[6].Substring(0, segmentDrills[6].Length - 1));

            //刀盘位置获取
            toolDTO.magazines = new List<int>();
            for (int i = 2; i < lines.Count; i++)
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
    }
}
