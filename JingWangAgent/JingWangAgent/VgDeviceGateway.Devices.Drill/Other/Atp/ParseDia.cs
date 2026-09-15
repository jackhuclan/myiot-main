using VgDeviceGateway.Devices.Drill.Other.Dto;

namespace VgDeviceGateway.Devices.Drill.Other.Atp
{
    public class ParseDia
    {
        public static List<ToolDTO> ParseFromFile(string orginDiaPath)
        {
            List<string> lines = new List<string>();
            List<ToolDTO> tools = new List<ToolDTO>();
            //按行获取文件内容
            using (StreamReader reader = new StreamReader(orginDiaPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                    ToolDTO tool = new ToolDTO();
                    List<char> delimiters = new List<char> { 'C', 'S', 'F', 'B', 'A', 'Z', 'H' };
                    var result = StringUtil.SplitString(line, delimiters.ToArray());
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
                    }
                    tools.Add(tool);
                }
            }
            return tools;
        }
    }
}
